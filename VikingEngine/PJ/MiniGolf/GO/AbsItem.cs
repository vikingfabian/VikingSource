using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.PJ.MiniGolf
{
    abstract class AbsItem : AbsGameObject
    {
        public AbsItem()
        {
            
        }

        virtual public void onPickup(Ball b, Physics.Collision2D collision)
        {
        }

        

        public override void DeleteMe()
        {
            GolfRef.objects.Remove(this);
            if (inUpdateList)
            {
                GolfRef.objects.RemoveFromUpdate(this);
            }
            base.DeleteMe();
        }

        virtual public bool BallObsticle { get { return false; } }
    }

    enum ObjectType
    {
        None,
        Ball,
        BumpWave,
        Coin,
        FlagPoint,
        Hole,
        BlockingBox,
        Bomb,
        ObsticleBug,
    }
}
