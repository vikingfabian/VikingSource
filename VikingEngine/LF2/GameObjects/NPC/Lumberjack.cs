using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace VikingEngine.LF2.GameObjects.NPC
{
    class Lumberjack : SalesMan
    {
        public Lumberjack(Map.WorldPosition tentPos, Data.Characters.NPCdata data)
            : base(tentPos, data)
        {
            aggresive = Aggressive.Attacking;
            NetworkShareObject();
        }
        public Lumberjack(System.IO.BinaryReader r)
            : base(r,  EnvironmentObj.MapChunkObjectType.Lumberjack)
        {
        }
        protected override void loadImage()
        {
            new Process.LoadImage(this, VoxelModelName.Lumberjack, BasicPositionAdjust);
            damageColors = new Effects.BouncingBlockColors(Data.MaterialType.red, Data.MaterialType.red_brown, Data.MaterialType.light_gray);
        }
        protected override VoxelModelName swordImage
        {
            get
            {
                return VoxelModelName.doubleaxe;
            }
        }
        public override HUD.DialogueData Interact_OpeningPhrase(Characters.Hero hero)
        {
            List<string> phrases = new List<string>
            {
                "Killing animals is a big hobby of mine!",
                "We should level the forest and build a town here",
                "Man should be in control of nature",
            };
            return new HUD.DialogueData(ToString(), phrases[Ref.rnd.Int(phrases.Count)]);
        }
        public override string ToString()
        {
            return "Lumberjack";
        }
        protected override EnvironmentObj.MapChunkObjectType dataType
        {
            get
            {
                return EnvironmentObj.MapChunkObjectType.Lumberjack;
            }
        }
        public override SpriteName CompassIcon
        {
            get { return SpriteName.GoodsWood; }
        }
        protected override bool Immortal
        {
            get { return false; }
        }
    }
}
