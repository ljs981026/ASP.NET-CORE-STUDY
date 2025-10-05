using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NetCore.Data.Classes
{
	public class UserRole
	{
        [Key]
        public string RoleId { get; set; }
        public DateTime ModifiedUtcDate { get; set; }
        public string RoleName { get; set; }
        public byte RolePriority { get; set; }

        public ICollection<UserRolesByUser> UserRolesByUser { get; set; }
    }
}

