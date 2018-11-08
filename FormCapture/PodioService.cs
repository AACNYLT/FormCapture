using PodioAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormCapture
{
    static class PodioService
    {
        public static async Task<List<Applicant>> GetApplicants()
        {
            var podio = new Podio("nyltformcapture", "jphJudEO239CPNoMoSRAJ81sbhfuaxdtquCgwUC11R2Pbdc3AqrpWHRzi0R1rfmZ");
            await podio.AuthenticateWithApp(21458112, "201991f4402f40e082dc1190872661ce");
            var itemResults = (await podio.ItemService.FilterItems(21458112)).Items;
            var applicants = new List<Applicant>();
            foreach (var item in itemResults)
            {
                applicants.Add(new Applicant
                {
                    Id = item.ItemId,
                    FirstName = item.Fields.Where(n => n.Label == "Name Tag").First().Values.First().First().ToObject<String>(),
                    LastName = item.Fields.Where(n => n.Label == "Last Name").First().Values.First().First().ToObject<String>(),
                });
            }
            return applicants;
        }
    }
}
