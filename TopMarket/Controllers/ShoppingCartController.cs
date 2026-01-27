using Microsoft.AspNet.Identity.Owin;
using System;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TopMarket.Models;
using TopMarket.Models.EntityFramework;
using TopMarket.Models.Payments;

namespace TopMarket.Controllers
{
	[Authorize]
	public class ShoppingCartController : Controller
	{
		private readonly ApplicationDbContext db = new ApplicationDbContext();
		private ApplicationSignInManager _signInManager;
		private ApplicationUserManager _userManager;

		public ShoppingCartController()
		{
		}

		public ShoppingCartController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
		{
			UserManager = userManager;
			SignInManager = signInManager;
		}

		public ApplicationSignInManager SignInManager
		{
			get { return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>(); }
			private set { _signInManager = value; }
		}

		public ApplicationUserManager UserManager
		{
			get { return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>(); }
			private set { _userManager = value; }
		}

		[AllowAnonymous]
		public ActionResult Index()
		{
			var cart = Session["Cart"] as ShoppingCart;
			if (cart != null && cart.Items.Any()) ViewBag.CheckCart = cart;
			return View();
		}

		public ActionResult VnpayReturn()
		{
			if (Request.QueryString.Count > 0)
			{
				var vnp_HashSecret = ConfigurationManager.AppSettings["vnp_HashSecret"];
				var vnpayData = Request.QueryString;
				var vnpay = new VnPayLibrary();

				foreach (string s in vnpayData)
				{
					if ((string.IsNullOrEmpty(s) == false)
						&& s.StartsWith("vnp_")) vnpay.AddResponseData(s, vnpayData[s]);
				}

				var orderCode = Convert.ToString(vnpay.GetResponseData("vnp_TxnRef"));
				var vnpayTranId = Convert.ToInt64(vnpay.GetResponseData("vnp_TransactionNo"));
				var vnp_ResponseCode = vnpay.GetResponseData("vnp_ResponseCode");
				var vnp_TransactionStatus = vnpay.GetResponseData("vnp_TransactionStatus");
				var vnp_SecureHash = Request.QueryString["vnp_SecureHash"];
				var terminalID = Request.QueryString["vnp_TmnCode"];
				var vnp_Amount = Convert.ToInt64(vnpay.GetResponseData("vnp_Amount")) / 100;
				var bankCode = Request.QueryString["vnp_BankCode"];

				var checkSignature = vnpay.ValidateSignature(vnp_SecureHash, vnp_HashSecret);
				if (checkSignature)
				{
					if ((vnp_ResponseCode == "00")
						&& (vnp_TransactionStatus == "00"))
					{
						var itemOrder = db.Orders.FirstOrDefault(x => x.Code == orderCode);
						if (itemOrder != null)
						{
							itemOrder.Status = 2; // Paid
							db.Orders.Attach(itemOrder);
							db.Entry(itemOrder).State = System.Data.Entity.EntityState.Modified;
							db.SaveChanges();
						}

						ViewBag.InnerText = "Thank you for using the service";
					}
					else
					{
						ViewBag.InnerText = "An error occurred. Error code: " + vnp_ResponseCode;
					}

					ViewBag.PaymentSuccess = "Payment amount: " + vnp_Amount.ToString() + " (VND)";
				}
			}

			return View();
		}

		[AllowAnonymous]
		public ActionResult CheckOut()
		{
			var cart = Session["Cart"] as ShoppingCart;
			if (cart != null && cart.Items.Any()) ViewBag.CheckCart = cart;
			return View();
		}

		[AllowAnonymous]
		public ActionResult CheckOutSuccess()
		{
			return View();
		}

		[AllowAnonymous]
		public ActionResult Partial_Item_Checkout()
		{
			var cart = Session["Cart"] as ShoppingCart;
			if (cart != null && cart.Items.Any()) return PartialView(cart.Items);
			return PartialView();
		}

		[AllowAnonymous]
		public ActionResult Partial_Item_Cart()
		{
			var cart = Session["Cart"] as ShoppingCart;
			if (cart != null && cart.Items.Any()) return PartialView(cart.Items);
			return PartialView();
		}

		[AllowAnonymous]
		public ActionResult ShowCount()
		{
			var cart = Session["Cart"] as ShoppingCart;
			if (cart != null) return Json(new { Count = cart.Items.Count }, JsonRequestBehavior.AllowGet);
			return Json(new { Count = 0 }, JsonRequestBehavior.AllowGet);
		}

		[AllowAnonymous]
		public ActionResult Partial_CheckOut()
		{
			var user = UserManager.FindByNameAsync(User.Identity.Name).Result;
			if (user != null) ViewBag.User = user;
			return PartialView();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult CheckOut(OrderViewModel req)
		{
			var code = new
			{
				Success = false,
				Code = -1,
				Url = string.Empty
			};

			if (ModelState.IsValid)
			{
				var cart = Session["Cart"] as ShoppingCart;
				if (cart != null)
				{
					var order = new Order
					{
						CustomerName = req.CustomerName,
						Phone = req.Phone,
						Address = req.Address,
						Email = req.Email,
						// 1: Unpaid
						// 2: Paid
						// 3: Complete
						// 4: Cancel
						Status = 1,
						TotalAmmount = cart.Items.Sum(x => x.Price * x.Quantity),
						PaymentMethods = req.PaymentMethods,
						DateCreated = DateTime.Now,
						DateModified = DateTime.Now,
						CreatedBy = req.Phone,
						Code = "OD" + new Random().Next(1000, 9999)
					};

					foreach (var x in cart.Items)
					{
						order.OrderDetails.Add(new OrderDetail
						{
							ProductId = x.ProductId,
							Quantity = x.Quantity,
							Price = x.Price
						});
					}

					db.Orders.Add(order);
					db.SaveChanges();
					cart.ClearCart();

					code = new
					{
						Success = true,
						Code = req.PaymentMethods,
						Url = string.Empty
					};

					if (req.PaymentMethods == 2)
					{
						var url = UrlPayment(req.TypePaymentVN, order.Code);
						code = new
						{
							Success = true,
							Code = req.PaymentMethods,
							Url = url
						};
					}
				}
			}

			return Json(code);
		}

		[HttpPost]
		[AllowAnonymous]
		public ActionResult AddToCart(int id, int quantity)
		{
			var code = new
			{
				Success = false,
				msg = string.Empty,
				code = -1,
				Count = 0
			};

			var product = db.Products.FirstOrDefault(x => x.Id == id);
			if (product != null)
			{
				var cart = Session["Cart"] as ShoppingCart ?? new ShoppingCart();
				var item = new ShoppingCartItem
				{
					ProductId = product.Id,
					ProductName = product.Title,
					CategoryName = product.ProductCategory.Title,
					Alias = product.Alias,
					Quantity = quantity,
					Price = product.PromotionPrice > 0
						? (decimal)product.PromotionPrice
						: product.Price,
					ProductImg = product.ProductImages.FirstOrDefault(x => x.IsDefault)?.Image
				};

				item.TotalPrice = item.Quantity * item.Price;
				cart.AddToCart(item, quantity);
				Session["Cart"] = cart;

				code = new
				{
					Success = true,
					msg = "Added to Cart",
					code = 1,
					Count = cart.Items.Count
				};
			}

			return Json(code);
		}

		[HttpPost]
		[AllowAnonymous]
		public ActionResult Update(int id, int quantity)
		{
			var cart = Session["Cart"] as ShoppingCart;
			if (cart != null)
			{
				cart.UpdateQuantity(id, quantity);
				return Json(new { Success = true });
			}

			return Json(new { Success = false });
		}

		[HttpPost]
		[AllowAnonymous]
		public ActionResult Delete(int id)
		{
			var code = new
			{
				Success = false,
				msg = string.Empty,
				code = -1,
				Count = 0
			};

			var cart = Session["Cart"] as ShoppingCart;
			if (cart != null)
			{
				var checkProduct = cart.Items.FirstOrDefault(x => x.ProductId == id);
				if (checkProduct != null)
				{
					cart.Remove(id);
					code = new
					{
						Success = true,
						msg = "Removed from Cart",
						code = 1,
						Count = cart.Items.Count
					};
				}
			}

			return Json(code);
		}

		[HttpPost]
		[AllowAnonymous]
		public ActionResult DeleteAll()
		{
			var cart = Session["Cart"] as ShoppingCart;
			if (cart != null)
			{
				cart.ClearCart();
				return Json(new { Success = true });
			}

			return Json(new { Success = false });
		}

		#region VNPAY Payment
		public string UrlPayment(int typePaymentVN, string orderCode)
		{
			var order = db.Orders.FirstOrDefault(x => x.Code == orderCode);
			if (order == null) return string.Empty;

			var vnp_Returnurl = ConfigurationManager.AppSettings["vnp_Returnurl"];
			var vnp_Url = ConfigurationManager.AppSettings["vnp_Url"];
			var vnp_TmnCode = ConfigurationManager.AppSettings["vnp_TmnCode"];
			var vnp_HashSecret = ConfigurationManager.AppSettings["vnp_HashSecret"];

			var vnpay = new VnPayLibrary();
			var price = (long)order.TotalAmmount * 100;

			vnpay.AddRequestData("vnp_Version", VnPayLibrary.VERSION);
			vnpay.AddRequestData("vnp_Command", "pay");
			vnpay.AddRequestData("vnp_TmnCode", vnp_TmnCode);
			vnpay.AddRequestData("vnp_Amount", price.ToString());

			if (typePaymentVN == 1)
			{
				vnpay.AddRequestData("vnp_BankCode", "VNPAYQR");
			}
			else if (typePaymentVN == 2)
			{
				vnpay.AddRequestData("vnp_BankCode", "VNBANK");
			}
			else if (typePaymentVN == 3)
			{
				vnpay.AddRequestData("vnp_BankCode", "INTCARD");
			}

			vnpay.AddRequestData("vnp_CreateDate", order.DateCreated.ToString("yyyyMMddHHmmss"));
			vnpay.AddRequestData("vnp_CurrCode", "VND");
			vnpay.AddRequestData("vnp_IpAddr", Utils.GetIpAddress());
			vnpay.AddRequestData("vnp_Locale", "vn");
			vnpay.AddRequestData("vnp_OrderInfo", "Payment order: " + order.Code);
			vnpay.AddRequestData("vnp_OrderType", "other");
			vnpay.AddRequestData("vnp_ReturnUrl", vnp_Returnurl);
			vnpay.AddRequestData("vnp_TxnRef", order.Code);

			string urlPayment = vnpay.CreateRequestUrl(vnp_Url, vnp_HashSecret);
			return urlPayment;
		}
		#endregion
	}
}
