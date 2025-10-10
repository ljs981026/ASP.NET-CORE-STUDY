//using NetCore.Data.DataModels;
using Microsoft.EntityFrameworkCore;
using NetCore.Data.Classes;
//using NetCore.Data.DataModels;
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
        private IPasswordHasher _hasher;

        public UserService(DBFirstDbContext context, IPasswordHasher hasher)
        {
            _context = context;
            _hasher = hasher;

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
             user = _context.Users.Where(u => u.UserId.Equals(userId)).FirstOrDefault();

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

            if (user == null)
            {
                // 접속실패횟수에 대한 증가
                int rowAffected;

                // Sql문 직접 작성
                // rowAffected = _context.Database.ExecuteSqlInterpolated($"Update dbo.[User] SET AccessFailedCount += 1 Where UserId={userId}");

                // sp로 처리
                rowAffected = _context.Database.ExecuteSqlInterpolated($"exec dbo.FailedLoginByUserId {userId}");
            }

            return user;
        }

        private bool CheckTheUserInfo(string userId, string password)
        {
            // any : 리스트 데이터 유무체크
            //return GetUserInfos().Where(u => u.UserId.Equals(userId) && u.Password.Equals(password)).Any();
            return GetUserInfo(userId, password) != null;
        }

        private User GetUserInfo(string userId)
        {
            return _context.Users.Where(u => u.UserId.Equals(userId)).FirstOrDefault();
        }

        private IEnumerable<UserRolesByUser> GetUserRolesByUserInfos(string userId)
        {
            var userRolesByUserInfos = _context.UserRolesByUsers.Where(uru => uru.UserId.Equals(userId)).ToList();

            foreach(var role in userRolesByUserInfos)
            {
                role.UserRole = GetUserRole(role.RoleId);
            }

            return userRolesByUserInfos.OrderByDescending(uru => uru.UserRole.RolePriority);
        }

        private UserRole GetUserRole(string roleId)
        {
            return _context.UserRoles.Where(ur => ur.RoleId.Equals(roleId)).FirstOrDefault();
        }

        private int RegisterUser(RegisterInfo register)
        {
            var passwordInfo = _hasher.SetPasswordInfo(register.UserId, register.Password);
            var utcNow = DateTime.UtcNow;

            var user = new User()
            {
                UserId = register.UserId.ToLower(),
                UserName = register.UserName,
                UserEmail = register.UserEmail,
                GUIDSalt = passwordInfo.GUIDSalt,
                RNGSalt = passwordInfo.RNGSalt,
                PasswordHash = passwordInfo.PasswordHash,
                AccessFailedCount = 0,
                IsMembershipWithdrawn = false,
                JoinedUtcDate = utcNow,
            };

            var userRolesByUser = new UserRolesByUser()
            {
                UserId = register.UserId.ToLower(),
                RoleId = "AssociateUser",
                OwnedUtcDate = utcNow,
            };

            _context.Add(user);
            _context.Add(userRolesByUser);

            return _context.SaveChanges();
        }

        private UserInfo GetUserInfoForUpdate(string userId)
        {
            var user = GetUserInfo(userId);
            var userInfo = new UserInfo()
            {
                UserId = null,
                UserName = user.UserName,
                UserEmail = user.UserEmail,
                ChangeInfo = new ChangeInfo()
                {
                    UserName = user.UserName,
                    UserEmail = user.UserEmail,                    
                }
            };

            return userInfo;
        }

        private int UpdateUser(UserInfo user)
        {
            var userInfo = _context.Users.Where(u => u.UserId.Equals(user.UserId)).FirstOrDefault();

            if (userInfo == null)
            {
                return 0;
            }

            int rowAffected = 0;
           
            bool check = _hasher.CheckThePasswordInfo(user.UserId, user.Password, userInfo.GUIDSalt, userInfo.RNGSalt, userInfo.PasswordHash);

            if (check)
            {
                _context.Update(userInfo);

                userInfo.UserName = user.UserName;
                userInfo.UserEmail = user.UserEmail;

                rowAffected = _context.SaveChanges();
            }
            return rowAffected;
        }

        private bool MatchTheUserInfo(LoginInfo login)
        {
            //return CheckTheUserInfo(login.UserId, login.Password);            
            var user = _context.Users.Where(u => u.UserId.Equals(login.UserId)).FirstOrDefault();

            if (user == null)
            {
                return false;
            }

            return _hasher.CheckThePasswordInfo(login.UserId, login.Password, user.GUIDSalt, user.RNGSalt, user.PasswordHash);
        }

        private bool CompareInfo(UserInfo user)
        {
            return user.ChangeInfo.Equals(user);
        }

        #endregion

        bool IUser.MatchTheUserInfo(LoginInfo login)
        {
            return MatchTheUserInfo(login);
        }

        User IUser.GetUserInfo(string userId)
        {
            return GetUserInfo(userId);
        }

        IEnumerable<UserRolesByUser> IUser.GetRolesOwnedByUser(string userId)
        {
            return GetUserRolesByUserInfos(userId);
        }

        int IUser.RegisterUser(RegisterInfo register)
        {
            return RegisterUser(register);
        }

        UserInfo IUser.GetUserInfoForUpdate(string userId)
        {
            return GetUserInfoForUpdate(userId);
        }

        int IUser.UpdateUser(UserInfo user)
        {
            return UpdateUser(user);
        }

        bool IUser.CompareInfo(UserInfo user)
        {
            return CompareInfo(user);
        }
    }
}
