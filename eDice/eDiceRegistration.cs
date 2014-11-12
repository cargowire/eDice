using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;

using eDice.SDK;

namespace eDice
{
    public class eDiceRegistration : IDiceRegistration
    {
        private IntPtr registrationHandle;

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
        public event EventHandler<DiceState> DiceRolled = delegate { };

        /// <summary>
        /// The device has been shaken
        /// </summary>
        public event EventHandler DiceShaken = delegate { };

        public event EventHandler<DiceState> DicePower = delegate { };

        public event EventHandler<DiceState> DiceConnect = delegate { };

        public event EventHandler<DiceState> DiceDisconnect = delegate { };

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

        public void StartMatch()
        {
            var structSize = Marshal.SizeOf(typeof(EDICE_START_PAIRING_INFOR));
            EDICE_START_PAIRING_INFOR startInfo;
		    startInfo.id = -1;

            IntPtr startInfoPtr = Marshal.AllocHGlobal(structSize);
            Marshal.StructureToPtr(startInfo, startInfoPtr, false);

            Vrlib.SetInteractionState(
                this.registrationHandle,
                (uint)EDICE_STATE_TYPE.EDICE_START_PAIRING,
                startInfoPtr,
                (uint)structSize);
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
                                var pState = (EDICE_STATE_INFOR)Marshal.PtrToStructure(dataPtr, typeof(EDICE_STATE_INFOR));
                                var statesOffset = Marshal.OffsetOf(typeof(EDICE_STATE_INFOR), "states");

                                var statePtr = new IntPtr(dataPtr.ToInt32() + statesOffset.ToInt32());
                                var structSize = Marshal.SizeOf(typeof(DICE_STATE));
                                var innerStructs = new List<DICE_STATE>();
                                for (int i = 0; i < pState.num; i++)
                                {
                                    innerStructs.Add((DICE_STATE)Marshal.PtrToStructure(statePtr, typeof(DICE_STATE)));
                                    statePtr = (IntPtr)((int)statePtr + structSize);

                                    this.DiceRolled(this, new DiceState() { Value = innerStructs[0].value });
                                }

                                //System.Diagnostics.Debug.WriteLine(
                                //    string.Format("E-Dice rolled ID %d, value%d ",
                                //        states[i].id, states[i].value));
                                //    EdiceMatchValue(pState->states[i].id);
                                //    m_listEDice.InsertString(-1,sText);
                                break;
                            }
                        case (uint)EDICE_STATE_TYPE.EDICE_SHAKE:
                            {
                                this.DiceShaken(this, EventArgs.Empty);

                                //PEDICE_STATE_INFOR pState = (PEDICE_STATE_INFOR)info->data;
                                //for(int i = 0 ;i < pState->num; i++)
                                //{
                                //    sText.Format(L"E-Dice shake ID %d, power %d",pState->states[i].id,pState->states[i].power);
                                //    m_listEDice.InsertString(-1,sText);

                                //}
                                break;
                            }
                        case (uint)EDICE_STATE_TYPE.EDICE_DROP:
                            {
                                //PEDICE_STATE_INFOR pState = (PEDICE_STATE_INFOR)info->data;
                                //for(int i = 0 ;i < pState->num; i++)
                                //{
                                //    sText.Format(L"E-Dice DROP ID %d num%d ",pState->states[i].id,pState->num);
                                //    m_listEDice.InsertString(-1,sText);
                                //}

                                break;
                            }
                        case (uint)EDICE_STATE_TYPE.EDICE_POWER:
                            {
                                this.DicePower(this, null);
                                //PEDICE_STATE_INFOR pState = (PEDICE_STATE_INFOR)info->data;
                                //sText.Format(L"E-Dice low power ID %d ",pState->states[0].id);
                                //m_listEDice.InsertString(-1,sText);
                                break;
                            }
                        case (uint)EDICE_STATE_TYPE.EDICE_CONNECT:
                            {
                                this.DiceConnect(this, null);
                                var pConnect = (EDICE_CONNECT_INFOR)Marshal.PtrToStructure(dataPtr, typeof(EDICE_CONNECT_INFOR));
                                //EDICE_CONNECT_INFOR pState = (EDICE_CONNECT_INFOR)structInfo.data;
                                //gDongleId = pState->id[0];
                                //sText.Format(L"E-Dice connect: num = %d id[0] = %d",pState->num,pState->id[0]);
                                //m_listEDice.InsertString(-1,sText);
                                break;
                            }
                        case (uint)EDICE_STATE_TYPE.EDICE_DISCONNECT:
                            {
                                this.DiceDisconnect(this, null);
                                //PEDICE_DISCONNECT_INFOR pState = (PEDICE_DISCONNECT_INFOR)info->data;
                                //sText.Format(L"E-Dice disconnect: num = %d ",pState->num);
                                //m_listEDice.InsertString(-1,sText);
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
    }
}