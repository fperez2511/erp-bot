using System;
using System.Configuration;
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

        public async Task<ServiceModel> GetOpportunities(OpportunitiesRequestModel model)
        {
            var uriBase = ConfigurationManager.AppSettings["opportunityAPI"];
            var uri = new Uri(uriBase + "?" + GetQueryString(model));
           

            var result = await Client.GetAsync(uri);
            var stringbuilder = new StringBuilder();
            var content = await result.Content.ReadAsStringAsync();
            var obj = JArray.Parse(content);

            stringbuilder.AppendLine(model.Message);
            foreach (var o in obj)
            {
                stringbuilder.Append(
                    $" * {o["Subject"]} from {o["BusinessAccount"]} for {Convert.ToDouble(o["Total"]).ToString("c0")}, estimated by {o["Estimation"]}\r\n");
            }

            var debugInfo = new StringBuilder();
            debugInfo.AppendLine($"DEBUG: Opportunities API RequestOpportunitiesRequestModel: \r\n{JsonConvert.SerializeObject(model)} \r\n");
            debugInfo.AppendLine($"DEBUG: Opportunities API Url: *{uri}*\r\n");
            return new ServiceModel
            {
                Message = stringbuilder.ToString(),
                DebugMessage = debugInfo.ToString()
            };
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