using Microsoft.AspNetCore.Identity;
using NursingCarePlatform.Web.Data;
using NursingCarePlatform.Web.Models;
using NursingCarePlatform.Web.Models.Responses;
using NursingCarePlatform.Web.Services.Interfaces;
using NursingCarePlatform.Web.ViewModels.Account;

namespace NursingCarePlatform.Web.Services.Implementations
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly NursingDbContext _context;

        public AccountService(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            NursingDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _context = context;
        }

        public async Task<ServiceResult> RegisterAsync(RegisterViewModel model)
        {
            var exists = await _userManager.FindByEmailAsync(model.Email);

            if (exists != null)
            {
                return new ServiceResult
                {
                    Success = false,
                    Message = "Email already exists."
                };
            }

            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,

                FirstName = model.FirstName,
                LastName = model.LastName,

                Address = model.Address,
                City = model.City,
                Governorate = model.Governorate,

                Gender = model.Gender,
                Age = model.Age,

                CreatedAt = DateTime.Now,
                AccountStatus = "Active"
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                return new ServiceResult
                {
                    Success = false,
                    Message = "Registration failed.",
                    Errors = result.Errors
                                   .Select(e => e.Description)
                                   .ToList()
                };
            }

            
            if (!await _roleManager.RoleExistsAsync(model.Role))
            {
                await _roleManager.CreateAsync(new IdentityRole(model.Role));
            }

            await _userManager.AddToRoleAsync(user, model.Role);

            // ===========================
            // Patient
            // ===========================
            if (model.Role == "Patient")
            {
                var patient = new Patient
                {
                    UserId = user.Id,

                    BloodType = model.BloodType,

                    MedicalHistory = model.MedicalHistory ?? string.Empty
                };

                _context.Patients.Add(patient);
            }

            // ===========================
            // Nurse
            // ===========================
            else if (model.Role == "Nurse")
            {
                var nurse = new Nurse
                {
                    UserId = user.Id,
                    YearsExperience = model.YearsExperience ?? 0,
                    Specialization = model.Specialization ?? string.Empty,
                    IsVerified = false
                };

                _context.Nurses.Add(nurse);
            }

            // ===========================
            // Admin
            // ===========================
            else if (model.Role == "Admin")
            {
                var admin = new Admin
                {
                    UserId = user.Id,
                    Role = "Admin"
                };

                _context.Admins.Add(admin);
            }

            await _context.SaveChangesAsync();

            return new ServiceResult
            {
                Success = true,
                Message = "Registration completed successfully."
            };
        }

        public async Task<ServiceResult> LoginAsync(LoginViewModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                return new ServiceResult
                {
                    Success = false,
                    Message = "Email not found."
                };
            }

            var result = await _signInManager.PasswordSignInAsync(
                user,
                model.Password,
                model.RememberMe,
                lockoutOnFailure: false);

            if (!result.Succeeded)
            {
                return new ServiceResult
                {
                    Success = false,
                    Message = "Invalid email or password."
                };
            }

            return new ServiceResult
            {
                Success = true,
                Message = "Login successful."
            };
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<ApplicationUser?> GetUserByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        public async Task<bool> IsInRoleAsync(ApplicationUser user, string role)
        {
            return await _userManager.IsInRoleAsync(user, role);
        }
    }
}