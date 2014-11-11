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

        public int LastDiceRoll { get; private set; }

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

        public bool HandleMessage(int message, IntPtr wParam, IntPtr lParam)
        {
            if (message == Vrlib.WM_EDICE)
            {
                switch ((uint)wParam)
                {
                    case (uint)EDICE_STATE_TYPE.EDICE_ROLLED:
                    {
                        uint size = 1024;
                        IntPtr info = IntPtr.Zero;

                        try
                        {
                            info = Marshal.AllocHGlobal(1024);
                            Vrlib.GetInteractionMessageInfo(lParam, info, ref size);

                            var structInfo =
                                (VR_MESSAGE_INFORMATION)Marshal.PtrToStructure(info, typeof(VR_MESSAGE_INFORMATION));

                            var pState = structInfo.data;

                            var offset1 = Marshal.OffsetOf(typeof(VR_MESSAGE_INFORMATION), "data");
                            var offset2 = Marshal.OffsetOf(typeof(EDICE_STATE_INFOR), "states");

                            var ptr = new IntPtr(info.ToInt32() + offset1.ToInt32());
                            ptr = new IntPtr(ptr.ToInt32() + offset2.ToInt32());
                            var structSize = Marshal.SizeOf(typeof(DICE_STATE));
                            var innerStructs = new List<DICE_STATE>();
                            for (int i = 0; i < pState.num; i++)
                            {
                                innerStructs.Add((DICE_STATE)Marshal.PtrToStructure(ptr, typeof(DICE_STATE)));
                                ptr = (IntPtr)((int)ptr + structSize);

                                this.DiceRolled(this, new DiceState() { Value = innerStructs[0].value });
                                LastDiceRoll = innerStructs[0].value;
                            }
                        }
                        finally
                        {
                            Marshal.FreeHGlobal(info);
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
                        //PEDICE_STATE_INFOR pState = (PEDICE_STATE_INFOR)info->data;
                        //sText.Format(L"E-Dice low power ID %d ",pState->states[0].id);
                        //m_listEDice.InsertString(-1,sText);
                        break;
                    }
                    case (uint)EDICE_STATE_TYPE.EDICE_CONNECT:
                    {
                        //PEDICE_CONNECT_INFOR pState = (PEDICE_CONNECT_INFOR)info->data;
                        //gDongleId = pState->id[0];
                        //sText.Format(L"E-Dice connect: num = %d id[0] = %d",pState->num,pState->id[0]);
                        //m_listEDice.InsertString(-1,sText);
                        break;
                    }
                    case (uint)EDICE_STATE_TYPE.EDICE_DISCONNECT:
                    {
                        //PEDICE_DISCONNECT_INFOR pState = (PEDICE_DISCONNECT_INFOR)info->data;
                        //sText.Format(L"E-Dice disconnect: num = %d ",pState->num);
                        //m_listEDice.InsertString(-1,sText);
                        break;
                    }
                }

                Vrlib.CloseInteractionMessageHandle(lParam);

                return true;
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