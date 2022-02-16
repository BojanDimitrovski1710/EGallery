using EGallery.Domain.DomainModels;
using EGallery.Domain.DTO;
using EGallery.Repository.Interface;
using EGallery.Services.Interface;
using GemBox.Document;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace EGallery.Services.Implementation
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly IRepository<ShoppingCart> _shoppingCartRepositorty;
        private readonly IRepository<Order> _orderRepositorty;
        private readonly IRepository<ProductInOrder> _productInOrderRepositorty;
        private readonly IUserRepository _userRepository;
        private readonly IRepository<EmailMessage> _mailRepository;

        public ShoppingCartService(IRepository<ShoppingCart> shoppingCartRepository, IRepository<ProductInOrder> productInOrderRepositorty, IRepository<Order> orderRepositorty, IUserRepository userRepository, IRepository<EmailMessage> mailRepository)
        {
            _shoppingCartRepositorty = shoppingCartRepository;
            _userRepository = userRepository;
            _orderRepositorty = orderRepositorty;
            _productInOrderRepositorty = productInOrderRepositorty;
            _mailRepository = mailRepository;
        }

        public bool deleteProductFromShoppingCart(string userId, Guid id)
        {
            if (!string.IsNullOrEmpty(userId) && id != null)
            {
                //Select * from Users Where Id LIKE userId

                var loggedInUser = this._userRepository.Get(userId);

                var userShoppingCart = loggedInUser.UserCart;

                var itemToDelete = userShoppingCart.ProductsInCart.Where(z => z.Product.Id.Equals(id)).FirstOrDefault();

                userShoppingCart.ProductsInCart.Remove(itemToDelete);

                this._shoppingCartRepositorty.Update(userShoppingCart);

                return true;
            }

            return false;
        }

        public ProductInShoppingCart getProduct(string userId, Guid id)
        {
            if (!string.IsNullOrEmpty(userId) && id != null)
            {
                //Select * from Users Where Id LIKE userId

                var loggedInUser = this._userRepository.Get(userId);

                var userShoppingCart = loggedInUser.UserCart;

                var itemToDisplay = userShoppingCart.ProductsInCart.Where(z => z.Product.Id.Equals(id)).FirstOrDefault();

                return itemToDisplay;
            }

            return null;
        }

        public ShoppingCartDto getShoppingCartInfo(string userId)
        {
            var loggedInUser = this._userRepository.Get(userId);

            var userShoppingCart = loggedInUser.UserCart;

            var AllProducts = userShoppingCart.ProductsInCart.ToList();

            var allProductPrice = AllProducts.Select(z => new
            {
                ProductPrice = z.Product.Price,
                Quanitity = z.Quantity
            }).ToList();

            var totalPrice = 0;


            foreach (var item in allProductPrice)
            {
                totalPrice += item.Quanitity * item.ProductPrice;
            }


            ShoppingCartDto scDto = new ShoppingCartDto
            {
                ProductInShoppingCarts = AllProducts,
                TotalPrice = totalPrice
            };


            return scDto;

        }

        public MemoryStream orderNow(string userId)
        {
            if (!string.IsNullOrEmpty(userId))
            {
                //Select * from Users Where Id LIKE userId

                var loggedInUser = this._userRepository.Get(userId);

                var userShoppingCart = loggedInUser.UserCart;

                EmailMessage mail = new EmailMessage();
                mail.MailTo = loggedInUser.Email;
                mail.Subject = "Successfully created order";
                mail.Status = false;

                Order order = new Order
                {
                    Id = Guid.NewGuid(),
                    User = loggedInUser,
                    UserId = userId
                };

                this._orderRepositorty.Insert(order);

                List<ProductInOrder> productInOrders = new List<ProductInOrder>();

                var result = userShoppingCart.ProductsInCart.Select(z => new ProductInOrder
                {
                    Id = Guid.NewGuid(),
                    ProductId = z.Product.Id,
                    SelectedProduct = z.Product,
                    OrderId = order.Id,
                    UserOrder = order,
                    Quantity = z.Quantity
                }).ToList();

                StringBuilder sb = new StringBuilder();

                var totalPrice = 0;

                sb.AppendLine("Your order is completed. The order conains: ");

                for (int i = 1; i <= result.Count(); i++)
                {
                    var item = result[i - 1];

                    totalPrice += item.Quantity * item.SelectedProduct.Price;

                    sb.AppendLine(i.ToString() + ". " + item.SelectedProduct.Name + " with price of: " + item.SelectedProduct.Price + "$ and Quantity: " + item.Quantity);
                }

                sb.AppendLine("Total price: " + totalPrice.ToString());


                mail.Content = sb.ToString();


                productInOrders.AddRange(result);

                foreach (var item in productInOrders)
                {
                    this._productInOrderRepositorty.Insert(item);
                }

                var templatePath = Path.Combine(Directory.GetCurrentDirectory(), "Invoice.docx");

                ComponentInfo.SetLicense("FREE-LIMITED-KEY");
                var template = DocumentModel.Load(templatePath);

                template.Content.Replace("{{OrderNumber}}", order.Id.ToString());

                template.Content.Replace("{{UserName}}", order.User.UserName.ToString());

                StringBuilder productsList = new StringBuilder();
                for (int i = 1; i <= result.Count(); i++)
                {
                    var item = result[i - 1];

                    productsList.AppendLine(i.ToString() + ") " + item.SelectedProduct.Name + " with price of: " + item.SelectedProduct.Price + "$ and Quantity: " + item.Quantity);
                }

                template.Content.Replace("{{ProductsList}}", productsList.ToString());

                template.Content.Replace("{{TotalPrice}}", totalPrice.ToString() + "$");

                var stream = new MemoryStream();

                template.Save(stream, new PdfSaveOptions());

                loggedInUser.UserCart.ProductsInCart.Clear();

                this._userRepository.Update(loggedInUser);
                this._mailRepository.Insert(mail);
                return stream;
            }
            return null;
        }
    }
}
