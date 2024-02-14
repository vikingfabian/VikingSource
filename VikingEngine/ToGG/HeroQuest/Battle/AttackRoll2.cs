using Microsoft.Xna.Framework;
using System.Collections.Generic;
using VikingEngine.ToGG.ToggEngine.BattleEngine;
using VikingEngine.ToGG.ToggEngine.Display2D;
using VikingEngine.ToGG.ToggEngine.GO;

namespace VikingEngine.ToGG.HeroQuest
{
    class AttackRoll2: AbsAttackRoll2
    {
        public Unit attacker;
        public AttackTargetGroup targets;        

        protected Graphics.ImageGroup images = new Graphics.ImageGroup();

        
        public BattleSetup setup;
        //public AttackRollDiceDisplay attackDice;
        public ListWithSelection<DefenderRoll> defenders;

        public bool endDefenceAnimation = false;
        bool simultaniousDefence;
        
        AttackDisplay display;
       
        protected float infoHeight;
        protected float percInfoTextHeight;

        //AttackRollDiceDisplay bumpMotionGroup;
        //Time nextBumpMotionTimer = new Time(Ref.rnd.Float(3f, 8f), TimeUnit.Seconds);
        //int bumpMotionDiceIndex = 0;

       
        int unusedSurges = 0;
        

        public AttackRoll2(Unit attacker, AttackTargetGroup targets,
            AttackDisplay display, StaminaAttackBoost staminaAttackBoost, bool local)
        {
            this.attacker = attacker;
            this.targets = targets;
            this.display = display;

            infoHeight = Engine.Screen.IconSize;
            percInfoTextHeight = 0.8f;

            setup = new BattleSetup(
                new List<AbsUnit> { attacker }, targets, staminaAttackBoost, local); 
        }

        public void initVisuals(VectorRect area)
        {
            attackRollResult.pierce = setup.attackerSetup.pierce;

            var attackDiceArray = setup.attackerSetup.attackDiceArray(attacker);

            AttackWheelsSize(out wheelsWidthCount, out width);
            Vector2 pos = area.Position;
            topLeft = pos;

            attackDice = new AttackRollDiceDisplay(true, setup, new List<AbsUnit> { attacker }, ref pos,
                attackDiceArray, wheelsWidthCount, width);
            listModifiers();

            defenders = new ListWithSelection<DefenderRoll>(targets.Count);
            foreach (var m in targets)
            {
                if (m.unit != null)
                {
                    DefenderRoll defender = new DefenderRoll(m);

                    var dSetup = setup.defenderSetup(m.unit);

                    defender.rollDisplay = new AttackRollDiceDisplay(
                        false, setup, new List<AbsUnit> { defender.unit }, ref pos,
                        dSetup.defDice.Array(),
                        wheelsWidthCount, width);
                    listModifiers(m.unit);
                    defenders.Add(defender, false);
                   
                }
            }
            defenders.selectedIndex = 0;


            float introtime, rolltime;
            toggRef.gamestate.attackRollSpeed.rollTimer(setup.AttackerSetup.attackStrength,
                out simultaniousDefence, out introtime, out rolltime);

            nextResultTimer = new Timer.Basic(rolltime, true);
            introViewTimeAdd = new Time(introtime);

            viewTargetsGui();

            void listModifiers(AbsUnit defender = null)
            {
                BattleModifierLabel label;

                if (defender == null)
                {
                    label = setup.attackerSetup.GetModifierLabel(false);
                }
                else
                {
                    label = setup.defenderSetup(defender).GetModifierLabel(false);
                }

                if (label != null)
                {
                    label.CreateLabel(ref pos, width, images);
                }

                pos.Y += AttackDisplay.WindowsSpacing();
            }
        }
        
        void viewTargetsGui()
        {
            hqRef.playerHud.unitsGui.refresh(targets.listUnits(), Display3D.UnitStatusGuiSettings.HealDamage);
        }

        protected void attackTypeTitle(ref Vector2 pos)
        {
            title(ref pos, setup.targets.AttackType.ToString());
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
            attacker.data.modelSettings.IconSource(attackerIcon, true);

            Graphics.Image attackStrength = new Graphics.Image(AttackType.IsMelee ? SpriteName.cmdStatsMelee : SpriteName.cmdStatsRanged,
                attackerIcon.RightTop, attackerIcon.Size, HudLib.AttackWheelLayer);

            Graphics.TextG attackStrengthText = new Graphics.TextG(LoadedFont.Regular,
                attackStrength.RightTop, Vector2.One, Graphics.Align.Zero, setup.AttackerSetup.baseAttackStrength.ToString(),
                Color.Black, HudLib.AttackWheelLayer);
            attackStrengthText.SetHeight(infoHeight);

            images.Add(attackerIcon);
            images.Add(attackStrength);
            images.Add(attackStrengthText);

            //Support
            if (setup.supportingUnits.total > 0)
            {
                Vector2 supportPos = pos;
                supportPos.X = attackStrengthText.MeasureRightPos() + Engine.Screen.IconSize * 0.5f;

                Graphics.Image supportIcon = new Graphics.Image(SpriteName.cmdSupporterIcon1, supportPos, new Vector2(infoHeight),
                    HudLib.AttackWheelLayer);
                Graphics.TextG supportTotalText = new Graphics.TextG(LoadedFont.Regular, supportIcon.RightTop,
                    Vector2.One, Graphics.Align.Zero, setup.supportingUnits.total.ToString(), Color.Black, HudLib.AttackWheelLayer);
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

        protected void hitPercInfo(ref Vector2 pos, SpriteName icon, float percent)
        {
            Graphics.Image hitIcon = new Graphics.Image(icon, pos, new Vector2(infoHeight), HudLib.AttackWheelLayer);
            Graphics.TextG hitPercText = new Graphics.TextG(LoadedFont.Regular, hitIcon.RightCenter, Vector2.One,
                Graphics.Align.CenterHeight, TextLib.PercentText(percent), Color.Black, HudLib.AttackWheelLayer);
            hitPercText.SetHeight(percInfoTextHeight);

            images.Add(hitIcon);
            images.Add(hitPercText);

            pos.X = hitPercText.MeasureRightPos() + Engine.Screen.IconSize * 0.2f;
        }

        protected Graphics.Image infoBackgroundStart(ref Vector2 topleft)
        {
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

        protected Vector2 initStartPos(VectorRect screenArea)
        {
            Vector2 startPos = new Vector2(
                screenArea.Center.X,
                screenArea.Y + screenArea.Height * 0.1f);

            return startPos;
        }

        public void beginAttack()
        {
            hqRef.gamestate.attackRollSpeed.onAttackRoll();

            attacker.onAttack();
            attacker.OnEvent(ToGG.Data.EventType.Attack, true, null);

            foreach (var m in defenders.list)
            {//Används bara för att trigga "onCommit"
                m.unit.hq().data.defence.collectDefence(attacker, m.unit.hq(), AttackType, true);
            }

            attackDice.dice.onRollClick();
            foreach (var m in defenders.list)
            {
                m.rollDisplay.dice.onRollClick();
            }
        }

        //public static void NetReadAttackStart(System.IO.BinaryReader r)
        //{
        //    Unit attacker = Unit.NetReadUnitId(r);
        //    Unit defender = Unit.NetReadUnitId(r);
        //    Battle.AttackType attacktype = new Battle.AttackType(r);
        //}

        //public void idleUpdate()
        //{
        //    updateDice();

        //    if (nextBumpMotionTimer.CountDown())
        //    {
        //        if (bumpMotionDiceIndex == 0)
        //        {
        //            if (bumpMotionGroup == null)
        //            {
        //                bumpMotionGroup = attackDice;
        //            }
        //            else
        //            {
        //                bumpMotionGroup = bumpMotionGroup == attackDice ? defenders.Selected().rollDisplay : attackDice;
        //            }
        //        }

        //        if (bumpMotionGroup.dice.bumpNextDie(ref bumpMotionDiceIndex))
        //        {
        //            //End of row
        //            bumpMotionDiceIndex = 0;
        //            nextBumpMotionTimer = new Time(Ref.rnd.Float(4f, 6f), TimeUnit.Seconds);
        //        }
        //        else
        //        {
        //            nextBumpMotionTimer.MilliSeconds = 400;
        //        }
        //    }
        //}

        public bool Update()
        {
            if (waitingOnDie != null)
            {
                if (waitingOnDie.state >= AttackRollDiceState.EndBounce)
                {
                    resolveDieResult(waitingOnDie);
                    waitingOnDie = null;
                }
            }
            else if (introViewTimeAdd.CountDown())
            {
               // bool moreDefence = defenders.HasSelection && defenders.Selected().rollDisplay.dice.hasMore();
                bool hasMoreAttacks = attackDice.dice.hasMore(); 

                if (hasMoreAttacks)
                {
                    if (nextResultTimer.Update())
                    {
                        BattleDiceSide rndDieSide = attackDice.dice.nextDie().NextRandom(attacker.Player.Dice);
                        //resolveDieResult(rndDieSide);
                        viewDieResult(rndDieSide);
                    }
                }
                else if (moreDefence())
                {
                    DefenderRoll defender = defenders.Selected();
                    if (!defender.gotFirstDefenceUpdate)
                    {
                        defender.gotFirstDefenceUpdate = true;
                        toggRef.hud.addInfoCardDisplay(new ToggEngine.Display2D.UnitCardDisplay(
                            defender.unit, ToggEngine.Display2D.UnitDisplaySettings.Defender, hqRef.playerHud));
                    }

                    if (nextResultTimer.Update())
                    {
                        if (simultaniousDefence)
                        {
                            for (int i = 0; i < defenders.list.Count; ++i)//each (var m in defenders.list)
                            {
                                stopDefenderDie(defenders.list[i], i);
                            }
                        }
                        else
                        {
                            stopDefenderDie(defender, defenders.selectedIndex);
                        }
                        //var die = defender.rollDisplay.dice.nextDie();
                        //var side = die.NextRandom(defender.unit.Player.Dice);

                        //defender.rollDisplay.dice.setNextResult(side.result);

                        //int blocks = side.blockValue();
                        //if (blocks > 0)
                        //{
                        //    defenceBobble().Add(blocks);
                        //    defender.result.blocks += blocks;
                        //}

                        //if (side.isEnabledAvoid())
                        //{
                        //    defenceBobble().SetAsDodge();
                        //    defender.result.dodge = true;
                        //}

                        //netWriteDefenceResult(side);
                    }
                }
                else //if (finalViewTime.Update())
                {
                    return true;
                }
            }

            updateDice();

            return false;

            bool moreDefence()
            {
                if (simultaniousDefence)
                {
                    foreach (var m in defenders.list)
                    {
                        if (m.rollDisplay.dice.hasMore())
                        {
                            return true;
                        }
                    }

                    return false;
                }
                else
                {
                    return defenders.HasSelection && defenders.Selected().rollDisplay.dice.hasMore();
                }   
            }

            void stopDefenderDie(DefenderRoll defender, int defenderIx)
            {
                if (defender.rollDisplay.dice.hasMore())
                {
                    var die = defender.rollDisplay.dice.nextDie();
                    var side = die.NextRandom(defender.unit.Player.Dice);

                    defender.rollDisplay.dice.setNextResult(side);

                    int blocks = side.blockValue();
                    if (blocks > 0)
                    {
                        defenceBobble(defender).Add(blocks);
                        defender.result.blocks += blocks;
                    }

                    if (side.isEnabledAvoid())
                    {
                        defenceBobble(defender).SetAsDodge();
                        defender.result.dodge = true;
                    }

                    netWriteDefenceResult(defenderIx, side);
                }
            }
        }

        public void applyDamageUpdate()
        {
            updateDice();
        }

        override protected void updateDice()
        {
            foreach (var m in defenders.list)
            {
                m.rollDisplay.dice.update();
            }

            base.updateDice();
            //attackDice.dice.update();
        }

        void netWriteDefenceResult(int defenderIx, BattleDiceSide side)
        {
            var w = Ref.netSession.BeginWritingPacket(Network.PacketType.hqDefenceResult, 
                Network.PacketReliability.Reliable);
            w.Write(display.player is Players.AiPlayer);
            w.Write((byte)side.result);
            w.Write((byte)defenderIx);
            w.Write((byte)defenders.list[defenderIx].result.blocks);
        }

        public void netReadDefenceResult(Network.ReceivedPacket packet)
        {
            BattleDiceResult sideresult = (BattleDiceResult)packet.r.ReadByte();
            
            int defendersSelectedIndex = packet.r.ReadByte();
            defenders.selectedIndex = defendersSelectedIndex;
            int totalBlocks = packet.r.ReadByte();
            if (defenders.selectedIndex == 1)
            {
                lib.DoNothing();
            }

            viewClientDefenceResult(sideresult, totalBlocks);
        }

        public bool nextDefender()
        {
            //finalViewTime.Reset();
            return defenders.Next(false);
        }

        public void clientUpdate()
        {
            foreach (var m in defenders.list)
            {
                m.rollDisplay.dice.update();
            }
            attackDice.dice.update();
        }

        DiceResultLabel defenceBobble(DefenderRoll defender)
        {
            if (defender.rollDisplay.resultLabel1 == null)
            {
                defender.rollDisplay.resultLabel1 = new DiceResultLabel(defender.rollDisplay, true, 2);
            }

            return defender.rollDisplay.resultLabel1;
        }

        

        DiceResultLabel surgeBobble()
        {
            if (attackDice.resultLabel2 == null)
            {
                attackDice.resultLabel2 = new DiceResultLabel(attackDice, false, 1);
            }

            return attackDice.resultLabel2;
        }

        
        
        void resolveDieResult(AttackRollDice die)
        {
            BattleDiceSide side = die.dieSide;

            bool critical;
            int hits = side.hitValue(out critical);

            display.player.SpectatorTargetPos = defenders.Selected().unit.squarePos;

            if (hits > 0)
            {
                addHits(hits, critical);
            }

            if (side.surgeValue() > 0)
            {
                onSurge(side.surgeValue());
            }

            //attackDice.dice.setNextResult(side.result);

            //if (isLocal)
            {
                var w = Ref.netSession.BeginWritingPacket(Network.PacketType.hqDiceRoll, Network.PacketReliability.Reliable);
                w.Write(AttackType.IsMelee);
                w.Write(display.player is Players.AiPlayer);
                w.Write((byte)side.result);
                attackRollResult.write(w);
            }
        }

        

        public void viewClientAttackResult(BattleDiceResult result, AttackRollResult totalRollResult)
        {
            attackDice.dice.setNextResult(new BattleDiceSide(0, result));

            if (totalRollResult.hits + totalRollResult.critiqualHits > 0)
            {
                damageBobble().set(totalRollResult.hits, totalRollResult.critiqualHits);
            }

            if (totalRollResult.surges > 0)
            {
                surgeBobble().set(totalRollResult.surges, 0);
            }
        }

        public void viewClientDefenceResult(BattleDiceResult result, int totalBlocks)
        {
            defenders.Selected().rollDisplay.dice.setNextResult(new BattleDiceSide(0, result));

            if (totalBlocks > 0)
            {
                defenceBobble(defenders.Selected()).set(totalBlocks, 0);
            }
        }

        void onSurge(int value)
        {
            attackRollResult.surges += value;
            surgeBobble().Add(value);

            unusedSurges += value;

            if (display.options != null)
            {
                Data.AbsSurgeOption surgeconv = display.localplayer.surgeOption;

                if (surgeconv != null)
                {
                    int uses = surgeconv.UseCount(ref unusedSurges);
                    if (uses > 0)
                    {
                        display.localplayer.surgeOption.onSurge(uses, display);
                    }
                }
            }
        }

        public void DeleteMe()
        {
            images.DeleteAll();
            attackDice.DeleteMe();
            foreach (var m in defenders.list)
            {
                m.DeleteMe();
            }
        }

        protected BattleDiceResult nextAttack(out bool blockedAttack)
        {
            blockedAttack = false;
            var result = attackDice.dice.nextDie().NextRandom(attacker.Player.Dice).result;

            if (result == BattleDiceResult.Hit1)
            {
                if (setup.hitBlocks > 0)
                {
                    setup.hitBlocks--;
                    blockedAttack = true;
                }
            }

            return result;
        }

        override protected AttackRollDiceDisplay nextDiceGroup(AttackRollDiceDisplay current)
        {
            if (current == null)
            {
                return attackDice;
            }

            return current == attackDice ? defenders.Selected().rollDisplay : attackDice;
        }

        public AttackType AttackType { get { return targets.First.attackType; } }
    }
}
