using HouraDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HouraBLL.Service.Contracts
{
    public interface IPostApplicationService
    {
        Task<Guid> ApplyForPostAsync(Guid postId, Guid applicantUserId);
        Task<bool> HandleApplicationStatusAsync(Guid applicationId, string newStatus);
        Task<IEnumerable<PostApplication>> GetApplicationsByPostIdAsync(Guid postId);
    }
}
