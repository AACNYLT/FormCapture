using FormCapture.Classes;
namespace FormCapture
{
    public class ApplicantMap: CsvHelper.Configuration.CsvClassMap<Applicant>
    {
        public ApplicantMap()
        {
            Map(n => n.Id).Name("Applicant ID");
            Map(n => n.FirstName).Name("First Name");
            Map(n => n.LastName).Name("Last Name");
        }
    }
}
