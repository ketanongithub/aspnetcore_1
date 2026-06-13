using System;

namespace ENyayPath.PICS.Application.Wallet.Dtos
{
    public class WalletDto
    {
        public Guid WalletId { get; set; }
        public Guid PrisonerId { get; set; }
        public decimal BalanceAmount { get; set; }
    }
}
