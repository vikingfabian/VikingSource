using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.Network
{
    struct Settings
    {
        
        //public float NetworkUpdateTime;
        public float SearchSessionIntervals;
        public SearchAndCreateSessionsStatus status;
       // public bool AutoCreateSessions;
        public bool HostMigration;

        public Settings(SearchAndCreateSessionsStatus status, float searchSessionIntervals)
        {
            //this.NetworkUpdateTime = networkUpdateTime;
            this.SearchSessionIntervals = searchSessionIntervals;
            this.status = status;
            //this.SearchSessions = SearchSessions;
            //this.AutoCreateSessions = AutoCreateSessions;
            HostMigration = false;
        }
    }

    enum SearchAndCreateSessionsStatus
    {
        NON,
        Search,
        Create,
        //SearchFirstThenCreate,
    }
}
