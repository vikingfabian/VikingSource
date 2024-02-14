using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.ToGG.HeroQuest.Players;
using VikingEngine.ToGG.ToggEngine.BattleEngine;
using VikingEngine.ToGG.ToggEngine.GO;

namespace VikingEngine.ToGG.HeroQuest
{
    class AttackDisplay
    {
        public StaminaAttackBoost staminaAttackBoost = new StaminaAttackBoost();

        public AttackDiplayButtons buttons;
        public AttackRoll2 attackRoll;
        public AttackResourceOptionsDisplay options;

        public LocalPlayer localplayer;
        public AbsGenericPlayer player;

        VectorRect leftArea, midArea, rightArea;

        public bool hasAttacked = false;
        Time applyDamageTime;
        Time finalViewTime;
        float resultIntroTime, damageExtendTime;

        int state_0Attack_1Damage_3end = 0;        

        public AttackDisplay(Unit attacker, AttackTargetGroup targets, LocalPlayer player)
        {
            this.localplayer = player;
            this.player = player;

            areaSetup();

            attackRoll = new AttackRoll2(
                attacker,
                targets,
                this, 
                staminaAttackBoost, 
                true);
            attackRoll.initVisuals(rightArea);

            buttons = new AttackDiplayButtons(midArea);
            options = new AttackResourceOptionsDisplay(leftArea, this);
        }

        public AttackDisplay(Unit attacker, AttackTargetGroup targets, AiPlayer player, 
            System.IO.BinaryWriter w)
        {
            attacker.writeIndex(w);
            targets.write(w);

            aiSetup(attacker, targets, player, true);
            attackRoll.setup.write(w);

            attackRoll.initVisuals(rightArea);
            beginAttack();
        }

        public AttackDisplay(AiPlayer player, System.IO.BinaryReader r)
        {
            Unit attacker = Unit.NetReadUnitId(r);
            AttackTargetGroup targets = new AttackTargetGroup(r);

            if (attacker != null)
            {
                aiSetup(attacker, targets, player, false);
                attackRoll.setup.read(r);

                attackRoll.initVisuals(rightArea);
                beginAttack();
            }
        }

        public void beginAttack()
        {
            float finalTime;
            hqRef.gamestate.attackRollSpeed.resultTimer(attackRoll.defenders.Count, 
                out resultIntroTime, out damageExtendTime, out finalTime);

            finalViewTime = new Time(finalTime);

            hasAttacked = true;
            removeButtons();

            if (options != null && options.specialArrow != null)
            {
                localplayer.Backpack().SpendItem(options.specialArrow.type, 1);

                switch (((Gadgets.SpecialArrow)options.specialArrow).specialType)
                {
                    case Gadgets.ArrowSpecialType.Piercing2:
                        attackRoll.attackRollResult.pierce += Gadgets.SpecialArrow.MailPierceValue;
                        break;
                }

            }

            attackRoll.beginAttack();

            if (attackRoll.attacker.IsHero &&
                attackRoll.attacker.IsLocal)//.data.hero
            {
                attackRoll.attacker.data.hero.availableStrategies.active.onAttackCommit(this);
            }
        }

        public void cancelAttack()
        {
            attackRoll.attacker.data.hero.stamina.Value += staminaAttackBoost.spentStamina;
        }

        public static float WindowsSpacing()
        {
            return Engine.Screen.BorderWidth * 2f;
        }

        void aiSetup(Unit attacker, AttackTargetGroup targets, AiPlayer player, bool local)
        {
            //this.aiplayer = player;
            this.player = player;

            if (targets.sel == null)
            {
                targets.selectFirst();
            }
            player.getAttackerArrow().updateAttackerArrow(attacker, targets.sel.position);

            areaSetup();

            attackRoll = new AttackRoll2(
                attacker,
                targets,
                this,
                staminaAttackBoost, 
                local);
            
            player.SpectatorTargetPos = targets.sel.unit.squarePos;
        }

        public bool updateAttack()
        {
            switch (state_0Attack_1Damage_3end)
            {
                case 0:           
                    if (attackRoll.Update())
                    {
                        state_0Attack_1Damage_3end = 1;
                        applyDamageTime = new Time(resultIntroTime);

                        if (attackRoll.attackRollResult.hits > 0)
                        {
                            new AnimateAttack(resultIntroTime, attackRoll, attackRoll.defenders.Selected());
                            
                            applyDamageTime.MilliSeconds += damageExtendTime;
                        }
                    }
                    break;

                case 1:
                    attackRoll.applyDamageUpdate();

                    if (applyDamageTime.CountDown())
                    {
                        if (attackRoll.nextDefender())
                        {
                            state_0Attack_1Damage_3end = 0;
                        }
                        else
                        {
                            state_0Attack_1Damage_3end = 2;
                        }
                    }
                    break;

                case 2:
                    if (finalViewTime.CountDown())
                    {
                        onAttackEnded();
                        return true;
                    }
                    break;
            }
            return false;
        }

        void onAttackEnded()
        {
            attackRoll.attacker.onAttackEnded(this, true);
            foreach (var m in attackRoll.targets)
            {
                m.unit.hq().onAttackEnded(this, false);
            }
        }

        public static void NetReadAttackResult(Network.ReceivedPacket packet)
        {
            bool gaveDamage = packet.r.ReadBoolean();

            if (gaveDamage)
            {
                RecordedDamageEvent rec = new RecordedDamageEvent(packet.r);
                rec.apply();
            }
            else
            {
                Unit.NetReadUnitId(packet.r)?.blockAnimation();
            }
        }

        public void refreshAttackCount()
        {
            attackRoll.DeleteMe();

            attackRoll = new  AttackRoll2(
                attackRoll.attacker,
                attackRoll.targets,
                this, staminaAttackBoost, true);
            attackRoll.initVisuals(rightArea);
        }

        void areaSetup()
        {
            int wheelsWidthCount;
            float width;
            AbsAttackAnimation.AttackWheelsSize(out wheelsWidthCount, out width);

            float buttonsWidth = width - Engine.Screen.IconSize * 3;

            float topBottomEdge =  Engine.Screen.SafeArea.Height * 0.1f;

            midArea = new VectorRect(
                Engine.Screen.CenterScreen,
                new Vector2(buttonsWidth, Engine.Screen.SafeArea.Height * 0.5f - topBottomEdge));
            midArea.X -= midArea.Width * 0.5f;

            leftArea = new VectorRect(
                Engine.Screen.CenterScreen.X - width * 0.5f - buttonsWidth, Engine.Screen.SafeArea.Y + topBottomEdge,
                buttonsWidth, Engine.Screen.SafeArea.Height - topBottomEdge * 2f);

            rightArea = new VectorRect(
              Engine.Screen.CenterScreen.X + width * 0.5f, Engine.Screen.SafeArea.Y + topBottomEdge,
               width, Engine.Screen.SafeArea.Height - topBottomEdge * 2f);
        }
       
        public void removeButtons()
        {
            if (buttons != null)
            {
                buttons.DeleteMe();
                buttons = null;
            }
        }

        public void DeleteMe()
        {
            player?.removeAttackerArrow();
            removeButtons();
            attackRoll?.DeleteMe();
            if (options != null)
            {
                options.DeleteMe();
            }
        }
    }
}
