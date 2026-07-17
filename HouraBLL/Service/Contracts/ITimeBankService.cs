using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HouraBLL.Service.Contracts
{
    public interface ITimeBankService
    {
        Task<bool> ExecuteTimeExchangeAsync(Guid serviceTransactionId);
        Task<bool> ProcessTimePurchaseAsync(Guid userId, int packageId, decimal amountPaid);
    }
}
