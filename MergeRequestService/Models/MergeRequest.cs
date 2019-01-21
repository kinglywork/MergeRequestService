using System;
using System.ComponentModel.DataAnnotations;

namespace MergeRequestService.Models
{
    public class MergeRequest
    {
        public int Id { get; set; }
        [Required]
        public string SourceBranch { get; set; }
        [Required]
        public string TargetBranch { get; set; }
        [Required]
        public int ChangeSetId { get; set; }
        public int? DevChangeSetId { get; set; }
        public string Reviewer { get; set; }
        public string Memo { get; set; }
        public string SubmitBy { get; set; }
        public DateTime SubmitAt { get; set; }
        public string UpdateBy { get; set; }
        public DateTime UpdateAt { get; set; }
    }
}