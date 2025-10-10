using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace NetCore.Data.ViewModels
{
    public class WithdrawnInfo
    {
        /// <summary>
        /// 사용자 아이디
        /// </summary>
        public string UserId { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "비밀번호를 입력하세요.")] // 필수 항목
        [MinLength(6, ErrorMessage = "비밀번호는 최소 6자 이상 입력하세요.")] // 최소 입력 설정
        [Display(Name = "비밀번호")]
        public string Password { get; set; }
    }
}
