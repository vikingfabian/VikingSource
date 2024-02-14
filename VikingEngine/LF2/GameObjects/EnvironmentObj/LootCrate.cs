using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.LF2.GameObjects.EnvironmentObj
{
    class LootCrate : EnvironmentObj.AbsInteractionObj
    {
        const float ChestScale = 0.2f;
        Map.Terrain.Area.Castle castle; 
        Corner corner;

        public LootCrate(Map.Terrain.Area.Castle castle, Corner corner, Map.WorldPosition pos)
            :base()
        {
            this.castle = castle;
            this.corner = corner;

            System.Diagnostics.Debug.WriteLine("Loot crate created, " + pos.ChunkGrindex.ToString());

            float scale;
            image = LF2.Data.ImageAutoLoad.AutoLoadImgInstace(VoxelModelName.chest_open, Chest.ChestTempImage, 0, 1);
            scale = ChestScale;
            image.position = pos.BlockTopFaceV3();

            image.position.Y = LfRef.chunks.GetScreen(pos).GetGroundY(pos);
            image.scale = lib.V3(scale);
        }

        public override void InteractEvent(Characters.Hero hero, bool start)
        {
            if (start)
            {
                if (castle.bossKeyCorner == corner)
                {
                    if (LfRef.gamestate.Progress.BossKey(castle.areaLevel, hero, true))
                    {
                        //set key progress and view it visually
                        new Effects.BossKey(Position, hero);
                    }
                }

                GameObjects.Gadgets.GadgetsCollection coll = CalcTreasure();
                var items = coll.ToList();
                foreach (var i in items)
                {
                    hero.Player.AddItem(i, true);
                }

                LfRef.gamestate.Progress.OpenLootCrate(castle.areaLevel, corner);
                this.DeleteMe();
                hero.InteractingWith = null;
                hero.pickupEffeckt();
            }
        }

        public GameObjects.Gadgets.GadgetsCollection CalcTreasure()
        {
            GameObjects.Gadgets.GadgetsCollection GadgetColl = new Gadgets.GadgetsCollection();

            int lootLevel = castle.areaLevel + 1;

            int amount = 1 + Ref.rnd.Int(2);
            for (int i = 0; i < amount; i++)
            {
                GadgetColl.AddItem(LootfestLib.GetRandomAnyGadget(lootLevel));

            }
            if (Ref.rnd.RandomChance(20))
            {
                GadgetColl.AddItem(LootfestLib.GetRandomRareGadget());
            }

            return GadgetColl;
        }

        public override GameObjects.InteractType ObjInteractType
        { //also undertype
            get
            {
                return GameObjects.InteractType.LootCrate;
            }
        }

        public override string ToString()
        {
            return "Loot crate";
        }

        public override string InteractionText
        {
            get
            {
                return "Loot";
            }
        }

        public override void Time_Update(UpdateArgs args)
        {
            if (args.halfUpdate == halfUpdateRandomBool)
            {
                Interact_SearchPlayer(this, false);
            }
        }

        override public NetworkShare NetworkShareSettings { get { return GameObjects.NetworkShare.None; } }
        public override int UnderType
        {
            get { return (int)MapChunkObjectType; }
        }
        virtual public GameObjects.EnvironmentObj.MapChunkObjectType MapChunkObjectType { get { return EnvironmentObj.MapChunkObjectType.LootCrate; } }
    }

}
