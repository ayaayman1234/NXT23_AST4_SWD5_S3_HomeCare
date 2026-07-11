using System.ComponentModel.DataAnnotations;

namespace NursingCarePlatform.Web.ViewModels.Admin
{
    // ==========================
    // List ViewModel
    // ==========================

    public class AdminSubscriptionPlanViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; } = "";

        public string Description { get; set; } = "";

        public decimal MonthlyFee { get; set; }

        /// <summary>Commission rate as percentage string, e.g. "5%"</summary>
        public string CommissionRateDisplay => $"{CommissionRate * 100:0.##}%";

        public decimal CommissionRate { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedAt { get; set; }

        public int ActiveSubscribersCount { get; set; }
    }

    // ==========================
    // Create / Edit ViewModel
    // ==========================

    public class CreateSubscriptionPlanViewModel
    {
        [Required]
        [StringLength(100)]
        [Display(Name = "Plan Name")]
        public string Name { get; set; } = "";

        [StringLength(500)]
        [Display(Name = "Description")]
        public string Description { get; set; } = "";

        [Required]
        [Range(0, 100000)]
        [Display(Name = "Monthly Fee (EGP)")]
        public decimal MonthlyFee { get; set; }

        [Required]
        [Range(0.0, 1.0)]
        [Display(Name = "Commission Rate (0.0 – 1.0, e.g. 0.05 = 5%)")]
        public decimal CommissionRate { get; set; }

        public bool IsActive { get; set; } = true;
    }

    // ==========================
    // Nurse Subscription ViewModel
    // ==========================

    public class AdminNurseSubscriptionViewModel
    {
        public int Id { get; set; }

        public string NurseName { get; set; } = "";

        public int NurseId { get; set; }

        public string PlanName { get; set; } = "";

        public decimal MonthlyFee { get; set; }

        public decimal CommissionRate { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string Status { get; set; } = "";

        public bool IsCurrentlyActive => Status == "Active" && EndDate >= DateTime.Now;
    }
}
