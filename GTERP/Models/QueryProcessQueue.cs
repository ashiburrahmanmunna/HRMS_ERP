using Microsoft.VisualBasic;
using System;
using System.ComponentModel.DataAnnotations;

namespace GTERP.Models
{
    public class QueryProcessQueue
    {
        public int Id { get; set; }
        public int IsExecuted { get; set; } = 0;
        [Required]
        public string Query { get; set; }
        [Required]
        public string ComId { get; set; }
        public string? ExcuteById { get; set; }
        public DateTime RequestedAt { get; set; } = DateTime.Now;
        public DateTime? PrcStarted { get; set; }
        public DateTime? RequestFrom { get; set; }
        public DateTime? RequestTo { get; set; }
        public DateTime? PrcEnd { get; set; }
        public string Type { get; set; }
        public string ErrorLog { get; set; }

    }
    public class QueryProcessQueueVm
    {
        public int Id { get; set; }
        public int IsExecuted { get; set; } = 0;
        public string RequestFrom { get; set; }
        public string RequestTo { get; set; }
        public string Requesteddate { get; set; }
        public string Requestetime { get; set; }
        public string PrcstDate { get; set; }
        public string PrcstTime { get; set; }
        public string PrcedDate { get; set; }
        public string PrcedTime { get; set; }
    }
}
