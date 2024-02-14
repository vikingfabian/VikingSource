using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using VikingEngine.Input;

namespace VikingEngine.LootFest.GO.EnvironmentObj
{
    class ButtonTutorialTriggerArea : AbsInteractionNoImageObj
    {
        IButtonMap btn;
        string text;

        public ButtonTutorialTriggerArea(GoArgs args, IButtonMap btn, string text)
            :base(args)
        {
            this.btn = btn;
            this.text = text;

            Vector3 halfSize = new Vector3(14);
            halfSize.Y = 64;

            this.WorldPos = args.startWp;

            CollisionAndDefaultBound = new Bounds.ObjectBound(Bounds.BoundShape.BoundingBox, args.startPos, halfSize, Vector3.Zero);
        }


        public override void InteractVersion2_interactEvent(VikingEngine.LootFest.GO.PlayerCharacter.AbsHero hero, bool start)
        {
            if (hero != null)
            {
                if (hero.player.interactDisplay == null || hero.player.interactDisplay is Display.ButtonTutorialLabel == false)
                {
                    hero.player.deleteInteractDisplay();
                    hero.player.interactDisplay = new Display.ButtonTutorialLabel(hero.player, btn, text);
                }
                else
                {
                    hero.player.interactDisplay.refresh(hero.player, this);
                }   
            }
            
        }

        protected override bool Interact2_HeroCollision(PlayerCharacter.AbsHero hero)
        {
            return hero.CollisionAndDefaultBound.MainBound.Intersect2(CollisionAndDefaultBound.MainBound) != null;//re turn CollisionBound.Intersect(hero.Position);
        }

        public override void Time_Update(UpdateArgs args)
        {
            base.Time_Update(args);
            if (localMember)
            {
                if (checkOutsideUpdateArea_StartChunk())
                {
                    DeleteMe();
                    return;
                }
            }
        }

        public override void AsynchGOUpdate(UpdateArgs args)
        {
            base.AsynchGOUpdate(args);
        }

        public override GameObjectType Type
        {
            get { return GameObjectType.Teleport; }
        }
        public override bool Interact_AutoInteract
        {
            get
            {
                return true;
            }
        }
    }
}
