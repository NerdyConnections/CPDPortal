using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CPDPortalMVC.Models;

namespace CPDPortalMVC.ViewModels
{
    public class ActivationViewModel
    {
        public UserActivationModel User { get; set; }
        public List<ProvinceModel> Provinces { get; set; }
        public List<TerritoryModel> Territories { get; set;}
    }
}