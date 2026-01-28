using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TopMarket.Models
{
    public abstract class CommonAbstract
    {
        public string CreatedBy { get; set; }

        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
    }
}