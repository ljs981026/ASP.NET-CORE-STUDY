//using NetCore.Data.DataModels;
using Microsoft.EntityFrameworkCore;
using NetCore.Data.Classes;
using NetCore.Data.ViewModels;
using NetCore.Services.Data;
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
        private DBFirstDbContext _context;

        public UserService(DBFirstDbContext context)
        {
            _context = context;
        }

        #region private methods
        // 내부에서만 사용
        private IEnumerable<User> GetUserInfos()
        {
            return _context.Users.ToList();
            // 데이터 모델: 데이터베이스와 연동해서 작업하기 위한 모델
            // 뷰 모델: 뷰를 위한 모델
            //return new List<User>()
            //{
            //    new User()
            //    {
            //        UserId = "seokjs",
            //        UserName = "이재석",
            //        UserEmail = "ljs981026@naver.com",
            //        Password = "123456"
            //    }
            //};
        }

        private User GetUserInfo(string userId, string password)
        {
            User user;

            // lambda
             user = _context.Users.Where(u => u.UserId.Equals(userId) && u.Password.Equals(password)).FirstOrDefault();

            // FromSqlRaw, FromSqlInterpolated
            // Table
            //user = _context.Users.FromSqlRaw("SELECT UserId, UserName, UserEmail, Password, IsMembershipWithdrawn, JoinedUtcDate FROM dbo.[User]")
            //        .Where(u => u.UserId.Equals(userId) && u.Password.Equals(password))
            //        .FirstOrDefault();

            // View
            //user = _context.Users.FromSqlRaw("SELECT UserId, UserName, UserEmail, Password, IsMembershipWithdrawn, JoinedUtcDate FROM dbo.uvwUser")
            //        .Where(u => u.UserId.Equals(userId) && u.Password.Equals(password))
            //        .FirstOrDefault();

            // Function
            //user = _context.Users.FromSqlInterpolated($"SELECT UserId, UserName, UserEmail, Password, IsMembershipWithdrawn, JoinedUtcDate FROM dbo.ufnUser({userId}, {password})")
            //        .FirstOrDefault();

            // Stored Procedure
            //user = _context.Users.FromSqlInterpolated($"exec dbo.uspCheckLoginByUserId {userId}, {password}")
            //    .AsEnumerable()
            //    .FirstOrDefault();
             
            return user;
        }

        private bool CheckTheUserInfo(string userId, string password)
        {
            // any : 리스트 데이터 유무체크
            //return GetUserInfos().Where(u => u.UserId.Equals(userId) && u.Password.Equals(password)).Any();
            return GetUserInfo(userId, password) != null;
        }
        #endregion

        bool IUser.MatchTheUserInfo(LoginInfo login)
        {
            return CheckTheUserInfo(login.UserId, login.Password);            
        }
    }
}
