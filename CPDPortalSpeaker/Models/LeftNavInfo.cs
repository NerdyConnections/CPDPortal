using CPDPortalSpeaker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CPDPortalMVC.Models
{
    public class LeftNavInfo
    {
      
            public int UserID { get; set; }
            public bool COIForm { get; set; }
            public bool PayeeForm { get; set; }
            public List<ConfirmedSession> MySession { get; set; }


    }
}