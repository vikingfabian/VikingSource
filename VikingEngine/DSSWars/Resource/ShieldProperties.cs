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
        public float blocksRefillTimeSecMultiply; //= DssConst.DefaultBlockRefillTimeSec
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
            content.newLine();
            content.Add(new RbText(".Block count", HudLib.TitleColor_Label));
            content.hspace();
            content.Add(new RbText(TextLib.PercentTextWithSymbol(blocksRefillTimeSecMultiply)));

            content.newLine();
            content.Add(new RbText(".Melee speed", HudLib.TitleColor_Label));
            content.hspace();
            content.Add(new RbText(TextLib.PercentAddText(meleeSpeedBonus)));

            content.newLine();
            content.Add(new RbImage(SpriteName.warsArmyTag_Shield));
            content.Add(new RbSpace());
            content.Add(new RbText(DssRef.lang.Conscript_ArmorHealth, HudLib.TitleColor_Label));
            content.hspace();
            content.Add(new RbText(TextLib.PlusMinus(armorBonus)));

            content.newLine();
            content.Add(new RbText(".Move speed", HudLib.TitleColor_Label));
            content.hspace();
            content.Add(new RbText(TextLib.PercentTextWithSymbol(moveSpeedMultiply)));
        }
    }
}
