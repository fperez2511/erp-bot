using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dx.acumatica.bot.app.Models
{
    public class OpportunitiesRequestModel
    {
        public OpportunitiesIntents Intent { get; set; }
        public string AccountName { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }

        public double Amount { get; set; }
        public bool GreaterOrLessThan { get; set; }
        public string Message { get; set; }
    }

    public enum OpportunitiesIntents
    {
        OppsByAccountName,
        OppsByTotal,
        OppsByEstimate
    }

    public class ServiceModel
    {
        public string Message { get; set; }
        public string DebugMessage { get; set; }
    }
}
