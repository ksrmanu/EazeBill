using System;
using System.Collections.Generic;

namespace StoreBilling.Models.ViewModels
{
    public class AccountsViewModel
    {
        public decimal TotalReceipts { get; set; }
        public decimal TotalPayments { get; set; }
        public decimal TotalExpenses { get; set; }
        public List<TransactionDto> Transactions { get; set; }
    }

    public class TransactionDto
    {
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public decimal Amount { get; set; }
    }
}