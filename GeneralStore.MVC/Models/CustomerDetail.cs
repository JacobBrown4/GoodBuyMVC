using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GeneralStore.MVC.Models
{
    public class CustomerDetail
    {
        public int CustomerId { get; set; }
        [Display(Name = "Full Name")]
        public string FullName { get; set; }
        public List<Transaction> Transactions { get; set; } = new List<Transaction>();
    }
}