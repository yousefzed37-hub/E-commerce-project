using System.ComponentModel.DataAnnotations;

namespace E_commerce_project.DTOs
{
    public class DtoLogin
    {
        [Required]
        public string userName { get; set; } = string.Empty;
        [Required]
        public string password { get; set; } = string.Empty;
    }
}
