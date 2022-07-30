using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotnetCoreProject.Models
{
    public class CartViewModel
    {
        public List<CartItem> cartItems { get; set; }
        public decimal GrandTotal { get; set; }
    }
}
