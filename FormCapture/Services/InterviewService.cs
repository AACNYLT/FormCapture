using FormCapture.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormCapture.Services
{
    static class InterviewService
    {
        public static List<Interview2017> getInterviews(int ApplicantId)
        {
            using (var context = new FormContext())
            {
                return context.Interviews.Where(n => n.ApplicantId == ApplicantId).ToList();
            }
        }

        public static async Task saveInterview(Interview2017 interview)
        {
            using (var context = new FormContext())
            {
                context.Interviews.Add(interview);
                await context.SaveChangesAsync();
            }
        }
        
        public static async Task updateInterview(Interview2017 interview)
        {
            using (var context = new FormContext())
            {
                context.Interviews.Update(interview);
                await context.SaveChangesAsync();
            }
        }


    }
}
