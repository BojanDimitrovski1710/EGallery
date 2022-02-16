using EGallery.Domain.DomainModels;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EGallery.Domain.DTO
{
    public class AddToShoppingCartDto
    {
        public Product SelectedProduct { get; set; }
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        //public string Image { get; set; }
    }
}
