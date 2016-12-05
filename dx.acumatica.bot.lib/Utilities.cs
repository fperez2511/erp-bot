using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dx.acumatica.bot.lib
{
    public static class Utilities
    {
        public static DateTime ConvertStringToDate(string dateString, bool isStart)
        {
            
            var dt = DateTime.Now;
            DateTime.TryParse(dateString, out dt);
            return dt;
        }
    }
}
