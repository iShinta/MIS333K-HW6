using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Ho_MinhTri_HW6.Models
{
   
    public class Customer
    {
        public Int32 CustomerID { get; set; }

        [Display(Name = "First Name")]
        public String FirstName { get; set; }

        [Display(Name = "Last Name")]
        public String LastName { get; set; }

        public String Gender { get; set; }

        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage ="Please enter a valid email address")]
        public String Email { get; set; }

        [Display(Name = "Average Sale")]
        public Decimal AverageSale { get; set; }
       
        [Display(Name ="Referred From")]
        public String ReferredFrom { get; set; }

        //Navigational property to frequency
        public virtual Frequency Frequency { get; set; }

    }
}