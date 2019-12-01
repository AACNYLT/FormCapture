using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FormCapture.Classes;
using Newtonsoft.Json;
using NYLT;
using Windows.Web.Http;

namespace FormCapture.Services
{
    static class ApplicantService
    {
        public static List<Applicant> getApplicants() {
            using (var context = new FormContext())
            {
                return context.Applicants.ToList();
            }
        }

        public async static Task<List<Applicant>> getApplicants(string URL)
        {
            var controller = new NYLT.Rest.RestController(Utilities.URLifyString(URL));
            return await controller.GetObjectAsync<List<Applicant>>("/");
        }

        public async static Task<Boolean> uploadApplicants(string PostURL, string GetURL)
        {
            using (var context = new FormContext())
            {
                var applicants = context.Applicants.ToList();
                var onServerApplicants = await getApplicants(GetURL);
                var newApplicants = applicants.Where(n => !onServerApplicants.Exists(m => m.Id == n.Id)).ToList();
                //var controller = new NYLT.Rest.RestController(Utilities.URLifyString(PostURL));
                //var response = await controller.PostObjectAsync("/", newApplicants);
                //return response != null;
                try
                {
                    var client = new HttpClient();
                    var json = JsonConvert.SerializeObject(newApplicants, new Newtonsoft.Json.Converters.IsoDateTimeConverter());
                    var content = new HttpStringContent(json, Windows.Storage.Streams.UnicodeEncoding.Utf8, "application/json");
                    var response = await client.PostAsync(new Uri(Utilities.URLifyString(PostURL)), content);
                    return response.IsSuccessStatusCode;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }
    }
}
