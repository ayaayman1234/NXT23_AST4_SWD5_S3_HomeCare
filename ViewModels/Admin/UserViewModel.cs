using System;

namespace NursingCarePlatform.Web.ViewModels.Admin
{
    public class UserViewModel
    {
        public string Id { get; set; } = string.Empty;

        public string FullName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Role { get; set; } = string.Empty;

        public string PhoneNumber { get; set; } = string.Empty;

        public string AccountStatus { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }
    }
}