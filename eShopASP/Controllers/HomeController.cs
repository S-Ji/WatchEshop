using eShopASP.DAL;
using eShopASP.Models.Home;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eShopASP.Controllers
{
    public class HomeController : Controller
    {
        dbMyOnlineShoppingEntities ctx = new dbMyOnlineShoppingEntities();
        public ActionResult Index(string search, int? page)
        {
            HomeIndexViewModel model = new HomeIndexViewModel();
            return View(model.CreateModel(search, 4, page));
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult AddToCart(int productId)
        {
            if (Session["cart"] == null)
            {
                List<Item> cart = new List<Item>();
                var product = ctx.Tbl_Product.Find(productId);
                cart.Add(new Item()
                {
                    Product = product,
                    Quantity = 1
                });
                Session["cart"] = cart;
                
            } else
                {
                List<Item> cart = (List<Item>)Session["cart"];

                var product = ctx.Tbl_Product.Find(productId);
                List<int> addedItems = new List<int>();
                foreach (var item in cart)
                {
                    addedItems.Add(item.Product.ProductId);
                }
                foreach (var item in cart.ToList())
                {
                    if (item.Product.ProductId == productId)
                    {
                        int prevQty = item.Quantity;
                        cart.Remove(item);
                        cart.Add(new Item()
                        {
                            Product = product,
                            Quantity = prevQty + 1
                        });
                        
                        break;
                    }
                    else
                    {
                        if (addedItems.Contains(productId))
                        {

                        } else
                        {
                            cart.Add(new Item()
                            {
                                Product = product,
                                Quantity = 1
                            });

                            break;
                        }
                    }
                }
                Session["cart"] = cart;
                }
            return Redirect("Index");
        }
        public ActionResult RemoveFromCart(int productId)
        {
            List<Item> cart = (List<Item>)Session["cart"];
            foreach (var item in cart)
            {
                if (item.Product.ProductId == productId)
                {
                    cart.Remove(item);
                    break;
                }
            }
            Session["cart"] = cart;
            return Redirect("Index");
        }
        public ActionResult Checkout()
        {
            return View();
        }

        public ActionResult CheckoutDetails()
        {
            return View();
        }

        public ActionResult DecreaseQty(int productId)
        {
            if(Session["cart"] != null)
            {
                List<Item> cart = (List<Item>)Session["cart"];
                var product = ctx.Tbl_Product.Find(productId);
                foreach (var item in cart)
                {
                    if(item.Product.ProductId == productId)
                    {

                    
                        int prevQty = item.Quantity;
                        if(prevQty > 0)
                        {
                            cart.Remove(item);
                            cart.Add(new Item()
                            {
                            Product = product,
                            Quantity = prevQty - 1
                            });
                        }
                        break;
                    }
                }
                Session["cart"] = cart;
            }
            return Redirect("Checkout");
        }
    }
}