using HouraDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HouraBLL.Service.Contracts
{
    public interface IReviewService
    {
        Task<bool> AddReviewAsync(Guid transactionId, Guid reviewerId, Guid revieweeId, int rating, string comment);
        Task<IEnumerable<Review>> GetUserReviewsAsync(Guid userId);
    }
}
