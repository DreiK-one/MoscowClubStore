using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store
{
    public interface INotificationService
    {
        void SendConfirmationCode(string cellPhone, int code);

        Task SendConfirmationCodeAsync(string cellPhone, int code);

        //void StartProcess(Order order);  For sending email

        //Task StartProcessAsync(Order order); For sending email async
    }
}
