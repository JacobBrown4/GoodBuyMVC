using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GeneralStore.MVC.Models
{
    public class ProductDetail
    {

        public int ProductId { get; set; }
        [Display(Name = "Product Name")]
        public string Name { get; set; }
        [Display(Name = "# In Stock")]
        public int InventoryCount { get; set; }
        public decimal Price { get; set; }
        [Display(Name = "It is food")]
        public bool IsFood { get; set; }
        public List<Transaction> Transactions { get; set; } = new List<Transaction>();
    }
}