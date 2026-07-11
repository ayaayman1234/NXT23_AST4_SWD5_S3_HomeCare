using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NursingCarePlatform.Web.Models;

namespace NursingCarePlatform.Web.Data
{
    public class NursingDbContext : IdentityDbContext<ApplicationUser>
    {
        public NursingDbContext(DbContextOptions<NursingDbContext> options)
            : base(options)
        {
        }

        public DbSet<Patient> Patients { get; set; }

        public DbSet<Nurse> Nurses { get; set; }

        public DbSet<Admin> Admins { get; set; }

        public DbSet<NurseDocument> NurseDocuments { get; set; }

        public DbSet<Verification> Verifications { get; set; }

        public DbSet<NursingService> NursingServices { get; set; }
        public DbSet<ServiceCategory> ServiceCategories { get; set; }

        public DbSet<NurseService> NurseServices { get; set; }

        public DbSet<Availability> Availabilities { get; set; }

        public DbSet<CareRequest> CareRequests { get; set; }

        public DbSet<MedicalChecklist> MedicalChecklists { get; set; }

        public DbSet<MyOffer> Offers { get; set; }

        public DbSet<Assignment> Assignments { get; set; }

        public DbSet<RecurringRequest> RecurringRequests { get; set; }

        public DbSet<Payment> Payments { get; set; }

        public DbSet<NursingNote> NursingNotes { get; set; }

        public DbSet<WorkHistory> WorkHistories { get; set; }

        public DbSet<Rating> Ratings { get; set; }

        public DbSet<Complaint> Complaints { get; set; }

        public DbSet<SOSEvent> SOSEvents { get; set; }

        public DbSet<Notification> Notifications { get; set; }

        public DbSet<Cancellation> Cancellations { get; set; }

        // ==========================
        // GAP 1 – Subscription Plans (nurse commission reduction)
        // ==========================

        public DbSet<SubscriptionPlan> SubscriptionPlans { get; set; }

        public DbSet<NurseSubscription> NurseSubscriptions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ==========================
            // Composite Key
            // ==========================

            modelBuilder.Entity<NurseService>()
                .HasKey(ns => new { ns.NurseId, ns.ServiceId });

            // ==========================
            // Decimal Precision
            // ==========================

            modelBuilder.Entity<NurseService>()
                .Property(ns => ns.Price)
                .HasPrecision(18, 2);

            modelBuilder.Entity<CareRequest>()
                .Property(cr => cr.BudgetMin)
                .HasPrecision(18, 2);

            modelBuilder.Entity<CareRequest>()
                .Property(cr => cr.BudgetMax)
                .HasPrecision(18, 2);

            modelBuilder.Entity<MyOffer>()
                .Property(o => o.ProposedPrice)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Payment>()
                .HasOne(p => p.CareRequest)
                .WithOne(c => c.Payment)
                .HasForeignKey<Payment>(p => p.CareRequestId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Payment>()
                .Property(p => p.Amount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Payment>()
                .Property(p => p.CommissionAmount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Payment>()
                .Property(p => p.NetAmount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Cancellation>()
                .Property(c => c.Fee)
                .HasPrecision(18, 2);

            modelBuilder.Entity<NursingNote>()
                .Property(n => n.GlucoseLevel)
                .HasPrecision(18, 2);

            modelBuilder.Entity<NursingNote>()
                .Property(n => n.Temperature)
                .HasPrecision(18, 2);

            // ==========================
            // CareRequest Relations
            // ==========================

            modelBuilder.Entity<CareRequest>()
                .HasOne(c => c.Patient)
                .WithMany()
                .HasForeignKey(c => c.PatientId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CareRequest>()
                .HasOne(c => c.Service)
                .WithMany(s => s.CareRequests)
                .HasForeignKey(c => c.ServiceId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<CareRequest>()
                .HasOne(c => c.Nurse)
                .WithMany()
                .HasForeignKey(c => c.NurseId)
                .OnDelete(DeleteBehavior.NoAction);

            // ==========================
            // Avoid Multiple Cascade Paths
            // ==========================

            modelBuilder.Entity<Assignment>()
                .HasOne(a => a.Nurse)
                .WithMany()
                .HasForeignKey(a => a.NurseId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Assignment>()
                .HasOne(a => a.CareRequest)
                .WithMany()
                .HasForeignKey(a => a.CareRequestId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<MyOffer>()
                .HasOne(o => o.Nurse)
                .WithMany()
                .HasForeignKey(o => o.NurseId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<MyOffer>()
                .HasOne(o => o.CareRequest)
                .WithMany(c => c.Offers)
                .HasForeignKey(o => o.CareRequestId)
                .OnDelete(DeleteBehavior.NoAction);

            // ==========================
            // Identity Relations
            // ==========================

            modelBuilder.Entity<Patient>()
                .HasOne(p => p.User)
                .WithMany()
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Nurse>()
                .HasOne(n => n.User)
                .WithMany()
                .HasForeignKey(n => n.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Admin>()
                .HasOne(a => a.User)
                .WithMany()
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Verification>()
                .HasOne(v => v.Admin)
                .WithMany()
                .HasForeignKey(v => v.AdminId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Verification>()
                .HasOne(v => v.NurseDocument)
                .WithMany()
                .HasForeignKey(v => v.NurseDocumentId)
                .OnDelete(DeleteBehavior.NoAction);

            // ==========================
            // Complaint Relations
            // ==========================

            modelBuilder.Entity<Complaint>()
                .HasOne(c => c.Patient)
                .WithMany()
                .HasForeignKey(c => c.PatientId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Complaint>()
                .HasOne(c => c.Nurse)
                .WithMany()
                .HasForeignKey(c => c.NurseId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<ServiceCategory>()
                .HasMany(c => c.Services)
                .WithOne(s => s.Category)
                .HasForeignKey(s => s.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
            // ==========================
            // Work History Relations
            // ==========================

            modelBuilder.Entity<WorkHistory>()
                .Property(w => w.TotalAmount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<WorkHistory>()
                .HasOne(w => w.Nurse)
                .WithMany()
                .HasForeignKey(w => w.NurseId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<WorkHistory>()
                .HasOne(w => w.Patient)
                .WithMany()
                .HasForeignKey(w => w.PatientId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<WorkHistory>()
                .HasOne(w => w.CareRequest)
                .WithMany()
                .HasForeignKey(w => w.CareRequestId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<WorkHistory>()
                .HasOne(w => w.Service)
                .WithMany()
                .HasForeignKey(w => w.ServiceId)
                .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<MedicalChecklist>()
                .HasOne(m => m.CareRequest)
                .WithOne(c => c.MedicalChecklist)
                .HasForeignKey<MedicalChecklist>(m => m.CareRequestId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<NursingNote>()
                .HasOne(n => n.Assignment)
                .WithOne(a => a.NursingNote)
                .HasForeignKey<NursingNote>(n => n.AssignmentId)
                .OnDelete(DeleteBehavior.Cascade);
            // ==========================
            // SOS Relations
            // ==========================

            modelBuilder.Entity<SOSEvent>()
                .HasOne(s => s.CareRequest)
                .WithMany()
                .HasForeignKey(s => s.CareRequestId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<SOSEvent>()
                .HasOne(s => s.TriggeredByUser)
                .WithMany()
                .HasForeignKey(s => s.TriggeredByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            // ==========================
            // GAP 1 – Subscription Plans
            // ==========================

            modelBuilder.Entity<SubscriptionPlan>()
                .Property(sp => sp.MonthlyFee)
                .HasPrecision(18, 2);

            modelBuilder.Entity<SubscriptionPlan>()
                .Property(sp => sp.CommissionRate)
                .HasPrecision(5, 4);

            modelBuilder.Entity<NurseSubscription>()
                .HasOne(ns => ns.Nurse)
                .WithMany()
                .HasForeignKey(ns => ns.NurseId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<NurseSubscription>()
                .HasOne(ns => ns.Plan)
                .WithMany(p => p.NurseSubscriptions)
                .HasForeignKey(ns => ns.PlanId)
                .OnDelete(DeleteBehavior.Restrict);

            // ==========================
            // GAP 2 – WorkHistory AssignmentId
            // ==========================

            modelBuilder.Entity<WorkHistory>()
                .HasOne(w => w.Assignment)
                .WithMany()
                .HasForeignKey(w => w.AssignmentId)
                .OnDelete(DeleteBehavior.SetNull);

            // ==========================
            // GAP 4 – Rating ApplicationUser links
            // ==========================

            modelBuilder.Entity<Rating>()
                .HasOne(r => r.RaterUser)
                .WithMany()
                .HasForeignKey(r => r.RaterUserGuid)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired(false);

            modelBuilder.Entity<Rating>()
                .HasOne(r => r.RatedUser)
                .WithMany()
                .HasForeignKey(r => r.RatedUserGuid)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired(false);

            // ==========================
            // GAP 5 – Complaint generic user references
            // ==========================

            modelBuilder.Entity<Complaint>()
                .HasOne(c => c.CreatedByUser)
                .WithMany()
                .HasForeignKey(c => c.CreatedByUserId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired(false);

            modelBuilder.Entity<Complaint>()
                .HasOne(c => c.AgainstUser)
                .WithMany()
                .HasForeignKey(c => c.AgainstUserId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired(false);

            modelBuilder.Entity<Complaint>()
                .HasOne(c => c.ComplaintCareRequest)
                .WithMany()
                .HasForeignKey(c => c.ComplaintCareRequestId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired(false);

        }

    }
}