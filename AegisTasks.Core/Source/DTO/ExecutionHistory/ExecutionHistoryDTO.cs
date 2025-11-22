using System;

namespace AegisTasks.Core.DTO
{
    public class ExecutionHistoryLineDTO
    {
        public int Id { get; set; }
        public string WorkflowId { get; set; }
        public string WorkflowName { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool Success { get; set; }
        public bool Active { get; set; }
    }
}