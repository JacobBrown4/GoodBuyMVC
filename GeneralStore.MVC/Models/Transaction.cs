using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace GeneralStore.MVC.Models
{
    public class Transaction
    {
        [Key]
        [Display(Name = "Transaction Id")]
        public int TransactionId { get; set; }
        [Required]
        [ForeignKey("Customer")]
        [Display(Name = "Customer")]
        public int CustomerId { get; set; }
        public virtual Customer Customer {get; set; }
        [Required]
        [Display(Name = "Product")]
        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }
        [Required]
        [Display(Name = "Item Count")]
        public int ItemCount { get; set; }
        [Display(Name = "Time of Transaction")]
        [Required]
        public DateTime TimeOfPurchase { get; set; }
    }
}