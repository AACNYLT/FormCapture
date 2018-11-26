using System.Collections.Generic;
using System.Linq;
using FormCapture.Classes;

namespace FormCapture
{
    static class ApplicantService
    {
        public static List<Applicant> getApplicants() {
            var context = new FormContext();
            return context.Applicants.ToList();
        }
    }
}
