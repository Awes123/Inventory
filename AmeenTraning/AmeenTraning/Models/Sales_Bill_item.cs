//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AmeenTraning.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Sales_Bill_item
    {
        public int Sales_Bill_item_ID { get; set; }
        public Nullable<int> Sales_Bill_No { get; set; }
        public Nullable<int> Product_ID { get; set; }
        public Nullable<decimal> Quantity { get; set; }
        public string Unit { get; set; }
        public Nullable<decimal> Price { get; set; }
        public Nullable<decimal> Discount { get; set; }
        public Nullable<decimal> GST { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public string Lots_Group { get; set; }
        public Nullable<decimal> Total { get; set; }
        public Nullable<System.DateTime> Date_Created { get; set; }
        public Nullable<System.DateTime> Date_Modified { get; set; }
    
        public virtual Products_Details Products_Details { get; set; }
    }
}
