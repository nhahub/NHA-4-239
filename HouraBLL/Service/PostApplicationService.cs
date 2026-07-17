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
    public class PostApplicationService : IPostApplicationService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PostApplicationService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> ApplyForPostAsync(Guid postId, Guid applicantUserId)
        {
            var post = await _unitOfWork.ServicePostings.GetByIdAsync(postId);
            if (post == null || !post.IsAvailable) throw new InvalidOperationException("هذا المنشور غير متاح حالياً.");

            // منع الشخص إنه يقدم على البوست بتاعه
            if (post.UserId == applicantUserId) throw new InvalidOperationException("لا يمكنك التقديم على المنشور الخاص بك.");

            var application = new PostApplication
            {
                PostId = postId,
                ApplicantUserId = applicantUserId,
                Status = "Pending",
                CreatedAt = DateTime.UtcNow
            };

            await _unitOfWork.PostApplications.AddAsync(application);
            await _unitOfWork.CompleteAsync();
            return application.ApplicationId;
        }

        public async Task<bool> HandleApplicationStatusAsync(Guid applicationId, string newStatus)
        {
            var app = await _unitOfWork.PostApplications.GetByIdAsync(applicationId);
            if (app == null || app.Status != "Pending") return false;

            app.Status = newStatus;
            _unitOfWork.PostApplications.Update(app);

            if (newStatus == "Accepted")
            {
                var post = await _unitOfWork.ServicePostings.GetByIdAsync(app.PostId);
                if (post == null) return false;

                post.IsAvailable = false;
                _unitOfWork.ServicePostings.Update(post);

                Guid providerId = post.PostType == "Offer" ? post.UserId : app.ApplicantUserId;
                Guid receiverId = post.PostType == "Offer" ? app.ApplicantUserId : post.UserId;

                var serviceTx = new ServiceTransaction
                {
                    PostId = post.PostId,
                    ApplicationId = app.ApplicationId,
                    ProviderId = providerId,
                    ReceiverId = receiverId,
                    DurationInMinutes = post.EstimatedDurationInMinutes,
                    Status = "Pending",
                    TransactionDate = DateTime.UtcNow
                };
                await _unitOfWork.ServiceTransactions.AddAsync(serviceTx);

                // ➕ التعديل الذكي: توليد إشعار تلقائي للمستفيد إن طلبه اتقبل!
                var notify = new Notification
                {
                    UserId = app.ApplicantUserId,
                    Message = $"تهانينا! تم قبول طلبك للخدمة: ({post.Title}). يمكنك البدء الآن.",
                    CreatedAt = DateTime.UtcNow
                };
                await _unitOfWork.Notifications.AddAsync(notify);
            }

            await _unitOfWork.CompleteAsync();
            return true;
        }

        public async Task<IEnumerable<PostApplication>> GetApplicationsByPostIdAsync(Guid postId)
        {
            return await _unitOfWork.PostApplications.FindAsync(a => a.PostId == postId);
        }
    }
}
