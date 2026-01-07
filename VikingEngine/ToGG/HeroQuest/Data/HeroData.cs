using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.HUD.RichBox;
using VikingEngine.ToGG.HeroQuest.Gadgets;

namespace VikingEngine.ToGG.HeroQuest
{
    class HeroData
    {
        const int RestBloodRageLost = 3;
        const int RestStaminaRegain = 3;
        public const int RestHealthRegain = 6;

        public ValueBar stamina;
        public ValueBar bloodrage;
        public HeroClass heroclass;
        public KillMarkVisuals killmark;

        public StrategyCardDeck availableStrategies;
        public Equipment equipment;

        public HeroData(int stamina, int bloodrage, HeroClass heroclass, KillMarkVisuals killMark)
        {
            this.stamina = new ValueBar(stamina, stamina);
            this.bloodrage = new ValueBar(0, bloodrage);
            this.heroclass = heroclass;
            this.killmark = killMark;
        }

        public static string RestDesc()
        {
            return "Rest Action: Regain " + RestStaminaRegain.ToString() + " " +
                LanguageLib.Stamina + ", and " +
                RestHealthRegain.ToString() + " health. Lose " +
                RestBloodRageLost.ToString() + " " + LanguageLib.BloodRage + ".";
        }

        public static void RestActionsDesc(List<AbsRichBoxMember> rb)
        {
            rb.Add(new RbNewLine());
            rb.Add(new RbText(TextLib.ValuePlusMinus(RestStaminaRegain)));
            rb.Add(new RbImage(SpriteName.cmdStamina));

            rb.Add(new RbNewLine());
            rb.Add(new RbText(TextLib.ValuePlusMinus(RestHealthRegain)));
            rb.Add(new RbImage(SpriteName.cmdStatsHealth));

            rb.Add(new RbNewLine());
            rb.Add(new RbText(TextLib.ValuePlusMinus(-RestBloodRageLost)));
            rb.Add(new RbImage(SpriteName.cmdIconBloodrageSmall));

        }

        public void rest(Unit unit)
        {
            int waterBottles = equipment.hasEquipment(
              new Gadgets.ItemFilter(ItemMainType.Trinket, (int)TrinketType.WaterBottle));

            int healthGain = RestHealthRegain + waterBottles * WaterBottle.HealthGain;

            unit.addStamina(RestStaminaRegain);
            new HealUnit(unit, healthGain, HealType.Nature, false);
            unit.addBloodrage(-RestBloodRageLost);
        }

        public void netWrite(System.IO.BinaryWriter w)
        {
            stamina.WriteByteStream(w);
            bloodrage.WriteByteStream(w);
        }

        public void netRead(System.IO.BinaryReader r)
        {
            stamina.ReadByteStream(r);
            bloodrage.ReadByteStream(r);
        }
    }

    enum HeroClass
    {
        None,
        Recruit,
        Knight,
        Archer,
        Thief,
        Dwarf,
        ShapeShifter,
    }
}
