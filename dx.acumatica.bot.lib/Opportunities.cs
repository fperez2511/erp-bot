using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dx.acumatica.bot.lib.webapi;


namespace dx.acumatica.bot.lib
{
    public class Opportunities
    {
        public async Task<string[][]> Get()
        {
            using (var client = new webapi.ScreenSoapClient())
            {
                
                var loginResult = await client.LoginAsync("admin", "amberF!nch99");
                var content = await client.CR304000GetSchemaAsync();

                await client.CR304000ClearAsync();
                var results = await client.CR304000ExportAsync(
                    new Command[]
                    {
                        content.OpportunitySummary.ServiceCommands.EveryOpportunityID,
                        content.OpportunitySummary.ServiceCommands.FilterDisplayName,
                        content.OpportunitySummary.BusinessAccount,
                        content.OpportunitySummary.Amount,
                        content.OpportunitySummary.ClassID,
                        content.OpportunitySummary.Contact,
                        content.OpportunitySummary.ContactDisplayName,
                        content.OpportunitySummary.Currency,
                        content.OpportunitySummary.Discount,
                        content.OpportunitySummary.Location,
                        content.OpportunitySummary.ManualAmount,
                        content.OpportunitySummary.NoteText,
                        content.OpportunitySummary.Reason,
                        content.OpportunitySummary.Source,
                        content.OpportunitySummary.Stage,
                        content.OpportunitySummary.Total,
                        content.OpportunitySummary.OpportunityID,
                        content.OpportunitySummary.Subject,
                        content.OpportunitySummary.Status,
                        content.Details.Estimation,
                        content.Details.OwnerEmployeeName,
                        content.Details.Details,
                        content.Details.Owner,
                        content.DetailsProbability.Probability,
                       new Field
                       {
                           ObjectName = content.OpportunitySummary.OpportunityID.ObjectName,
                           FieldName = "LastModifiedDateTime"
                       },
                                         new Field
                       {
                           ObjectName = content.OpportunitySummary.BusinessAccount.ObjectName,
                           FieldName = "AccountName"
                       }
                    },
                    new Filter[]
                    {
                        
                    },
                    0, true, false
                    );

                return results.ExportResult;
            }
        }
    }
}
