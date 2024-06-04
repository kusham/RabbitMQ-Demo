using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductBilling.API.Models
{
    public class Billing
    {
        [Key]
        public Guid Id { get; set; }
        public decimal Amount { get; set; }
        public DateTime CreatedAt { get; set; }
        public required string CustomerName { get; set; }
        public string? CustomerEmail { get; set; }
        public required string BillingAddress { get; set; }
        public BillingStatus Status { get; set; } = BillingStatus.Pending;

        public PaymentMethod PaymentMethod { get; set; } = PaymentMethod.CreditCard;
        public DateTime? PaymentDate { get; set; }
        public DateTime? DueDate { get; set; }
        public string? Notes { get; set; }
        public Currency Currency { get; set; } = Currency.USD;

        [NotMapped]
        public string InvoiceNumber
        {
            get
            {
                string datePart = CreatedAt.ToString("yyyyMMdd");
                string namePart = CustomerName.Length > 3 ? CustomerName.Substring(0, 3).ToUpper() : CustomerName.ToUpper();
                string amountPart = ((int)(Amount * 100)).ToString();
                return $"{datePart}-{namePart}-{amountPart}";
            }
        }

    }
    public enum BillingStatus
    {
        Pending,
        Paid,
        Overdue,
        Cancelled
    }
    public enum PaymentMethod
    {
        CreditCard,
        PayPal,
        BankTransfer,
        Cash,
        Check,
        Other
    }
    public enum Currency
    {
        USD,
        EUR,
        GBP,
        INR,
        JPY,
        CNY,
        AUD,
        CAD,
        Other
    }

}
