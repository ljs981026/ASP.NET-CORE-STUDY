using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using NetCore.Data.ViewModels;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NetCore.Web.Controllers
{
    public class DataController : Controller
    {
        private IDataProtector _protection;

        public DataController(IDataProtectionProvider provider)
        {
            _protection = provider.CreateProtector("NetCore.data.v1");
        }

        // GET: /<controller>/
        [HttpGet]
        public IActionResult AES()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AES(AESInfo aes)
        {
            string message = string.Empty;
            if (ModelState.IsValid)
            {
                string userInfo = aes.UserId + aes.Password;
                aes.EncUserInfo = _protection.Protect(userInfo); // 암호화 정보
                // aes.DecUserInfo = _protection.Unprotect(userInfo); // 복호화 정보
                aes.DecUserInfo = _protection.Unprotect(aes.EncUserInfo);

                ViewData["Message"] = "암복호화가 성공적으로 이루어졌습니다.";

                return View(aes);
            }
            else
            {
                message = "암복호화를 위한 정보를 올바르게 입력하세요.";
            }
            ModelState.AddModelError(string.Empty, message);  
            return View(aes);
        }
    }
}

