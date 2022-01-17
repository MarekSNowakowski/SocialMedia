using SocialMedia.Core.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace SocialMedia.Core.Repositories
{
    public interface IUserDataRepository
    {
        public Task<IEnumerable<UserData>> BrowseAllAsync();
        public Task<UserData> GetAsync(int id);
        public Task AddAsync(UserData s);
        public Task DelAsync(int id);
        public Task UpdateAsync(UserData s);
    }
}
