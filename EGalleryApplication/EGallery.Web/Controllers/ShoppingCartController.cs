using EGallery.Domain.DomainModels;
using EGallery.Domain.DTO;
using EGallery.Domain.Identity;
using EGallery.Repository;
using EGallery.Services.Implementation;
using EGallery.Services.Interface;
using GemBox.Document;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EGallery.Web.Controllers
{
    public class ShoppingCartController : Controller
    {
        
        private readonly IShoppingCartService _shoppingCartService;
        public ShoppingCartController(IShoppingCartService shoppingCartService)
        {
            _shoppingCartService = shoppingCartService;
        }
        public IActionResult Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return View(this._shoppingCartService.getShoppingCartInfo(userId));
        }

        public IActionResult DeleteProductFromShoppingCart(Guid productId)
        {
            //delete

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = this._shoppingCartService.deleteProductFromShoppingCart(userId, productId);

            return RedirectToAction("Index", "ShoppingCart");
        }

        public IActionResult ViewProduct (Guid productId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = this._shoppingCartService.getProduct(userId, productId);

            return View(result);
        }

        public IActionResult SubmitOrder()
        {

            return RedirectToAction("ShowInvoice", "ShoppingCart");
        }

        public FileContentResult ShowInvoice(MemoryStream stream)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = this._shoppingCartService.orderNow(userId);

            return File(result.ToArray(), new PdfSaveOptions().ContentType, "ExportInvoice.pdf");
        }
    }
}
