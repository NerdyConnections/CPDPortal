using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CPDPortalMVC.Models;

namespace CPDPortalMVC.Util
{
    public  class ListHelper
    {

        public static List<ProvinceModel> GetProvinces()
        {
            var list = new List<ProvinceModel>
            {
                 new ProvinceModel{Id = "1", Name = "AB", Checked = false},
                 new ProvinceModel{Id = "2", Name = "BC", Checked = false},
                 new ProvinceModel{Id = "3", Name = "MB", Checked = false},
                 new ProvinceModel{Id = "4", Name = "NS", Checked = false},
                 new ProvinceModel{Id = "5", Name = "NB", Checked = false},
                 new ProvinceModel{Id = "6", Name = "NL", Checked = false},
                 new ProvinceModel{Id = "7", Name = "ON", Checked = false},
                 new ProvinceModel{Id = "8", Name = "PEI", Checked = false},
                 new ProvinceModel{Id = "9", Name = "QC", Checked = false},
                 new ProvinceModel{Id = "10", Name = "SK", Checked = false},

            };

            return list;


        }


        public static List<TerritoryModel> GetTerritories()
        {
            var list = new List<TerritoryModel>
            {
                 new TerritoryModel{Id = "1", TerritoryID = "41", Checked = false},
                 new TerritoryModel{Id = "2", TerritoryID = "43", Checked = false},
                 new TerritoryModel{Id = "3", TerritoryID = "44", Checked = false},
                 new TerritoryModel{Id = "4", TerritoryID = "47", Checked = false},
                 new TerritoryModel{Id = "5", TerritoryID = "48", Checked = false},
                 new TerritoryModel{Id = "6", TerritoryID = "LTC Bone", Checked = false},


            };

            return list;


        }

        public static List<ProvinceModel> SelectedProvincesFromModel(UserActivationModel pr)
        {
            var Provicelist = GetProvinces();
            List<ProvinceModel> list = new List<ProvinceModel>();            

            for (int i = 0; i < pr.Provinces.Count; i++)
            {
                if (pr.Provinces[i].Checked == true)
                {
                    Provicelist[i].Checked = true;
                }

            }

            list = Provicelist.Where(x => x.Checked == true).ToList();
            return list;      
            
        }

        //public static List<TerritoryModel> SelectedTerritoriesFromModel(UserActivationModel pr)
        //{
        //    var Territorylist = GetTerritories();
        //    List<TerritoryModel> list = new List<TerritoryModel>();


        //    for (int i = 0; i < pr.Territories.Count; i++)
        //    {
        //        if (pr.Territories[i].Checked == true)
        //        {
        //            Territorylist[i].Checked = true;
        //        }
        //    }

        //    list = Territorylist.Where(x => x.Checked == true).ToList();
        //    return list;    
            
        //}

    }
}