using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HumanityService.Services.Interfaces
{
    public interface INotificationService
    {
        void NotifyUser(string recipient, string subject, string message);
    }
}
