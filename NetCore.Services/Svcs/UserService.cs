using NetCore.Data.DataModels;
using NetCore.Data.ViewModels;
using NetCore.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetCore.Services.Svcs
{
    // interface를 상속받은 후에 명시적으로 interface 구현
    public class UserService : IUser
    {
        #region private methods
        // 내부에서만 사용
        private IEnumerable<User> GetUserInfos()
        {
            // 데이터 모델: 데이터베이스와 연동해서 작업하기 위한 모델
            // 뷰 모델: 뷰를 위한 모델
            return new List<User>()
            {
                new User()
                {
                    UserId = "seokjs",
                    UserName = "이재석",
                    UserEmail = "ljs981026@naver.com",
                    Password = "123456"
                }
            };
        }

        private bool CheckTheUserInfo(string userId, string password)
        {
            // any : 리스트 데이터 유무체크
            return GetUserInfos().Where(u => u.UserId.Equals(userId) && u.Password.Equals(password)).Any();
        }
        #endregion

        bool IUser.MatchTheUserInfo(LoginInfo login)
        {
            return CheckTheUserInfo(login.UserId, login.Password);            
        }
    }
}
