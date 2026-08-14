using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.GameObject.ObjectPointer;
using VikingEngine.DSSWars.Players;

namespace VikingEngine.DSSWars.Communication
{
    abstract class AbsFlag
    {
        public IntVector2 tilePos;
        public bool inCullingView = false;
    }

    class RelationFlag : AbsFlag
    {
        public bool cityPos;
        public PFaction pfaction;

        public RelationType relation;
        //Vector2 screenCenter;
        public Graphics.ImageAdvanced flag = null;
        public Graphics.Image bg = null;
        public Graphics.Image relationIcon = null;
        public Graphics.ImageGroupParent2D ImageGroup;

        long positionUpdate = 0;
        public Vector2 position;

        public RelationFlag(PFaction faction)
        {
            this.pfaction = faction;
            //if (faction == 75)
            //{
            //    lib.DoNothing();
            //}
            //DssRef.world.faction(faction)?.refreshMainCity();
        }

        public void updatePos(LocalPlayer player, Faction faction)
        {
            if (positionUpdate != Ref.TotalFrameCount)
            {
                positionUpdate = Ref.TotalFrameCount;
                Vector3 wp = Vector3.Zero;
                var landAreaCenter = faction.landAreaCenter(out cityPos);
                wp.X = landAreaCenter.X + 0.5f;
                wp.Z = landAreaCenter.Y - 6;

                position = player.playerData.view.From3DToScreenPos(wp);
            }
        }
    }

    class QuestFlag : AbsFlag
    {
        public Graphics.Image icon = null;
        public AbsWorldObject GameObject = null;
    }
}
