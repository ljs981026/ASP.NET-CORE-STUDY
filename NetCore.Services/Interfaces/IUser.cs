using NetCore.Data.Classes;
using NetCore.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetCore.Services.Interfaces
{
    public interface IUser
    {
        bool MatchTheUserInfo(LoginInfo login);
        User GetUserInfo(string userId);
        IEnumerable<UserRolesByUser> GetRolesOwnedByUser(string userId);

        /// <summary>
        /// 사용자 가입
        /// </summary>
        /// <param name="register">사용자 가입용 뷰 모델</param>
        /// <returns></returns>
        int RegisterUser(RegisterInfo register);
    }
}
