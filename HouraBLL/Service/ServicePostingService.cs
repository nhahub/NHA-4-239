using HouraBLL.Service.Contracts;
using HouraDAL.Entities;
using HouraDAL.Repository.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HouraBLL.Service
{
    public class ServicePostingService : IServicePostingService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ServicePostingService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> CreatePostAsync(Guid userId, int categoryId, string postType, string title, string description, int durationInMinutes)
        {
            var post = new ServicePosting
            {
                UserId = userId,
                CategoryId = categoryId,
                PostType = postType, 
                Title = title,
                Description = description,
                EstimatedDurationInMinutes = durationInMinutes,
                IsAvailable = true,
                CreatedAt = DateTime.UtcNow
            };

            await _unitOfWork.ServicePostings.AddAsync(post);

            await _unitOfWork.CompleteAsync();
            return post.PostId;
        }

        public async Task<IEnumerable<ServicePosting>> GetActivePostingsAsync(string? searchTerm = null)
        {
            // لو مفيش سرش، هات كل المتاح
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return await _unitOfWork.ServicePostings.FindAsync(p => p.IsAvailable == true);
            }

            // لو فيه سرش، فلتر بناءً على العنوان أو الوصف اللي كتبه اليوزر في الـ input
            return await _unitOfWork.ServicePostings.FindAsync(p =>
                p.IsAvailable == true &&
                (p.Title.Contains(searchTerm) || p.Description.Contains(searchTerm)));
        }

        public async Task<IEnumerable<ServicePosting>> GetPostingsByCategoryAsync(int categoryId)
        {
            return await _unitOfWork.ServicePostings.FindAsync(p => p.CategoryId == categoryId && p.IsAvailable == true);
        }

        public async Task<bool> TogglePostAvailabilityAsync(Guid postId, bool isAvailable)
        {
            var post = await _unitOfWork.ServicePostings.GetByIdAsync(postId);
            if (post == null) return false;

            post.IsAvailable = isAvailable;
            _unitOfWork.ServicePostings.Update(post);
            await _unitOfWork.CompleteAsync();
            return true;
        }

        public async Task<IEnumerable<ServicePosting>> GetPostingsByUserAsync(Guid userId)
        {
            return await _unitOfWork.ServicePostings.FindAsync(p => p.UserId == userId);
        }
    }
}