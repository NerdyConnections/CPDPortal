using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace CPDPortalMVC.Util
{
    public class PDFResult : ActionResult
    {
        public byte[] bytes;

        public PDFResult(byte[] bytes)
        {
            this.bytes = bytes;
        }


        public override void ExecuteResult(ControllerContext context)
        {

            if (bytes == null || bytes.Length == 0)
            {
                HttpContext.Current.Response.Write("Could not display Pdf");
            }
            else
            {
                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.Buffer = true;
                HttpContext.Current.Response.ContentType = "application/pdf";
                HttpContext.Current.Response.BinaryWrite(bytes);
            }

        }
    }
}