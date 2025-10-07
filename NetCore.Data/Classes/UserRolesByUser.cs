using System;
using System.ComponentModel.DataAnnotations;

namespace NetCore.Data.Classes
{
	public class UserRolesByUser
	{
        [Key]
        public string UserId { get; set; }
        [Key]
        public string RoleId { get; set; }
        public DateTime OwnedUtcDate { get; set; }

        public UserRole UserRole { get; set; }
        public User User { get; set; }
    }
}

