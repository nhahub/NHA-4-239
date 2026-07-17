using HouraDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HouraBLL.Service.Contracts
{
    public interface IServicePostingService
    {
        Task<Guid> CreatePostAsync(Guid userId, int categoryId, string postType, string title, string description, int durationInMinutes);
        Task<IEnumerable<ServicePosting>> GetActivePostingsAsync(string? searchTerm = null);
        Task<IEnumerable<ServicePosting>> GetPostingsByCategoryAsync(int categoryId);
        Task<bool> TogglePostAvailabilityAsync(Guid postId, bool isAvailable);
        Task<IEnumerable<ServicePosting>> GetPostingsByUserAsync(Guid userId);
    }
}
