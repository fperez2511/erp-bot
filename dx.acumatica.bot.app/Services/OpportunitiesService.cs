using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using dx.acumatica.bot.app.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace dx.acumatica.bot.app.Services
{
    public class OpportunitiesService : IOpportunitiesService
    {
        private static readonly HttpClient Client = new HttpClient();

        public async Task<string> GetOpportunities(OpportunitiesRequestModel model)
        {
            var uri = new Uri("http://localhost:3979/api/opportunities?" + GetQueryString(model));
           

            var result = await Client.GetAsync(uri);
            var stringbuilder = new StringBuilder();
            var content = await result.Content.ReadAsStringAsync();
            var obj = JArray.Parse(content);

            foreach (var o in obj)
            {
                stringbuilder.Append(
                    $" * {o["Subject"]} from {o["BusinessAccount"]} for {o["Total"]}, estimated by {o["Estimation"]}\r\n");
            }
            stringbuilder.AppendLine(JsonConvert.SerializeObject(model));
            return stringbuilder.ToString();
        }

        public string GetQueryString(object obj)
        {
            var properties = from p in obj.GetType().GetProperties()
                             where p.GetValue(obj, null) != null
                             select p.Name + "=" + HttpUtility.UrlEncode(p.GetValue(obj, null).ToString());

            return string.Join("&", properties.ToArray());
        }
    }
}