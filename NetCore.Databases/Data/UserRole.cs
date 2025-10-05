using System;
using System.Collections.Generic;

namespace NetCore.Databases.Data
{
    public partial class UserRole
    {
        public UserRole()
        {
            UserRolesByUser = new HashSet<UserRolesByUser>();
        }

        public string RoleId { get; set; }
        public DateTime ModifiedUtcDate { get; set; }
        public string RoleName { get; set; }
        public byte RolePriority { get; set; }

        public ICollection<UserRolesByUser> UserRolesByUser { get; set; }
    }
}
