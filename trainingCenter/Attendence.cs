//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace trainingCenter
{
    using System;
    using System.Collections.Generic;
    
    public partial class Attendence
    {
        public int St_Att_ID { get; set; }
        public int St_ID { get; set; }
        public int G_ID { get; set; }
        public System.DateTime Att_Date { get; set; }
        public bool Payment_State { get; set; }
    
        public virtual Student Student { get; set; }
        public virtual GroupName GroupName { get; set; }
    }
}
