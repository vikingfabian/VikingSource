using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Conscript;
using VikingEngine.DSSWars.GameObject.DetailObj.Data;
using VikingEngine.HUD.RichBox;

namespace VikingEngine.DSSWars.Resource
{
    struct ShieldProperties
    {
        public float meleeSpeedBonus;
        public float blocksRefillTimeSecMultiply;
        public int armorBonus;
        public float moveSpeedMultiply;

        public ShieldProperties()
        {
            moveSpeedMultiply = 1.0f;
        }

        public static void AddToConscript(ref SoldierData soldierData, ref ConscriptProfile conscript, bool ranged)
        {
            soldierData.modelData.shield = conscript.shield;
            if (conscript.shield != ItemResourceType.NONE) 
            {
                var shieldData = DssVar.Shields[conscript.shield];

                if (!ranged && shieldData.meleeSpeedBonus != 0)
                {
                    soldierData.attackTimePlusCoolDown /= 1f + shieldData.meleeSpeedBonus;
                }

                soldierData.blocksRefillTimeSec /= shieldData.blocksRefillTimeSecMultiply;
                soldierData.basehealth += shieldData.armorBonus;
                soldierData.walkingSpeed *= shieldData.moveSpeedMultiply;
            }
        }

        public void ToHud(RichBoxContent content)
        {
            content.icontext(SpriteName.cmdParry, string.Format(DssRef.lang.Conscript_BlockPerSecond, TextLib.PercentTextWithSymbol(blocksRefillTimeSecMultiply)));
            HudLib.LabelAndText(content, SpriteName.WarsAttackSpeedIcon, DssRef.lang.Conscript_AttackSpeed + " " + TextLib.Parentheses(DssRef.lang.WarsResourceGroup_MeleeHandWeapons), TextLib.PercentAddText(meleeSpeedBonus));
            HudLib.LabelAndText(content, SpriteName.warsArmyTag_Shield, DssRef.lang.Conscript_ArmorHealth, TextLib.PlusMinus(armorBonus));
            HudLib.LabelAndText(content, SpriteName.cmdMoveDown, DssRef.lang.Conscript_Mobility, TextLib.PercentTextWithSymbol(moveSpeedMultiply));
            
        }
    }
}
