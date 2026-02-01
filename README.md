# TopMarket - E-commerce Website

**Online marketplace** for fresh organic food built with **ASP.NET MVC 5** (.NET Framework), Entity
Framework 6, and Bootstrap-based frontend.\
Includes user-friendly shopping experience + complete admin management panel

------------------------------------------------------------------------

## Tech Stack

### Backend

-   ASP.NET MVC 5 (.NET Framework 4.7.2 or similar)
-   Entity Framework 6 (Code-First)
-   ASP.NET Identity (authentication & roles)
-   OWIN & Microsoft.Owin packages
-   VnPay payment gateway integration

### Frontend

-   Bootstrap 4/5 (mixed with custom assets)
-   jQuery, Owl Carousel, Font Awesome
-   CKEditor + CKFinder
-   AdminLTE-inspired admin dashboard
-   Razor Views (.cshtml)

### Tools & Others

-   Visual Studio
-   SQL Server / LocalDB
-   Git & GitHub

------------------------------------------------------------------------

## Project Structure

``` plaintext
TopMarket/
├── App_Start/                    # Configuration (routes, bundles, auth, identity)
│   ├── BundleConfig.cs
│   ├── FilterConfig.cs
│   ├── IdentityConfig.cs
│   ├── RouteConfig.cs
│   └── Startup.Auth.cs
│
├── Controllers/                  # Frontend controllers
│   ├── AccountController.cs
│   ├── BlogController.cs
│   ├── ContactController.cs
│   ├── HomeController.cs
│   ├── ManageController.cs
│   ├── MenuController.cs
│   ├── ProductsController.cs
│   ├── ReviewController.cs
│   ├── ShoppingCartController.cs
│   └── WishlistController.cs
│
├── Models/                       # Entities, ViewModels, helpers
│   ├── EntityFramework/          # EF entities
│   ├── Payments/
│   │   └── VnPayLibrary.cs
│   └── ViewModels, CommonAbstract...
│
├── Views/                        # Razor views
│   ├── Account/
│   ├── Blog/
│   ├── Contact/
│   ├── Home/
│   ├── Manage/
│   ├── Menu/
│   ├── Products/
│   ├── Review/
│   ├── ShoppingCart/
│   ├── Wishlist/
│   └── Shared/
│
├── Areas/
│   └── Admin/                    # Admin panel
│       ├── Controllers/
│       └── Views/
│
├── Common/                       # Helpers & utilities
├── DataSeed/                     # Sample data seeders
├── Content/                      # CSS, JS, images, plugins
├── Scripts/                      # JavaScript libraries
├── Uploads/                      # Uploaded images & files
├── Migrations/                   # EF migrations (if present)
└── Web.config
```

------------------------------------------------------------------------

## Features

### Customer

-   User registration, login, profile management (ASP.NET Identity)
-   Product catalog with categories
-   Product details, reviews & ratings
-   Shopping cart & wishlist
-   Checkout with VnPay online payment
-   Blog section
-   Responsive UI (Bootstrap + custom theme)

### Admin Panel (Areas/Admin)

-   CRUD: products, categories, orders, blogs, users, roles
-   Order & status management
-   System settings management
-   Rich text editor (CKEditor + CKFinder)
-   Role-based authorization

------------------------------------------------------------------------

## Prerequisites

-   Visual Studio 2019 / 2022\
-   .NET Framework 4.7.2 or higher\
-   SQL Server Express / LocalDB\
-   IIS Express or local IIS\
-   Modern browser (Chrome / Edge / Firefox)

------------------------------------------------------------------------

## Getting Started

### Clone Repository

``` bash
git clone https://github.com/longnt3003/top-market.git
cd top-market
```

### Setup & Run

1.  Open the project in Visual Studio.

2.  Restore NuGet packages:

    ``` powershell
    Update-Package -reinstall
    ```

3.  Configure connection string in `Web.config`.

4.  Run EF migrations / seed data:

    ``` powershell
    Update-Database
    ```

5.  Run the project (F5 or Ctrl+F5).

-   Customer site: `http://localhost:{port}`\
-   Admin panel: `http://localhost:{port}/Admin`

### Default Admin Account (if seeded)

-   **Email**: admin@gmail.com\
-   **Password**: admin123\

------------------------------------------------------------------------

## Useful Commands

  Command                       Description
  ----------------------------- ------------------------------------
  `Update-Package -reinstall`   Restore / reinstall NuGet packages
  `Update-Database`             Apply EF migrations
  `Add-Migration <name>`        Create new EF migration
  `F5` (Visual Studio)          Build & run the application

------------------------------------------------------------------------

## License

MIT License -- see the `LICENSE` file for details.

------------------------------------------------------------------------

## Author

**Nguyễn Thành Long**\
GitHub: [@longnt3003](https://github.com/longnt3003)\
Ho Chi Minh City, Vietnam

If you find this project useful, please give it a ⭐ or fork it.\
Happy coding!
