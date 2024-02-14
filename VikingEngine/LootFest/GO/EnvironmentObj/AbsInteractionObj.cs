using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.LootFest.GO.Characters;

namespace VikingEngine.LootFest.GO.EnvironmentObj
{
    //interface IInteractionObj
    //{
    //    Vector3 Position { get; }
    //    void InteractEvent(PlayerCharacter.AbsHero hero, bool start);
    //    GameObjectType InteractType { get; }
    //    bool InRange(PlayerCharacter.AbsHero hero);
    //    string InteractionText { get; }
    //    bool autoInteract { get; }
    //}

    abstract class AbsInteractionObj : AbsVoxelObj 
    {
        public AbsInteractionObj(GoArgs args)
            : base(args)
        { }
        //public AbsInteractionObj(System.IO.BinaryReader r)
        //    : base(r)
        //{

        //}
        //virtual public void InteractEvent(PlayerCharacter.AbsHero hero, bool start) { }
        public override void Time_Update(UpdateArgs args)
        {
            base.Time_Update(args);
            if (args.halfUpdate == halfUpdateRandomBool)
            {
                Interact2_SearchPlayer(false);
            }
        }
        
        //public override GameObjectType Type
        //{
        //    get { return GameObjectType.InteractionObj; }
        //}

        //virtual public GameObjectType InteractType { get { return Type; } }
        

        //public static void Interact_SearchPlayer(IInteractionObj obj, bool threaded)
        //{
        //    //look for a hero to interact with
        //    PlayerCharacter.AbsHero hero = LfRef.LocalHeroes[Ref.rnd.Int(LfRef.LocalHeroes.Count)];
        //    if (obj.InRange(hero))
        //    {
        //        if (threaded)
        //            new Process.UnthreadedInteractPrompt(obj, hero);
        //        else
        //            hero.InteractPrompt(obj);
        //    }
        //}
        //virtual public bool InRange(PlayerCharacter.AbsHero hero)
        //{
        //    return PositionDiff(hero).Length() < 4;
        //}
        //virtual public bool Interact_LinkClick(HUD.IMenuLink link, PlayerCharacter.AbsHero hero, HUD.AbsMenu doc)
        //{ throw new NotImplementedException(); }
        //virtual public string InteractionText { get { return this.ToString(); } }
        //virtual public bool autoInteract {get{ return false;} }
        
        //virtual public HUD.DialogueData Interact_OpeningPhrase(PlayerCharacter.AbsHero hero) { throw new NotImplementedException(); }
        //virtual public File Interact_TalkMenu(PlayerCharacter.AbsHero hero) { throw new NotImplementedException(); }
        //virtual public File Interact_MenuTab(int tab, PlayerCharacter.AbsHero hero)
        //{ throw new NotImplementedException("Interact_MenuTab"); }
        
        protected override RecieveDamageType recieveDamageType
        {
            get
            {
                return RecieveDamageType.NoRecieving;
            }
        }
    }

    abstract class AbsInteractionNoImageObj : AbsNoImageObj
    {
        protected bool runInteractionCheck = true;
        public AbsInteractionNoImageObj(GoArgs args)
            : base(args)
        { }
        //public AbsInteractionNoImageObj(System.IO.BinaryReader r)
        //    : base(r)
        //{

        //}
       // virtual public void InteractEvent(PlayerCharacter.AbsHero hero, bool start) { }
        public override void Time_Update(UpdateArgs args)
        {
            base.Time_Update(args);
            //if (runInteractionCheck && args.halfUpdate == halfUpdateRandomBool)
            //{
            //    Interact_SearchPlayer(this, false);
            //}
            if (runInteractionCheck && args.halfUpdate == halfUpdateRandomBool)
            {
                Interact2_SearchPlayer(false);
                //Interact_SearchPlayer(this, false);
            }
        }

        //public override GameObjectType Type
        //{
        //    get { return GameObjectType.InteractionObj; }
        //}

        //virtual public GameObjectType InteractType { get { return Type; } }


        //public void Interact_SearchPlayer(IInteractionObj obj, bool threaded)
        //{
        //    //look for a hero to sell stuff to
        //    PlayerCharacter.AbsHero hero = LfRef.LocalHeroes.GetRandom();// LfRef.LocalHeroes[Ref.rnd.Int(LfRef.LocalHeroes.Count)];
        //    if (obj.InRange(hero))
        //    {
        //        //hero.InteractPrompt(obj);
        //        if (threaded)
        //            new Process.UnthreadedInteractPrompt(obj, hero);
        //        else
        //        {
        //            if (obj.autoInteract)
        //                obj.InteractEvent(hero, true);
        //            else
        //                hero.InteractPrompt(obj);

        //        }
        //    }
        //}
        //virtual public bool InRange(PlayerCharacter.AbsHero hero)
        //{
        //    return PositionDiff(hero).Length() < 4;
        //}
        //virtual public bool Interact_LinkClick(HUD.IMenuLink link, PlayerCharacter.AbsHero hero, HUD.AbsMenu doc)
        //{ throw new NotImplementedException(); }
        //virtual public string InteractionText { get { return this.ToString(); } }
        //virtual public bool autoInteract { get { return false; } }
        //virtual public HUD.DialogueData Interact_OpeningPhrase(PlayerCharacter.AbsHero hero) { throw new NotImplementedException(); }
        //virtual public File Interact_TalkMenu(PlayerCharacter.AbsHero hero) { throw new NotImplementedException(); }
        //virtual public File Interact_MenuTab(int tab, PlayerCharacter.AbsHero hero)
        //{ throw new NotImplementedException("Interact_MenuTab"); }

        
    }
}
