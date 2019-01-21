using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MergeRequestService.Models;

namespace MergeRequestService.Services
{
    public interface IMergeRequestMailSender
    {
        void Send(MergeRequestMail mail);
    }
}
