using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.LF2.GameObjects.NPC
{
    class HorseTransport : AbsNPC
    {
        static readonly Data.TempBlockReplacementSett HorseTempImage = new Data.TempBlockReplacementSett(new Color(183,131,69), new Vector3(2, 3, 4));
        TalkingHorse talkingHorse;
        
        Graphics.AbsVoxelObj horse;

        public HorseTransport(Map.WorldPosition startPos,Data.Characters.NPCdata data)
            : base(startPos, data)
        {
            createHorse(startPos.ToV3() + new Vector3(2, 0, 2), true);

            socialLevel = SocialLevel.Interested;
            aggresive = Aggressive.Flee;
           // Health = float.MaxValue;
            SetStartY();

            spawnPos = startPos;
            NetworkShareObject();
        }

        public HorseTransport(System.IO.BinaryReader r, GameObjects.EnvironmentObj.MapChunkObjectType npcType)
            : base(System.IO.BinaryReader, npcType)
        {
            createHorse(GameObjects.AbsUpdateObj.ReadPosition(System.IO.BinaryReader), false);
            loadImage();
        }


        void createHorse(Vector3 pos, bool local)
        {
            horse = LF2.Data.ImageAutoLoad.AutoLoadImgInstace(VoxelModelName.horsetransport, HorseTempImage, 0, 0); // LootfestLib.Images.StandardObjInstance(VoxelModelName.horsetransport);
            horse.position = pos;
            horse.scale = lib.V3(0.5f);
            talkingHorse = new TalkingHorse(horse.position, local);
        }

        public override void ObjToNetPacket(System.IO.BinaryWriter writer)
        {
            base.ObjToNetPacket(writer);
            GameObjects.AbsUpdateObj.WritePosition(horse.position, writer);
        }

        public override void SetStartY()
        {
            if (horse != null)
            {
                base.SetStartY();
                Map.WorldPosition wp = new Map.WorldPosition(horse.position);
                horse.position.Y = LfRef.chunks.GetScreen(wp).GetGroundY(wp);
            }
        }

        protected override void loadImage()
        {
            ByteVector2 race = rndRace_hair_skin();

            new Process.ModifiedImage(this,
                VoxelModelName.npc_male,
                new List<ByteVector2>
                    {
                new ByteVector2((byte)Data.MaterialType.blue_gray, (byte)Data.MaterialType.brown), //tunic
                new ByteVector2((byte)Data.MaterialType.skin, race.Y), //skinn 
                new ByteVector2((byte)Data.MaterialType.brown, race.X), //hair
                    },
                new List<Process.AddImageToCustom>
                {
                    new Process.AddImageToCustom(VoxelModelName.horse_hat, (byte)Data.MaterialType.brown, race.X, false)
                }, BasicPositionAdjust);
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
                return 16;
            }
        }

        public override HUD.DialogueData Interact_OpeningPhrase(Characters.Hero hero)
        {

            List<string> say = null;

            if (Map.World.RunningAsHost)
            {
                if (LfRef.gamestate.Progress.GeneralProgress == Data.GeneralProgress.TalkToFather2_HogKilled)
                {
                    say = new List<string> { "You should talk to your father before you go anywhere" };
                }
                else if (LfRef.gamestate.Progress.GeneralProgress == Data.GeneralProgress.TalkToGuardCaptain1 && !hero.Player.Progress.TakenGifts)
                {
                    say = new List<string> { "Don't forget the weapons your father is offering you" };
                }
            }
            if (say == null)
            {
                say = new List<string>
                {
                    "Want a ride?",
                    "Safe and fast transportation!",
                    "Your legs need some rest",
                };
            }
            return new HUD.DialogueData("Horse transport", say[Ref.rnd.Int(say.Count)]);

            
        }

        protected override void mainTalkMenu(ref File file, Characters.Hero hero)
        {
            //file.Properties.ParentPage = MainMenuKey;
            hero.Player.MyMoneyToMenu(file);

            if (Map.World.RunningAsHost && LfRef.gamestate.Progress.GeneralProgress == Data.GeneralProgress.HorseTravel)
            {
                file.AddIconTextLink(SpriteName.IconQuestExpression, "About travel", (int)TalkingNPCTalkLink.TakeQuest1);
            }
            else
            {
                file.Add(new LargeShopButtonData(SpriteName.IconTravel, "Travel", "Pay for a quick travel to a friendly settlement",
                    LootfestLib.TravelCost, hero.Player.Progress.Items.Money, new MenuLink(hero.Player, quickTravel, 0)));
            }

           
        }

        protected override bool LinkClick(ref File file, Characters.Hero hero, TalkingNPCTalkLink name, HUD.IMenuLink link)
        {

            switch (name)
            {
                case TalkingNPCTalkLink.TakeQuest1:
                    LfRef.gamestate.Progress.GeneralProgress = Data.GeneralProgress.TalkToGuardCaptain1;
                    Data.Progress.BeginQuestDialogue(Data.GeneralProgress.HorseTravel, hero.Player);
                    
                    return true;
                case TalkingNPCTalkLink.Exit:
                    return true;
                   
            }

            return false;
        }

        void quickTravel(Players.Player p, int non)
        {
            if (p.Progress.Items.Money >= LootfestLib.TravelCost)
            {
                p.SelectTravelDestination();
            }
        }


        public override void Time_Update(UpdateArgs args)
        {
            base.Time_Update(args);
            talkingHorse.Update(args.allMembersCounter);
        }
        protected override EnvironmentObj.MapChunkObjectType dataType
        {
            get { return EnvironmentObj.MapChunkObjectType.Horse_Transport; }
        }
        protected override bool hasQuestExpression
        {
            get {
                if (!Map.World.RunningAsHost)
                    return false;
                return (LfRef.gamestate.Progress.GeneralProgress == Data.GeneralProgress.HorseTravel && Map.World.RunningAsHost) 
                    ||
                    (LfRef.gamestate.Progress.GeneralProgress >= Data.GeneralProgress.AskAround &&
                          questTip.Type != Data.Characters.QuestTipType.NON);}
        }
        public override void DeleteMe(bool local)
        {
            base.DeleteMe(local);
            horse.DeleteMe();
        }
        public override SpriteName CompassIcon
        {
            get { return SpriteName.IconTravel; }
        }
        protected override bool Immortal
        {
            get { return true; }
        }
    }

    class TalkingHorse : VikingEngine.LF2.GameObjects.EnvironmentObj.IInteractionObj
    {
        LF2.ObjMultiBound bound;
        bool talkedOnce = false;
        Vector3 pos;
        const string Name = "Horse";
        public TalkingHorse(Vector3 pos, bool local)
        {
            if (local)
            {

                Map.WorldPosition wp = new Map.WorldPosition(pos);
                wp.WorldGrindex.Y = Map.WorldPosition.ChunkStandardHeight + 1;
                pos.Y = LfRef.chunks.GetScreen(wp).GetGroundY(wp);
            }
            this.pos = pos;

            bound = new LF2.ObjMultiBound(new LF2.BoundData2[]
            {
              new LF2.BoundData2(new Physics.StaticBoxBound(new VectorVolume(Vector3.Zero, new Vector3(1.7f, 1.5f, 2.1f))), new Vector3(-3.4f, 0, 0)),
              new LF2.BoundData2(new Physics.StaticBoxBound(new VectorVolume(Vector3.Zero, new Vector3(3.4f, 1.2f, 1.4f))), new Vector3(0.5f, 0, 0)),
            });
            
            bound.UpdatePosition2(Rotation1D.D0, pos);
        }

        public Vector3 Position { get { return pos; } }
        public void InteractEvent(Characters.Hero hero, bool start) { }

        public bool Interact_LinkClick(HUD.IMenuLink link, Characters.Hero hero, HUD.AbsMenu doc)
        {
            return !talkedOnce;
        }
        public InteractType InteractType { get { return GameObjects.InteractType.SpeakDialogue; }  }
        public bool InRange(Characters.Hero hero) { return (hero.Position - pos).Length() < 6f; }
        public string InteractionText { get { return Name; } }
        public HUD.DialogueData Interact_OpeningPhrase(Characters.Hero hero)
        {
            return null;
        }

        int numInteractions = 0;
        public File Interact_TalkMenu(Characters.Hero hero)
        {
            List<IQuestDialoguePart> say;
            ++numInteractions;

            if (numInteractions == 20)
            {
                say = new List<IQuestDialoguePart>
                {
                    new QuestDialogueSpeach("For the 20th time, NEIGH!!", LoadedSound.Dialogue_DidYouKnow),
                };
            }
            else if (numInteractions == 21)
            {
                say = new List<IQuestDialoguePart>
                {
                    new QuestDialogueSpeach("What do you expect me to say?", LoadedSound.Dialogue_Question),
                     new QuestDialogueSpeach("Horses don't speak, retard!", LoadedSound.Dialogue_DidYouKnow),
                     new QuestDialogueSpeach("Go and bug someone else", LoadedSound.Dialogue_DidYouKnow),
                };
            }
            else
            {
                say = new List<IQuestDialoguePart>
            {
                 new QuestDialogueSpeach("Neigh!", LoadedSound.Dialogue_Neutral),

            };
            }
            

            new QuestDialogue(
                say, this, hero.Player);

            talkedOnce = true;
            return null;
        }

        public void Update(ISpottedArrayCounter<GameObjects.AbsUpdateObj> active)
        {
            if (!talkedOnce)
            {
                EnvironmentObj.AbsInteractionObj.Interact_SearchPlayer(this, false);
            }
            active.Reset();
            while (active.Next())
            {
                if (active.GetMember.SolidBody)
                {
                    if (LF2.AbsObjBound.SolidBodyIntersect(active.GetMember,active.GetMember.CollisionBound, null, bound)) //null, pos, bound, active.Member, active.Member.Position, active.Member.CollisionBound))
                        return;
                }
            }
        }
        public override string ToString()
        {
            return Name;
        }
        public File Interact_MenuTab(int tab, Characters.Hero hero)
        { throw new NotImplementedException("Interact_MenuTab"); }
    }
}
