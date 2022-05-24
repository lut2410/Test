using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WebApplication.Application.Interfaces;
using WebApplication.Infrastructure.Contexts;
using WebApplication.Infrastructure.Entities;

namespace WebApplication.Implementation
{
    public class UserService : IUserService
    {
        private readonly InMemoryContext _dbContext;

        public UserService(InMemoryContext dbContext)
        {
            _dbContext = dbContext;

            // this is a hack to seed data into the in memory database. Do not use this in production.
            _dbContext.Database.EnsureCreated();
        }

        /// <inheritdoc />
        public async Task<User?> GetAsync(int id, CancellationToken cancellationToken = default)
        {
            User? user = await _dbContext.Users.AsNoTracking()
                                        .Where(user => user.Id == id)
                                         .Include(x => x.ContactDetail)
                                         .FirstOrDefaultAsync(cancellationToken);

            return user;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<User>> FindAsync(string? givenNames, string? lastName, CancellationToken cancellationToken = default)
        {
            var users = await _dbContext.Users.AsNoTracking()
                .Where(user =>
            (string.IsNullOrEmpty(givenNames) || user.GivenNames == givenNames)
            && (string.IsNullOrEmpty(lastName) || user.LastName == lastName)
            )
                .Include(x => x.ContactDetail)
                .ToListAsync(cancellationToken);
            return users;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<User>> GetPaginatedAsync(int page, int count, CancellationToken cancellationToken = default)
        {
            var users = await _dbContext.Users.AsNoTracking()
                    .Include(x => x.ContactDetail)
                    .Skip((page - 1) * count)
                    .Take(count)
                    .ToListAsync(cancellationToken);
            return users;
        }

        /// <inheritdoc />
        public async Task<User> AddAsync(User user, CancellationToken cancellationToken = default)
        {
            var existingUser = await _dbContext.Users.Where(u => u.Id == user.Id)
                                         .FirstOrDefaultAsync(cancellationToken);
            if (existingUser == null)
            {
                _dbContext.Users.Add(user);
                if (await _dbContext.SaveChangesAsync(cancellationToken) > 0)
                {
                    return user;
                }
                else
                    return default;
            }
            return existingUser;
        }

        /// <inheritdoc />
        public async Task<User> UpdateAsync(User user, CancellationToken cancellationToken = default)
        {
            var existingUser = await GetAsync(user.Id, cancellationToken);
            if (existingUser == null)
            {
                return default;
            }
            existingUser.LastName = user.LastName;
            existingUser.GivenNames = user.GivenNames;
            if (existingUser.ContactDetail == null)
            {
                existingUser.ContactDetail = new ContactDetail();
            }
            existingUser.ContactDetail.EmailAddress = user.ContactDetail?.EmailAddress ?? default;
            existingUser.ContactDetail.MobileNumber = user.ContactDetail?.MobileNumber ?? default;

            _dbContext.Users.Update(existingUser);
            if (await _dbContext.SaveChangesAsync(cancellationToken) > 0)
            {
                return existingUser;
            }
            return default;
        }

        /// <inheritdoc />
        public async Task<User?> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var user = await GetAsync(id, cancellationToken);
            if (user == null)
            {
                return default;
            }
            _dbContext.Users.Remove(user);
            if (await _dbContext.SaveChangesAsync(cancellationToken) > 0)
            {
                return user;
            }
            return default;
        }

        /// <inheritdoc />
        public async Task<int> CountAsync(CancellationToken cancellationToken = default)
        {
            return await _dbContext.Users.AsNoTracking()
              .CountAsync(cancellationToken);
        }
    }
}
