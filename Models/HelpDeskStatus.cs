using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SyncfusionHelpDesk.Data
{
    public class HelpDeskStatus
    {
        public string ID { get; set; }
        public string Text { get; set; }

        public static List<HelpDeskStatus> Statuses = 
            new List<HelpDeskStatus>() {
        new HelpDeskStatus(){ ID= "New", Text= "New" },
        new HelpDeskStatus(){ ID= "Open", Text= "Open" },
        new HelpDeskStatus(){ ID= "Urgent", Text= "Urgent" },
        new HelpDeskStatus(){ ID= "Closed", Text= "Closed" },
        };
    }
}
