using System.Web.Mvc;
using System.Web.Routing;

namespace TopMarket
{
	public class RouteConfig
	{
		public static void RegisterRoutes(RouteCollection routes)
		{
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

			routes.MapRoute(
				name: "Contact",
				url: "contact",
				defaults: new { controller = "Contact", action = "Index", id = UrlParameter.Optional },
				namespaces: new[] { "TopMarket.Controllers" }
			);
			routes.MapRoute(
				name: "CheckOut",
				url: "checkout",
				defaults: new { controller = "ShoppingCart", action = "CheckOut", id = UrlParameter.Optional },
				namespaces: new[] { "TopMarket.Controllers" }
			);
			routes.MapRoute(
				name: "vnpay_return",
				url: "vnpay-return",
				defaults: new { controller = "ShoppingCart", action = "VnpayReturn", id = UrlParameter.Optional },
				namespaces: new[] { "TopMarket.Controllers" }
			);
			routes.MapRoute(
				name: "ShoppingCart",
				url: "shopping-cart",
				defaults: new { controller = "ShoppingCart", action = "Index", id = UrlParameter.Optional },
				namespaces: new[] { "TopMarket.Controllers" }
			);
			routes.MapRoute(
				name: "ProductCategory",
				url: "product-category/{alias}-{id}",
				defaults: new { controller = "Products", action = "ProductCategory", id = UrlParameter.Optional },
				namespaces: new[] { "TopMarket.Controllers" }
			);
			routes.MapRoute(
				name: "ProductDetail",
				url: "product-detail/{alias}-p{id}",
				defaults: new { controller = "Products", action = "Detail", id = UrlParameter.Optional },
				namespaces: new[] { "TopMarket.Controllers" }
			);
			routes.MapRoute(
				name: "Products",
				url: "product",
				defaults: new { controller = "Products", action = "Index", id = UrlParameter.Optional },
				namespaces: new[] { "TopMarket.Controllers" }
			);
			routes.MapRoute(
				name: "BlogDetail",
				url: "{alias}-n{id}",
				defaults: new { controller = "Blog", action = "Detail", id = UrlParameter.Optional },
				namespaces: new[] { "TopMarket.Controllers" }
			);
			routes.MapRoute(
				name: "BlogList",
				url: "blog",
				defaults: new { controller = "Blog", action = "Index", alias = UrlParameter.Optional },
				namespaces: new[] { "TopMarket.Controllers" }
			);
			routes.MapRoute(
				name: "Default",
				url: "{controller}/{action}/{id}",
				defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
				namespaces: new[] { "TopMarket.Controllers" }
			);
		}
	}
}
