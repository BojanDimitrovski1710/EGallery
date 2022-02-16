using EGallery.Domain.DomainModels;
using EGallery.Domain.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace EGallery.Domain.DTO
{
    public class ProductDTO
    {
        public EGalleryApplicationUser loggedInUser { get; set; }
        public List<Product> Products { get; set; }
    }
}
