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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Composite Key
            modelBuilder.Entity<NurseService>()
                .HasKey(ns => new { ns.NurseId, ns.ServiceId });

            // Decimal Precision
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
                .Property(p => p.Amount)
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

            // Avoid Multiple Cascade Paths
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
                .WithMany()
                .HasForeignKey(o => o.CareRequestId)
                .OnDelete(DeleteBehavior.NoAction);

            // Identity Relations
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
        }
    }
}