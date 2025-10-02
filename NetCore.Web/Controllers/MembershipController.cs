using Microsoft.AspNetCore.Mvc;
using NetCore.Web.Models;

namespace NetCore.Web.Controllers
{
    public class MembershipController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken] // 위조방지토큰을 통해 view로부터 받은 post data가 유효한지 검증
        // post 액션 방식의 메서드에는 기본적으로 지정을 해줘야함
        public IActionResult Login(LoginInfo login)
        {
            string message = string.Empty;

            if (ModelState.IsValid) // 모델의 상태를 파악해서 값이 유효하게 들어온지 확인 
            {
                // 데이터베이스에서 가져왔다고 가정
                string userId = "jadejs";
                string password = "123456";

                if (login.UserId.Equals(userId) && login.Password.Equals(password))
                {
                    TempData["Message"] = "로그인이 성공적으로 이루어졌습니다.";
                    return RedirectToAction("Index", "Membership"); // 로그인 성공시 index라는 뷰와 membership이라는 컨트롤러로 이동
                }
                else
                {
                    message = "로그인되지 않았습니다.";
                }
            } 
            else
            {
                message = "로그인정보를 올바르게 입력하세요.";
            }
            // view로 리턴하면 로그인이 실패를 했다는 뜻이므로 모델 상태에다가 에러 모델을 하나 추가를 함
            ModelState.AddModelError(string.Empty, message);
            return View(login);
        }
    }
}
