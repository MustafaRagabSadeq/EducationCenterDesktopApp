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
    
    public partial class Student
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Student()
        {
            this.Attendences = new HashSet<Attendence>();
            this.Student_Group = new HashSet<Student_Group>();
            this.Student_Group1 = new HashSet<Student_Group>();
        }
    
        public int St_ID { get; set; }
        public string St_Name { get; set; }
        public string St_Phone { get; set; }
        public string St_Parent_Phone { get; set; }
        public string St_Gender { get; set; }
        public string St_Address { get; set; }
        public Nullable<int> St_Age { get; set; }
        public string St_Grade { get; set; }
        public string St_School_Name { get; set; }
        public string St_Language { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Attendence> Attendences { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Student_Group> Student_Group { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Student_Group> Student_Group1 { get; set; }
        public override string ToString()
        {
            return St_ID.ToString();
        }
    }
}
