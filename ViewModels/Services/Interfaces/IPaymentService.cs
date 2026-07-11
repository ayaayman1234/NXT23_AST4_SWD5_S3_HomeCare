using NursingCarePlatform.Web.Models.Responses;
using NursingCarePlatform.Web.ViewModels.Payment;

namespace NursingCarePlatform.Web.Services.Interfaces
{
    public interface IPaymentService
    {
        
        Task<CreatePaymentViewModel?> GetPaymentForRequestAsync(int careRequestId);

        
        Task<ServiceResult> CreatePaymentAsync(CreatePaymentViewModel model);

        
        Task<List<PaymentHistoryViewModel>> GetAllPaymentsAsync();

        
        Task<PaymentDetailsViewModel?> GetPaymentDetailsAsync(int paymentId);

       
        Task<List<PaymentHistoryViewModel>> GetPaymentHistoryAsync();
    }
}