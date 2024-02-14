using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.Graphics;

namespace VikingEngine.LF2.GameObjects.NPC
{
    

    class GranPa : AbsNPC
    {
        public GranPa(Map.WorldPosition pos, Data.Characters.NPCdata data)
            : base(pos, data)
        {
            socialLevel = SocialLevel.Interested;
            aggresive = Aggressive.Flee;
            Health = float.MaxValue;
            NetworkShareObject();
        }

        public GranPa(System.IO.BinaryReader r, GameObjects.EnvironmentObj.MapChunkObjectType npcType)
            : base(r, npcType)
        {
            loadImage();
        }

        protected override WeaponAttack.DamageData attackDamage
        {
            get
            {
                return new WeaponAttack.DamageData(2, WeaponAttack.WeaponUserType.Friendly, this.ObjOwnerAndId);
            }
        }
#if !CMODE
        public override HUD.DialogueData Interact_OpeningPhrase(Characters.Hero hero)
        {
            //I knew another young man that reminds me of you, but he grew up
            return new HUD.DialogueData(this, new List<string>
                {
                    "Good day my son!"
                });
        }
#endif
        protected override bool LinkClick(ref File file, Characters.Hero hero, TalkingNPCTalkLink name, HUD.IMenuLink link)
        {
#if !CMODE
            switch (name)
            {
                case TalkingNPCTalkLink.TakeQuest1:
                    List<IQuestDialoguePart> mission = null;
                    switch (LfRef.gamestate.Progress.GeneralProgress)
                    {
                        case Data.GeneralProgress.TalkToGranPa1:
                            LfRef.gamestate.Progress.GeneralProgress++;
                            Data.Progress.BeginQuestDialogue(Data.GeneralProgress.TalkToGranPa1, hero.Player);
                            break;
                        case Data.GeneralProgress.TalkToGranPa2_BoughtApples:
                            if (heroGotApples(hero))
                            {
                                LfRef.gamestate.Progress.GeneralProgress++;
                                Data.Progress.BeginQuestDialogue(Data.GeneralProgress.TalkToGranPa2_BoughtApples, hero.Player);
                            }
                            else
                            {
                                mission = new List<IQuestDialoguePart>
                                {
                                    new QuestDialogueSpeach("You need to collect " + Data.Gadgets.BluePrintLib.ApplePieNumApples.ToString() + "apples", LoadedSound.MenuNotAllowed),
                                };
                            }
                            break;
                        case Data.GeneralProgress.TalkToGranPa3_HasApplePie:
                            if (hero.Player.Progress.Items.Goods.GotItemAnyQuality(Gadgets.GoodsType.Apple_pie, 1))
                            {
                                LfRef.gamestate.Progress.GeneralProgress++;
                                Data.Progress.BeginQuestDialogue(Data.GeneralProgress.TalkToGranPa3_HasApplePie, hero.Player);
                                hero.Player.SetQuestLogTutorial();
                            }
                            else
                            {
                                mission = new List<IQuestDialoguePart>
                                {
                                    new QuestDialogueSpeach("Where is my apple pie?!", LoadedSound.MenuNotAllowed)
                                };
                            }
                            break;
                    }
                    if (mission != null)
                        new QuestDialogue(mission, this, hero.Player);
                    return true;
            }
            return base.LinkClick(ref file, hero, name, link);
#else
            throw new Exception();
#endif
        }

        bool heroGotApples(Characters.Hero hero)
        {
            return hero.Player.Progress.Items.Goods.GotItemAnyQuality(Gadgets.GoodsType.Apple, Data.Gadgets.BluePrintLib.ApplePieNumApples);
        }

        protected override void mainTalkMenu(ref File file, Characters.Hero hero)
        {
#if !CMODE
            //file.Properties.ParentPage = MainMenuKey;
            SpriteName buttonImage = SpriteName.NO_IMAGE;
            
            if (LfRef.gamestate.Progress.GeneralProgress == Data.GeneralProgress.TalkToGranPa1 || 
                LfRef.gamestate.Progress.GeneralProgress == Data.GeneralProgress.TalkToGranPa2_BoughtApples || 
                LfRef.gamestate.Progress.GeneralProgress == Data.GeneralProgress.TalkToGranPa3_HasApplePie)
            {
                string questName;
                switch (LfRef.gamestate.Progress.GeneralProgress)
                {
                    default:
                        questName = "Quest";
                        break;
                    case Data.GeneralProgress.TalkToGranPa2_BoughtApples:
                        questName = "Give apples";
                        if (!heroGotApples(hero))
                            buttonImage = SpriteName.LFMenuRectangleGray;
                        break;
                    case Data.GeneralProgress.TalkToGranPa3_HasApplePie:
                        questName = "Give apple pie";
                        break;
                }

                file.AddIconTextLink(SpriteName.IconQuestExpression, questName, null,new HUD.Link((int)TalkingNPCTalkLink.TakeQuest1), buttonImage);
            }
#endif
        }
      
        protected override void loadImage()
        {
            new Process.LoadImage(this, VoxelModelName.granpa2, BasicPositionAdjust);
            damageColors = new Effects.BouncingBlockColors(Data.MaterialType.white, Data.MaterialType.blue_gray, Data.MaterialType.skin);
        }


        override protected Graphics.AnimationsSettings AnimationsSettings
        {
            get { return new Graphics.AnimationsSettings(3,  1.1f, 1); }
        }
        

        protected override float waitingModeTime
        {
            get
            {
                return base.waitingModeTime + 6000;
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

        override protected VoxelModelName swordImage
        {
            get { return VoxelModelName.stick; }
        }
        
        protected override EnvironmentObj.MapChunkObjectType dataType
        {
            get { return EnvironmentObj.MapChunkObjectType.Granpa; }
        }

        const float WalkingSpeed = 0.0012f;
        override protected float walkingSpeed
        { get { return WalkingSpeed; } }

        protected override bool hasQuestExpression
        {
            get 
            {
                if (!Map.World.RunningAsHost)
                    return false;
                return  LfRef.gamestate.Progress.GeneralProgress == Data.GeneralProgress.TalkToGranPa1 ||
                    LfRef.gamestate.Progress.GeneralProgress == Data.GeneralProgress.TalkToGranPa2_BoughtApples ||
                    LfRef.gamestate.Progress.GeneralProgress == Data.GeneralProgress.TalkToGranPa3_HasApplePie;
            }
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
