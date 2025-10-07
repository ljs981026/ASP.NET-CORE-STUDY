using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using NetCore.Data.ViewModels;
using NetCore.Services.Interfaces;
using NetCore.Services.Svcs;
using NetCore.Web.Models;

namespace NetCore.Web.Controllers
{
    [Route("Membership")]
    [Authorize(Roles = "AssociateUser,GeneralUser,SuperUser,SystemUser")]
    public class MembershipController : Controller
    {
        // 전역변수로 인터페이스를 설정
        // 이것은 의존성 주입방식이 아니라 이렇게 사용은 안함
        // private IUser _user = new UserService();
        // 의존성 주입 방식으로 전환 - 생성자 주입
        private IUser _user;
        private HttpContext _context;

        public MembershipController(IHttpContextAccessor accessor, IUser user)
        {
            _context = accessor.HttpContext;
            _user = user;
        }

        #region private methods
        /// <summary>
        /// 로컬 url인지 외부url인지 체크
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(MembershipController.Index), "Membership");
            }
        }
        #endregion

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("Login")]
        [AllowAnonymous] // 모든 사람의 접근을 허용해야함
        public IActionResult Login(string returnUrl)
        {
            ViewData["ReturnUrl"] = returnUrl;

            return View();
        }
        
        [HttpPost("Login")]
        [ValidateAntiForgeryToken] // 위조방지토큰을 통해 view로부터 받은 post data가 유효한지 검증
        [AllowAnonymous]
        // 데이터 프로젝트 => 서비스 프로젝트 => 웹 프로젝트
        // 따라서 웹프로젝트는 데이터 프로젝트까지 참조 가능
        // post 액션 방식의 메서드에는 기본적으로 지정을 해줘야함
        public async Task<IActionResult> LoginAsync(LoginInfo login, string returnUrl)
        {
            ViewData["ReturnUrl"] = returnUrl;

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
                    // 시원보증과 승인권한
                    var userInfo = _user.GetUserInfo(login.UserId);
                    var roles = _user.GetRolesOwnedByUser(login.UserId);
                    var userTopRole = roles.FirstOrDefault();
                    string userDataInfo = userTopRole.UserRole.RoleName + "|" +
                        userTopRole.UserRole.RolePriority.ToString() + "|" +
                        userInfo.UserName + "|" +
                        userInfo.UserEmail;

                    //_context.User.Identity.Name => 사용자 아이디

                    var identity = new ClaimsIdentity(claims: new[] {
                        new Claim(type: ClaimTypes.Name, value: userInfo.UserId),
                        new Claim(type: ClaimTypes.Role,
                        value: userTopRole.RoleId //+ "|" + userTopRole.UserRole.RoleName + "|" + userTopRole.UserRole.RolePriority.ToString()
                        ),
                        // 사용자가 활용하기 위해 필요한 데이터를 담아놓은 공간
                        new Claim(type: ClaimTypes.UserData,
                                  value: userDataInfo// userTopRole.UserRole.RoleName + "|" + userTopRole.UserRole.RolePriority.ToString()
                        )
                    }, authenticationType: CookieAuthenticationDefaults.AuthenticationScheme);

                    await _context.SignInAsync(
                        scheme: CookieAuthenticationDefaults.AuthenticationScheme,
                        principal:new ClaimsPrincipal(identity: identity),
                        properties:new AuthenticationProperties()
                        {
                            // 지속여부
                            IsPersistent = login.RememberMe,
                            ExpiresUtc = login.RememberMe ? DateTime.UtcNow.AddDays(7) : DateTime.UtcNow.AddMinutes(30)
                        });

                    TempData["Message"] = "로그인이 성공적으로 이루어졌습니다.";
                    //return RedirectToAction("Index", "Membership"); // 로그인 성공시 index라는 뷰와 membership이라는 컨트롤러로 이동
                    return RedirectToLocal(returnUrl);
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
            return View("Login", login);
        }

        [HttpGet("LogOut")]
        public async Task<IActionResult> LogOutAsync()
        {
            await _context.SignOutAsync(scheme: CookieAuthenticationDefaults.AuthenticationScheme);

            TempData["Message"] = "로그아웃이 성공적으로 이루어졌습니다. <br />웹사이트를 원활히 이용하시려면 로그인하세요.";

            return RedirectToAction("Index", "Membership");
        }

        [HttpGet("Forbidden")]
        public IActionResult Forbidden()
        {
            StringValues paramReturnUrl;
            bool exists = _context.Request.Query.TryGetValue("returnUrl", out paramReturnUrl);
            // 온전한 url형태
            paramReturnUrl = exists ? _context.Request.Host.Value + paramReturnUrl[0] : string.Empty;

            ViewData["Message"] = $"귀하는 {paramReturnUrl} 경로로 접근하려고 했습니다만,<br />" +
                    "인증된 사용자도 접근하지 못하는 페이지가 있습니다.<br />" +
                    "담당자에게 해당페이지의 접근권한에 대해 문의하세요.";

            return View();
        }
    }
}
