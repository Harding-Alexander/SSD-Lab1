using Microsoft.AspNetCore.Identity;
using SSD_Lab1.Models;

namespace SSD_Lab1.Data
{
    public class SeedData
    {
        public static AppSecrets appSecrets { get; set; }
        public static async Task InitializeAsync(IServiceProvider serviceProivder)
        {
            var roleManager = serviceProivder.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProivder.GetRequiredService<UserManager<ApplicationUser>>();
            var context = serviceProivder.GetRequiredService<ApplicationDbContext>();

            if (appSecrets.SupervisorPassword.Length < 6)
                throw new Exception("Supervisor password must be at least 6 characters. Update your secrets file.");

            if (appSecrets.EmployeePassword.Length < 6)
                throw new Exception("Employee password must be at least 6 characters. Update your secrets file.");

            string[] roles = new[] { "Supervisor", "Employee" };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole(role));
            }

            var supEmail = "supervisor@lab.local";
            if (await userManager.FindByEmailAsync(supEmail) == null)
            {
                var sup = new ApplicationUser
                {
                    UserName = supEmail,
                    Email = supEmail,
                    FirstName = "Sup",
                    LastName = "Ervisor",
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(sup, appSecrets.SupervisorPassword);
                await userManager.AddToRoleAsync(sup, "Supervisor");
            }

            var empEmail = "employee@lab.local";
            if (await userManager.FindByEmailAsync(empEmail) == null)
            {
                var emp = new ApplicationUser
                {
                    UserName = empEmail,
                    Email = empEmail,
                    FirstName = "Em",
                    LastName = "Ployee",
                    EmailConfirmed = true,
                };
                await userManager.CreateAsync(emp, appSecrets.EmployeePassword);
                await userManager.AddToRoleAsync(emp, "Employee");
            }

            if (!context.Company.Any())
            {
                context.Company.AddRange(
                    new Company { Name = "Acme Co", YearsInBusiness = 12, Website = "https://acme.example", Province = "ON" },
                    new Company { Name = "Beta LLC", YearsInBusiness = 5, Website = "https://beta.example", Province = "QC" }
                );
                await context.SaveChangesAsync();
            }
        }
    }
}
