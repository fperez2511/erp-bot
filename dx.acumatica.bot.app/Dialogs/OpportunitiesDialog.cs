using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using dx.acumatica.bot.app.Models;
using dx.acumatica.bot.app.Services;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Internals.Fibers;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using Newtonsoft.Json;

namespace dx.acumatica.bot.app.Dialogs
{
    [Serializable]
    public sealed class OpportunitiesDialog : LuisDialog<object>
    {
        private readonly IOpportunitiesService service;

        public OpportunitiesDialog(ILuisService luis, IOpportunitiesService service)
            : base(luis)
        {
            SetField.NotNull(out this.service, nameof(service), service);
        }

        [LuisIntent("")]
        public async Task None(IDialogContext context, LuisResult result)
        {
            if (result.Query.ToLowerInvariant().Contains("debug"))
            {
                var isDebug = result.Query.ToLowerInvariant().Contains("on") ||
                              result.Query.ToLowerInvariant().Contains("true");
                context.UserData.SetValue("debug", isDebug);

                await context.PostAsync($"OK, I set debug to {isDebug.ToString().ToLowerInvariant()}.");

                context.Wait(MessageReceived);
                return;
            }


            var message = new StringBuilder();
            message.AppendLine("Hi!");
            message.AppendLine("I'm a bot that can help you find opportunities in your Acumatica ERP.");
            message.AppendLine("You can ask me for things like:");
            message.AppendLine(" * show me opportunities from GOLDRIVER");
            message.AppendLine(" * show me opportunities over $10,000,000");
            message.AppendLine(" * show me opportunities less than $5,000,000");
            message.AppendLine(" * show me opportunity estimates for 6/1/2017 *(coming soon)*");
            message.AppendLine(
                "\r\nYou can also turn **debug on** or **debug off** to get more data about each request.");
            await context.PostAsync(message.ToString());

            context.Wait(MessageReceived);
        }

        [LuisIntent("oppsByAccountName")]
        public async Task OppsByAccountName(IDialogContext context, LuisResult result)
        {
            var model = new OpportunitiesRequestModel
            {
                Intent = OpportunitiesIntents.OppsByAccountName,
                AccountName = result.Entities.FirstOrDefault(e => e.Type == "accountName").Entity
            };
            model.Message = "Here are the opportunities for **_" + model.AccountName.ToUpperInvariant() + "_** \r\n";
            var serviceCall = await service.GetOpportunities(model);
            await context.PostAsync(serviceCall.Message);

            await
                WriteDebug(context,
                    "DEBUG OppsByAccountName, Luis Model:\r\n" + JsonConvert.SerializeObject(result),
                    serviceCall.DebugMessage);

            context.Wait(MessageReceived);
        }

        private static async Task WriteDebug(IDialogContext context, params string[] messages)
        {
            var isDebug = false;
            context.UserData.TryGetValue("debug", out isDebug);
            if (isDebug)
            {
                foreach (var message in messages)
                {
                    await context.PostAsync(message);
                }
            }
        }


        [LuisIntent("oppsByTotal")]
        public async Task OppsByTotal(IDialogContext context, LuisResult result)
        {
            var money = Regex.Replace(result.Entities.FirstOrDefault(e => e.Type == "builtin.money").Entity, "[^0-9.]",
                "");
            var greaterThan = true;
            var item = result.Entities.FirstOrDefault(e => e.Type == "lessThan");
            if (item?.Entity != null)
            {
                greaterThan = false;
            }
            var model = new OpportunitiesRequestModel
            {
                Intent = OpportunitiesIntents.OppsByTotal,
                Amount = Convert.ToDouble(money),
                GreaterOrLessThan = greaterThan
            };
            model.Message =
                $"Here are the opportunties **_{(greaterThan ? "greater" : "less")} than {model.Amount.ToString("C0")}_**";
            var serviceCall = await service.GetOpportunities(model);
            await context.PostAsync(serviceCall.Message);

            await
                WriteDebug(context,
                    "DEBUG OppsByTotal, Luis Model:\r\n" + JsonConvert.SerializeObject(result),
                    serviceCall.DebugMessage);

            context.Wait(MessageReceived);
        }


        [LuisIntent("oppsByEstimateDate")]
        public async Task OppsByEstimate(IDialogContext context, LuisResult result)
        {
            var message = "oppsByEstimateDate  " + JsonConvert.SerializeObject(result);
            await context.PostAsync(message);
            // await context.PostAsync(await service.GetOpportunities(new OpportunitiesRequestModel()));
            context.Wait(MessageReceived);
        }
    }
}