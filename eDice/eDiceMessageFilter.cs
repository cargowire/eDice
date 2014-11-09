/*
using System.Security.Permissions;
using System.Windows.Forms;

namespace eDice
{
    [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
    public class eDiceMessageFilter : IMessageFilter
    {
        private eDiceRegistration registration;
        public eDiceMessageFilter(eDiceRegistration registration)
        {
            this.registration = registration;
        }

        public bool PreFilterMessage(ref Message m)
        {
            return this.registration.HandleMessage(m.Msg, m.WParam, m.LParam);
        }
    }
}
*/