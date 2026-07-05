using Microsoft.EntityFrameworkCore;
using NursingCarePlatform.Web.Data;
using NursingCarePlatform.Web.Models;
using NursingCarePlatform.Web.Services.Interfaces;
using NursingCarePlatform.Web.ViewModels;

namespace NursingCarePlatform.Web.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly ApplicationDbContext _context;
        private const decimal PlatformCommissionRate = 0.15m; // 15% commission-based model

        public PaymentService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> CreatePaymentAsync(PaymentViewModel model)
        {
            var careRequestExists = await _context.CareRequests
                .AnyAsync(cr => cr.Id == model.CareRequestId);

            if (!careRequestExists)
                throw new InvalidOperationException("Invalid CareRequestId.");

            // Payment is linked directly to CareRequestId, not to a specific Assignment.
            var payment = new Payment
            {
                CareRequestId = model.CareRequestId,
                Amount = model.Amount,
                PaymentMethod = model.PaymentMethod,
                TransactionReference = model.TransactionReference,
                PaymentDate = DateTime.UtcNow,
                PaymentStatus = "Pending"
            };

            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();

            return payment.Id;
        }

        public async Task<PaymentDetailsViewModel?> GetPaymentDetailsAsync(int id)
        {
            var payment = await _context.Payments
                .Include(p => p.CareRequest)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (payment == null)
                return null;

            var commissionAmount = Math.Round(payment.Amount * PlatformCommissionRate, 2);

            return new PaymentDetailsViewModel
            {
                Id = payment.Id,
                CareRequestId = payment.CareRequestId,
                Amount = payment.Amount,
                CommissionAmount = commissionAmount,
                NetAmountToNurse = payment.Amount - commissionAmount,
                PaymentMethod = payment.PaymentMethod,
                TransactionReference = payment.TransactionReference,
                PaymentDate = payment.PaymentDate,
                PaymentStatus = payment.PaymentStatus
            };
        }
    }
}