using System;
using System.Threading.Tasks;
using DatingApp2.API.Models;
using Microsoft.EntityFrameworkCore;

namespace DatingApp2.API.Data
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DataContext _context;
        public AuthRepository(DataContext context)
        {
            _context = context;

        }
        public async Task<User> Login(string username, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.UserName == username);

            if(user == null)
            {
                return null;
            }
            if(!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
            {
                return null;
            }
            return user;
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var computeHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for(int i = 0; i<computeHash.Length; i++)
                {
                    if (computeHash[i] != passwordHash[i]) {return false;}
                }
            }
            return true;
        }

        private void CreatePasswordHash(string password, out byte[] passswordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passswordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
            
        }

        public async Task<User> Register(User user, string password)
        {
            byte[] passswordHash, passwordSalt;
            CreatePasswordHash(password, out passswordHash, out passwordSalt);

            user.PasswordHash = passswordHash;
            user.PasswordSalt = passwordSalt;

            await  _context.Users.AddAsync(user);
            await  _context.SaveChangesAsync();

            return user;
        }

        public async Task<bool> UserExists(string username)
        {
            if(await _context.Users.AnyAsync(x => x.UserName == username)) {return true;}
            return false;
        }
    }
}