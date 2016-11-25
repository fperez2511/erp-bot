using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Runtime.Caching;
using System.Threading.Tasks;
using System.Web.Http;
using dx.acumatica.bot.app.Models;
using dx.acumatica.bot.lib;
using Newtonsoft.Json;

namespace dx.acumatica.bot.app.Controllers
{
    [RoutePrefix("api/opportunities")]
    public class OpportunitiesController : ApiController
    {
        private string cacheKey = "opportunitiesCacheKey";
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
                filteredList = list.Where(d => Compare(d["BusinessAccount"], model.AccountName)).ToList();
            }

            return new HttpResponseMessage()
            {
                Content =
                    new ObjectContent(typeof(List<Dictionary<string, string>>), filteredList, new JsonMediaTypeFormatter(),
                        new MediaTypeHeaderValue("application/json"))
            };
        }

        internal bool Compare(string val1, string val2)
        {
            return val1.Trim().Equals(val2.Trim(), StringComparison.InvariantCultureIgnoreCase);
        }
    }

    public class MemoryCacher
    {
        public object GetValue(string key)
        {
            MemoryCache memoryCache = MemoryCache.Default;
            return memoryCache.Get(key);
        }

        public bool Add(string key, object value, DateTimeOffset absExpiration)
        {
            MemoryCache memoryCache = MemoryCache.Default;
            return memoryCache.Add(key, value, absExpiration);
        }

        public void Delete(string key)
        {
            MemoryCache memoryCache = MemoryCache.Default;
            if (memoryCache.Contains(key))
            {
                memoryCache.Remove(key);
            }
        }
    }
}
