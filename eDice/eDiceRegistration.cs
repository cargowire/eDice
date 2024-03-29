﻿using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Collections.Generic;

using eDice.SDK;

namespace eDice
{
    public class eDiceRegistration : IDiceRegistration
    {
        private IntPtr registrationHandle;
        private List<int> dongleIds = new List<int>();

        /// <summary>
        /// A registration for e-dice events
        /// </summary>
        /// <param name="registrationHandle"></param>
        public eDiceRegistration(IntPtr registrationHandle)
        {
            this.registrationHandle = registrationHandle;
        }

        /// <summary>
        /// The dice has been rolled
        /// </summary>
        public event EventHandler<DiceStateEventArgs> DiceRolled = delegate { };

        /// <summary>
        /// The dice has been shaken
        /// </summary>
        public event EventHandler<DiceStateEventArgs> DiceShaken = delegate { };

        /// <summary>
        /// The dice power has been reported
        /// </summary>
        public event EventHandler<DiceStateEventArgs> DicePower = delegate { };

        /// <summary>
        /// The dice has been dropped
        /// </summary>
        public event EventHandler<DiceStateEventArgs> DiceDropped = delegate { };

        /// <summary>
        /// Dice have connected
        /// </summary>
        public event EventHandler<DongleEventArgs> DongleConnected = delegate { };

        /// <summary>
        /// Dice have disconnected
        /// </summary>
        public event EventHandler<DongleEventArgs> DongleDisconnected = delegate { };

        /// <summary>
        /// Gets the paired devices
        /// </summary>
        public ReadOnlyCollection<int> ConnectedDongles 
        {
            get
            {
                return this.dongleIds.AsReadOnly();
            } 
        } 

        internal IntPtr HookMessage(int message, IntPtr wParam, IntPtr lParam)
        {
            if (message < 0)
            {
                // You need to call CallNextHookEx without further processing
                // and return the value returned by CallNextHookEx
                return new IntPtr(User32.CallNextHookEx(IntPtr.Zero, message, wParam, lParam));
            }

            var actualMessage = (User32.CWPSTRUCT)Marshal.PtrToStructure(lParam, typeof(User32.CWPSTRUCT));
            this.HandleMessage(actualMessage.message, actualMessage.wparam, actualMessage.lparam);

            return new IntPtr(User32.CallNextHookEx(IntPtr.Zero, message, wParam, lParam));
        }

        public void Pair()
        {
            var dongles = (this.dongleIds != null && this.dongleIds.Any()) ? this.dongleIds : new List<int>() { -1 };
            foreach (var dongleId in dongles)
            {
                IntPtr startInfoPtr = IntPtr.Zero;
                try
                {
                    var structSize = Marshal.SizeOf(typeof(EDICE_START_PAIRING_INFOR));
                    EDICE_START_PAIRING_INFOR startInfo;
                    startInfo.id = dongleId;

                    startInfoPtr = Marshal.AllocHGlobal(structSize);
                    Marshal.StructureToPtr(startInfo, startInfoPtr, false);

                    Vrlib.SetInteractionState(
                        this.registrationHandle,
                        (uint)EDICE_STATE_TYPE.EDICE_START_PAIRING,
                        startInfoPtr,
                        (uint)structSize);
                }
                finally
                {
                    Marshal.FreeHGlobal(startInfoPtr);
                }
            }
        }

        public void Unpair()
        {
            foreach (int dongleId in this.dongleIds)
            {
                IntPtr endInfoPtr = IntPtr.Zero;
                try
                {
                    var structSize = Marshal.SizeOf(typeof(EDICE_END_PAIRING_INFOR));
                    EDICE_END_PAIRING_INFOR endInfo;
                    endInfo.id = dongleId;

                    endInfoPtr = Marshal.AllocHGlobal(structSize);
                    Marshal.StructureToPtr(endInfo, endInfoPtr, false);

                    Vrlib.SetInteractionState(
                        this.registrationHandle,
                        (uint)EDICE_STATE_TYPE.EDICE_END_PAIRING,
                        endInfoPtr,
                        (uint)structSize);
                }
                finally
                {
                    Marshal.FreeHGlobal(endInfoPtr);
                }
            }
        }

        /// <summary>
        /// Gets paired devices
        /// Note: This currently always seems to return zero (so too does the C++ sample project).
        /// The only alternative therefore is to call pair regularly or set some kind of timeout from 
        /// 'shaken' (the most often occuring dice activity) and call 'Pair' if it has not occured after a certain time.
        /// </summary>
        /// <param name="dongleId">The dongle id</param>
        /// <returns>A list of paired dice ids</returns>
        public List<int> GetPairedDevices(int? dongleId = null)
        {
            IntPtr pairingInfoPtr = IntPtr.Zero;
            try
            {
                uint structSize = 1024;
                EDICE_PAIRING_INFOR pairingInfo = new EDICE_PAIRING_INFOR();
                pairingInfo.id = dongleId ?? this.dongleIds.FirstOrDefault();

                pairingInfoPtr = Marshal.AllocHGlobal((int)structSize);
                Marshal.StructureToPtr(pairingInfo, pairingInfoPtr, false);

                var ids = new List<int>();

                if (Vrlib.GetInteractionState(
                    this.registrationHandle,
                    (uint)EDICE_STATE_TYPE.EDICE_QUERY_PAIRED,
                    pairingInfoPtr,
                    ref structSize))
                {

                    var paired =
                        (EDICE_PAIRING_INFOR)Marshal.PtrToStructure(pairingInfoPtr, typeof(EDICE_PAIRING_INFOR));

                    var dicesOffset = Marshal.OffsetOf(typeof(EDICE_PAIRING_INFOR), "dices");

                    var idPtr = new IntPtr(pairingInfoPtr.ToInt32() + dicesOffset.ToInt32());
                    for (int i = 0; i < paired.num; i++)
                    {
                        ids.Add(Marshal.ReadInt32(idPtr));
                        idPtr = (IntPtr)((int)idPtr + sizeof(int));
                    }
                }
                return ids;
            }
            finally
            {
                Marshal.FreeHGlobal(pairingInfoPtr);
            }
        }

        public bool HandleMessage(int message, IntPtr wParam, IntPtr lParam)
        {
            if (message == Vrlib.WM_EDICE)
            {
                uint size = 1024;
                IntPtr info = IntPtr.Zero;

                info = Marshal.AllocHGlobal(1024);
                Vrlib.GetInteractionMessageInfo(lParam, info, ref size);

                var structInfo = (VR_MESSAGE_INFORMATION)Marshal.PtrToStructure(info, typeof(VR_MESSAGE_INFORMATION));
                var dataOffset = Marshal.OffsetOf(typeof(VR_MESSAGE_INFORMATION), "data");
                var dataPtr = new IntPtr(info.ToInt32() + dataOffset.ToInt32());

                try
                {
                    switch ((uint)wParam)
                    {
                        case (uint)EDICE_STATE_TYPE.EDICE_ROLLED:
                            {
                                var diceStates = this.GetStateInformation(dataPtr);

                                this.DiceRolled(
                                    this,
                                    new DiceStateEventArgs(
                                        diceStates.Select(
                                            s => new DiceState()
                                            {
                                                Id = s.id,
                                                Value = s.value,
                                                Power = s.power
                                            }).ToList()));

                                break;
                            }
                        case (uint)EDICE_STATE_TYPE.EDICE_SHAKE:
                            {
                                var diceStates = this.GetStateInformation(dataPtr);

                                this.DiceShaken(
                                    this,
                                    new DiceStateEventArgs(
                                        diceStates.Select(
                                            s => new DiceState()
                                            {
                                                Id = s.id,
                                                Value = null,
                                                Power = s.power
                                            }).ToList()));
                                break;
                            }
                        case (uint)EDICE_STATE_TYPE.EDICE_DROP:
                            {
                                var diceStates = this.GetStateInformation(dataPtr);

                                this.DiceDropped(
                                    this,
                                    new DiceStateEventArgs(
                                        diceStates.Select(
                                            s => new DiceState()
                                            {
                                                Id = s.id,
                                                Value = null,
                                                Power = null
                                            }).ToList()));

                                break;
                            }
                        case (uint)EDICE_STATE_TYPE.EDICE_POWER:
                            {
                                var diceStates = this.GetStateInformation(dataPtr);

                                this.DicePower(
                                    this,
                                    new DiceStateEventArgs(
                                        diceStates.Select(
                                            s => new DiceState()
                                            {
                                                Id = s.id,
                                                Value = null,
                                                Power = s.power
                                            }).ToList()));

                                break;
                            }
                        case (uint)EDICE_STATE_TYPE.EDICE_CONNECT:
                            {
                                this.dongleIds = this.GetConnectionInformation<EDICE_DISCONNECT_INFOR>(dataPtr);

                                this.DongleConnected(
                                    this,
                                    new DongleEventArgs(this.dongleIds));
                                break;
                            }
                        case (uint)EDICE_STATE_TYPE.EDICE_DISCONNECT:
                            {
                                var ids = this.GetConnectionInformation<EDICE_DISCONNECT_INFOR>(dataPtr);
                                ids.ForEach(id => this.dongleIds.Remove(id));

                                this.DongleDisconnected(
                                    this,
                                    new DongleEventArgs(ids));
                                break;
                            }
                    }

                    Vrlib.CloseInteractionMessageHandle(lParam);

                    return true;
                }
                finally
                {
                    Marshal.FreeHGlobal(info);
                }
            }

            return false;
        }

        public void Unregister()
        {
            Vrlib.Unregister(this.registrationHandle);
            this.registrationHandle = IntPtr.Zero;
        }

        public void Dispose()
        {
            if (this.registrationHandle != IntPtr.Zero)
            {            
                Vrlib.Unregister(this.registrationHandle);
            }
        }

        private List<DICE_STATE> GetStateInformation(IntPtr ediceStateInforPtr)
        {
            var pState = (EDICE_STATE_INFOR)Marshal.PtrToStructure(ediceStateInforPtr, typeof(EDICE_STATE_INFOR));
            var statesOffset = Marshal.OffsetOf(typeof(EDICE_STATE_INFOR), "states");

            var statePtr = new IntPtr(ediceStateInforPtr.ToInt32() + statesOffset.ToInt32());
            var structSize = Marshal.SizeOf(typeof(DICE_STATE));
            var innerStructs = new List<DICE_STATE>();
            for (int i = 0; i < pState.num; i++)
            {
                innerStructs.Add((DICE_STATE)Marshal.PtrToStructure(statePtr, typeof(DICE_STATE)));
                statePtr = (IntPtr)((int)statePtr + structSize);
            }

            return innerStructs;
        }

        private List<int> GetConnectionInformation<T>(IntPtr ediceStateInforPtr)
            where T : DiceConnectionInformation
        {
            var pConnect = (T)Marshal.PtrToStructure(ediceStateInforPtr, typeof(T));

            var idsOffset = Marshal.OffsetOf(typeof(EDICE_CONNECT_INFOR), "id");

            var idPtr = new IntPtr(ediceStateInforPtr.ToInt32() + idsOffset.ToInt32());
            var ids = new List<int>();
            for (int i = 0; i < pConnect.num; i++)
            {
                ids.Add(Marshal.ReadInt32(idPtr));
                idPtr = (IntPtr)((int)idPtr + sizeof(int));
            }

            return ids;
        }
    }
}