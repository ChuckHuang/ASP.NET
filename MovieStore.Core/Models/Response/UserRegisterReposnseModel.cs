using System;
using System.Collections.Generic;
using System.Text;

namespace MovieStore.Core.Models.Request
{   //ViewModels, we are creating these model classes just for the Views/Client
    //It will have only properties that are required by the view, no more or no less
    //ViewModels are also useful when you want to combine multiple models
    //like different properties from different classes
    //they are called ViewModels in MVC
    //DTO(Data transfer objects) in API's
    public class UserRegisterReposnseModel
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
