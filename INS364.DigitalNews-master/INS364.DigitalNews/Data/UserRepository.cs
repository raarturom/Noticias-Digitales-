using INS364.DigitalNews.Context;
using INS364.DigitalNews.Models;
using INS364.DigitalNews.ViewModels.Auth;
using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace INS364.DigitalNews.Data
{
    public class UserRepository : IUserRepository
    {
        private NewsDbContext _context { get; }

        public UserRepository(NewsDbContext context)
        {
            _context = context;
        }

        public async Task<UserModel> GetUserInfo(LoginViewModel loginViewModel)
        {
            UserModel user = await _context.Users
                .Where(entity => entity.Username == loginViewModel.Username &&
                                 entity.Password == loginViewModel.Password)
                .FirstOrDefaultAsync();

            return user;
        }

        public async Task<bool> RegisterUserAsync(SignupViewModel signupViewModel)
        {
            bool success = false;

            try
            {
                UserModel user = new UserModel()
                {
                    Username = signupViewModel.Username,
                    Password = signupViewModel.Password,
                    Email = signupViewModel.Email,
                    Firstname = signupViewModel.Firstname,
                    Lastname = signupViewModel.Lastname,

                };

                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();
                success = true;
            }

            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }

            return success;
        }

        public async Task<bool> GetAnyUserDuplicates(string username)
        {
            UserModel user = await _context.Users
                .Where(entity => entity.Username == username)
                .FirstOrDefaultAsync();
            
            return user == null;
        }
    }
}