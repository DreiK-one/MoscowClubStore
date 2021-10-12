using System;
using System.Diagnostics;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Store.Messages
{
    public class DebugNotificationService : INotificationService
    {
        public void SendConfirmationCode(string cellPhone, int code)
        {
            Debug.WriteLine("Cell phone: {0}, code: {1:0000}.", cellPhone, code);
        }

        public Task SendConfirmationCodeAsync(string cellPhone, int code)
        {
            Debug.WriteLine("Cell phone: {0}, code: {1:0000}.", cellPhone, code);

            return Task.CompletedTask;
        }

        //public void StartProcess(Order order) For sending Email 
        //{
        //    Debug.WriteLine("Order ID {0}", order.Id);
        //    Debug.WriteLine("Delivery: {0}", (object) order.Delivery.Description);
        //    Debug.WriteLine("Payment: {0}", (object) order.Payment.Description);
        //}

        //public Task StartProcessAsync(Order order) For sending Email async
        //{
        //    StartProcess(order);

        //    return Task.CompletedTask;
        //}
    }
}
