using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NetCore.Data.ViewModels;
using NetCore.Web.Extensions;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NetCore.Web.Controllers
{
    [Route("Data")]
    public class DataController : Controller
    {
        private IDataProtector _protection;
        private HttpContext _context;
        private string _sessionKeyCartName = "_sessionCartKey";

        public DataController(IDataProtectionProvider provider, IHttpContextAccessor accessor)
        {
            _protection = provider.CreateProtector("NetCore.data.v1");
            _context = accessor.HttpContext;
        }

        #region AES
        // GET: /<controller>/
        [HttpGet("AES")]
        [Authorize(Roles = "GeneralUser,SuperUser,SystemUser")]
        public IActionResult AES()
        {
            return View();
        }

        [HttpPost("AES")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "GeneralUser,SuperUser,SystemUser")]
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
        #endregion

        #region private methods
        private List<ItemInfo> GetCartInfos(ref string message)
        {
            var cartInfos = _context.Session.Get<List<ItemInfo>>(key: _sessionKeyCartName);

            if (cartInfos == null || cartInfos.Count() < 1)
            {
                message = "장바구니에 담긴 상품이 없습니다.";
            }

            return cartInfos;
        }

        private void SetCartInfos(ItemInfo item, List<ItemInfo> cartInfos = null)
        {
            if (cartInfos == null)
            {
                cartInfos = _context.Session.Get<List<ItemInfo>>(_sessionKeyCartName);

                if (cartInfos == null)
                {
                    cartInfos = new List<ItemInfo>();
                }
            }

            cartInfos.Add(item);

            _context.Session.Set<List<ItemInfo>>(_sessionKeyCartName, cartInfos);
        }
        #endregion

        [HttpPost("AddCart")]
        [ValidateAntiForgeryToken]
        public IActionResult AddCart()
        {
            SetCartInfos(new ItemInfo() { ItemNo = Guid.NewGuid(), ItemName = DateTime.UtcNow.Ticks.ToString() });

            return RedirectToAction("Cart", "Data");
        }

        [HttpPost("RemoveCart")]
        [ValidateAntiForgeryToken]
        public IActionResult RemoveCart()
        {
            string message = string.Empty;

            var cartInfos = GetCartInfos(ref message);

            if (cartInfos != null && cartInfos.Count() > 0)
            {
                _context.Session.Remove(key: _sessionKeyCartName);
            }

            return RedirectToAction("Cart", "Data");
        }

        [HttpGet("Cart")]
        public IActionResult Cart()
        {
            string message = string.Empty;

            var cartInfos = GetCartInfos(ref message);

            ViewData["Message"] = message;

            return View(cartInfos);
        }
    }
}

