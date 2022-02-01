using CPDPortalSpeaker.Models;
using CPDPortalSpeaker.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace CPDPortalSpeaker.Controllers
{
    public class BaseController : Controller
    {
        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);
            UserModel um = UserHelper.GetLoggedInUser();
            //update program name to French if user swich language to french
            if (um != null)
            {
                foreach (var item in um.SpeakerProgramDetails)
                {
                    
                    if (item.ProgramID == 8 && (Session[Constants.CULTURE].ToString() == Constants.ENGLISH))
                    {
                        item.ProgramName = "Dyslipidemia in Primary Care: Implications of the 2021 CCS Dyslipidemia Guidelines";
                        foreach (var session in item.MySession)
                        {
                            StringBuilder sb = new StringBuilder(session.ConfirmedDate);
                            sb.Replace("Janvier", "January").Replace("Février", "February").Replace("Mars", "March").Replace("Avril", "April").Replace("Mai", "May").Replace("Juin", "June").Replace("Juillet", "July").Replace("Aaoût", "August").Replace("Septembre", "September").Replace("Octobre", "October").Replace("Novembre", "November").Replace("Décembre", "December");
                            session.ConfirmedDate = sb.ToString();
                        }
                    }//if french convert the programname and all months to french
                    else if (item.ProgramID == 8 && (Session[Constants.CULTURE].ToString() == Constants.FRENCH))
                    {
                        item.ProgramName = "Dyslipidémie en soins primaires :  Implications des lignes directrices 2021 de la SCC sur la dyslipidémie";
                        foreach (var session in item.MySession)
                        {
                            StringBuilder sb = new StringBuilder(session.ConfirmedDate);
                            sb.Replace("January", "Janvier").Replace("February", "Février").Replace("March", "Mars").Replace("April", "Avril").Replace("May", "Mai").Replace("June", "Juin").Replace("July", "Juillet").Replace("August", "Aaoût").Replace("September", "Septembre").Replace("October", "Octobre").Replace("November", "Novembre").Replace("December", "Décembre");
                            session.ConfirmedDate = sb.ToString();
                        }
                    }
                }
            }
            //if in the url the parameter lang=fr is present, make the page french
            if (requestContext.HttpContext.Request.QueryString["lang"] != null)
            {
                if (requestContext.HttpContext.Request.QueryString["lang"].ToString().Equals("fr"))
                {
                    Session[Constants.CULTURE] = Constants.FRENCH;
                }
            }
            if (Session[Constants.CULTURE] == null)
                Session[Constants.CULTURE] = Constants.ENGLISH;

            Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(Session[Constants.CULTURE].ToString());//determines the resources to be loaded for the page
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(Session[Constants.CULTURE].ToString());//Culture determines date,number,currency
        }

       
    }
}