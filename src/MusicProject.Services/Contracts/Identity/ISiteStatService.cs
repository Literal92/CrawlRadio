using System.Collections.Generic;
using System.Threading.Tasks;
using MusicProject.Entities.Identity;
using System.Security.Claims;
using MusicProject.ViewModels.Identity;

namespace MusicProject.Services.Contracts.Identity
{
    public interface ISiteStatService
    {
        Task<List<User>> GetOnlineUsersListAsync(int numbersToTake, int minutesToTake);

        Task<List<User>> GetTodayBirthdayListAsync();

        Task UpdateUserLastVisitDateTimeAsync(ClaimsPrincipal claimsPrincipal);

        Task<AgeStatViewModel> GetUsersAverageAge();
    }
}