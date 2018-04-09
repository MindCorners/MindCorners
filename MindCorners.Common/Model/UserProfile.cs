//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MindCorners.Common.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class UserProfile
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public UserProfile()
        {
            this.GridStates = new HashSet<GridState>();
        }
    
        public System.Guid Id { get; set; }
        public Nullable<System.Guid> OrganizationId { get; set; }
        public short Type { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public System.DateTime StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public System.DateTime DateCreated { get; set; }
        public System.DateTime DateModified { get; set; }
        public System.Guid CreatorId { get; set; }
        public System.Guid ModifierId { get; set; }
        public string User_Id { get; set; }
        public byte[] ProfileImage { get; set; }
        public string MiddleName { get; set; }
        public Nullable<System.DateTime> DateDeleted { get; set; }
        public string ProfileImageString { get; set; }
        public string FullName { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GridState> GridStates { get; set; }
        public virtual Organization Organization { get; set; }
        public virtual AspNetUser AspNetUser { get; set; }
    }
}