using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TopMarket.Models.EntityFramework
{
    [Table("tb_MenuCategory")]
    public class MenuCategory : CommonAbstract
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required(ErrorMessage = "Category Name cannot be blank!")]
        [StringLength(150)]
        public string Title { get; set; } 
        public string Alias { get; set; }
        public string Link { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public int Position { get; set; }
    }
}