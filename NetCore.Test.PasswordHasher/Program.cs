using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace NetCore.Test.PasswordHasher
{
    class Program
    {
        // 데이터베이스에서
        // Password 컬럼을 대신해서 GUIDSalt, RNGSalt, PasswordHash 값이 필요함
        static void Main(string[] args)
        {
            Console.Write("아이디를 입력하세요: ");
            string userId = Console.ReadLine();

            Console.Write("비밀번호를 입력하세요: ");
            string password = Console.ReadLine();

            // 계속 바뀌는 값 선언
            string guidSalt = Guid.NewGuid().ToString();

            string rngSalt = GetRNGSalt();

            string passwordHash = GetPasswordHash(userId, password, guidSalt, rngSalt);

            // 데이터베이스의 비밀번호정보와 지금 입력한 비밀번호정보를 비교해서 같은 해시값이 나오면 로그인이 되도록 처리
            bool check = CheckThePasswordInfo(userId, password, guidSalt, rngSalt, passwordHash); 

            Console.WriteLine($"UserId:{userId}");
            Console.WriteLine($"Password: {password}");
            Console.WriteLine($"GUIDSalt: {guidSalt}");
            Console.WriteLine($"RNGSalt: {rngSalt}");
            Console.WriteLine($"Hashed: {passwordHash}");
            Console.WriteLine($"check: {(check ? "비밀번호 정보가 일치":"불일치")}");

            //아 이 디 를  입 력 하 세 요 : jadejs
            //비 밀 번 호 를  입 력 하 세 요 : 123456
            //UserId: jadejs
            //Password: 123456
            //RNGSalt: bRzwMQjqyO9OByVHqaicmQ ==
            //Hashed: RMrb9lDKshUxDC20I8Rjo0GgsLFVhO / DQPAPmBb / 4KI =
        }

        private static string GetRNGSalt()
        {
            // generate a 128-bit salt using a cryptographically strong random sequence of nonzero values
            byte[] salt = new byte[128 / 8];
            using (var rngCsp = new RNGCryptoServiceProvider())
            {
                rngCsp.GetNonZeroBytes(salt);
            }

            return Convert.ToBase64String(salt);
        }

        private static string GetPasswordHash(string userId, string password, string guidSalt, string rngSalt)
        {
            // derive a 256-bit subkey (use HMACSHA256 with 100,000 iterations)
            // Pbkdf2
            // Password d-based key derivation function 2
            // 키를 통해서 어떠한 값을 파생시키는 것 (비밀번호 값) 
            return Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: userId + password + guidSalt,
                salt: Encoding.UTF8.GetBytes(rngSalt),
                prf: KeyDerivationPrf.HMACSHA512,
                iterationCount: 100000,
                numBytesRequested: 256 / 8));
        }

        private static bool CheckThePasswordInfo(string userId, string password, string guidSalt, string rngSalt, string passwordHash)
        {
            return GetPasswordHash(userId, password, guidSalt, rngSalt).Equals(passwordHash);
        }
    }
}

//아 이 디 를  입 력 하 세 요 : seokjs
//비 밀 번 호 를  입 력 하 세 요 : 123456
//UserId: seokjs
//Password: 123456
//GUIDSalt: e46535fc-4f26-41d8-859c-218369722ee9
//RNGSalt: NXpitoLQ5IKCF6q3wzIrdQ==
//Hashed: KGbQIhOTyoODUzk3aKkV9CGbrO9QEiigsAZq9ywzstI=
//check: 비 밀 번 호  정 보 가  일 치

