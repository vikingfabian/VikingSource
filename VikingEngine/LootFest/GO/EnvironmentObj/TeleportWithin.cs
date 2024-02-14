using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.LootFest.Map;
using VikingEngine.LootFest.GO.PlayerCharacter;
using VikingEngine.Input;
using VikingEngine.LootFest.BlockMap;

namespace VikingEngine.LootFest.GO.EnvironmentObj
{
    class TeleportWithin : AbsInteractionNoImageObj
    {
         /* Static readonly */
        public static readonly Vector3 HalfSize = new Vector3(2f, 4f, 2f);

        /* Fields */
        //Map.WorldPosition toWp;
        TeleportLocationId toLocation;

        public TeleportWithin(GoArgs args)//, Map.WorldPosition toWp)
            : base(args)
        {
            //this.toWp = toWp;
            toLocation = (TeleportLocationId)args.characterLevel;
            this.WorldPos = args.startWp;
            
            CollisionAndDefaultBound = new Bounds.ObjectBound(new BoundData2(new VikingEngine.Physics.StaticBoxBound(
                new VectorVolume(args.startPos, HalfSize)), Vector3.Zero));
        }


        public override void InteractVersion2_interactEvent(PlayerCharacter.AbsHero hero, bool start)
        {
            if (start)
            {
                if (hero != null && hero.player.inGamePlay)
                {
                    hero.teleportToLocation(toLocation);//.teleportWithinLevel(toWp);
                }
            }
        }

        protected override bool Interact2_HeroCollision(PlayerCharacter.AbsHero hero)
        {
            return hero.CollisionAndDefaultBound.MainBound.Intersect2(CollisionAndDefaultBound.MainBound) != null;
        }
        override public bool Interact_AutoInteract { get { return true; } }

        public override void Time_Update(UpdateArgs args)
        {
            if (localMember)
            {
                if (checkOutsideUpdateArea_ActiveChunk())
                {
                    DeleteMe();
                    return;
                }
            }
            base.Time_Update(args);
        }
        public override GameObjectType Type
        {
            get { return GameObjectType.TeleportWithin; }
        }
    }
}
