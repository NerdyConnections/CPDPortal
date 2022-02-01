using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CPDPortalMVC.Models
{
    public class NationalInvitation7Model
    {
        public int ProgramRequestID { get; set; }
        public string ContactFirstName { get; set; }
        public string ContactLastName { get; set; }
        public string ContactPhone { get; set; }
        public string ContactEmail { get; set; }

        public string ProgramDate { get; set; }
        public string ProgramStartTime { get; set; }
        public string ProgramEndTime { get; set; }
        public string ProgramLocation { get; set; }
        public string Speaker1 { get; set; }
        public string Speaker2 { get; set; }
        public string Moderator { get; set; }
        public string ProvinceCode { get; set; }
        public string ProvinceName { get; set; }
        public string FileName { get; set; }
        public int SessionCreditID { get; set; }
        public decimal SessionCreditValue { get; set; }
        public string SessionCredit { get; set; }
        public bool ProgramModule1 { get; set; }
        public bool ProgramModule2 { get; set; }
        public bool ProgramModule3 { get; set; }
        public bool ProgramModule4 { get; set; }
        public bool ProgramModule5 { get; set; }
        public bool ProgramModule6 { get; set; }
        public bool ProgramModule7 { get; set; }
        public bool ProgramModule8 { get; set; }
        public bool ProgramModule9 { get; set; }
        public bool ProgramModule10 { get; set; }
        public bool DisplayPDF { get; set; }
        public DateTime? LastUpated { get; set; }
    }
}