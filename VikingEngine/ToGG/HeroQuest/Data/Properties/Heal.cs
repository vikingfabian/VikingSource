using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.ToGG.Data.Property;
using VikingEngine.ToGG.ToggEngine.Display2D;
using VikingEngine.ToGG.ToggEngine.GO;

namespace VikingEngine.ToGG.HeroQuest.Data.Property
{
    class DarkHeal : AbsMonsterAction
    {
        BellValue heal;
        int range;

        public DarkHeal()
        { }

        public DarkHeal(int heal, int range)
        {
            this.heal = new BellValue(heal);
            this.range = range;
        }

        public override void writeData(BinaryWriter w)
        {
            base.writeData(w);
            heal.write(w);
            w.Write((byte)range);
        }

        public override void readData(BinaryReader r)
        {
            base.readData(r);
            heal = BellValue.Read(r);
            range = r.ReadByte();
        }

        public override AbsPerformUnitAction ai_useAction(AbsUnit activeUnit, SpecialActionPriority priority)
        {
            if (!used)
            {
                FindMaxValuePointer<AbsUnit> bestTarget = new FindMaxValuePointer<AbsUnit>();

                var friendly = HeroQuest.hqRef.players.CollectFriendlyUnits(activeUnit);
                while (friendly.Next())
                {
                    if (friendly.sel != activeUnit)
                    {
                        int heal = friendly.sel.needHealing(HealType.Dark);
                        if (heal > 0 && activeUnit.InRangeAndSight(friendly.sel.squarePos, range, true, false))
                        {
                            bestTarget.Next(heal, friendly.sel);
                        }
                    }
                }

                if (bestTarget.HasMember)
                {
                    used = true;

                    return new PerformHealAction(activeUnit, this, bestTarget.maxMember,
                        new HealSettings(heal.Next(activeUnit.Player.Dice), HealType.Dark));
                }
            }
            return null;
        }

        public override SpriteName Icon => SpriteName.cmdDarkHeal;

        public override string Name => "Dark heal";

        public override string Desc => LanguageLib.SpecialActionDescStart + "Restore " +
            heal.IntervalToString() + " health to an undead unit. Range " + range.ToString();

        public override SpecialActionClass ActionClass => SpecialActionClass.FriendlyBuff;
               
        public override MonsterActionType Type => MonsterActionType.DarkHeal;
    }

    class PerformHealAction : AbsPerformUnitAction
    {
        //HealSettings healSett;
        HealUnit heal;

        public PerformHealAction(System.IO.BinaryReader r)
            : base(r)
        { }

        public PerformHealAction(AbsUnit parentUnit, AbsMonsterAction parentAction, AbsUnit target, 
            HealSettings healSett)
            : base(parentUnit, parentAction)
        {
            //this.healSett = healSett;
            heal = new HealUnit(target.hq(), healSett, false, false);
            //this.target = target;
        }

        public override void onBegin()
        {
            sayAction();
        }

        public override bool update()
        {
            base.update();

            if (timeStamp.event_ms(500))
            {
                if (heal.reciever != null)
                {
                    spectatorPos = heal.reciever.squarePos;
                    heal.apply();
                    //if (isLocalAction)
                    //{
                    //    //target.hq().heal(heal);
                    //}
                }
            }

            return timeStamp.msPassed(1500);            
        }

        protected override void netWrite(BinaryWriter w)
        {
            base.netWrite(w);
            //Debug.WriteCheck(w);
            //target.hq().netWriteUnit(w);
            heal.write(w);
        }

        protected override void netRead(BinaryReader r)
        {
            base.netRead(r);
            //Debug.ReadCheck(r);
            //target = Unit.NetReadUnit(r);
            heal = new HealUnit(r); //.read(r);
        }
        
        public override ToggEngine.QueAction.QueActionType Type => ToggEngine.QueAction.QueActionType.PerformHeal;
    }
}
