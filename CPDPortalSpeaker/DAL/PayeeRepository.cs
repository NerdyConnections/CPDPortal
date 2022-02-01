using CPDPortal.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Text;
using System.Web;
using CPDPortalSpeaker.Models;
using CPDPortalSpeaker.Util;

namespace CPDPortalSpeaker.DAL
{
    public class PayeeRepository : BaseRepository
    {

        public PayeeModel GetPayee(int UserId)
        {
            PayeeModel payee = new PayeeModel();

            var query = Entities.PayeeInfoes.Where(p => p.UserID == UserId).SingleOrDefault();


            if (query != null)
            {

                payee.UserId = query.UserID;
                payee.PaymentMethod = query.PaymentMethod;
                payee.PayableTo = query.ChequePayableTo;
                payee.IRN = query.InternalRefNum;
                payee.MailingAddress1 = query.MailingAddr1;
                payee.MailingAddress2 = query.MailingAddr2;
                payee.AttentionTo = query.AttentionTo;
                payee.City = query.City;
                payee.Province = query.ProvinceID;
                payee.PostalCode = query.PostalCode;
                payee.TaxNumber = query.TaxNumber;
                payee.Instructions = query.AdditionalInstructions;

            }

            return payee;
        }


        public void AddPayee(PayeeModel payee)
        {
            CPDPortal.Data.PayeeInfo payeeinfo = new CPDPortal.Data.PayeeInfo();

            payeeinfo.UserID = payee.UserId.Value;
            payeeinfo.PaymentMethod = payee.PaymentMethod;
            payeeinfo.ChequePayableTo = payee.PayableTo;
            payeeinfo.InternalRefNum = payee.IRN;
            payeeinfo.MailingAddr1 = payee.MailingAddress1;
            payeeinfo.MailingAddr2 = payee.MailingAddress2;
            payeeinfo.AttentionTo = payee.AttentionTo;
            payeeinfo.City = payee.City;
            payeeinfo.ProvinceID = payee.Province;
            payeeinfo.PostalCode = payee.PostalCode;
            payeeinfo.TaxNumber = payee.TaxNumber;
            payeeinfo.AdditionalInstructions = payee.Instructions;
            payeeinfo.LastUpdated = DateTime.Now;
            Entities.PayeeInfoes.Add(payeeinfo);
            Entities.SaveChanges();

        }


        public bool IsSubmitted(int UserID)
        {
            bool retVal = false;

            var IsPayee = Entities.UserRegistrations.Where(x => x.UserID == UserID).FirstOrDefault();

            if(IsPayee != null)
            {
                if(IsPayee.PayeeForm == true)
                {
                    retVal = true;

                }

            }

            return retVal;
        }


        public void UpdatePayeeInformation(int userid)
        {
            CPDPortal.Data.UserRegistration UserReg = new CPDPortal.Data.UserRegistration();

            var user = Entities.UserRegistrations.Where(x => x.UserID == userid).SingleOrDefault();

            if (user != null)
            {
                user.PayeeForm = true;
                Entities.SaveChanges();                              

            }

            else
            {
                UserReg.UserID = userid;
                UserReg.PayeeForm = true;
                Entities.UserRegistrations.Add(UserReg);
                Entities.SaveChanges();

            }

        }

    }
}