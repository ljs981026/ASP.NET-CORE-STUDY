using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace NetCore.Data.Classes
{
	public class User
	{
        [Key]
        public string UserId { get; set; }
        public bool IsMembershipWithdrawn { get; set; }
        public DateTime JoinedUtcDate { get; set; }
        public string Password { get; set; }
        public int AccessFailedCount { get; set; }
        public string UserEmail { get; set; }
        public string UserName { get; set; }

        public ICollection<UserRolesByUser> UserRolesByUser { get; set; }
    }
}

