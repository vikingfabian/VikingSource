using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace VikingEngine.LF2.GameObjects.NPC
{
    class Messenger : AbsNPC
    {
        public Messenger(Map.WorldPosition pos)
            : base(pos, null)
        {
            socialLevel = SocialLevel.HelpfulLooter;
            aggresive = Aggressive.Flee;
            Health = float.MaxValue;

            //spawn monsters
            const int NumMonsters = 4;
            for (int i = 0; i < NumMonsters; i++)
            {
                new Characters.Monsters.Hog(pos, 0);
            }
            NetworkShareObject();
        }
        public Messenger(System.IO.BinaryReader r, GameObjects.EnvironmentObj.MapChunkObjectType npcType)
            : base(r, npcType)
        {
            loadImage();
        }
        override protected Graphics.AnimationsSettings AnimationsSettings
        {
            get { return StandardCharacterAnimations; }
        }
        

        protected override void loadImage()
        {

            new Process.LoadImage(this, VoxelModelName.messenger, BasicPositionAdjust);
        }
        public override HUD.DialogueData Interact_OpeningPhrase(Characters.Hero hero)
        {
            List<string> result;
            if ( monsterFree)
            {
                result = new List<string>
                {
                    "I owe you one!",
                    "Phew! happy to be alive",
                    "It is dangerous to go alone!",
                };
            }
            else
            {
                result = new List<string> { "Help!", "I can't hold on much longer!", "Get me out of here!" };
            }
            

            return new HUD.DialogueData(this, result);
        }
        override public File Interact_TalkMenu(Characters.Hero hero)
        {
            if (LfRef.gamestate.Progress.GeneralProgress <= Data.GeneralProgress.HelpMessenger)
            {
                if (!monsterFree)
                {
                    string say;
                    switch (Ref.rnd.Int(3))
                    {
                        default:
                            say = "Help me!";
                            break;
                        case 1:
                            say = "The monsters are attacking me!";
                            break;
                        case 2:
                            say = "Get rid of the monsters!";
                            break;


                    }

                    new QuestDialogue(
                        new List<IQuestDialoguePart>
                        {
                             new QuestDialogueSpeach(say, LoadedSound.Dialogue_DidYouKnow),

                        }, 
                        this, hero.Player);

                    return null;
                }
                return base.Interact_TalkMenu(hero);
            }
            else
            {
                string say;
                if (monsterFree)
                {
                    say = "Thank you for helping me out!";
                }
                else
                {
                    say = "I'd better move on";
                }
                new QuestDialogue(
                         new List<IQuestDialoguePart>{new QuestDialogueSpeach(say, LoadedSound.Dialogue_QuestAccomplished)}, this, hero.Player);
                return null;
            }
           
        }
        protected override void mainTalkMenu(ref File file, Characters.Hero hero)
        {
#if !CMODE
            if (LfRef.gamestate.Progress.GeneralProgress == Data.GeneralProgress.HelpMessenger)
            {
                file.AddIconTextLink(SpriteName.IconQuestExpression, "Message", (int)TalkingNPCTalkLink.TakeQuest1);
            }
            
#endif
        }
       

         protected override bool LinkClick(ref File file, Characters.Hero hero, TalkingNPCTalkLink name, HUD.IMenuLink link)
        {
#if !CMODE
            switch (name)
            {
                case TalkingNPCTalkLink.TakeQuest1:
                    LfRef.gamestate.Progress.GeneralProgress++;
                    Data.Progress.BeginQuestDialogue(Data.GeneralProgress.HelpMessenger, hero.Player);
                    break;
            }
            return true;
#endif
         }

        

        bool monsterFree = false;

        public override void AIupdate(GameObjects.UpdateArgs args)
        {

            base.AIupdate(args);
            //check if he is free from monsters
            const float MonsterFreeRadius = Map.WorldPosition.ChunkWidth * 1.2f;
            bool monsterFree = true;
            args.allMembersCounter.Reset();
            while (args.allMembersCounter.Next())
            //foreach (AbsUpdateObj obj in active)
            {
                if (args.allMembersCounter.GetMember.Type == ObjectType.Character && args.allMembersCounter.GetMember.WeaponTargetType == WeaponAttack.WeaponUserType.Enemy)
                {
                    float dist = distanceToObject(args.allMembersCounter.GetMember);
                    if (dist < MonsterFreeRadius)
                    {
                        monsterFree = false;
                        break;
                    }
                }
            }
            this.monsterFree = monsterFree;

        }

        public override void DeleteMe(bool local)
        {
            base.DeleteMe(local);
            if (LfRef.gamestate.Progress.GeneralProgress == Data.GeneralProgress.HelpMessenger)
            {
                LfRef.gamestate.Progress.GeneralProgress = Data.GeneralProgress.TalkToFather_MessengerReset;
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
                return 42;
            }
        }


        override protected VoxelModelName swordImage
        {
            get { return VoxelModelName.stick; }
        }
        //override protected bool willStoreInChunk { get { return false; } }
        protected override EnvironmentObj.MapChunkObjectType dataType
        {
            get { return EnvironmentObj.MapChunkObjectType.Messenger; }
        }
        public override string ToString()
        {
            return "Messenger";
        }
        protected override bool hasQuestExpression
        {
            get {
                if (!Map.World.RunningAsHost)
                    return false;
                return LfRef.gamestate.Progress.GeneralProgress == Data.GeneralProgress.HelpMessenger; }
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
