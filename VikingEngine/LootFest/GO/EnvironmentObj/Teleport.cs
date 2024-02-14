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
    /// <summary>
    /// Move between the levels in lootfest3.
    /// Look at the public static readonly Vector3 HalfSize to see the size.
    /// </summary>
    class Teleport : AbsInteractionNoImageObj
    {
        /* Static readonly */
        public static readonly Vector3 HalfSize = new Vector3(2f);

        /* Fields */
        public LevelEnum fromLevel;
        public TeleportLocationId toLocation;
        public Dir4 enterDirection;

        VikingEngine.LootFest.GO.Bounds.ObjectBound displayLabelBound;

        /// <summary>
        /// Look at the public static readonly Vector3 HalfSize to see the size.
        /// </summary>
        public Teleport(GoArgs args, Dir4 enterDirection,
            LevelEnum fromLevel, TeleportLocationId toLocation)
            : base(args)
        {
            this.toLocation = toLocation;
            this.fromLevel = fromLevel;
            this.WorldPos = args.startWp;
            this.enterDirection = enterDirection;
            rotation = conv.ToRadians(enterDirection).Add(MathHelper.Pi);

            position = args.startPos;

            CollisionAndDefaultBound = new Bounds.ObjectBound(new BoundData2(new VikingEngine.Physics.StaticBoxBound(
                    new VectorVolume(position, HalfSize)), Vector3.Zero));
            displayLabelBound = new Bounds.ObjectBound(new BoundData2(new VikingEngine.Physics.StaticBoxBound(
                    new VectorVolume(position, HalfSize * 3)), Vector3.Zero));

        }


        public override void InteractVersion2_interactEvent(PlayerCharacter.AbsHero hero, bool start)
        {
            if (start)
            {
                if (hero != null && hero.player.inGamePlay)
                {
                    //var currentLvl = LfRef.levels2.GetLevelUnsafe(fromLevel);
                    //if (currentLvl != null)
                    //{
                    //    currentLvl.onTeleport(hero, toLevel);
                    //}
                    Debug.CrashIfThreaded();
                    //hero.teleportToLocation(toLocation);
                    hero.teleportInteraction(toLocation, false, this);
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

                PlayerCharacter.AbsHero hero = LfRef.LocalHeroes.GetRandom();


                //if (toLocation != TeleportLocationId.NUM &&
                //    toLocation != LevelEnum.Lobby )
                    //&&
                    //fromLevel != LevelEnum.Tutorial)
                //{
                    //PlayerCharacter.AbsHero hero = LfRef.LocalHeroes.GetRandom();

                    //bool open = true;//hero.Player.Storage.completedLevels[(int)toLocation].unlocked;

                if (hero.CollisionAndDefaultBound.Intersect2(displayLabelBound) != null)
                {
                    hero.teleportInteraction(toLocation, true, this);
                    //if (hero.Player.interactDisplay == null ||
                    //    hero.Player.interactDisplay is Display.LevelProgressLabel == false)
                    //{
                    //    hero.Player.deleteInteractDisplay();
                    //    hero.Player.interactDisplay = new Display.LevelProgressLabel(hero.Player, this);
                    //}
                    //else
                    //{
                    //    hero.Player.interactDisplay.refresh(hero.Player, this);
                    //}
                }
                //}
            }
            base.Time_Update(args);
        }

        public override void DeleteMe(bool local)
        {
            base.DeleteMe(local);
            displayLabelBound.DeleteMe();
            CollisionAndDefaultBound.DeleteMe();
        }

        public override GameObjectType Type
        {
            get { return GameObjectType.Teleport; }
        }
    }

}
