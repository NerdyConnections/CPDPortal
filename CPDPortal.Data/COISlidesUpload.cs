//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CPDPortal.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class COISlidesUpload
    {
        public int id { get; set; }
        public int UserID { get; set; }
        public int ProgramID { get; set; }
        public Nullable<bool> COISlides { get; set; }
        public string COISlidesExt { get; set; }
        public Nullable<System.DateTime> LastUpdated { get; set; }
    
        public virtual UserInfo UserInfo { get; set; }
    }
}