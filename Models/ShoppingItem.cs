using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace shoppinglist.Models
{
    public class ShoppingItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int qte { get; set; }
        
        public  DateTime CreatedAt { get; set; }
    }
}
