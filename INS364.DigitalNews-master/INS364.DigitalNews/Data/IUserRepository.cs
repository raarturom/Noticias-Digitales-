using INS364.DigitalNews.Models;
using INS364.DigitalNews.ViewModels.Auth;
using System.Threading.Tasks;

namespace INS364.DigitalNews.Data
{
    public interface IUserRepository
    {
        Task<UserModel> GetUserInfo(LoginViewModel loginViewModel);

        Task<bool> RegisterUserAsync(SignupViewModel signupViewModel);

        Task<bool> GetAnyUserDuplicates(string username);
    }
}