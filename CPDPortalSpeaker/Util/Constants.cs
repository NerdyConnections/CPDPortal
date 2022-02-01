using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CPDPortalSpeaker.Util
{
    public static class Constants
    {
       /*culture - related constants*/
            public static readonly string ENGLISH = "en-CA";
            public static readonly string FRENCH = "fr-CA";
            public static readonly string CULTURE = "CULTURE";
            public static readonly string ENGLISH_STR = "English";
            public static readonly string FRENCH_STR = "Français";
        
       
        public static string GetCurrentLanguage()
        {
            string retVal = string.Empty;

            //if (HttpContext.Current.Session[Constants.CULTURE] == null)
            //    HttpContext.Current.Session[Constants.CULTURE] = Constants.ENGLISH;
            //else
            if (HttpContext.Current.Session[Constants.CULTURE] == null)
                retVal = FRENCH_STR;
            else
            {
                if ((string)HttpContext.Current.Session[Constants.CULTURE] == Constants.ENGLISH)
                    retVal = FRENCH_STR;
                else
                    retVal = ENGLISH_STR;
            }


            return retVal;
        }
        /*end of culture related constants */
        public static readonly string SalesDirector = "Sales Director";
        public static readonly string SalesRep = "Sales Representative";
        public static readonly string RegionalManager = "Regional Manager";
        public static readonly string HeadOffice = "Head Office";
        public static readonly string Speaker = "Speaker";
        public static readonly string Admin = "Admin";

        public static readonly string USER = "USER";


        public static readonly string LogoImagePath = "/Images/CPDLOGO/";

        public static readonly string NA = "N/A";
        public static readonly string SubmitPostSessionMaterials = "Submit Post Session Materials";
        public static readonly string SpeakerNA = "Speaker Not Available for the selected date(s): Please click on the “pencil” icon and select a different speaker or change the session date";
        public static readonly string SpeakerDeclined = "Speaker Declined Participation: Please click on the “pencil” icon and select a different speaker ";
        public static readonly string ModeratorNA = "Moderator Not Available for the selected date(s): Please click on the “pencil” icon and select a different moderator or change the session date";
        public static readonly string ModeratorDeclined = "Moderator Declined Participation: Please click on the “pencil” icon and select a different moderator ";

        public static readonly string VenueNA = "The Venue is not available for the selected date(s): Please click on the “pencil” icon and enter new venue details";
        public enum UserRole
        {
            SalesRep = 1,
            RegionalManager = 2,
            HeadOffice = 3,
            Speaker = 4,
            Admin = 5

        }
        public enum Therapeutic
        {
            CV = 1,
            Bone = 2,
            CVBone = 3

        }

    }
}