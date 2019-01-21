using System.Collections.Generic;
using MergeRequestService.Models;

namespace MergeRequestService.Services
{
    public interface IMergeRequestMailGenerator
    {
        string GenerateMergeRequests(List<MergeRequest> mergeRequests);
        string GenerateMailBody(string mergeRequests);
    }
}