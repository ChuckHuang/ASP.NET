using System;
using System.Collections.Generic;
using System.Text;

namespace MovieStore.Core.Models.Response
{
    public class UserLoginReponseModel
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public IList<string> Roles { get; set; }
    }
}
