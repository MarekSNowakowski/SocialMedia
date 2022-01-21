using SocialMedia.Infrastructure.Commands;
using SocialMedia.Infrastructure.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SocialMedia.Infrastructure.Services
{
    public interface IUserDataService
    {
        Task<IEnumerable<UserDataDTO>> BrowseAllAsync();
        Task<UserDataDTO> GetUserDataAsync(int id);
        Task AddUserDataAsync(CreateUserData userData);
        Task EditUserDataAsync(int id, EditUserData userData);
        Task DeleteUserDataAsync(int id);
    }
}
