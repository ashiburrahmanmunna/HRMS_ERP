using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GTERP.Models
{
    public class ButtonPermission
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string ComId { get; set; }
        public bool IsShowEarn { get; set; }
        public bool IsShowSettlement { get; set; }
        public bool IsShowSendEmail { get; set; }
    }
}
