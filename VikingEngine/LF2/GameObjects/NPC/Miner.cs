using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace VikingEngine.LF2.GameObjects.NPC
{
    class Miner : SalesMan
    {
        public Miner(Map.WorldPosition tentPos, Data.Characters.NPCdata data)
            : base(tentPos, data)
        {
            aggresive = Aggressive.Defending;
            NetworkShareObject();
        }
        public Miner(System.IO.BinaryReader r)
            : base(r,  EnvironmentObj.MapChunkObjectType.Miner)
        {
        }
        protected override void loadImage()
        {
            new Process.LoadImage(this, VoxelModelName.Miner, BasicPositionAdjust);
            damageColors = new Effects.BouncingBlockColors(Data.MaterialType.cyan, Data.MaterialType.brown, Data.MaterialType.skin);
        }
        protected override VoxelModelName swordImage
        {
            get
            {
                return VoxelModelName.pickaxe;
            }
        }
        public override HUD.DialogueData Interact_OpeningPhrase(Characters.Hero hero)
        {
            List<string> phrases = new List<string>
            {
                "I hate nights",
                "No, Im not Steve!",
                "Lots of creepy monsters around here",
            };
            return new HUD.DialogueData(ToString(), phrases[Ref.rnd.Int(phrases.Count)]);
        }
        public override string ToString()
        {
            return "Miner";
        }
        protected override EnvironmentObj.MapChunkObjectType dataType
        {
            get
            {
                return EnvironmentObj.MapChunkObjectType.Miner;
            }
        }
        public override SpriteName CompassIcon
        {
            get { return SpriteName.GoodsMetalIron; }
        }
    }
}
