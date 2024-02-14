using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace VikingEngine.LF2.GameObjects.NPC
{
    class WarVeteran : AbsNPC
    {
        bool haveSaidGossip = false;
        bool haveSaidQuestLog = false;
        bool haveSaidGoal = false;
        //bool haveSaidWeaponBelt = false;

        public WarVeteran(Map.WorldPosition pos, Data.Characters.NPCdata data)
            : base(pos, data)
        {
            socialLevel = SocialLevel.Interested;
            aggresive = Aggressive.Attacking;
            Health = float.MaxValue;
            NetworkShareObject();
        }
       public WarVeteran(System.IO.BinaryReader r, GameObjects.EnvironmentObj.MapChunkObjectType npcType)
            : base(r, npcType)
        {
            loadImage();
        }
#if !CMODE
        public override HUD.DialogueData Interact_OpeningPhrase(Characters.Hero hero)
        {
             return new HUD.DialogueData(this, new List<string>
                {
                    "Battles were more honorable when I was young",
			        "I might be retired, but I can still cleave a man with one swing",
                    "There is still some fighting left in this body!"
                });
        }
#endif
        protected override bool LinkClick(ref File file, Characters.Hero hero, TalkingNPCTalkLink name, HUD.IMenuLink link)
        {
#if !CMODE
            switch (name)
            {
                case TalkingNPCTalkLink.TakeQuest1:
                    LfRef.gamestate.Progress.SetVisitedArea(LfRef.worldOverView.AreasInfo[Map.Terrain.AreaType.Castle][0].position, null, true);
                    LfRef.gamestate.Progress.GeneralProgress = Data.GeneralProgress.AskAround;
                    Data.Progress.BeginQuestDialogue(Data.GeneralProgress.TalkToWarVeteran, hero.Player);
                    break;
                case TalkingNPCTalkLink.TakeQuest2:
                    Data.Progress.BeginQuestDialogue(Data.GeneralProgress.WarVeteranInfo_Gossip, hero.Player);
                    haveSaidGossip = true;
                    break;
                case TalkingNPCTalkLink.TakeQuest3:
                    Data.Progress.BeginQuestDialogue(Data.GeneralProgress.WarVeteranInfo_Progress, hero.Player);
                    haveSaidQuestLog = true;
                    break;
                case TalkingNPCTalkLink.TakeQuest4:
                    Data.Progress.BeginQuestDialogue(Data.GeneralProgress.WarVeteranInfo_Goal, hero.Player);
                    haveSaidGoal = true;
                    break;
            }
            return true;
#else
            throw new Exception();
#endif
        }

        protected override void mainTalkMenu(ref File file, Characters.Hero hero)
        {
#if !CMODE
            //file.Properties.ParentPage = MainMenuKey;

            if (Map.World.RunningAsHost)
            {
                if (LfRef.gamestate.Progress.GeneralProgress == Data.GeneralProgress.TalkToWarVeteran)
                {
                    file.AddIconTextLink(SpriteName.IconQuestExpression, "Seen magicians?", (int)TalkingNPCTalkLink.TakeQuest1);
                }
                else if (LfRef.gamestate.Progress.GeneralProgress == Data.GeneralProgress.AskAround)
                {
                    file.AddIconTextLink(SpriteName.IconQuestExpression, "About gossip", null, 
                        new HUD.Link((int)TalkingNPCTalkLink.TakeQuest2), haveSaidGossip? SpriteName.LFMenuRectangleGray : SpriteName.NO_IMAGE);
                    file.AddIconTextLink(SpriteName.IconQuestExpression, "Quest log", null,
                        new HUD.Link((int)TalkingNPCTalkLink.TakeQuest3), haveSaidQuestLog ? SpriteName.LFMenuRectangleGray : SpriteName.NO_IMAGE);
                    file.AddIconTextLink(SpriteName.IconQuestExpression, "Final goal", null,
                        new HUD.Link((int)TalkingNPCTalkLink.TakeQuest4), haveSaidGoal ? SpriteName.LFMenuRectangleGray : SpriteName.NO_IMAGE);
                   
                }
            }
#endif
        }

        protected override void loadImage()
        {
            new Process.LoadImage(this, VoxelModelName.war_veteran, BasicPositionAdjust);
            damageColors = new Effects.BouncingBlockColors(Data.MaterialType.red_brown, Data.MaterialType.light_brown, Data.MaterialType.skin);
        }
        override protected Graphics.AnimationsSettings AnimationsSettings
        {
            get { return StandardCharacterAnimations; }
        }
        protected override float waitingModeTime
        {
            get
            {
                return base.waitingModeTime + 4000;
            }
        }

        protected override bool willTalk
        {
            get { return true; }
        }
        protected override float maxWalkingLength
        {
            get
            {
                return 15;
            }
        }
        protected override float scale
        {
            get
            {
                return 0.15f;
            }
        }

        override protected VoxelModelName swordImage
        {
            get { return VoxelModelName.warvet_sword; }
        }

        protected override EnvironmentObj.MapChunkObjectType dataType
        {
            get { return EnvironmentObj.MapChunkObjectType.War_veteran; }
        }
        protected override bool hasQuestExpression
        {
            get 
            {  
                return LfRef.gamestate.Progress.GeneralProgress == Data.GeneralProgress.TalkToWarVeteran && Map.World.RunningAsHost
                    ||
                    (LfRef.gamestate.Progress.GeneralProgress == Data.GeneralProgress.AskAround && 
                    !(haveSaidGoal && haveSaidGossip && haveSaidQuestLog));
            }
        }
        const float WalkingSpeed = 0.0013f;
        override protected float walkingSpeed
        { get { return WalkingSpeed; } }

        public override string ToString()
        {
            return "War veteran";
        }

        public override SpriteName CompassIcon
        {
            get { return SpriteName.NO_IMAGE; }
        }
        protected override bool Immortal
        {
            get { return true; }
        }
    }
}
