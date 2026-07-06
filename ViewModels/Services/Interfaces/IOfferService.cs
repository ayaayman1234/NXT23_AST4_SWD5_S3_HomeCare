using NursingCarePlatform.Web.Models;
using NursingCarePlatform.Web.Models.Responses;
using NursingCarePlatform.Web.ViewModels.Offer;

namespace NursingCarePlatform.Web.Services.Interfaces
{
    public interface IOfferService
    {
        Task<SendOfferViewModel?> GetRequestAsync(int requestId);

        Task<ServiceResult> SendOfferAsync(
            string userId,
            SendOfferViewModel model);
        Task<List<MyOffer>> GetOffersForRequestAsync(int requestId);
        Task AcceptOfferAsync(int offerId);

        Task RejectOfferAsync(int offerId);
    }
}