using System;
using System.Collections.Generic;
using MergeRequestService.Models;
using MergeRequestService.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MergeRequestService.Test
{
    [TestClass]
    public class MergeRequestMailContentHtmlGeneratorTest
    {
        private MergeRequestMailGenerator _mergeRequestMailGenerator;

        [TestInitialize]
        public void Initialize()
        {
            _mergeRequestMailGenerator = new MergeRequestMailGenerator(new MailHtmlTemplate());
        }
        
        [TestMethod]
        public void ShouldGenerateCorrectlyWhenMultipleDevAndQaMergeRequests()
        {
            var mergeRequests = new List<MergeRequest>
            {
                new MergeRequest
                {
                    SourceBranch = "PDM-144",
                    TargetBranch = "DEV",
                    ChangeSetId = 127,
                    DevChangeSetId = null,
                    Reviewer = "King",
                    Memo = "memo3"
                },
                new MergeRequest
                {
                    SourceBranch = "PDM-144",
                    TargetBranch = "DEV",
                    ChangeSetId = 128,
                    DevChangeSetId = null,
                    Reviewer = "King",
                    Memo = "memo4"
                },
                new MergeRequest
                {
                    SourceBranch = "PDM-145",
                    TargetBranch = "DEV",
                    ChangeSetId = 130,
                    DevChangeSetId = null,
                    Reviewer = "King",
                    Memo = "memo5"
                },
                new MergeRequest
                {
                    SourceBranch = "PDM-145",
                    TargetBranch = "DEV",
                    ChangeSetId = 129,
                    DevChangeSetId = null,
                    Reviewer = "King",
                    Memo = "memo6"
                },
                new MergeRequest
                {
                    SourceBranch = "PDM-144",
                    TargetBranch = "QA",
                    ChangeSetId = 123,
                    DevChangeSetId = 125,
                    Reviewer = "King",
                    Memo = "memo1"
                },
                new MergeRequest
                {
                    SourceBranch = "PDM-144",
                    TargetBranch = "QA",
                    ChangeSetId = 124,
                    DevChangeSetId = 126,
                    Reviewer = "King",
                    Memo = "memo2"
                }
            };

            var content = _mergeRequestMailGenerator.GenerateMergeRequests(mergeRequests);

            Assert.AreEqual(@"<div><strong><u>DEV -> QA:</u></strong></div>
<div>PDM-144: 125,126</div>
<br><div><strong><u>PDM-144 -> DEV:</u></strong></div>
<div>127 memo3</div>
<div>128 memo4</div>
<br><div><strong><u>PDM-145 -> DEV:</u></strong></div>
<div>129 memo6</div>
<div>130 memo5</div>
<br>", content);
        }
    }
}