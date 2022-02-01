using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CPDPortalMVC.Models
{
    public class PresenterPayment
    {
        

            public int ProgramRequestID { get; set; }
            public int userid { get; set; }
            public string ProgramDate { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string PaymentAmount { get; set; }
            public string PaymentSentDate { get; set; }
            

            public string PaymentMethod { get; set; }
        
    }
}