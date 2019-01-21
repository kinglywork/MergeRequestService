using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MergeRequestService.Services
{
    public class MailHtmlTemplate : IMailTemplate
    {
        public string GenerateSectionTitle(string fromBranch, string toBranch)
        {
            return $"<div><strong><u>{fromBranch} -> {toBranch}:</u></strong></div>";
        }

        public string GenerateQaChangeSet(string sourceBranch, string devChangeSetIds)
        {
            return $"<div>{sourceBranch}: {devChangeSetIds}</div>";
        }

        public string GenerateDevChangeSet(string changeSetId, string memo)
        {
            return $"<div>{changeSetId} {memo}</div>";
        }

        public string GenerateNewLine()
        {
            return "<br>";
        }

        public string GenerateMailBody(string mergeRequests)
        {
            return $@"<div>Hi team,</div>
<br>
<div>Please review and help to merge the change sets below:</div>
<br>
{mergeRequests}
<br>
<div>Thanks</div>
<div>ECSI CN Team</div>
";
        }
    }
}