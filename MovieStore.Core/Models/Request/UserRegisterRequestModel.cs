using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MovieStore.Core.Models.Request
{
    public class UserRegisterRequestModel
    {   //Data Annoation is using for validation in ASP.NET//FrameWork
        [Required]
        [EmailAddress]
        [StringLength(50)]
        public string Email { get; set; }
        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }
        [Required]
        [StringLength(50)]
        public string LastName { get; set; }
        [Required]
        [StringLength(20,ErrorMessage ="Make sure password length is between 8 and 20", MinimumLength =8)]
        //Password should be strong, at least one Upper letter and lower letter; a number; a symbol
        [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[#$^+=!*()@%&]).{8,}$", ErrorMessage = "Password Should have minimum 8 with at least one upper, lower, number and special character")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
