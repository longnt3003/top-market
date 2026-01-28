using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TopMarket.Models
{
    public class OrderViewModel
    {
        [Required(ErrorMessage = "Customer name cannot be blank")]
        public string CustomerName { get; set; }

        [Required(ErrorMessage = "Phone number cannot be blank")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Address cannot be blank")]
        public string Address { get; set; }
        public string Email { get; set; }
        public string CustomerId { get; set; }
        public int PaymentMethods { get; set; }
        public int TypePaymentVN { get; set; }
    }
}