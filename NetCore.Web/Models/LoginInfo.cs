using System.ComponentModel.DataAnnotations;

namespace NetCore.Web.Models
{
    public class LoginInfo
    {
        [Required(ErrorMessage = "사용자 아이디를 입력하세요.")] // 필수 항목이기 때문
        [MinLength(6, ErrorMessage = "사용자 아이디는 최소 6자 이상 입력하세요.")] // 최소 입력 설정
        [Display(Name = "사용자 아이디")]
        public string UserId { get; set; }

        [Required(ErrorMessage = "비밀번호를 입력하세요.")] // 필수 항목
        [MinLength(6, ErrorMessage = "비밀번호는 최소 6자 이상 입력하세요.")] // 최소 입력 설정
        [Display(Name = "비밀번호")]
        public string Password { get; set; }
    }
}
