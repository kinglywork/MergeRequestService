namespace MergeRequestService.Services
{
    public interface IMailSendingJobService
    {
        void SendTodayMergeRequestMail();
    }
}