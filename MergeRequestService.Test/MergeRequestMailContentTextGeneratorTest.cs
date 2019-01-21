using System;
using System.Collections.Generic;
using MergeRequestService.Models;
using MergeRequestService.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MergeRequestService.Test
{
    [TestClass]
    public class MergeRequestMailContentTextGeneratorTest
    {
        private MergeRequestMailGenerator _mergeRequestMailGenerator;

        [TestInitialize]
        public void Initialize()
        {
            _mergeRequestMailGenerator = new MergeRequestMailGenerator(new MailTextTemplate());
        }

        [TestMethod]
        public void ShouldUseChangeSetIdWhenMergeToDev()
        {
            var mergeRequests = new List<MergeRequest>
            {
                new MergeRequest
                {
                    SourceBranch = "PDM-144",
                    TargetBranch = "DEV",
                    ChangeSetId = 123,
                    DevChangeSetId = null,
                    Reviewer = "King",
                    Memo = "memo"
                }
            };

            var content = _mergeRequestMailGenerator.GenerateMergeRequests(mergeRequests);

            Assert.AreEqual(@"PDM-144 -> DEV:
123 memo

", content);
        }

        [TestMethod]
        public void ShouldSplitBySourceBranchWhenMergeToDev()
        {
            var mergeRequests = new List<MergeRequest>
            {
                new MergeRequest
                {
                    SourceBranch = "PDM-144",
                    TargetBranch = "DEV",
                    ChangeSetId = 123,
                    DevChangeSetId = null,
                    Reviewer = "King",
                    Memo = "memo"
                },
                new MergeRequest
                {
                    SourceBranch = "PDM-145",
                    TargetBranch = "DEV",
                    ChangeSetId = 125,
                    DevChangeSetId = null,
                    Reviewer = "King",
                    Memo = "memo2"
                }
            };

            var content = _mergeRequestMailGenerator.GenerateMergeRequests(mergeRequests);

            Assert.AreEqual(@"PDM-144 -> DEV:
123 memo

PDM-145 -> DEV:
125 memo2

", content);
        }

        [TestMethod]
        public void ShouldSplitByChangeSetIdWhenMultipleChangeSetsInSameSourceBranch()
        {
            var mergeRequests = new List<MergeRequest>
            {
                new MergeRequest
                {
                    SourceBranch = "PDM-144",
                    TargetBranch = "DEV",
                    ChangeSetId = 123,
                    DevChangeSetId = null,
                    Reviewer = "King",
                    Memo = "memo"
                },
                new MergeRequest
                {
                    SourceBranch = "PDM-144",
                    TargetBranch = "DEV",
                    ChangeSetId = 125,
                    DevChangeSetId = null,
                    Reviewer = "King",
                    Memo = "memo2"
                }
            };

            var content = _mergeRequestMailGenerator.GenerateMergeRequests(mergeRequests);

            Assert.AreEqual(@"PDM-144 -> DEV:
123 memo
125 memo2

", content);
        }

        [TestMethod]
        public void ShouldOrderChangeSetsWhenMultipleChangeSetsInSameSourceBranch()
        {
            var mergeRequests = new List<MergeRequest>
            {
                new MergeRequest
                {
                    SourceBranch = "PDM-144",
                    TargetBranch = "DEV",
                    ChangeSetId = 125,
                    DevChangeSetId = null,
                    Reviewer = "King",
                    Memo = "memo"
                },
                new MergeRequest
                {
                    SourceBranch = "PDM-144",
                    TargetBranch = "DEV",
                    ChangeSetId = 123,
                    DevChangeSetId = null,
                    Reviewer = "King",
                    Memo = "memo2"
                }
            };

            var content = _mergeRequestMailGenerator.GenerateMergeRequests(mergeRequests);

            Assert.AreEqual(@"PDM-144 -> DEV:
123 memo2
125 memo

", content);
        }

        [TestMethod]
        public void ShouldUseDevChangeSetIdWhenMergeToQa()
        {
            var mergeRequests = new List<MergeRequest>
            {
                new MergeRequest
                {
                    SourceBranch = "PDM-144",
                    TargetBranch = "QA",
                    ChangeSetId = 123,
                    DevChangeSetId = 125,
                    Reviewer = "King",
                    Memo = "memo"
                }
            };

            var content = _mergeRequestMailGenerator.GenerateMergeRequests(mergeRequests);

            Assert.AreEqual(@"DEV -> QA:
PDM-144: 125

", content);
        }

        [TestMethod]
        public void ShouldMergeQaChangeSetsInOneLineWhenRequestsHaveSameSource()
        {
            var mergeRequests = new List<MergeRequest>
            {
                new MergeRequest
                {
                    SourceBranch = "PDM-144",
                    TargetBranch = "QA",
                    ChangeSetId = 123,
                    DevChangeSetId = 125,
                    Reviewer = "King",
                    Memo = "memo"
                },
                new MergeRequest
                {
                    SourceBranch = "PDM-144",
                    TargetBranch = "QA",
                    ChangeSetId = 124,
                    DevChangeSetId = 126,
                    Reviewer = "King",
                    Memo = "memo"
                }
            };

            var content = _mergeRequestMailGenerator.GenerateMergeRequests(mergeRequests);

            Assert.AreEqual(@"DEV -> QA:
PDM-144: 125,126

", content);
        }

        [TestMethod]
        public void ShouldMergeQaChangeSetsInOneLineInOrderWhenRequestsHaveSameSource()
        {
            var mergeRequests = new List<MergeRequest>
            {
                new MergeRequest
                {
                    SourceBranch = "PDM-144",
                    TargetBranch = "QA",
                    ChangeSetId = 123,
                    DevChangeSetId = 126,
                    Reviewer = "King",
                    Memo = "memo"
                },
                new MergeRequest
                {
                    SourceBranch = "PDM-144",
                    TargetBranch = "QA",
                    ChangeSetId = 124,
                    DevChangeSetId = 125,
                    Reviewer = "King",
                    Memo = "memo"
                }
            };

            var content = _mergeRequestMailGenerator.GenerateMergeRequests(mergeRequests);

            Assert.AreEqual(@"DEV -> QA:
PDM-144: 125,126

", content);
        }

        [TestMethod]
        public void ShouldSplitQaChangeSetsToDifferentLineWhenRequestsHaveDifferentSource()
        {
            var mergeRequests = new List<MergeRequest>
            {
                new MergeRequest
                {
                    SourceBranch = "PDM-144",
                    TargetBranch = "QA",
                    ChangeSetId = 123,
                    DevChangeSetId = 125,
                    Reviewer = "King",
                    Memo = "memo"
                },
                new MergeRequest
                {
                    SourceBranch = "PDM-145",
                    TargetBranch = "QA",
                    ChangeSetId = 124,
                    DevChangeSetId = 126,
                    Reviewer = "King",
                    Memo = "memo"
                }
            };

            var content = _mergeRequestMailGenerator.GenerateMergeRequests(mergeRequests);

            Assert.AreEqual(@"DEV -> QA:
PDM-144: 125
PDM-145: 126

", content);
        }

        [TestMethod]
        public void ShouldDistinctDevChangeSetIdWhenRepeat()
        {
            var mergeRequests = new List<MergeRequest>
            {
                new MergeRequest
                {
                    SourceBranch = "PDM-144",
                    TargetBranch = "QA",
                    ChangeSetId = 123,
                    DevChangeSetId = 125,
                    Reviewer = "King",
                    Memo = "memo"
                },
                new MergeRequest
                {
                    SourceBranch = "PDM-144",
                    TargetBranch = "QA",
                    ChangeSetId = 123,
                    DevChangeSetId = 125,
                    Reviewer = "King",
                    Memo = "memo"
                }
            };

            var content = _mergeRequestMailGenerator.GenerateMergeRequests(mergeRequests);

            Assert.AreEqual(@"DEV -> QA:
PDM-144: 125

", content);
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

            Assert.AreEqual(@"DEV -> QA:
PDM-144: 125,126

PDM-144 -> DEV:
127 memo3
128 memo4

PDM-145 -> DEV:
129 memo6
130 memo5

", content);
        }
    }
}