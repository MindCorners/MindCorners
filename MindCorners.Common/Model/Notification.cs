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
    
    public partial class Notification
    {
        public System.Guid Id { get; set; }
        public Nullable<System.Guid> SenderId { get; set; }
        public string Body { get; set; }
        public System.DateTime DateCreated { get; set; }
        public System.DateTime DateModified { get; set; }
        public System.Guid CreatorId { get; set; }
        public System.Guid ModifierId { get; set; }
        public Nullable<System.DateTime> DateDeleted { get; set; }
        public Nullable<System.Guid> SourceId { get; set; }
        public int Type { get; set; }
        public Nullable<System.Guid> UserId { get; set; }
        public Nullable<System.DateTime> ReadDate { get; set; }
    }
}
