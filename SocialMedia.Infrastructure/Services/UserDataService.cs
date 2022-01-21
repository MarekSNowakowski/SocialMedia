using SocialMedia.Core.Domain;
using SocialMedia.Core.Repositories;
using SocialMedia.Infrastructure.Commands;
using SocialMedia.Infrastructure.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedia.Infrastructure.Services
{
    public class UserDataService : IUserDataService
    {
        private readonly IUserDataRepository _userDataRepository;

        public UserDataService(IUserDataRepository userDataRepository)
        {
            _userDataRepository = userDataRepository;
        }

        private IEnumerable<UserDataDTO> MapUserData(IEnumerable<UserData> userData)
        {
            return userData.Select(x => new UserDataDTO()
            {
                Id = x.Id,
                Username = x.Username,
                Email = x.Email,
                Birthday = x.Birthday,
                RegistrationTime = x.RegistrationTime
            });
        }

        private UserDataDTO MapUserData(UserData userData)
        {
            return new UserDataDTO()
            {
                Id = userData.Id,
                Username = userData.Username,
                Email = userData.Email,
                Birthday = userData.Birthday,
                RegistrationTime = userData.RegistrationTime
            };
        }

        public async Task<IEnumerable<UserDataDTO>> BrowseAllAsync()
        {
            var z = await _userDataRepository.BrowseAllAsync();
            if (z != null)
                return MapUserData(z);
            else
                return null;
        }

        public async Task<UserDataDTO> GetUserDataAsync(int id)
        {
            UserData z = await _userDataRepository.GetAsync(id);
            if (z != null)
                return MapUserData(z);
            else
                return null;
        }

        public async Task AddUserDataAsync(CreateUserData userData)
        {
            UserData newUserData = new UserData()
            {
                Username = userData.Username,
                Email = userData.Email,
                Birthday = userData.Birthday,
                RegistrationTime = DateTime.Now
            };

            await _userDataRepository.AddAsync(newUserData);
        }

        public async Task DeleteUserDataAsync(int id)
        {
            await _userDataRepository.DelAsync(id);
        }

        public async Task EditUserDataAsync(int id, EditUserData userData)
        {
            UserData updateUserData = await _userDataRepository.GetAsync(id);
            updateUserData.Username = userData.Username;
            updateUserData.Email = userData.Email;
            updateUserData.Birthday = userData.Birthday;

            await _userDataRepository.UpdateAsync(updateUserData);
        }
    }
}
