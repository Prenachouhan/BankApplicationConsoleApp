using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankApplication.Entities
{
    public class Transaction
    {
        [Key]
        public int TransactionId { get; set; }
        [ForeignKey("Account")]
        public int AccountId { get; set; }
        public Account Account { get; set; }
        [Column("transactionType", TypeName = "varchar(250)")]
        public string TransactionType { get; set; }
        [Column("amount", TypeName = "float")]
        public double Amount { get; set; }
        [Column("transactionDate", TypeName = "datetime")]
        public DateTime TransactionDate { get; set; }
        [Column("remarks", TypeName = "varchar(250)")]
        public string Remarks { get; set; }

        [Column("balance", TypeName = "float")]
        public double? Balance { get; set; }
            
    }
}
