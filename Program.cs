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
        public static async Task Main(string[] args)
        {
            // Set default culture to Egyptian Pound (EGP) formatting instead of USD ($)
            var egyptCulture = new System.Globalization.CultureInfo("en-EG");
            System.Globalization.CultureInfo.DefaultThreadCurrentCulture = egyptCulture;
            System.Globalization.CultureInfo.DefaultThreadCurrentUICulture = egyptCulture;

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
            builder.Services.AddScoped<ICancellationService, CancellationService>();
            builder.Services.AddScoped<INotificationService, NotificationService>();
            builder.Services.AddScoped<IOfferService, OfferService>();
            builder.Services.AddScoped<IAssignmentService, AssignmentService>();
            builder.Services.AddScoped<IWorkHistoryService, WorkHistoryService>();
            builder.Services.AddScoped<INursingNoteService, NursingNoteService>();
            builder.Services.AddScoped<IMedicalChecklistService, MedicalChecklistService>();
            builder.Services.AddScoped<IPaymentService, PaymentService>();
            builder.Services.AddScoped<IRatingService, RatingService>();
            builder.Services.AddScoped<IRecurringRequestService, RecurringRequestService>();
            builder.Services.AddScoped<ISOSService, SOSService>();
            builder.Services.AddScoped<ISubscriptionService, SubscriptionService>();

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
            // ==========================================
            // Seed Admin Account
            // ==========================================

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
                var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                var db = services.GetRequiredService<NursingDbContext>();

                // Create Roles
                string[] roles =
                {
        "Admin",
        "Patient",
        "Nurse"
    };

                foreach (var role in roles)
                {
                    if (!await roleManager.RoleExistsAsync(role))
                        await roleManager.CreateAsync(new IdentityRole(role));
                }

                // Create Admin User

                string adminEmail = "admin@nursing.com";
                string adminPassword = "Admin123!";

                var adminUser = await userManager.FindByEmailAsync(adminEmail);

                if (adminUser == null)
                {
                    adminUser = new ApplicationUser
                    {
                        UserName = adminEmail,
                        Email = adminEmail,
                        FirstName = "System",
                        LastName = "Admin",
                        EmailConfirmed = true
                    };

                    var result = await userManager.CreateAsync(adminUser, adminPassword);

                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(adminUser, "Admin");

                        db.Admins.Add(new Admin
                        {
                            UserId = adminUser.Id
                        });

                        await db.SaveChangesAsync();
                    }
                }
            }

            app.Run();
        }
    }
}