// using Microsoft.EntityFrameworkCore;
// using Skyway2.Common;
// using Skyway2.InterFaces;
//
// namespace Skyway2.Repositories;
//
// public class PaymentRepository(AppDbContext _payment) : IPaymentRepository
// {
//     public async Task<Response<bool>> UpdatePaymentStatus(int ticketId)
//     {
//         var response = new Response<bool>();
//         response.payload = false;
//
//         try
//         {
//             var ticket = await _payment.Tickets
//                 .FirstOrDefaultAsync(t => t.TicketId == ticketId);
//
//             if (ticket == null)
//             {
//                 response.statusCode = 404;
//                 response.devMessage = $"No ticket with ID {ticketId} was found!";
//                 response.messageLevel = MessageLevel.Admin;
//                 return response;
//             }
//
//             ticket.PaymentId = 2;
//
//             await _payment.SaveChangesAsync();
//
//             response.statusCode = 200;
//             response.payload = true;
//         }
//         catch (Exception e)
//         {
//             response.statusCode = 500;
//             response.message = "خطا در بروزرسانی وضعیت پرداخت";
//             response.devMessage = e.Message;
//             response.messageLevel = MessageLevel.Admin;
//         }
//
//         return response;
//     }
// }