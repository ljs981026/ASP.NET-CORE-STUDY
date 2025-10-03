using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace NetCore.Data.DataModels
{
    // 데이터 어노테이션
    public class User
    {
        // 기본키 지정
        [Key, StringLength(50), Column(TypeName = "varchar(50)")]
        public string UserId { get; set; }
        // 필수값
        [Required, StringLength(100),  Column(TypeName = "nvarchar(100)")]
        public string UserName { get; set; }
        // 필수값
        [Required, StringLength(320), Column(TypeName = "varchar(320)")]
        public string UserEmail { get; set; }
        [Required, StringLength(130), Column(TypeName = "varchar(130)")]
        public string Password { get; set; }
        
        // 회원탈퇴여부 추가
        [Required]
        public bool IsMembershipWithdrawn { get; set; }
        
        [Required]
        public DateTime JoinedUtcDate { get; set; }
        
        // FK 지정
        [ForeignKey(name: "UserId")]
        public virtual ICollection<UserRolesByUser> UserRolesByUser { get; set; }
    }
}
