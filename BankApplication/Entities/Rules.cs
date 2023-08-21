using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankApplication.Entities
{
    public class Rules
    {
        [Key]
        public int RuleId { get; set; }
       
        [Column("ruleName", TypeName = "varchar(250)")]
        public string RuleName { get; set; }
        [Column("interest", TypeName = "float")]
        public double Interest { get; set; }
        [Column("ruleDate", TypeName = "datetime")]
        public DateTime RuleDate { get; set; }
    }
}
