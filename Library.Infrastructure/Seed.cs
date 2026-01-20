using Library.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Library.Infrastructure
{
    public class Seed
    {
        public static async Task SeedData(DataContext context, UserManager<AppUser> userManager)
        {
            if(!userManager.Users.Any())
            {
                var users = new List<AppUser>
                {
                    new AppUser{DisplayName="franek", UserName="Franco123", Email="franek@test.com",Bio=""},
                    new AppUser{DisplayName ="asia", UserName="asia123", Email="asia@test.com",Bio=""}
                };

                foreach(var user in users)
                {
                    await userManager.CreateAsync(user, "Hase!l0123");
                }

            }

            // Sprawdzamy asynchronicznie, czy są już jacyś autorzy
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
                        },
                        new Book
                        {
                            Id = Guid.NewGuid(),
                            Title = "Animal Farm",
                            Genre = BookGenre.Fiction,
                            PublishedDate = new DateTime(1945, 8, 17),
                            ISBN = "9780451526342",
                            IsAvailable = true,
                            Publisher = "Secker & Warburg"
                        }
                    }
                },
                new Author
                {
                    Id = Guid.NewGuid(),
                    FirstName = "Henryk",
                    LastName = "Sienkiewicz",
                    Biography = "Polish journalist and novelist, Nobel Prize in Literature 1905.",
                    Nationality = "Polish",
                    DateOfBirth = new DateTime(1846, 5, 5),
                    Books = new List<Book>
                    {
                        new Book
                        {
                            Id = Guid.NewGuid(),
                            Title = "Quo Vadis",
                            Genre = BookGenre.History,
                            PublishedDate = new DateTime(1896, 1, 1),
                            ISBN = "9780781284501",
                            IsAvailable = true,
                            Publisher = "Gebethner i Wolff"
                        }
                    }
                }
            };

            await context.Authors.AddRangeAsync(authors);
            await context.SaveChangesAsync();
        }
    }
}