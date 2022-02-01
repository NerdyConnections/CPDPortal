using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CPDPortalMVC.Models
{
    public class EvalForm2Model
    {

        public string ProgramDate { get; set; }
        public string ProgramLocation { get; set; }
        public string Speaker1 { get; set; }
        public string Speaker2 { get; set; }
        public bool CaseNewHorizons { get; set; }
            public string FileName { get; set; }
        public bool CoreDeck { get; set; }
        public bool Case1 { get; set; }
        public bool Case2 { get; set; }
        public bool Case3 { get; set; }
        public bool Case4 { get; set; }
        public bool Case5 { get; set; }
        public bool DisplayPDF { get; set; }
        public DateTime? LastUpated { get; set; }
    }
}