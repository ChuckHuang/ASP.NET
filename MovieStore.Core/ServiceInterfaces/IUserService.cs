using MovieStore.Core.Models.Request;
using MovieStore.Core.Models.Response;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MovieStore.Core.ServiceInterfaces
{
    public interface IUserService
    {
        Task<UserRegisterReposnseModel> RegisterUser(UserRegisterRequestModel requestModel);

        Task<UserLoginReponseModel> ValidateUser(string email, string password);
    }
}
