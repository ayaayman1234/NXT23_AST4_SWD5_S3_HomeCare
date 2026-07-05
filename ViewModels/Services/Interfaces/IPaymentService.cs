
using NursingCarePlatform.Web.ViewModels;

namespace NursingCarePlatform.Web.Services.Interfaces
{
    public interface IPaymentService
    {
        Task<int> CreatePaymentAsync(PaymentViewModel model);
        Task<PaymentDetailsViewModel?> GetPaymentDetailsAsync(int id);
    }
}