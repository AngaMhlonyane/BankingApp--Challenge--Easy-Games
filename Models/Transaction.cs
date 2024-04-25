using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BankingApp__easygames.Models
{
    public class Transaction
    {

        [Key]
        public int TransactionID { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public int TransactionTypeID { get; set; }

        [Required]
        public int ClientID { get; set; }

        [StringLength(100)]
       
        public string Comment { get; set; }
        
        [ForeignKey(nameof(TransactionTypeID))]
        public TransactionType TransactionType { get; set; }

        [ForeignKey(nameof(ClientID))]
        public Client Client { get; set; }


    }
}