using SocialMedia.Core.Domain;
using SocialMedia.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedia.Infrastructure.Repositories
{
    public class UserDataRepository : IUserDataRepository
    {
        private readonly AppDbContext _appDbContext;

        public UserDataRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task AddAsync(UserData s)
        {
            try
            {
                _appDbContext.UserData.Add(s);
                _appDbContext.SaveChanges();
                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                await Task.FromException(ex);
            }
        }

        public async Task<IEnumerable<UserData>> BrowseAllAsync()
        {
            return await Task.FromResult(_appDbContext.UserData);
        }

        public async Task DelAsync(int id)
        {
            try
            {
                _appDbContext.UserData.Remove(_appDbContext.UserData.FirstOrDefault(x => x.Id == id));
                _appDbContext.SaveChanges();
                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                await Task.FromException(ex);
            }
        }

        public async Task<UserData> GetAsync(int id)
        {
            return await Task.FromResult(_appDbContext.UserData.FirstOrDefault(x => x.Id == id));
        }

        public async Task UpdateAsync(UserData s)
        {
            try
            {
                var z = _appDbContext.UserData.FirstOrDefault(x => x.Id == s.Id);

                z.Username = s.Username;
                z.Email = s.Email;
                z.Birthday = s.Birthday;

                _appDbContext.SaveChanges();
                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                await Task.FromException(ex);
            }
        }
    
    }
}
