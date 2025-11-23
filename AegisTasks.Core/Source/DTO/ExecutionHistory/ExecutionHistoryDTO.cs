using System;

namespace AegisTasks.Core.DTO
{
    public class ExecutionHistoryLineDTO
    {
        // Nombres de campos para uso consistente en UI o DataGridView
        public static readonly string FIELD_ID = nameof(Id);
        public static readonly string FIELD_WORKFLOWID = nameof(WorkflowId);
        public static readonly string FIELD_WORKFLOWNAME = nameof(WorkflowName);
        public static readonly string FIELD_STARTDATE = nameof(StartDate);
        public static readonly string FIELD_ENDDATE = nameof(EndDate);
        public static readonly string FIELD_SUCCESS = nameof(Success);
        public static readonly string FIELD_ACTIVE = nameof(Active);

        public int Id { get; set; }
        public string WorkflowId { get; set; }
        public string WorkflowName { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool Success { get; set; }
        public bool Active { get; set; }
    }
}