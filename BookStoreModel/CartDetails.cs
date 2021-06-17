using System;
using System.Collections.Generic;
using System.Text;

namespace BookStoreModel
{
    public class CartDetails
    {
        public int BookID { get; set; }
        public int CartID { get; set; }
        public string BookName { get; set; }
        public string Author { get; set; }
        public int Price { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public string Image { get; set; }
    }
}
