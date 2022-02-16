using EGallery.Domain.DomainModels;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EGallery.Domain.DTO
{
    public class ShoppingCartDto
    {
        public List<ProductInShoppingCart> ProductInShoppingCarts { get; set; }
        public double TotalPrice { get; set; }
    }
}
