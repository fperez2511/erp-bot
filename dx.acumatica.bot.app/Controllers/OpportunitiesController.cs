using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;
using dx.acumatica.bot.app.Models;
using dx.acumatica.bot.app.Modules;
using dx.acumatica.bot.lib;
using Newtonsoft.Json;

namespace dx.acumatica.bot.app.Controllers
{
    [RoutePrefix("api/opportunities")]
    public class OpportunitiesController : ApiController
    {
        private string cacheKey = "opportunitiesCacheKey1";
        [Route("")]
        [HttpGet]
        public async Task<HttpResponseMessage> Get([FromUri]OpportunitiesRequestModel model)
        {
            var cache = new MemoryCacher();
            var list = (List<Dictionary<string, string>>) cache.GetValue(cacheKey);
            if (list == null)
            {
                list = new List<Dictionary<string, string>>();
                var opportunities = new Opportunities();
                var result = await opportunities.Get();

                var items = result;

                for (var i = 1; i < items.Length; i++)
                {
                    var dict = new Dictionary<string, string>();
                    for (var j = 0; j < items[i].Length; j++)
                    {
                        dict.Add(items[0][j], items[i][j]);
                    }
                    list.Add(dict);
                }
                cache.Add(cacheKey, list, DateTimeOffset.UtcNow.AddHours(1));
            }
            var filteredList = list;
            if (model.Intent == OpportunitiesIntents.OppsByAccountName)
            {
                filteredList = list.Where(d => CompareAreEqual(d["BusinessAccount"], model.AccountName)).ToList();
            }

            if (model.Intent == OpportunitiesIntents.OppsByTotal)
            {
                filteredList = list.Where(d => CompareIsGgreater(d["Total"], model.Amount, model.GreaterOrLessThan)).ToList();
            }

            return new HttpResponseMessage()
            {
                Content =
                    new ObjectContent(typeof(List<Dictionary<string, string>>), filteredList, new JsonMediaTypeFormatter(),
                        new MediaTypeHeaderValue("application/json"))
            };
        }

        internal bool CompareAreEqual(string val1, string val2)
        {
            return val1.Trim().Equals(val2.Trim(), StringComparison.InvariantCultureIgnoreCase);
        }

        internal bool CompareIsGgreater(string val1, double val2, bool isGreater)
        {
            var num1 = Convert.ToDouble(val1);
            var num2 = Convert.ToDouble(val2);
            return isGreater ? num1 >= num2 : num1 <= num2;
        }
    }
}
