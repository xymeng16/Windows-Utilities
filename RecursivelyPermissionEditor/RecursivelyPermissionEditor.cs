using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.AccessControl;
namespace RecursivelyPermissionEditor
{
    enum Mode
    {
        ADD, REMOVE
    };

    enum Type
    {
        FILE, DIRECTORY
    };
    class Program
    {
        static void Main(string[] args)
        {
            if(args.Length < 1)
            {
                Console.Out.WriteLine("Usage: RecurPermissionEditor [dir1] [dir2] ...");
            }
            for (int i = 0; i < args.Length; i++)
            {
                DFSDirTraverse(args[i]);
            }
            
        }
        public static void DFSDirTraverse(string dir)
        {
            try
            {
                foreach (var d in Directory.GetDirectories(dir))
                {
                    ModifySecurity(Type.DIRECTORY, d, "Everyone", FileSystemRights.FullControl, AccessControlType.Allow, Mode.ADD);
                    foreach (var f in Directory.GetFiles(d))
                    {
                        ModifySecurity(Type.FILE, f, "Everyone", FileSystemRights.FullControl, AccessControlType.Allow, Mode.ADD);
                    }
                    DFSDirTraverse(d);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Removes an ACL entry on the specified file for the specified account.
        /// </summary>
        /// <param name="fileName">The name of the file needed to be modified.</param>
        /// <param name="account">This parameter delcares which user that the rights belong to.</param>
        /// <param name="rights">Thr rights to be added or removed.</param>
        /// <param name="controlType">The type of the access control.</param>
        
        public static void ModifySecurity(Type type, string fileName, string account,
            FileSystemRights rights, AccessControlType controlType, Mode mode)
        {
            FileSystemSecurity Security;

            // Get a FileSecurity object that represents the
            // current security settings.
            if (Type.FILE == type)
                Security = File.GetAccessControl(fileName);
            else
                Security = Directory.GetAccessControl(fileName);

            if (Mode.ADD == mode)
                Security.AddAccessRule(new FileSystemAccessRule(account, rights, controlType));
            // Remove the FileSystemAccessRule from the security settings.
            else
                Security.RemoveAccessRule(new FileSystemAccessRule(account, rights, controlType));

            // Set the new access settings.
            if (Type.FILE == type)
                File.SetAccessControl(fileName, Security as FileSecurity);
            else
                Directory.SetAccessControl(fileName, Security as DirectorySecurity);

        }
    }
}

