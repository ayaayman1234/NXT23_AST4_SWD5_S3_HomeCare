using Microsoft.EntityFrameworkCore;
using NursingCarePlatform.Web.Data;
using NursingCarePlatform.Web.Models;
using NursingCarePlatform.Web.Models.Responses;
using NursingCarePlatform.Web.Services.Interfaces;
using NursingCarePlatform.Web.ViewModels.Payment;

namespace NursingCarePlatform.Web.Services.Implementations
{
    public class PaymentService : IPaymentService
    {
        private readonly NursingDbContext _context;

        private const decimal CommissionRate = 0.10m;

        public PaymentService(NursingDbContext context)
        {
            _context = context;
        }

        public async Task<CreatePaymentViewModel?> GetPaymentForRequestAsync(int careRequestId)
        {
            var request = await _context.CareRequests
                .Include(r => r.Patient)
                    .ThenInclude(p => p.User)
                .Include(r => r.Nurse)
                    .ThenInclude(n => n.User)
                .Include(r => r.Service)
                .FirstOrDefaultAsync(r => r.Id == careRequestId);

            if (request == null)
                return null;
            bool alreadyPaid = await _context.Payments
    .AnyAsync(x => x.CareRequestId == careRequestId);

            if (alreadyPaid)
                return null;

            return new CreatePaymentViewModel
            {
                CareRequestId = request.Id,
                Amount = request.BudgetMax,
                PaymentMethod = string.Empty,
                TransactionReference = string.Empty,

                PatientName =
                    request.Patient.User.FirstName + " " +
                    request.Patient.User.LastName,

                NurseName = request.Nurse == null
                    ? "Not Assigned"
                    : request.Nurse.User.FirstName + " " +
                      request.Nurse.User.LastName,

                ServiceName = request.Service.Name
            };
        }

        public async Task<ServiceResult> CreatePaymentAsync(CreatePaymentViewModel model)
        {
            if (model.Amount <= 0)
            {
                return new ServiceResult
                {
                    Success = false,
                    Message = "Invalid payment amount."
                };
            }

            var request = await _context.CareRequests
                .FirstOrDefaultAsync(x => x.Id == model.CareRequestId);

            if (request == null)
            {
                return new ServiceResult
                {
                    Success = false,
                    Message = "Care Request not found."
                };
            }

            bool alreadyPaid = await _context.Payments
                .AnyAsync(x => x.CareRequestId == model.CareRequestId);

            if (alreadyPaid)
            {
                return new ServiceResult
                {
                    Success = false,
                    Message = "Payment already exists."
                };
            }

            decimal commission = model.Amount * CommissionRate;
            decimal netAmount = model.Amount - commission;

            var payment = new Payment
            {
                CareRequestId = model.CareRequestId,
                Amount = model.Amount,
                CommissionAmount = commission,
                NetAmount = netAmount,
                PaymentMethod = model.PaymentMethod,

                TransactionReference =
                    string.IsNullOrWhiteSpace(model.TransactionReference)
                    ? Guid.NewGuid().ToString("N")[..12].ToUpper()
                    : model.TransactionReference,

                PaymentDate = DateTime.Now,
                PaymentStatus = "Completed"
            };

            _context.Payments.Add(payment);

            request.RequestStatus = "Completed";

            await _context.SaveChangesAsync();
            if (request.NurseId != null)
            {
                var nurse = await _context.Nurses
                    .FirstOrDefaultAsync(x => x.Id == request.NurseId);

                if (nurse != null)
                {
                    request.Nurse.IsAvailable = true;
                }
            }

            return new ServiceResult
            {
                Success = true,
                Message = "Payment completed successfully.",
                DataId = payment.Id
            };
        }

        public async Task<List<PaymentHistoryViewModel>> GetAllPaymentsAsync()
        {
            return await _context.Payments
                .Include(p => p.CareRequest)
                    .ThenInclude(r => r.Patient)
                        .ThenInclude(p => p.User)
                .OrderByDescending(p => p.PaymentDate)
                .Select(p => new PaymentHistoryViewModel
                {
                    Id = p.Id,
                    CareRequestId = p.CareRequestId,
                    PatientName = p.CareRequest.Patient.User.FirstName + " " +
                                  p.CareRequest.Patient.User.LastName,
                    Amount = p.Amount,
                    PaymentMethod = p.PaymentMethod,
                    PaymentStatus = p.PaymentStatus,
                    PaymentDate = p.PaymentDate
                })
                .ToListAsync();
        }

        public async Task<PaymentDetailsViewModel?> GetPaymentDetailsAsync(int paymentId)
        {
            var payment = await _context.Payments
                .Include(p => p.CareRequest)
                    .ThenInclude(r => r.Patient)
                        .ThenInclude(p => p.User)
                .Include(p => p.CareRequest)
                    .ThenInclude(r => r.Nurse)
                        .ThenInclude(n => n.User)
                .Include(p => p.CareRequest)
                    .ThenInclude(r => r.Service)
                .FirstOrDefaultAsync(p => p.Id == paymentId);

            if (payment == null)
                return null;

            return new PaymentDetailsViewModel
            {
                Id = payment.Id,
                CareRequestId = payment.CareRequestId,

                PatientName =
                    payment.CareRequest.Patient.User.FirstName + " " +
                    payment.CareRequest.Patient.User.LastName,

                NurseName =
                    payment.CareRequest.Nurse == null
                    ? "-"
                    : payment.CareRequest.Nurse.User.FirstName + " " +
                      payment.CareRequest.Nurse.User.LastName,

                ServiceName = payment.CareRequest.Service.Name,

                Amount = payment.Amount,
                CommissionAmount = payment.CommissionAmount,
                NetAmount = payment.NetAmount,
                PaymentMethod = payment.PaymentMethod,
                TransactionReference = payment.TransactionReference,
                PaymentDate = payment.PaymentDate,
                PaymentStatus = payment.PaymentStatus
            };
        }

        public async Task<List<PaymentHistoryViewModel>> GetPaymentHistoryAsync()
        {
            return await GetAllPaymentsAsync();
        }
    }
}