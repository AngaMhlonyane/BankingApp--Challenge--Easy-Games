using System.ComponentModel.DataAnnotations;

namespace BankingApp__easygames.Models
{
    public class Client
    {
        [Key]
        public int ClientID { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        [StringLength(50)]
        public string Surname { get; set; }

        [Required]
        public decimal ClientBalance { get; set; }

        public  List<Transaction> Transactions { get; set;}
    }

}