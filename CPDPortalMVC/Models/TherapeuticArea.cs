using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CPDPortalMVC.Models
{
    public class TherapeuticArea
    {

        public int TherapeuticID { get; set; }
        public string TherapeuticName { get; set; }
        public string TherapeuticTitle { get; set; }
        public string TherapeuticImageURL { get; set; }
        public int ActivePrograms { get; set; }
    }
}