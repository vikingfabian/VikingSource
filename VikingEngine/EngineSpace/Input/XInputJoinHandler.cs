using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.Input
{
    class XInputJoinHandler
    {
        bool[] connected;
        public XInputJoinHandler()
        {   
            connected = new bool[XInput.controllers == null? 0 : XInput.controllers.Count];            
        }

        public bool ConnectEvent()
        {
            bool change =  false;

            for(int i = 0; i < connected.Length; ++i)
            {
                if (XInput.controllers[i].Connected!= connected[i])
                {
                    change = true;
                    connected[i] = XInput.controllers[i].Connected;
                }
            }

            return change;
        }

        public List<InputSource> ListConneted()
        {   
            List<InputSource> result = new List<InputSource>();

            if (Ref.steam.isInitialized)
            {
                Ref.steam.input.ListConneted(result);
            }

            if (result.Count == 0 &&
                XInput.controllers != null)
            {
                for (int i = 0; i < XInput.controllers.Count; ++i)
                {
                    if (XInput.controllers[i].Connected)
                    {
                        result.Add(new InputSource(InputSourceType.XController, i));
                    }
                }
            }

            return result;
        }

    }
}
