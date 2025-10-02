using NetCore.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetCore.Services.Interfaces
{
    public interface IUser
    {
        bool MatchTheUserInfo(LoginInfo login);
    }
}
