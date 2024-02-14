using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.PJ.Moba
{
    class MobaPlayState : AbsPJGameState
    {
        public MobaPlayState(List2<GamerData> joinedGamers)
            : base(true)
        {
            MobaLib.Init();
            
            this.joinedLocalGamers = joinedGamers;
            
            

            MobaRef.gamers = new List<LocalGamer>(joinedGamers.Count);
            new GO.ObjectCollection();
            new Map();
            
            foreach (var m in joinedGamers)
            {
                new LocalGamer(m);
            }

            new AsynchUpdateable(asynchUpdate, "MOBA main asynch update", 0);
            //Ref.asynchUpdate.AddUpdateThread(asynchUpdate, "MOBA main asynch update", 0);
        }

        

        public override void Time_Update(float time)
        {
            base.Time_Update(time);

            MobaRef.objects.Update();

            foreach (var m in MobaRef.gamers)
            {
                m.update();
            }

            if (Input.Keyboard.KeyDownEvent(Microsoft.Xna.Framework.Input.Keys.PageDown))
            {
                new LobbyState(true);
            }
        }

        bool asynchUpdate(int id, float time)
        {
            MobaRef.objects.UpdateAsynch();
            return false;
        }
    }
}
