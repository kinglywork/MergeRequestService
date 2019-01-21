using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MergeRequestService.Services
{
    public interface IMailTemplate
    {
        /// <summary>
        /// FromBranch -> ToBranch
        /// </summary>
        string GenerateSectionTitle(string fromBranch, string toBranch);

        /// <summary>
        /// SourceBranch: DevChangeSetIds(join by comma)
        /// </summary>
        string GenerateQaChangeSet(string sourceBranch, string devChangeSetIds);

        /// <summary>
        /// ChangeSetId Memo
        /// </summary>
        string GenerateDevChangeSet(string changeSetId, string memo);

        /// <summary>
        /// Empty Line
        /// </summary>
        string GenerateNewLine();

        string GenerateMailBody(string mergeRequests);
    }
}