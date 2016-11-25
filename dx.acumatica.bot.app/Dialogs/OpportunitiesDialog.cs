using System;
using System.Linq;
using System.Threading.Tasks;
using dx.acumatica.bot.app.Models;
using dx.acumatica.bot.app.Services;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Internals.Fibers;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace dx.acumatica.bot.app.Dialogs
{
    [Serializable]
    public sealed class OpportunitiesDialog : LuisDialog<object>
    {
        private IOpportunitiesService service;

        public OpportunitiesDialog(ILuisService luis, IOpportunitiesService service)
            : base(luis)
        {
            SetField.NotNull(out this.service, nameof(service), service);
        }

        [LuisIntent("")]
        public async Task None(IDialogContext context, LuisResult result)
        {
            var message = $"Sorry I did not understand: " + string.Join(", ", result.Intents.Select(i => i.Intent));
            await context.PostAsync(message);
            await context.PostAsync(await service.GetOpportunities(new OpportunitiesRequestModel()));
            context.Wait(MessageReceived);
        }

        [LuisIntent("oppsByAccountName")]
        public async Task OppsByAccountName(IDialogContext context, LuisResult result)
        {
            var message = "oppsByAccountName  " + JsonConvert.SerializeObject(result);
            await context.PostAsync(message);
            var model = new OpportunitiesRequestModel
            {
                AccountName = result.Entities.FirstOrDefault(e => e.Type == "accountName").Entity
            };
            await context.PostAsync(await service.GetOpportunities(model));
            context.Wait(MessageReceived);
        }


        [LuisIntent("oppsByTotal")]
        public async Task OppsByTotal(IDialogContext context, LuisResult result)
        {
            var message = "oppsByTotal  " + JsonConvert.SerializeObject(result);
            await context.PostAsync(message);
            await context.PostAsync(await service.GetOpportunities(new OpportunitiesRequestModel()));
            context.Wait(MessageReceived);
        }


        [LuisIntent("oppsByEstimateDate")]
        public async Task OppsByEstimate(IDialogContext context, LuisResult result)
        {
            var message = "oppsByEstimateDate  " + JsonConvert.SerializeObject(result);
            await context.PostAsync(message);
            await context.PostAsync(await service.GetOpportunities(new OpportunitiesRequestModel()));
            context.Wait(MessageReceived);
        }
    }
}