using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.ToGG.HeroQuest
{
    class AttackDieResult
    {
        BattleDiceSide rndDieSide;

        bool critical;
        byte blocked = 0;
        int damageGiven = 0;
        int hits;
        int usedBlocksCount = 0;
        int defenderStartHealth;
        

        public AttackDieResult(BattleDiceSide rndDieSide, int defenderBlocks)
        {
            this.rndDieSide = rndDieSide;
            
            hits = rndDieSide.hitValue(out critical);
            
            //activePlayer.SpectatorTargetPos = defender.squarePos;


            for (int i = 0; i < hits; ++i)
            {
                bool isHit = true;

                if (!critical)
                {
                    if (defenderBlocks > 0)
                    {
                        isHit = false;
                        usedBlocksCount++;
                        blocked++;
                        defenderBlocks--;

                        //attackwheels.blockEffect();
                    }
                }

                if (isHit)
                {
                    damageGiven++;
                    //defender.TakeHit(attacker);
                }
            }

            

            //if (rndDieSide.result == BattleDiceResult.Surge)
            //{
            //    onSurge();
            //}

            //attackwheels.setNextResult(rndDieSide.result);

            //if (defender.Dead)
            //{
            //    //Killed unit
            //    attacker.Player.onDestroyedUnit(attacker, defender);
            //    hqLib.KillRageReward((Unit)attacker, true, (Unit)defender);//((Unit)attacker).killReward(true);
            //    foreach (var m in attacks.supportingUnits.units)
            //    {
            //        var supp = ((Unit)m.unit);
            //        if (supp != null)
            //        {
            //            hqLib.KillRageReward(supp, false, (Unit)defender);
            //        }
            //    }

            //    if (display.options != null)
            //    {
            //        display.options.refreshAttackerStats();
            //    }
            //    endAnimation = true;
            //}

            //if (isLocal)
            //{
            //    var w = Ref.netSession.BeginWritingPacket(Network.PacketType.playerAttackResult, Network.PacketReliability.Reliable);
            //    w.Write(display.player is AiPlayer);
            //    w.Write((byte)rndDieSide.result);
            //    w.Write(blocked);
            //}
        }

        //public bool applyResult(AttackDisplay display, bool local)
        //{
        //    bool endAnimation = false;

        //    display.attackAnimation.attackDice.dice.setNextResult(rndDieSide.result);
        //    defenderStartHealth = display.attackAnimation.defenders.Selected().unit.health.Value;

        //    for (int i = 0; i < damageGiven; ++i)
        //    {
        //        display.attackAnimation.defenders.Selected().unit.TakeHit(display.attackAnimation.attacker);
        //    }

        //    if (display.attackAnimation.defenders.Selected().unit.Dead)
        //    {
        //        //Killed unit
        //        display.attackAnimation.attacker.Player.onDestroyedUnit(display.attackAnimation.attacker, display.attackAnimation.defenders.Selected().unit);
        //        hqLib.KillRageReward(display.attackAnimation.attacker, true, display.attackAnimation.defenders.Selected().unit);//((Unit)attacker).killReward(true);
        //        foreach (var m in  display.attackAnimation.attacks.supportingUnits.units)
        //        {
        //            var supp = ((Unit)m.unit);
        //            if (supp != null)
        //            {
        //                hqLib.KillRageReward(supp, false, display.attackAnimation.defenders.Selected().unit);
        //            }
        //        }

        //        if (display.options != null)
        //        {
        //            display.options.refreshAttackerStats();
        //        }
        //        endAnimation = true;
        //    }

        //    return endAnimation;
        //}

    }
}
