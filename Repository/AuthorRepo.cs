using LibraryManagementAPI.Data;
using LibraryManagementAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementAPI.Repository
{
    public class AuthorRepo : IWorkRepo<Author>
    {
        private readonly AppDbContext _context;

        public AuthorRepo(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Author?> AddAsync(Author author)
        {
           
            if (author == null)
            {
                return null;
            }
            await _context.Authors.AddAsync(author);
            await _context.SaveChangesAsync();
            return author;
            throw new NotImplementedException();
        }

        public async Task<Author?> DeleteAsync(Author author,int id)
        {
            try
            {
                
                await _context.Books.FindAsync(id);
                if (author == null) { return null; }

                _context.Authors.Remove(author);

                await _context.SaveChangesAsync();

                return null;
            }
            catch (Exception ex)
            {
                throw new Exception("Error while deleting the category.", ex);
            }
        }

        public async Task<IEnumerable<Author>> GetAllAsync()
        {
            return await _context.Authors.ToListAsync();

        }


        public async Task<Author?> GetByIdAsync(int id)
        {
          return await _context.Authors.FindAsync(id);

        }

        public async Task<Author?> GetByNameAsync(string name)
        {
            return await _context.Authors.FindAsync(name);
        }

        public async Task<Author?> UpdateAsync(Author author , int id)
        {
            await _context.Authors.FindAsync(id);
            if (author == null) { return null; }
            _context.Update(author);
            await _context.SaveChangesAsync();
            return author;

        }

    }
}
