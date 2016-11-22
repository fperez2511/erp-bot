using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;
using dx.acumatica.bot.lib;
using Newtonsoft.Json;

namespace dx.acumatica.bot.app.Controllers
{
    public class OpportunitiesController : ApiController
    {
        public async Task<HttpResponseMessage> Get()
        {
            var list = new List<Dictionary<string, string>>();
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
            return new HttpResponseMessage()
            {
                Content =
                    new ObjectContent(typeof (List<Dictionary<string, string>>), list, new JsonMediaTypeFormatter(),
                        new MediaTypeHeaderValue("application/json"))
            };
        }


    }
}
