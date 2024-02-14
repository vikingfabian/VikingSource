using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.ToGG.HeroQuest.Net
{
    class WaitingRequests
    {
        public List<HeroQuest.Net.AbsNetRequest> members = new List<Net.AbsNetRequest>();
        public List<NetRequestCallback> callbacks = new List<NetRequestCallback>();

        public bool TryPull(ushort id, out HeroQuest.Net.AbsNetRequest request)
        {
            for (int i = 0; i < members.Count; ++i)
            {
                if (id == members[i].id)
                {
                    request = members[i];
                    members.RemoveAt(i);
                    return true;
                }
            }

            request = null;
            return false;
        }

        public void add(AbsNetRequest request)
        {
            for (int i = 0; i < callbacks.Count; ++i)
            {
                if (request.id == callbacks[i].id)
                {
                    Debug.Log("REQ:: found stored callback " + request.id.ToString());
                    //Got callback from the host before recieving the request
                    request.requestCallback(callbacks[i].available);
                    callbacks.RemoveAt(i);
                    return;
                }
            }

            Debug.Log("REQ:: adding request " + request.id.ToString());
            members.Add(request);
        }

        public void debugDelayedAdd(AbsNetRequest request)
        {
            new Timer.TimedAction1ArgTrigger<AbsNetRequest>(add, request, 2000);
        }
    }

    class NetRequestCallback
    {
        public ushort id;
        public bool available;

        public NetRequestCallback(ushort id, bool available)
        {
            this.id = id; this.available = available;
        }
    }
}
