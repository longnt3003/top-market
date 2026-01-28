using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TopMarket.Models.EntityFramework
{
    [Table("tb_Order")]
    public class Order : CommonAbstract
    {
        public Order()
        {
            this.OrderDetails = new HashSet<OrderDetail>();
        }
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string Code { get; set; }
        [Required(ErrorMessage = "Customer name cannot be blank")]
        public string CustomerName { get; set; }
        [Required(ErrorMessage = "Phone number cannot be blank")]
        public string Phone { get; set; }
        [Required(ErrorMessage = "Address cannot be blank")]
        public string Address { get; set; }
        public string Email { get; set; }
        public decimal TotalAmmount { get; set; }
        public int Quantity { get; set; }
        public int PaymentMethods { get; set; }
        public int Status { get; set; }
        public string CustomerId { get; set; }

        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}