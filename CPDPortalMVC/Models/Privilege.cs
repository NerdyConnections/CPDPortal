using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CPDPortalMVC.Models
{ 
    public class Privilege
    {

        public int PrivilegeID { get; set; }
        public string Description { get; set; }
        public int SponsorID { get; set; }
        public bool? CV { get; set; }
        public bool? Bone { get; set; }
        public bool? Neuro { get; set; }
        public bool? Vaccines { get; set; }
        public bool? Oncology { get; set; }
        public bool? Immunology { get; set; }
        public bool? RareDisease { get; set; }
       
    }
}