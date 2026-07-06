using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace NursingCarePlatform.Web.ViewModels.Nurse
{
    public class DocumentViewModel
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Document Type")]
        public string DocumentType { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Upload File")]
        public IFormFile File { get; set; } = null!;
    }
}