using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotnetCoreProject.Models
{
    public class CartItem
    {

        public int productId { get; set; }
        public string productName { get; set; }

        public int quantity { get; set; }
        public decimal price { get; set; }
        public decimal total { get { return quantity* price; } }
        public string image { get; set; }

        public CartItem() { }

        public CartItem(Product product)
        {
            this.productId = product.Id;
            this.productName = product.Name;
            this.price = product.Price;
            this.quantity = 1;
            this.image = product.Image;
        }

    }
}
