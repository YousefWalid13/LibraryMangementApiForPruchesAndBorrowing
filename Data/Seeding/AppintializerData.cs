using LibraryManagementAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementAPI.Data
{
    public static class AppDbInitializer
    {
        public static async Task SeedAsync(AppDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            // تأكد إن الداتابيز موجودة
            context.Database.Migrate();

            // 🟢 Seed Roles
            if (!roleManager.Roles.Any())
            {
                await roleManager.CreateAsync(new IdentityRole("Admin"));
                await roleManager.CreateAsync(new IdentityRole("Librarian"));
                await roleManager.CreateAsync(new IdentityRole("Member"));
            }

            // 🟢 Seed Users
            if (!userManager.Users.Any())
            {
                var admin = new ApplicationUser
                {
                  FullName = "System Admin",
                    UserName = "admin@library.com",
                    Email = "admin@library.com",
                    EmailConfirmed = true,
                    CreatedAt = DateTime.UtcNow,
                    Role = "Admin"
                };
                await userManager.CreateAsync(admin, "Admin@123");
                await userManager.AddToRoleAsync(admin, "Admin");

                var librarian = new ApplicationUser
                {
                    FullName = "Main Librarian",
                    UserName = "librarian@library.com",
                    Email = "librarian@library.com",
                    EmailConfirmed = true,
                    CreatedAt = DateTime.UtcNow,
                    Role = "Librarian"
                };
                await userManager.CreateAsync(librarian, "Librarian@123");
                await userManager.AddToRoleAsync(librarian, "Librarian");
            }

            // 🟢 Seed Authors
            if (!context.Authors.Any())
            {
                var authors = new List<Author>
                {
                    new Author { Name = "Naguib Mahfouz", Bio = "Egyptian Nobel-winning author", DateOfBirth = new DateTime(1911,12,11)},
                    new Author { Name = "Taha Hussein", Bio = "The Dean of Arabic Literature", DateOfBirth = new DateTime(1889,11,15)},
                    new Author { Name = "Ahmed Khaled Tawfik", Bio = "Egyptian novelist and physician", DateOfBirth = new DateTime(1962,6,10)}
                };
                context.Authors.AddRange(authors);
                await context.SaveChangesAsync();
            }

            // 🟢 Seed Categories
            if (!context.Categories.Any())
            {
                var categories = new List<Category>
                {
                    new Category { Name = "Arabic Literature", Description = "Classical and modern Arabic novels" },
                    new Category { Name = "Science Fiction", Description = "Sci-fi and fantasy books" },
                    new Category { Name = "History", Description = "Books about history and civilizations" }
                };
                context.Categories.AddRange(categories);
                await context.SaveChangesAsync();
            }

            // 🟢 Seed Books
            if (!context.Books.Any())
            {
                var firstAuthor = context.Authors.FirstOrDefault();
                var firstCategory = context.Categories.FirstOrDefault();

                if (firstAuthor != null && firstCategory != null) // ✅ تأكيد إن في Author و Category
                {
                    var books = new List<Book>
        {
            new Book
            {
                Title = "Palace Walk",
                ISBN = "9789774247782",
                PublishedDate = new DateTime(1956,1,1),
                Price = 150,
                CopiesAvailable = 5,
                AuthorId = firstAuthor.Id,
                CategoryId = firstCategory.Id
            },
            new Book
            {
                Title = "The Days",
                ISBN = "9789774247324",
                PublishedDate = new DateTime(1929,1,1),
                Price = 100,
                CopiesAvailable = 3,
                AuthorId = firstAuthor.Id,
                CategoryId = firstCategory.Id
            }
        };

                    context.Books.AddRange(books);
                    await context.SaveChangesAsync();
                }
            }

        }
    }
}
