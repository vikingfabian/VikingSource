using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.LF2.GameObjects.NPC
{
#if !CMODE
    class GuardCaptain : AbsNPCWithGifts
    {

#if WINDOWS
        //static Debug.TextOutput debugOutput = new Debug.TextOutput("GuardCaptain");
#endif

        public GuardCaptain(Map.WorldPosition pos,Data.Characters.NPCdata data)
            : base(pos, data)
        {
            setupGifts();
            aggresive = Aggressive.Attacking;
            NetworkShareObject();
        }

        public GuardCaptain(System.IO.BinaryReader r, GameObjects.EnvironmentObj.MapChunkObjectType npcType)
            : base(r, npcType)
        {
            setupGifts();
            loadImage();
        }

        void setupGifts()
        {
            gifts = new Gadgets.GadgetsCollection();
            gifts.AddItem(new Gadgets.WeaponGadget.HandWeapon(Gadgets.WeaponGadget.StandardHandWeaponType.IronSword));
            gifts.AddItem(new Gadgets.WeaponGadget.Bow(Data.Gadgets.BluePrint.LongBow));
            gifts.AddItem(new Gadgets.WeaponGadget.HandWeapon(Gadgets.WeaponGadget.StandardHandWeaponType.Spear));
            gifts.AddItem(new Gadgets.Armor(Gadgets.GoodsType.Iron, true));
            //gifts.AddItem(new Gadgets.Jevelery(Gadgets.Skill.Recylcling_bowman));
            gifts.AddItem(new Gadgets.Goods(Gadgets.GoodsType.Apple_pie, Gadgets.Quality.High, 8));
            gifts.AddItem(new Gadgets.Goods(Gadgets.GoodsType.Iron, Gadgets.Quality.High, 3));
            gifts.AddItem(new Gadgets.Goods(Gadgets.GoodsType.Gold, Gadgets.Quality.High, 2));
            gifts.AddItem(new Gadgets.Goods(Gadgets.GoodsType.Mithril, Gadgets.Quality.High, 1));
            gifts.AddItem(new Gadgets.Goods(Gadgets.GoodsType.Mithril, Gadgets.Quality.Medium, 2));

            gifts.AddMoney(40);
        }

        protected override void loadImage()
        {
            ByteVector2 race = rndRace_hair_skin();
            Data.MaterialType cloth = Data.MaterialType.red;
            new Process.ModifiedImage(this,
                VoxelModelName.npc_male,
                new List<ByteVector2>
                    {
                       new ByteVector2((byte)Data.MaterialType.blue_gray, (byte)cloth), //tunic
                        new ByteVector2((byte)Data.MaterialType.skin, race.Y), //skinn 
                        new ByteVector2((byte)Data.MaterialType.brown, race.X), //hair
                    },
                new List<Process.AddImageToCustom>
                {
                    new Process.AddImageToCustom(VoxelModelName.guardCaptainHead, (byte)Data.MaterialType.brown, race.X, false)
                }, BasicPositionAdjust);

            setDamageColors(cloth, race);
        }

        public override HUD.DialogueData Interact_OpeningPhrase(Characters.Hero hero)
        {
            return new HUD.DialogueData(this, "Good day soldier!");
        }

        protected override void mainTalkMenu(ref File file, Characters.Hero hero)
        {
            //file.Properties.ParentPage = MainMenuKey;
            //if (Map.World.RunningAsHost)
            //{
            //    if (LfRef.gamestate.Progress.GeneralProgress == Data.GeneralProgress.TalkToGuardCaptain1 ||
            //        LfRef.gamestate.Progress.GeneralProgress == Data.GeneralProgress.TalkToGuardCaptain2)
            //    {
            //        file.AddIconTextLink(SpriteName.IconQuestExpression, 
            //            (LfRef.gamestate.Progress.GeneralProgress == Data.GeneralProgress.TalkToGuardCaptain1? "Report for duty" : "Nest destroyed"), 
            //            (int)TalkingNPCTalkLink.TakeQuest1);
            //    }
            //}
            //if (LfRef.gamestate.Progress.GeneralProgress >= Data.GeneralProgress.TalkToGranPa1)
            //{
            //    file.AddTextLink("About nests", (int)TalkingNPCTalkLink.Gossip1);
            //}
            //if (!takenGifts(hero.Player))
            //{
            //    file.AddTextLink("Collect reward", (int)TalkingNPCTalkLink.PickGift);
            //}

        }


        override protected void tookGiftEvent(Players.Player player) { player.Progress.guardCaptainRewards--; }
        override protected bool takenGifts(Players.Player player) { return player.Progress.guardCaptainRewards <= 0; } 
        

        protected override bool LinkClick(ref File file, Characters.Hero hero, TalkingNPCTalkLink name, HUD.IMenuLink link)
        {
            
            switch (name)
            {
                default:
                    return base.LinkClick(ref file, hero, name, link);

                case TalkingNPCTalkLink.PickGift:
                    if (hero != null)
                    {
                        viewGifts(hero.Player, ref file);
                    }
                    break;
                case TalkingNPCTalkLink.Gossip1:
                    new QuestDialogue(new List<IQuestDialoguePart>
                                {
                                    new QuestDialogueSpeach("There should be a couple of hidden monster nests", LoadedSound.Dialogue_Neutral),
                                    new QuestDialogueSpeach("You can expect a reward for each one you destroy", LoadedSound.Dialogue_DidYouKnow),
                                }, this, hero.Player);
                    return true;
                case TalkingNPCTalkLink.TakeQuest1:
                    
                    switch (LfRef.gamestate.Progress.GeneralProgress)
                    {
                        
                        case Data.GeneralProgress.TalkToGuardCaptain1:
                            Data.Progress.BeginQuestDialogue(LfRef.gamestate.Progress.GeneralProgress, hero.Player);
                            LfRef.gamestate.Progress.GeneralProgress = Data.GeneralProgress.DestroyMonsterSpawn;
                            
                            return true;
                        case Data.GeneralProgress.TalkToGuardCaptain2:
                            //if (Engine.XGuide.IsTrial)
                            //{
                            //    new QuestDialogue(new List<IQuestDialoguePart>
                            //    {
                            //        new QuestDialogueSpeach("The trial ends here", LoadedSound.Dialogue_Neutral),
                            //        new QuestDialogueSpeach("You must purchase the game to continue", LoadedSound.Dialogue_DidYouKnow),
                            //    }, this, hero.Player);
                            //}
                            //else
                            {
                                Data.Progress.BeginQuestDialogue(LfRef.gamestate.Progress.GeneralProgress, hero.Player);
                                LfRef.gamestate.Progress.GeneralProgress = Data.GeneralProgress.TalkToGranPa1;
                            }
                            return true;
                    }
                    break;
                case TalkingNPCTalkLink.Exit:
                    return true;
            }

            return false;
        }
        override protected float scale
        {
            get
            {
                return 0.17f;
            }
        }
        protected override EnvironmentObj.MapChunkObjectType dataType
        {
            get { return EnvironmentObj.MapChunkObjectType.Guard_Captain; }
        }

        protected override bool hasQuestExpression
        {

            get {
                if (!Map.World.RunningAsHost)
                    return false;
                return LfRef.gamestate.Progress.GeneralProgress == Data.GeneralProgress.TalkToGuardCaptain1 || 
                    LfRef.gamestate.Progress.GeneralProgress == Data.GeneralProgress.TalkToGuardCaptain2; }
        }
        //public override string ToString()
        //{
        //    return "Guard captain";
        //}
        override protected VoxelModelName swordImage
        {
            get { return VoxelModelName.Sword1; }
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
#endif
}
