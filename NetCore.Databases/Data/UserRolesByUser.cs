using System;
using System.Collections.Generic;

namespace NetCore.Databases.Data
{
    public partial class UserRolesByUser
    {
        public string UserId { get; set; }
        public string RoleId { get; set; }
        public DateTime OwnedUtcDate { get; set; }

        public UserRole Role { get; set; }
        public User User { get; set; }
    }
}
