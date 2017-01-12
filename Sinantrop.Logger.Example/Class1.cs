using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace Sinantrop.Logger.Example
{
    class Class1
    {
        const int NO_ERROR = 0;
        const int ERROR_INSUFFICIENT_BUFFER = 122;

        enum SID_NAME_USE
        {
            SidTypeUser = 1,
            SidTypeGroup,
            SidTypeDomain,
            SidTypeAlias,
            SidTypeWellKnownGroup,
            SidTypeDeletedAccount,
            SidTypeInvalid,
            SidTypeUnknown,
            SidTypeComputer
        }

        [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern bool LookupAccountSid(
          string lpSystemName,
          [MarshalAs(UnmanagedType.LPArray)] byte[] Sid,
          StringBuilder lpName,
          ref uint cchName,
          StringBuilder ReferencedDomainName,
          ref uint cchReferencedDomainName,
          out SID_NAME_USE peUse);


        private static byte[] SidToBytes(string sid)
        {
            try
            {
                sid = sid.Replace("_Classes", "");
                var siSid = new SecurityIdentifier(sid);                
                byte[] bytes = new byte[siSid.BinaryLength];
                siSid.GetBinaryForm(bytes, 0);

                return bytes;
            }
            catch (Exception)
            {                
                return null;
            }
        }

        [STAThread]
        static void Main(string[] args)
        {
            SidDemmystifier();


            Console.Read();
        }

        private static void SidDemmystifier()
        {
            string[] sidkeys = Registry.Users.GetSubKeyNames();

            foreach (string sidkey in sidkeys)
            {
                if (sidkey.Contains("DEFAULT"))
                    continue;

                byte[] bytes = SidToBytes(sidkey);

                if (bytes == null)
                    continue;

                StringBuilder name = new StringBuilder();
                uint cchName = (uint) name.Capacity;
                StringBuilder referencedDomainName = new StringBuilder();
                uint cchReferencedDomainName = (uint) referencedDomainName.Capacity;
                SID_NAME_USE sidUse;


                int err = NO_ERROR;
                if (!LookupAccountSid(null, bytes, name, ref cchName, referencedDomainName, ref cchReferencedDomainName, out sidUse))
                {
                    err = System.Runtime.InteropServices.Marshal.GetLastWin32Error();
                    if (err == ERROR_INSUFFICIENT_BUFFER)
                    {
                        name.EnsureCapacity((int) cchName);
                        referencedDomainName.EnsureCapacity((int) cchReferencedDomainName);
                        err = NO_ERROR;
                        if (!LookupAccountSid(null, bytes, name, ref cchName, referencedDomainName, ref cchReferencedDomainName, out sidUse))
                            err = System.Runtime.InteropServices.Marshal.GetLastWin32Error();
                    }
                }
                if (err == 0)
                    Console.WriteLine(@"Found account {0} : {1}\{2}", sidUse, referencedDomainName.ToString(), name.ToString());
                else
                    Console.WriteLine(@"Error : {0}", err);
            }
        }
    }
}
