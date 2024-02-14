using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using VikingEngine.ToGG.ToggEngine.GO;

namespace VikingEngine.ToGG.ToggEngine.BattleEngine
{
    abstract class AbsAttackAnimation
    {
       // public const float AttackTime = 800;
        protected Graphics.ImageGroup2D images = new Graphics.ImageGroup2D();

        protected Time introViewTimeAdd = new Time(0.4f, TimeUnit.Seconds);
        protected Timer.Basic nextResultTimer = new Timer.Basic(800, true);
        protected Time finalViewTime = new Time(1.5f, TimeUnit.Seconds);
        protected List<Graphics.Mesh> supporters = new List<Graphics.Mesh>();
        public AbsUnit attacker, defender;
        public AttackType attackType;
        //protected
        public int retreats = 0;
        
        public SlotMashineWheelStrip attackwheels;
        public BattleEngine.AbsBattleSetup attacks;
        protected AbsGenericPlayer activePlayer;

        protected int wheelsWidthCount;
        protected float width;
        protected float infoHeight;
        protected float percInfoHeight;
        protected Vector2 topLeft;

        public AbsAttackAnimation(AbsUnit attacker, AbsUnit defender, AttackType AttackType, AbsGenericPlayer activePlayer)
        {
            infoHeight = (int)(Engine.Screen.IconSize * 0.8f);
            percInfoHeight = (int)(infoHeight * 0.8f);

            this.activePlayer = activePlayer;
            
            this.attackType = AttackType;
            this.attacker = attacker;
            this.defender = defender;

            if (activePlayer is Commander.Players.AiPlayer)
            {
                introViewTimeAdd.AddSeconds(0.2f);
            }
            activePlayer.SpectatorTargetPos = attacker.squarePos;
            
            activePlayer.getAttackerArrow().updateAttackerArrow(attacker, defender.squarePos);
        }

        virtual public void beginAttack()
        {
            attacker.AttackedThisTurn = true;
            if (attackType.IsMelee)
            {
                new CloseCombatEffect(attacker.squarePos, defender.squarePos);
            }
            else
            {
                new RangedCombatEffect(attacker.squarePos, defender.squarePos);
            }
        }

        protected BattleDice[] multiplyDie(BattleDice die, int count)
        {
            BattleDice[] result = new BattleDice[count];
            for (int i = 0; i < count; ++i)
            {
                result[i] = die;
            }

            return result;
        }

        protected void placement(VectorRect screenArea, out Vector2 startPos)
        {
            AttackWheelsSize(out wheelsWidthCount, out width);

            startPos = initStartPos(screenArea);

            topLeft = startPos;
        }

        public static void AttackWheelsSize(out int wheelsWidthCount, out float width)
        {
            wheelsWidthCount = (int)((Engine.Screen.Height * 0.52f) / (SlotMashineWheel.Size.X + SlotMashineWheel.SpacingX));
            width = wheelsWidthCount * (SlotMashineWheel.Size.X + SlotMashineWheel.SpacingX) - SlotMashineWheel.SpacingX;
        }

        virtual protected Vector2 initStartPos(VectorRect screenArea)
        {
            Vector2 startPos;

            if (activePlayer.pData.Type == Engine.PlayerType.Ai)
            {
                startPos.X = Engine.Screen.SafeArea.Right - width;
            }
            else
            {
                const float SideAdj = 1f;

                Vector2 defenderPos2D = Ref.draw.Camera.From3DToScreenPos(VectorExt.AddX(defender.soldierModel.Position, SideAdj), Engine.Draw.defaultViewport);
                startPos.X = defenderPos2D.X;

                if (startPos.X + width > screenArea.Right)
                {
                    defenderPos2D = Ref.draw.Camera.From3DToScreenPos(VectorExt.AddX(defender.soldierModel.Position, -SideAdj), Engine.Draw.defaultViewport);
                    startPos.X = defenderPos2D.X - width;
                }
            }
            startPos.Y = Engine.Screen.Height * 0.3f;

            return startPos;
        } 

        protected void attackTypeTitle(ref Vector2 pos)
        {
            title(ref pos, attackType.ToString());
        }

        protected void title(ref Vector2 pos, string text)
        {
            
            var title = new Graphics.TextG(LoadedFont.Regular, VectorExt.AddX(pos, width * 0.5f), Engine.Screen.TextSizeV2,
                Graphics.Align.CenterWidth, text, Color.White, HudLib.AttackWheelLayer);

            VectorRect titleArea = new VectorRect(pos.X, pos.Y, width, title.MeasureText().Y);
            titleArea.AddYRadius(Engine.Screen.BorderWidth);

            Graphics.Image titleBg = new Graphics.Image(SpriteName.WhiteArea,
                titleArea.Position, titleArea.Size, HudLib.AttackWheelLayer + 1);
            titleBg.Color = Color.Black;
            titleBg.Opacity = 0.9f;

            images.Add(title); images.Add(titleBg);
            pos.Y = titleBg.Bottom;
        }

        protected void attackerAndSupportInfo(ref Vector2 pos, BattleDice die)
        {
            var bgImage = infoBackgroundStart(ref pos);

            Graphics.ImageAdvanced attackerIcon = new Graphics.ImageAdvanced(SpriteName.NO_IMAGE,
                pos, new Vector2(infoHeight), HudLib.AttackWheelLayer, false);
            attacker.Data.modelSettings.IconSource(attackerIcon, true);

            Graphics.Image attackStrength = new Graphics.Image(attacks.targets.IsMelee? SpriteName.cmdStatsMelee : SpriteName.cmdStatsRanged,
                attackerIcon.RightTop, attackerIcon.Size, HudLib.AttackWheelLayer);

            Graphics.TextG attackStrengthText = new Graphics.TextG(LoadedFont.Regular,
                attackStrength.RightTop, Vector2.One, Graphics.Align.Zero, attacks.AttackerSetup.baseAttackStrength.ToString(),
                Color.Black, HudLib.AttackWheelLayer);
            attackStrengthText.SetHeight(infoHeight);

            images.Add(attackerIcon);
            images.Add(attackStrength);
            images.Add(attackStrengthText);

            //Support
            if (attacks.supportingUnits.total > 0)
            {
                Vector2 supportPos = pos;
                supportPos.X = attackStrengthText.MeasureRightPos() + Engine.Screen.IconSize * 0.5f;

                Graphics.Image supportIcon = new Graphics.Image(SpriteName.cmdSupporterIcon1, supportPos, new Vector2(infoHeight),
                    HudLib.AttackWheelLayer);
                Graphics.TextG supportTotalText = new Graphics.TextG(LoadedFont.Regular, supportIcon.RightTop,
                    Vector2.One, Graphics.Align.Zero, attacks.supportingUnits.total.ToString(), Color.Black, HudLib.AttackWheelLayer);
                supportTotalText.SetHeight(infoHeight);
                images.Add(supportIcon); images.Add(supportTotalText);
            }

            pos.Y += infoHeight + Engine.Screen.BorderWidth;

            {//Hit percent
                Vector2 hitPercInfoStartPos = pos;

                foreach (var m in die.sides)
                {
                    if (m.chance > 0)
                    {
                        hitPercInfo(ref hitPercInfoStartPos, BattleDice.ResultIcon(m.result), m.chance);
                    }
                }
                pos.Y += infoHeight;
            }

            infoBackgroundEnd(ref pos, bgImage);
            
        }

        protected Graphics.Image infoBackgroundStart(ref Vector2 topleft)
        {
            //infoHeight * 2f + Engine.Screen.BorderWidth * 5f
            Graphics.Image bgImage = new Graphics.Image(SpriteName.WhiteArea,
                topleft, new Vector2(width, 1), HudLib.AttackWheelLayer + 1);
            bgImage.Opacity = 0.8f;
            images.Add(bgImage);

            topleft += new Vector2(Engine.Screen.BorderWidth);

            return bgImage;
        }

        protected void infoBackgroundEnd(ref Vector2 infoBottom, Graphics.Image bgImage)
        {
            infoBottom.X = topLeft.X;
            infoBottom.Y += Engine.Screen.BorderWidth;
            bgImage.SetBottom(infoBottom.Y, true);
        }

        protected void hitPercInfo(ref Vector2 pos, SpriteName icon, float percent)
        {
            Graphics.Image hitIcon = new Graphics.Image(icon, pos, new Vector2(infoHeight), HudLib.AttackWheelLayer);
            Graphics.TextG hitPercText = new Graphics.TextG(LoadedFont.Regular, hitIcon.RightCenter, Vector2.One,
                Graphics.Align.CenterHeight, TextLib.PercentText(percent), Color.Black, HudLib.AttackWheelLayer);
            hitPercText.SetHeight(percInfoHeight);

            images.Add(hitIcon);
            images.Add(hitPercText);

            pos.X = hitPercText.MeasureRightPos() + Engine.Screen.IconSize * 0.2f;
        }
        

        //protected void listAllModifiers(ref Vector2 pos)
        //{//List all modifiers 
        //    var modifiers = attacks.allModifiers();

        //    float modInfoHeight = infoHeight * 0.8f;

        //    pos.Y += Engine.Screen.IconSize* 0.5f;
        //    foreach (var m in modifiers)
        //    {
        //        m.CreateLabel(ref pos, modInfoHeight, width, images);
        //        pos.Y += Engine.Screen.BorderWidth;
        //    }
        //}


        /// <returns>Exit animation</returns>
        abstract public bool Update();
       

        //abstract protected void onResult(BattleDiceResult result, bool blockedAttack);

        //protected void blockEffect()
        //{
        //    blocks.Add(new BlockHitEffect(attackWheels[currentAttack], true));
        //}

        abstract protected BattleDiceResult nextAttack(out bool blockedAttack);

        //virtual protected bool hasMoreAttacks()
        //{
            
        //}

        protected void supporterEffects(AttackSupport supportingUnits)
        {
            foreach (var m in supportingUnits.units)
            {
                int givesSupport = 1;
                if (!attackType.Is(AttackUnderType.BackStab))
                {
                    if (m.unit.HasProperty(UnitPropertyType.Flank_support))
                    {
                        givesSupport = 2;
                    }
                }

                SupportGui(m.unit, givesSupport, supporters);

                if (attackType.IsMelee)
                {
                    new CloseCombatEffect(m.unit.squarePos, defender.squarePos);
                }
                else
                {
                    new RangedCombatEffect(m.unit.squarePos, defender.squarePos);
                }
            }
        }

        public static void SupportGui(AbsUnit u, int support, List<Graphics.Mesh> images)
        {
            //Graphics.Mesh supportIcon = new Graphics.Mesh(LoadedMesh.plane, u.soldierModel.Position,
            //    new Vector3(0.5f), Graphics.TextureEffectType.Flat, 
            //    support == 1 ? SpriteName.cmdIconSupport_1 : SpriteName.cmdIconSupport_2,
            //    Color.White);
            //        //new Graphics.TextureEffect(Graphics.TextureEffectType.Flat, 
            //        //support == 1 ? SpriteName.cmdIconSupport_1 : SpriteName.cmdIconSupport_2), 0.5f);
            //supportIcon.Y = 0.7f;

            //images.Add(supportIcon);
        }

        virtual public void DeleteMe()
        {
            images.DeleteAll();
            attackwheels.DeleteMe();

            foreach (var icon in supporters)
            {
                icon.DeleteMe();
            }
            

            activePlayer.removeAttackerArrow();
        }
    }
    
}
