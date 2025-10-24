using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.EntityComponent;
using VikingEngine.DSSWars.Resource;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace VikingEngine.DSSWars
{
    partial class WorldData
    {
        public ResourceOverview[] factionResourceOverviews = new ResourceOverview[64 * CityResoureIndex.COUNT];

        void init_FactionComponents()
        {
            factionResourceOverviews = new ResourceOverview[factions.Array.Length * CityResoureIndex.COUNT];

            for (int i = 0; i < factions.Array.Length; i++)
            {
                if (factions.Array[i] != null)
                { 
                    factions.Array[i].resourceComponentStartIndex = i * CityResoureIndex.COUNT;
                }
            }
        }

        public void factionComponentsAdd(Faction faction)
        {
            faction.resourceComponentStartIndex = faction.myIndex * CityResoureIndex.COUNT;

            if (factions.Array.Length * CityResoureIndex.COUNT >= factionResourceOverviews.Length)
            {
                int startIndex = factionResourceOverviews.Length;
                Array.Resize(ref factionResourceOverviews, factionResourceOverviews.Length * 2);
            }
        }


        //public ResourceOverview res_wood = new ResourceOverview();
        //public ResourceOverview res_fuel = new ResourceOverview();
        //public ResourceOverview res_stone = new ResourceOverview();
        //public ResourceOverview res_rawFood = new ResourceOverview();
        //public ResourceOverview res_food = new ResourceOverview();
        //public ResourceOverview res_beer = new ResourceOverview();
        //public ResourceOverview res_coolingfluid = new ResourceOverview();
        //public ResourceOverview res_skinLinnen = new ResourceOverview();

        //public ResourceOverview res_ironore = new ResourceOverview();
        //public ResourceOverview res_TinOre = new ResourceOverview();
        //public ResourceOverview res_CupperOre = new ResourceOverview();
        //public ResourceOverview res_LeadOre = new ResourceOverview();
        //public ResourceOverview res_SilverOre = new ResourceOverview();
        //public ResourceOverview res_GoldOre = new ResourceOverview();

        //public ResourceOverview res_iron = new ResourceOverview();
        //public ResourceOverview res_Tin = new ResourceOverview();
        //public ResourceOverview res_Cupper = new ResourceOverview();
        //public ResourceOverview res_Lead = new ResourceOverview();
        //public ResourceOverview res_Silver = new ResourceOverview();
        //public ResourceOverview res_RawMithril = new ResourceOverview();
        //public ResourceOverview res_Sulfur = new ResourceOverview();

        //public ResourceOverview res_Bronze = new ResourceOverview();
        //public ResourceOverview res_Steel = new ResourceOverview();
        //public ResourceOverview res_CastIron = new ResourceOverview();
        //public ResourceOverview res_BloomeryIron = new ResourceOverview();
        //public ResourceOverview res_Mithril = new ResourceOverview();

        //public ResourceOverview res_Palisade = new ResourceOverview();
        //public ResourceOverview res_Toolkit = new ResourceOverview();
        //public ResourceOverview res_Wagon2Wheel = new ResourceOverview();
        //public ResourceOverview res_Wagon4Wheel = new ResourceOverview();
        //public ResourceOverview res_BlackPowder = new ResourceOverview();
        //public ResourceOverview res_GunPowder = new ResourceOverview();
        //public ResourceOverview res_LedBullet = new ResourceOverview();

        //public ResourceOverview res_sharpstick = new ResourceOverview();
        //public ResourceOverview res_BronzeSword = new ResourceOverview();
        //public ResourceOverview res_shortsword = new ResourceOverview();
        //public ResourceOverview res_Sword = new ResourceOverview();
        //public ResourceOverview res_LongSword = new ResourceOverview();
        //public ResourceOverview res_HandSpear = new ResourceOverview();
        //public ResourceOverview res_MithrilSword = new ResourceOverview();

        //public ResourceOverview res_Warhammer = new ResourceOverview();
        //public ResourceOverview res_twohandsword = new ResourceOverview();
        //public ResourceOverview res_knightslance = new ResourceOverview();
        //public ResourceOverview res_SlingShot = new ResourceOverview();
        //public ResourceOverview res_ThrowingSpear = new ResourceOverview();
        //public ResourceOverview res_bow = new ResourceOverview();
        //public ResourceOverview res_longbow = new ResourceOverview();
        //public ResourceOverview res_crossbow = new ResourceOverview();
        //public ResourceOverview res_MithrilBow = new ResourceOverview();

        //public ResourceOverview res_HandCannon = new ResourceOverview();
        //public ResourceOverview res_HandCulvertin = new ResourceOverview();
        //public ResourceOverview res_Rifle = new ResourceOverview();
        //public ResourceOverview res_Blunderbuss = new ResourceOverview();

        //public ResourceOverview res_BatteringRam = new ResourceOverview();
        //public ResourceOverview res_ballista = new ResourceOverview();
        //public ResourceOverview res_Manuballista = new ResourceOverview();
        //public ResourceOverview res_Catapult = new ResourceOverview();
        //public ResourceOverview res_SiegeCannonBronze = new ResourceOverview();
        //public ResourceOverview res_ManCannonBronze = new ResourceOverview();
        //public ResourceOverview res_SiegeCannonIron = new ResourceOverview();
        //public ResourceOverview res_ManCannonIron = new ResourceOverview();

        //public ResourceOverview res_paddedArmor = new ResourceOverview();
        //public ResourceOverview res_HeavyPaddedArmor = new ResourceOverview();
        //public ResourceOverview res_BronzeArmor = new ResourceOverview();
        //public ResourceOverview res_mailArmor = new ResourceOverview();
        //public ResourceOverview res_heavyMailArmor = new ResourceOverview();
        //public ResourceOverview res_LightPlateArmor = new ResourceOverview();
        //public ResourceOverview res_FullPlateArmor = new ResourceOverview();
        //public ResourceOverview res_MithrilArmor = new ResourceOverview();
    }
}
