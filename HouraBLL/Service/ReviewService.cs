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
    public class ReviewService : IReviewService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ReviewService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> AddReviewAsync(Guid transactionId, Guid reviewerId, Guid revieweeId, int rating, string comment)
        {
            var tx = await _unitOfWork.ServiceTransactions.GetByIdAsync(transactionId);
            if (tx == null || tx.Status != "Completed")
                throw new InvalidOperationException("لا يمكنك تقييم خدمة لم تكتمل بعد.");

            var review = new Review
            {
                ServiceTransactionId = transactionId,
                ReviewerUserId = reviewerId,
                RevieweeUserId = revieweeId,
                Rating = rating,
                Comment = comment,
                CreatedAt = DateTime.UtcNow
            };

            await _unitOfWork.Reviews.AddAsync(review);
            return await _unitOfWork.CompleteAsync() > 0;
        }

        public async Task<IEnumerable<Review>> GetUserReviewsAsync(Guid userId)
        {
            return await _unitOfWork.Reviews.FindAsync(r => r.RevieweeUserId == userId);
        }
    }
}
