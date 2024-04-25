namespace BankingApp__easygames.Models
{
    public class TransactionType
    {
        public int TransactionTypeID { get; set; }
        public TransactionTypeName TransactionTypeName { get; set; }
    }

   public enum TransactionTypeName
    { debit,
      credit,
    }


}
