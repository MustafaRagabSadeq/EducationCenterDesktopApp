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
    
    public partial class Schedule
    {
        public int Schedule_ID { get; set; }
        public Nullable<int> Instructor_ID { get; set; }
        public Nullable<int> Teacher_ID { get; set; }
        public Nullable<int> Group_ID { get; set; }
        public Nullable<int> Subject_ID { get; set; }
        public int Room_ID { get; set; }
        public System.DateTime date { get; set; }
        public Nullable<System.TimeSpan> Start_Time { get; set; }
        public Nullable<System.TimeSpan> End_Time { get; set; }
        public string week { get; set; }
    
        public virtual GroupName GroupName { get; set; }
        public virtual Instructor Instructor { get; set; }
        public virtual Room Room { get; set; }
        public virtual Subject Subject { get; set; }
        public virtual Teacher Teacher { get; set; }
    }
}