using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.LF2.GameObjects.EnvironmentObj
{
    interface IInteractionObj
    {
        Vector3 Position { get; }
        void InteractEvent(Characters.Hero hero, bool start);
        bool Interact_LinkClick(HUD.IMenuLink link, Characters.Hero hero, HUD.AbsMenu doc);
        InteractType InteractType { get; }
        bool InRange(Characters.Hero hero);
        string InteractionText { get; }
        HUD.DialogueData Interact_OpeningPhrase(Characters.Hero hero);
        File Interact_TalkMenu(Characters.Hero hero);
        File Interact_MenuTab(int tab, Characters.Hero hero);
    }

    abstract class AbsInteractionObj : AbsVoxelObj, IInteractionObj 
    {
        public AbsInteractionObj()
            : base()
        { }
        public AbsInteractionObj(System.IO.BinaryReader r)
            : base(r)
        {

        }
        virtual public void InteractEvent(Characters.Hero hero, bool start) { }
        public override void Time_Update(UpdateArgs args)
        {
            base.Time_Update(args);
            if (args.halfUpdate == halfUpdateRandomBool)
            {
                Interact_SearchPlayer(this, false);
            }
        }
        
        public override ObjectType Type
        {
            get { return ObjectType.InteractionObj; }
        }

        virtual public InteractType InteractType { get { return ObjInteractType; } }
        

        public static void Interact_SearchPlayer(IInteractionObj obj, bool threaded)
        {
            //look for a hero to sell stuff to
            Characters.Hero hero = LfRef.LocalHeroes[Ref.rnd.Int(LfRef.LocalHeroes.Count)];
            if (obj.InRange(hero))
            {
                //hero.InteractPrompt(obj);
                if (threaded)
                    new Process.UnthreadedInteractPrompt(obj, hero);
                else
                    hero.InteractPrompt(obj);
            }
        }
        virtual public bool InRange(Characters.Hero hero)
        {
            return PositionDiff(hero).Length() < 4;
        }
        virtual public bool Interact_LinkClick(HUD.IMenuLink link, Characters.Hero hero, HUD.AbsMenu doc)
        { throw new NotImplementedException(); }
        virtual public string InteractionText { get { return this.ToString(); } }
        virtual public HUD.DialogueData Interact_OpeningPhrase(Characters.Hero hero) { throw new NotImplementedException(); }
        virtual public File Interact_TalkMenu(Characters.Hero hero) { throw new NotImplementedException(); }
        virtual public File Interact_MenuTab(int tab, Characters.Hero hero)
        { throw new NotImplementedException("Interact_MenuTab"); }
        
        protected override RecieveDamageType recieveDamageType
        {
            get
            {
                return RecieveDamageType.NoRecieving;
            }
        }
    }

    abstract class AbsInteractionNoImageObj : AbsNoImageObj, IInteractionObj
    {
        protected bool runInteractionCheck = true;
        public AbsInteractionNoImageObj()
            : base()
        { }
        public AbsInteractionNoImageObj(System.IO.BinaryReader r)
            : base(r)
        {

        }
        virtual public void InteractEvent(Characters.Hero hero, bool start) { }
        public override void Time_Update(UpdateArgs args)
        {
            base.Time_Update(args);
            if (runInteractionCheck && args.halfUpdate == halfUpdateRandomBool)
            {
                Interact_SearchPlayer(this, false);
            }
        }

        public override ObjectType Type
        {
            get { return ObjectType.InteractionObj; }
        }

        virtual public InteractType InteractType { get { return ObjInteractType; } }


        public static void Interact_SearchPlayer(IInteractionObj obj, bool threaded)
        {
            //look for a hero to sell stuff to
            Characters.Hero hero = LfRef.LocalHeroes[Ref.rnd.Int(LfRef.LocalHeroes.Count)];
            if (obj.InRange(hero))
            {
                //hero.InteractPrompt(obj);
                if (threaded)
                    new Process.UnthreadedInteractPrompt(obj, hero);
                else
                    hero.InteractPrompt(obj);
            }
        }
        virtual public bool InRange(Characters.Hero hero)
        {
            return PositionDiff(hero).Length() < 4;
        }
        virtual public bool Interact_LinkClick(HUD.IMenuLink link, Characters.Hero hero, HUD.AbsMenu doc)
        { throw new NotImplementedException(); }
        virtual public string InteractionText { get { return this.ToString(); } }
        virtual public HUD.DialogueData Interact_OpeningPhrase(Characters.Hero hero) { throw new NotImplementedException(); }
        virtual public File Interact_TalkMenu(Characters.Hero hero) { throw new NotImplementedException(); }
        virtual public File Interact_MenuTab(int tab, Characters.Hero hero)
        { throw new NotImplementedException("Interact_MenuTab"); }

        
    }
}
