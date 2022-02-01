using CPDPortalMVC.CustomValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CPDPortalMVC.Models
{
    public class EventModule
    {
        public int ProgramRequestID { get; set; }
       
        public int SessionCreditID { get; set; }
        public string SessionCredit { get; set; }
        //1,1.5,2, 2.75
        public decimal SessionCreditValue { get; set; }
        public int ModuleLimit { get; set; }
        public bool ProgramModule1 { get; set; }
        public bool ProgramModule2 { get; set; }
        public bool ProgramModule3 { get; set; }
        public bool ProgramModule4 { get; set; }
        public bool ProgramModule5 { get; set; }
        public bool ProgramModule6 { get; set; }
        public bool ProgramModule7 { get; set; }
        public bool ProgramModule8 { get; set; }
        public bool ProgramModule9 { get; set; }
        public bool ProgramModule10 { get; set; }


    }
}