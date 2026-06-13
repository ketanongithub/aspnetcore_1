using System;

namespace ENyayPath.PICS.Application.Recharge.Dtos
{
    public class RechargeDto
    {
        public Guid TransactionId { get; set; }
        public Guid PrisonerId { get; set; }
        public int RechargeType { get; set; }
        public decimal Amount { get; set; }
        public string? ReferenceNumber { get; set; }
        public DateTime TransactionDate { get; set; }
    }
}
