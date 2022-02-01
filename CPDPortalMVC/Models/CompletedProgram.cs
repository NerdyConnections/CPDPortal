using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CPDPortalMVC.Models
{
    public class CompletedProgram
    {
        


            public int ProgramRequestID { get; set; }
            public int Speakeruserid { get; set; }
            public string ConfirmedSessionDate { get; set; }
            public string LocationName { get; set; }
            public string ProgramName { get; set; }
           

        



        }
}