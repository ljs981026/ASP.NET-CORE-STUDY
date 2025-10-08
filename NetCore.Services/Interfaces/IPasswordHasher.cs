using System;
namespace NetCore.Services.Interfaces
{
	public interface IPasswordHasher
	{
        string GetGUIDSalt();

        string GetRNGSalt();

        string GetPasswordHash(string userId, string password, string guidSalt, string rngSalt);

        bool MatchTheUserInfo(string userId, string password);
    }
}

