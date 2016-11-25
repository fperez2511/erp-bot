using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Autofac;
using Microsoft.Bot.Builder.Dialogs.Internals;
using Microsoft.Bot.Builder.Internals.Fibers;
using Microsoft.Bot.Connector;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace dx.acumatica.bot.app
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        private readonly ILifetimeScope scope;
        public MessagesController(ILifetimeScope scope)
        {
            SetField.NotNull(out this.scope, nameof(scope), scope);
        }
        public async Task<HttpResponseMessage> Post([FromBody] Activity activity, CancellationToken token)
        {
            if (activity != null)
            {
                switch (activity.GetActivityType())
                {
                    case ActivityTypes.Message:
                        using (var scope = DialogModule.BeginLifetimeScope(this.scope, activity))
                        {
                            var postToBot = scope.Resolve<IPostToBot>();
                            await postToBot.PostAsync(activity, token);
                        }

                        break;
                }
            }

            return new HttpResponseMessage(HttpStatusCode.Accepted);
        }



        private Activity HandleSystemMessage(Activity message)
        {
            if (message.Type == ActivityTypes.DeleteUserData)
            {
                // Implement user deletion here
                // If we handle user deletion, return a real message
            }
            else if (message.Type == ActivityTypes.ConversationUpdate)
            {
                // Handle conversation state changes, like members being added and removed
                // Use Activity.MembersAdded and Activity.MembersRemoved and Activity.Action for info
                // Not available in all channels
            }
            else if (message.Type == ActivityTypes.ContactRelationUpdate)
            {
                // Handle add/remove from contact lists
                // Activity.From + Activity.Action represent what happened
            }
            else if (message.Type == ActivityTypes.Typing)
            {
                // Handle knowing tha the user is typing
            }
            else if (message.Type == ActivityTypes.Ping)
            {
            }

            return null;
        }
    }
}