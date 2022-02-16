using EGallery.Domain.DomainModels;
using EGallery.Domain.DTO;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace EGallery.Services.Interface
{
    public interface IShoppingCartService
    {
        ShoppingCartDto getShoppingCartInfo(string userId);
        bool deleteProductFromShoppingCart(string userId, Guid id);
        MemoryStream orderNow(string userId);

        ProductInShoppingCart getProduct(string userId, Guid id);
    }
}
