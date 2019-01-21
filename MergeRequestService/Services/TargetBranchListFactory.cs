using System.Collections.Generic;
using MergeRequestService.Models;

namespace MergeRequestService.Services
{
    public class TargetBranchListFactory : ITargetBranchListFactory
    {
        public const string BranchDev = "DEV";
        public const string BranchQa = "QA";

        public List<Branch> Create()
        {
            return new List<Branch>
            {
                new Branch
                {
                    Name = BranchDev
                },
                new Branch
                {
                    Name = BranchQa
                }
            };
        }
    }
}