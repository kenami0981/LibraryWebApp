using Library.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Library.Infrastructure
{
    public class Seed
    {
        public static async Task SeedData(
            DataContext context,
            UserManager<AppUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            var roleNames = new[] { "User", "Admin" };

            foreach (var roleName in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            var admin = await userManager.FindByNameAsync("admin");
            if (admin == null)
            {
                admin = new AppUser
                {
                    DisplayName = "admin",
                    UserName = "admin",
                    Email = "admin@admin.com",
                    Bio = ""
                };

                await userManager.CreateAsync(admin, "Zaq12wsx!");
                await userManager.AddToRoleAsync(admin, "Admin");
            }

            var user = await userManager.FindByNameAsync("user123");
            if (user == null)
            {
                user = new AppUser
                {
                    DisplayName = "user",
                    UserName = "user123",
                    Email = "user@user.com",
                    Bio = ""
                };

                await userManager.CreateAsync(user, "Zaq12wsx!");
                await userManager.AddToRoleAsync(user, "User");
            }

            var existingUsers = userManager.Users.ToList();

            foreach (var u in existingUsers)
            {
                var roles = await userManager.GetRolesAsync(u);
                if (!roles.Any())
                {
                    await userManager.AddToRoleAsync(u, "User");
                }
            }

            if (await context.Authors.AnyAsync()) return;

            var authors = new List<Author>
            {
                new Author
                {
                    Id = Guid.NewGuid(),
                    FirstName = "George",
                    LastName = "Orwell",
                    Biography = "English novelist and essayist.",
                    Nationality = "British",
                    DateOfBirth = new DateTime(1903, 6, 25),
                    Books = new List<Book>
                    {
                        new Book
                        {
                            Id = Guid.NewGuid(),
                            Title = "1984",
                            Genre = BookGenre.Fiction,
                            PublishedDate = new DateTime(1949, 6, 8),
                            ISBN = "9780451524935",
                            IsAvailable = true,
                            Publisher = "Secker & Warburg"
                        }
                    }
                }
            };

            await context.Authors.AddRangeAsync(authors);
            await context.SaveChangesAsync();
        }
    }
}

