using System;
using System.Collections.Generic;

namespace NetCore.Databases.Data
{
    public partial class User
    {
        public User()
        {
            UserRolesByUser = new HashSet<UserRolesByUser>();
        }

        public string UserId { get; set; }
        public bool IsMembershipWithdrawn { get; set; }
        public DateTime JoinedUtcDate { get; set; }
        public string Password { get; set; }
        public string UserEmail { get; set; }
        public string UserName { get; set; }

        public ICollection<UserRolesByUser> UserRolesByUser { get; set; }
    }
}
