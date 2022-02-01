using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CPDPortalMVC.Models;

namespace CPDPortalMVC.Util
{
    public class TerritoryComparer: IEqualityComparer<TerritoryModel>
    {
        public int GetHashCode(TerritoryModel co)
        {
            if (co == null)
            {
                return 0;
            }
            return co.Id.GetHashCode();
        }

        public bool Equals(TerritoryModel x1, TerritoryModel x2)
        {
            if (object.ReferenceEquals(x1, x2))
            {
                return true;
            }
            if (object.ReferenceEquals(x1, null) ||
                object.ReferenceEquals(x2, null))
            {
                return false;
            }
            return x1.Id == x2.Id;
        }
    }
}