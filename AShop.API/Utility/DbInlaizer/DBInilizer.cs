using AShop.API.Data;
using AShop.API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Threading.Tasks;

namespace AShop.API.Utility.DbInlaizer
{
    public class DBInilizer(ApplicationDbContext context,RoleManager<IdentityRole> roleManager,
        UserManager<ApplicationUser> userManager):IDBInilizer
    {
        private readonly ApplicationDbContext context=context;
        private readonly RoleManager<IdentityRole> roleManager=roleManager;
        private readonly UserManager<ApplicationUser> userManager=userManager;
        public async Task Inilize()
        {
            try
            {
                if (context.Database.GetPendingMigrations().Any())
                {
                    context.Database.Migrate();
                }
            }
            catch(Exception e) { 
            Console.WriteLine(e.Message);
            }
            if (!await roleManager.Roles.AnyAsync())
            {
                await roleManager.CreateAsync(new(StaticData.SuperAdmin));
                await roleManager.CreateAsync(new(StaticData.Admin));
                await roleManager.CreateAsync(new(StaticData.Customer));
                await roleManager.CreateAsync(new(StaticData.Company));
            }
            await userManager.CreateAsync(new()
            {
                FirstName = "Super",
                LastName = "Admin",
                UserName = "Super@Admin",
                Gender = Gender.Male,
                BirthDate = new DateTime(2000, 05, 11),
                Email = "adnin@ashop.com",

            }, "Admin@69");
            var user = await userManager.FindByEmailAsync("adnin@ashop.com");

            await userManager.AddToRoleAsync(user, StaticData.SuperAdmin);
        }
    }
}
