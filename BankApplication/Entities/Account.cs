using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankApplication.Entities
{
    public class Account
    {
        [Key]
        public int AccountId { get; set; }
        [Column("accountInfo", TypeName = "varchar(250)")]
        public string AccountInfo { get; set; }

        public ICollection<Transaction> Transactions { get; set; }
    }
}
