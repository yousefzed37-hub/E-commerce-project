using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace E_commerce_project.DTOs
{
    public class DtoNewUser
    {
        [Required]
        public string userName { get; set; } = string.Empty;
        [Required]
        public string email { get; set; } = string.Empty;
        [Required]
        public string password { get; set; } = string.Empty;
        [Phone]
        public string phoneNumber { get; set; } = string.Empty;
    }
}
