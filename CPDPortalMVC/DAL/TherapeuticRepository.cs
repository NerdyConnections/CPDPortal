using CPDPortalMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CPDPortalMVC.DAL
{
    public class TherapeuticRepository:BaseRepository
    {
        public List<Program> GetPrograms(int therapeuticID)
        {
            Program objProgram = new Program();
            ProgramRepository pr = new ProgramRepository();

            List<Program> liProgram = null;
            liProgram = Entities.TherapeuticPrograms.Where(u => u.TherapeuticID == therapeuticID && (u.Program.Archive==false||u.Program.Archive == null)).
            Select(u => new Program
            {
                ProgramID = u.Program.ProgramID,
                ProgramID_CHRC = u.Program.ProgramID_CHRC,
                ProgramName = u.Program.ProgramName,
                DevelopedBy = u.Program.DevelopedBy,
                CertifiedBy = u.Program.CertifiedBy,
                ExpirationDate = u.Program.ExpirationDate,
                TargetAudience = u.Program.TargetAudience,
                CreditHours = u.Program.CreditHours,
               
                ProgramCompleted = u.Program.ProgramCompleted,
                 Archived = u.Program.Archive ?? false

            }).ToList();
            //do not display archived program
            foreach (Program pg in liProgram)
            {
               
                pg.EventsCompleted = pr.GetEventsCompleted(pg.ProgramID);
            }

            return liProgram;



        }
        public List<Models.Program> GetArchivedPrograms(int therapeuticID)
        {
            Program objProgram = new Program();
            ProgramRepository pr = new ProgramRepository();

            List<Models.Program> liProgram = null;
            liProgram = Entities.TherapeuticPrograms.Where(u => u.TherapeuticID == therapeuticID && u.Program.Archive==true).
            Select(u => new Program
            {
                ProgramID = u.Program.ProgramID,
                ProgramID_CHRC = u.Program.ProgramID_CHRC,
                ProgramName = u.Program.ProgramName,
                DevelopedBy = u.Program.DevelopedBy,
                CertifiedBy = u.Program.CertifiedBy,
                ExpirationDate = u.Program.ExpirationDate,
                TargetAudience = u.Program.TargetAudience,
                CreditHours = u.Program.CreditHours,

                ProgramCompleted = u.Program.ProgramCompleted,
                Archived = u.Program.Archive ?? false

            }).ToList();
            //only want to get Archived Program
            //remove the program if it is not archived
            foreach (Program pg in liProgram)
            {
               
                //status ==4 is completed
                pg.EventsCompleted = pr.GetEventsCompleted(pg.ProgramID);
            }

            return liProgram;



        }
    }
}