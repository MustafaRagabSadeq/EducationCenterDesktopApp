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
    
    public partial class Teacher_Year
    {
        public int Teacher_Year_ID { get; set; }
        public int Teacher_ID { get; set; }
        public int AcademicYear_ID { get; set; }
    
        public virtual AcademicYear AcademicYear { get; set; }
        public virtual Teacher Teacher { get; set; }
    }
}
