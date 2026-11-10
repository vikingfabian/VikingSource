using Microsoft.Xna.Framework;
using System;
using VikingEngine.Core.Wand.Map;
using VikingEngine.Graphics;

namespace VikingEngine.Core.Wand.GO.Creature
{
    abstract class AbsCreature : AbsGameObject
    {

        public void SetPos(MapPlacement placementData)
        {
            this.currentPos = placementData;
            refreshPos();
        }

        protected void refreshPos()
        {
            model.position = WP.TileToWp(currentPos.tilePos);
            WP.DirToQuaterion(model, currentPos.direction);

        }
    }
}
