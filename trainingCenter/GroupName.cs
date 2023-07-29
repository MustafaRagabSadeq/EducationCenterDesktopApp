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
    
    public partial class GroupName
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public GroupName()
        {
            this.Attendences = new HashSet<Attendence>();
            this.Schedules = new HashSet<Schedule>();
            this.Student_Group = new HashSet<Student_Group>();
        }
    
        public int G_ID { get; set; }
        public string G_Name { get; set; }
        public Nullable<int> G_NoOfSession { get; set; }
        public Nullable<double> G_PriceOfSession { get; set; }
        public Nullable<int> G_Capacity { get; set; }
        public Nullable<System.DateTime> G_DateOFCreation { get; set; }
        public Nullable<int> Teacher_ID { get; set; }
        public double G_TotalPrice { get; set; }
        public string Grade { get; set; }
        public Nullable<int> AcademicYear_ID { get; set; }
        public Nullable<int> Sub_ID { get; set; }
    
        public virtual AcademicYear AcademicYear { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Attendence> Attendences { get; set; }
        public virtual Subject Subject { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Schedule> Schedules { get; set; }
        public virtual Teacher Teacher { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Student_Group> Student_Group { get; set; }
        public override string ToString()
        {
            return G_ID.ToString();
        }
    }
}
