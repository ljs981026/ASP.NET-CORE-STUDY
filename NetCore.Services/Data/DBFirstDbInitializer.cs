using NetCore.Data.Classes;
using NetCore.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCore.Services.Data
{
    public class DBFirstDbInitializer
    {
        private DBFirstDbContext _context;
        private IPasswordHasher _hasher;

        public DBFirstDbInitializer(DBFirstDbContext context, IPasswordHasher hasher) 
        {
            _context = context;
            _hasher = hasher; 
        }

        /// <summary>
        /// 초기 데이터를 심는다
        /// </summary>
        public int PlantSeedData()
        {
            int rowAffected = 0;
            string userId = "seokjs";
            string password = "123456";
            var passwordInfo = _hasher.SetPasswordInfo(userId, password);
            var nowUtc = DateTime.UtcNow;

            _context.Database.EnsureCreated();

            if (!_context.Users.Any())
            {
                var users = new List<User>()
                {
                    new User()
                    {
                        UserId = userId.ToLower(),
                        UserName = "Seed 사용자",
                        UserEmail = "seokjs@gmail.com",
                        GUIDSalt = passwordInfo.GUIDSalt,
                        RNGSalt = passwordInfo.RNGSalt,
                        PasswordHash = passwordInfo.PasswordHash,
                        AccessFailedCount = 0,
                        IsMembershipWithdrawn = false,
                        JoinedUtcDate = nowUtc,
                    }
                };

                _context.Users.AddRange(users);
                rowAffected += _context.SaveChanges();
            }

            if (!_context.UserRoles.Any())
            {
                var userRoles = new List<UserRole>()
                {
                    new UserRole()
                    {
                        RoleId = "AssociateUser",
                        RoleName = "준사용자",
                        RolePriority = 1,
                        ModifiedUtcDate = nowUtc
                    },
                    new UserRole()
                    {
                        RoleId = "GeneralUser",
                        RoleName = "일반사용자",
                        RolePriority = 2,
                        ModifiedUtcDate = nowUtc
                    },
                    new UserRole()
                    {
                        RoleId = "SuperUser",
                        RoleName = "향상된 사용자",
                        RolePriority = 3,
                        ModifiedUtcDate = nowUtc
                    },
                    new UserRole()
                    {
                        RoleId = "SystemUser",
                        RoleName = "시스템 사용자",
                        RolePriority = 4,
                        ModifiedUtcDate = nowUtc
                    }
                };

                _context.UserRoles.AddRange(userRoles);
                rowAffected += _context.SaveChanges();
            }

            if (!_context.UserRolesByUsers.Any())
            {
                var usersByUsers = new List<UserRolesByUser>()
                {
                    new UserRolesByUser()
                    {
                        UserId = userId.ToLower(),
                        RoleId = "GeneralUser",
                        OwnedUtcDate = nowUtc
                    },
                    new UserRolesByUser()
                    {
                        UserId = userId.ToLower(),
                        RoleId = "SuperUser",
                        OwnedUtcDate = nowUtc
                    },
                    new UserRolesByUser()
                    {
                        UserId = userId.ToLower(),
                        RoleId = "SystemUser",
                        OwnedUtcDate = nowUtc
                    }
                };

                _context.UserRolesByUsers.AddRange(usersByUsers);
                rowAffected += _context.SaveChanges();
            }
            return rowAffected;
        }
    }
}
