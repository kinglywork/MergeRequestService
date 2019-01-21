using System.Collections.Generic;
using MergeRequestService.Models;

namespace MergeRequestService.Services
{
    public interface ITargetBranchListFactory
    {
        List<Branch> Create();
    }
}