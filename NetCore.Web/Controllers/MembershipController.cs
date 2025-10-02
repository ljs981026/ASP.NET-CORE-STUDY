using Microsoft.AspNetCore.Mvc;
using NetCore.Data.ViewModels;
using NetCore.Services.Interfaces;
using NetCore.Services.Svcs;
using NetCore.Web.Models;

namespace NetCore.Web.Controllers
{
    public class MembershipController : Controller
    {
        // 전역변수로 인터페이스를 설정
        // 이것은 의존성 주입방식이 아니라 이렇게 사용은 안함
        // private IUser _user = new UserService();
        // 의존성 주입 방식으로 전환 - 생성자 주입
        private IUser _user;

        public MembershipController(IUser user)
        {
            _user = user;
        }

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
        // 데이터 프로젝트 => 서비스 프로젝트 => 웹 프로젝트
        // 따라서 웹프로젝트는 데이터 프로젝트까지 참조 가능
        // post 액션 방식의 메서드에는 기본적으로 지정을 해줘야함
        public IActionResult Login(LoginInfo login)
        {
            string message = string.Empty;

            if (ModelState.IsValid) // 모델의 상태를 파악해서 값이 유효하게 들어온지 확인 
            {
                // 데이터베이스에서 가져왔다고 가정
                //string userId = "seokjs";
                //string password = "123456";
                
                // 뷰모델이 데이터 프로젝트로 이동
                // 서비스 개념 => 서비스 프로젝트 구성
                // 프로젝트 분리를하여 서비스를 재사용화, 모듈화로 효율적 관리
                //if (login.UserId.Equals(userId) && login.Password.Equals(password))
                if (_user.MatchTheUserInfo(login))
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
