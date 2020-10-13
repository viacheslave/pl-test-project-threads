using System;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Principal;

namespace Plarium.Test.FourThreads.Extensions
{
    internal static class SecurityExtensions
    {
        // Gets file/folder owner
        public static string GetOwner(this FileSystemSecurity fileSystemSecurity)
        {
            IdentityReference identityReference = fileSystemSecurity.GetOwner(typeof(SecurityIdentifier));
            IdentityReference account = identityReference.Translate(typeof(NTAccount));

            return account.Value;
        }

        // Gets a string representation of file/folder effective current user allowed permissions.
        // Checks if current user has direct permissions or belongs to a group
        // Skips GENERIC permissions
        public static string GetEffectivePermissions(this FileSystemSecurity fileSystemSecurity)
        {
            FileSystemRights effectiveFileSystemRights = default(FileSystemRights);

            WindowsIdentity currentUser = WindowsIdentity.GetCurrent();
            WindowsPrincipal principal = new WindowsPrincipal(currentUser);
            
            AuthorizationRuleCollection rules = fileSystemSecurity.GetAccessRules(true, true, typeof(NTAccount));
            foreach (AuthorizationRule rule in rules)
            {
                FileSystemAccessRule fsAccessRule = rule as FileSystemAccessRule;
                if (fsAccessRule == null || fsAccessRule.AccessControlType == AccessControlType.Deny)
                {
                    continue;
                }

                NTAccount ntAccount = rule.IdentityReference as NTAccount;
                if (ntAccount == null)
                {
                    continue;
                }

                if (principal.IsInRole(ntAccount.Value) || 
                    string.Compare(currentUser.Name, ntAccount.Value, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    effectiveFileSystemRights |= ParseSecurityEnumValue(effectiveFileSystemRights, fsAccessRule.FileSystemRights);
                }
            }

            return effectiveFileSystemRights.ToString();
        }

        private static FileSystemRights ParseSecurityEnumValue(FileSystemRights effectiveFileSystemRights, FileSystemRights flags)
        {
            Enum.GetValues(typeof(FileSystemRights))
                .Cast<FileSystemRights>()
                .ToList()
                .ForEach(permission =>
                {
                    if ((flags & permission) == permission)
                    {
                        effectiveFileSystemRights |= permission;
                    }
                });

            return effectiveFileSystemRights;
        }
    }
}