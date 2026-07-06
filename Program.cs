using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NursingCarePlatform.Web.Data;
using NursingCarePlatform.Web.Models;
using NursingCarePlatform.Web.Services.Implementations;
using NursingCarePlatform.Web.Services.Interfaces;

namespace NursingCarePlatform.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // ==============================
            // Database
            // ==============================

            var connectionString = builder.Configuration
                .GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

            builder.Services.AddDbContext<NursingDbContext>(options =>
                options.UseSqlServer(connectionString));

            // ==============================
            // Identity
            // ==============================

            builder.Services
                .AddDefaultIdentity<ApplicationUser>(options =>
                {
                    options.SignIn.RequireConfirmedAccount = false;

                    options.Password.RequireDigit = true;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequiredLength = 6;
                })
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<NursingDbContext>();

            // ==============================
            // MVC
            // ==============================

            builder.Services.AddControllersWithViews();
            builder.Services.AddRazorPages();

            // ==============================
            // Dependency Injection
            // ==============================

            builder.Services.AddScoped<IAccountService, AccountService>();
            builder.Services.AddScoped<IAdminService, AdminService>();
            builder.Services.AddScoped<IPatientService, PatientService>();
            builder.Services.AddScoped<INurseService, NurseManagementService>();
            builder.Services.AddScoped<IComplaintService, ComplaintService>();
            builder.Services.AddScoped<INotificationService, NotificationService>();
            builder.Services.AddScoped<IOfferService, OfferService>();
            builder.Services.AddScoped<IAssignmentService, AssignmentService>();
            builder.Services.AddScoped<IWorkHistoryService, WorkHistoryService>();
            builder.Services.AddScoped<INursingNoteService, NursingNoteService>();
            builder.Services.AddScoped<IMedicalChecklistService, MedicalChecklistService>();

            // ==============================
            // Database Error Page
            // ==============================

            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            var app = builder.Build();

            // ==============================
            // Configure Pipeline
            // ==============================

            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.MapRazorPages();

            app.Run();
        }
    }
}