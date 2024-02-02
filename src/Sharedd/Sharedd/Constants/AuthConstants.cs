using System;
namespace Shared.Constants
{
    public class AuthConstants
    {
        #region Roles
        public static class Roles
        {
            public const string SUPER_ADMIN = "Super Admin";
            public const string ADMIN = "Admin";
            public const string PARENT = "Parent";
            public const string STUDENT = "Student";
            public const string BUS_DRIVER = "Bus Driver";
            public const string STAFF = "Staff";
        }
        #endregion


        #region Policies
        public static class Policies
        {
            public const string ROOTS = "Root Accounts";
            public const string ADMINS = "Admin Accounts";
        }
        #endregion
    }
}

