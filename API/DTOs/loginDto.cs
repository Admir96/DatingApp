using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    public class loginDto // DTOs (data transfer objets) primarno radi passworda, jer ne mozes bez DTO raditi sa hash pasword
    {
         [Required]
        public string username { get; set; }
         [Required]
        public string password { get; set; }   

    }
}