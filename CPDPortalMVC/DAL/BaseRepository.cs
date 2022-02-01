using CPDPortal.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CPDPortalMVC.DAL
{
    public class BaseRepository
    {
        private CPDPortalEntities ent = null;
        public CPDPortalEntities Entities
        {
            get
            {
                if (ent == null)
                {
                    ent = new CPDPortalEntities();
                }
                return ent;

            }
        }
    }
}