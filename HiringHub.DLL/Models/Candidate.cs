using System.ComponentModel.DataAnnotations;

namespace HiringHub.DLL.Models
{
    public class Candidate
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(30)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(30)]
        public string LastName { get; set; }

        [RegularExpression(@"^\+?[1-9]\d{1,14}$", ErrorMessage = "Invalid phone number format.")]
        public string PhoneNumber { get; set; }

        [Required]
        [StringLength(50)]
       // [RegularExpression(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$", ErrorMessage = "Invalid email format.")]
        [EmailAddress]
        public string Email { get; set; }
        public string CallTimeInterval { get; set; }

        [StringLength(50)]
        [RegularExpression(@"^(https:\/\/(www\.)?linkedin\.com\/(in|company)\/[A-Za-z0-9_-]+\/?)$", ErrorMessage = "Invalid LinkedIn URL format.")]
        public string LinkedInProfileUrl { get; set; }


        [StringLength(50)]
        [RegularExpression(@"^(https:\/\/github\.com\/[A-Za-z0-9_-]+\/?([A-Za-z0-9_-]+\/?)?)$", ErrorMessage = "Invalid GitHub URL format.")]
        public string GitHubProfileUrl { get; set; }

        [Required]
        public string Comment { get; set; }
    }
}
