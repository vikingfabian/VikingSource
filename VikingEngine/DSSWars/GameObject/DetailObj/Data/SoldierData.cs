using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Conscript;
using VikingEngine.DSSWars.Resource;
using VikingEngine.HUD.RichBox;
using VikingEngine.ToGG.MoonFall.GO;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace VikingEngine.DSSWars.GameObject.DetailObj.Data
{
    struct SoldierData
    {
        public float blockChance = DssConst.DefaultBlockChance;
        /// <summary>
        /// Max blocks are refills per second
        /// </summary>
        public float blocksRefillTimeSec = DssConst.DefaultBlockRefillTimeSec; 

        public int basehealth = DssConst.Soldier_DefaultHealth;
        public bool arrowWeakness = false;
        public AttackType mainAttack = 0;
        public AttackType secondaryAttack = 0;
        public int bonusProjectiles = 0;
        public int attackDamage = 0, attackDamageSea = 0, attackDamageStructure = 0;
        public float blockReducingAttack_Inv = 1f;
        public int attackSplashCount = 0;
        public int secondaryAttackDamage = 0;
        public float attackTimePlusCoolDown = 0;

        public float attackRange = 0;
        public float secondaryAttackRange = 0;

        public bool factionColoredModel = true;
        public LootFest.VoxelModelName modelName = 0;
        public SoldierModelData modelData;
        public int modelVariationCount = 1;
        public SpriteName icon = SpriteName.MissingImage;
        public bool hasBannerMan = true;

        public int defaultArmyPlacement = 0;
        public float upkeepMultiplier = 1f;//DssLib.SoldierDefaultEnergyUpkeep;
        public float animalFoodMultiplier = 0;
        public float rotationSpeed= DssConst.SoldierGroupStandardRotatingSpeed;
        public float walkingSpeed = DssConst.Men_StandardWalkingSpeed;
        public float weightClass = 0.5f;
        public float lightWagonSpeed = DssConst.Men_StandardWalkingSpeed;
        public float heavyWagonSpeed = DssConst.Men_StandardWalkingSpeed;
        public bool canAttackCharacters = true;
        public bool canAttackStructure = true;
        public float modelScale = DssConst.Men_ModCharacterScale;

        //public float upkeepPerSoldier = DssLib.SoldierDefaultUpkeep;
        //public float copperUpkeepPerSoldier = 0;
        
        public float boundRadius = DssVar.StandardBoundRadius;
        public float groupSpacing = DssVar.DefaultGroupSpacing;
        public Vector3 attackStart = new Vector3(DssConst.Men_StandardModelScale * 0.5f, DssConst.Men_StandardModelScale * 0.4f, DssConst.Men_StandardModelScale * 0.5f);
        public float groupSpacingRndOffset = DssVar.StandardBoundRadius * 0.3f;
        
        public int rowWidth = DssConst.SoldierGroup_RowWidth;
        public int columnsDepth = DssConst.SoldierGroup_ColumnsDepth;
        public int workForcePerUnit = 1;
        //public int animalsPerUnit = 1;


        public Vector3 modelToShadowScale = new Vector3(0.4f, 1f, 0.32f);

        public SoldierData()
        { }


        public void StatsToHud(RichBoxContent content)
        {
            HudLib.LabelAndText(content, SpriteName.WarsSpecializeField, string.Format(DssRef.lang.Conscript_DamagePerSecondInAreaX, DssRef.lang.Conscript_Specialization_Field),
                TextLib.OneDecimal(DPS_land()));

            HudLib.LabelAndText(content, SpriteName.WarsSpecializeSiege, string.Format(DssRef.lang.Conscript_DamagePerSecondInAreaX, DssRef.lang.Conscript_Specialization_Siege),
                TextLib.OneDecimal(DPS_structure()));

            HudLib.LabelAndText(content, SpriteName.WarsSpecializeSea, string.Format(DssRef.lang.Conscript_DamagePerSecondInAreaX, DssRef.lang.Conscript_Specialization_Sea),
                TextLib.OneDecimal(DPS_sea()));

            HudLib.LabelAndText(content, SpriteName.WarsResource_Sword, DssRef.lang.Conscript_WeaponDamage, attackDamage.ToString());
            HudLib.LabelAndText(content, SpriteName.WarsAttackSpeedIcon, DssRef.lang.Conscript_AttackSpeed, TextLib.OneDecimal(TimeExt.MillsSecToSec(attackTimePlusCoolDown)));
            content.space();
            content.Add(new RbText(TextLib.Parentheses( DssRef.lang.Hud_Time_ValuePerSecond), HudLib.InfoYellow_Light));
            HudLib.LabelAndText(content, SpriteName.warsArmyTag_Shield, DssRef.lang.SoldierStats_Health, basehealth.ToString());
            //HudLib.LabelAndText(content, SpriteName.cmdParry, DssRef.lang.Conscript_BlockPerSecond, TextLib.OneDecimal(1f / blocksRefillTimeSec));
            content.newLine();
            content.Add(new RbImage(SpriteName.cmdParry));
            content.space();
            content.Add(new RbText(string.Format(DssRef.lang.Conscript_BlockPerSecond, TextLib.OneDecimal(1f / blocksRefillTimeSec))));
            HudLib.LabelAndText(content, SpriteName.WarsMobilityIcon, DssRef.lang.Conscript_Mobility, TextLib.TwoDecimal(mobilityValue()));
        }

        public void applySkillBonus(float skillBonus, float mobileBonus)
        {
            if (skillBonus == 0 || skillBonus == 1)
            {
                skillBonus = 1;
            }
            else
            {
                attackDamage = Convert.ToInt32(attackDamage * skillBonus);
                attackDamageStructure = Convert.ToInt32(attackDamageStructure * skillBonus);
                attackDamageSea = Convert.ToInt32(attackDamageSea * skillBonus);
                basehealth = MathExt.MultiplyInt(basehealth, skillBonus);
            }

            if (mobileBonus != 0) 
            {
                walkingSpeed += mobileBonus * walkingSpeed;
            }
        }

        public LootFest.VoxelModelName RandomModelName()
        {
            if (modelVariationCount == 1)
            {
                return modelName;
            }

            double rnd = Ref.peRnd.Double();
            if (modelVariationCount >= 3 && rnd < 0.1)
            {
                return modelName + 2;
            }
            else if (rnd < 0.35)
            {
                return modelName + 1;
            }
            else
            {
                return modelName;
            }
        }

        public int MaxBlockCount()
        {
            return Bound.Min((int)(1f / blocksRefillTimeSec + 0.9f), 1);
        }

        public float DPS_land()
        {
            return attackDamage / (attackTimePlusCoolDown / TimeExt.SecondToMs);
        }
        public float DPS_sea()
        {
            return attackDamageSea / (attackTimePlusCoolDown / TimeExt.SecondToMs);
        }
        public float DPS_structure()
        {
            return attackDamageStructure / (attackTimePlusCoolDown / TimeExt.SecondToMs);
        }

        public float animalFoodUpkeep(int unitCount)
        {
            return animalFoodMultiplier * unitCount * DssRef.storage.ruleset_instance.mountFoodUpkeep;
        }

        public int UnitCount()
        {
            return rowWidth * columnsDepth;
        }
        public int UnitCount(ArmyType armyTypeFilter)
        {
            if (armyTypeFilter == ArmyType.ArmyMen)
            {
                return DssConst.SoldierGroup_GuardCount;
            }
            else
            {
                return rowWidth * columnsDepth;
            }
        }
        public int workForceCount()
        {
            return rowWidth * columnsDepth * workForcePerUnit;
        }

        public int workForceCount(ArmyType armyTypeFilter)
        {
            if (armyTypeFilter == ArmyType.CityGuard)
            {
                return DssConst.SoldierGroup_GuardCount;
            }
            else
            {
                return rowWidth * columnsDepth * workForcePerUnit;
            }
        }

        public void CavalrySetup()
        {
            rowWidth = ItemPropertyColl.MountRowWidth;
            columnsDepth = ItemPropertyColl.MountColumnDepth;
            groupSpacing = DssVar.DefaultGroupSpacing * 1.4f;
            boundRadius = DssVar.StandardBoundRadius * 1.4f;
        }
        //public void ElephantSetup()
        //{
        //    //rowWidth = ItemPropertyColl.ElephantRowWidth;
        //    //columnsDepth = ItemPropertyColl.ElephantCumnDepth;
        //    groupSpacing = DssVar.DefaultGroupSpacing * 2.6f;
        //    boundRadius = DssVar.StandardBoundRadius * 2.5f;
        //}
        public void BalcongSetup()
        {

        }
        public void WagonSetup()
        {
            rowWidth = ItemPropertyColl.WagonRowWidth;
            columnsDepth = ItemPropertyColl.WagonColumnDepth;
            groupSpacing = DssVar.DefaultGroupSpacing * 2.5f;
            boundRadius = DssVar.StandardBoundRadius * 2f;
            upkeepMultiplier *= 4;
        }


        const float MobilityMultiplySpeed = WorldData.TileSubDivitions * TimeExt.SecondToMs;
        public float mobilityValue()
        {
            return walkingSpeed * MobilityMultiplySpeed;
        }

        public static float Mobility(float speed)
        { 
            return speed * MobilityMultiplySpeed;
        }

       
        //public int Upkeep()
        //{
        //    return Convert.ToInt32(rowWidth * columnsDepth * upkeepPerSoldier);
        //}

        public Vector3 ShadowModelScale()
        {
            return modelToShadowScale * DssConst.Men_StandardModelScale;
        }
    }
}
