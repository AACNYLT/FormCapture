using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
