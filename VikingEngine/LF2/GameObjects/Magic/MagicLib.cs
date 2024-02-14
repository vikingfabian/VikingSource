using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.LF2.GameObjects.Magic
{
    static class MagicLib
    {
        public static Dictionary<Gadgets.GoodsType, MagicElement> GemToMagic;
        public static readonly List<Gadgets.GoodsType> GemTypes = new List<Gadgets.GoodsType> 
            { Gadgets.GoodsType.Crystal, Gadgets.GoodsType.Diamond, Gadgets.GoodsType.Ruby, Gadgets.GoodsType.sapphire };

        static readonly List<Gadgets.GoodsType> MagicIngrediences = new List<Gadgets.GoodsType>
            {
                Gadgets.GoodsType.Animal_paw,
                Gadgets.GoodsType.Black_tooth,
                Gadgets.GoodsType.Bladder_stone,
                Gadgets.GoodsType.Blood_finger_herb,
                Gadgets.GoodsType.Blue_rose_herb,
                Gadgets.GoodsType.Coal,
                Gadgets.GoodsType.Flint,
                Gadgets.GoodsType.Frog_heart_herb,
                Gadgets.GoodsType.Fur,
                Gadgets.GoodsType.Granite,
                Gadgets.GoodsType.Grapes,
                Gadgets.GoodsType.Honny,
                Gadgets.GoodsType.Horn,
                Gadgets.GoodsType.Ink,
                Gadgets.GoodsType.Marble,
                Gadgets.GoodsType.Monster_egg,
                Gadgets.GoodsType.Nose_horn,
                Gadgets.GoodsType.Plastma,
                Gadgets.GoodsType.Red_eye,
                Gadgets.GoodsType.Rib,
                Gadgets.GoodsType.Sandstone,
                Gadgets.GoodsType.Scaley_skin,
                Gadgets.GoodsType.Seed,
                Gadgets.GoodsType.Sharp_teeth,
                Gadgets.GoodsType.Tusks,
                Gadgets.GoodsType.Wax,
                Gadgets.GoodsType.Wine,
            };

        public static void NewWorld()
        {
            List<MagicElement> magicTypes = new List<MagicElement>();
            for (MagicElement type = MagicElement.NoMagic + 1; type < MagicElement.NUM; type++)
            {
                magicTypes.Add(type);
            }

            GemToMagic = new Dictionary<Gadgets.GoodsType, MagicElement>();
            for (int i = 0; i < GemTypes.Count; i++)
            {
                int rndIx = Data.RandomSeed.Instance.Next(magicTypes.Count);

                GemToMagic.Add(GemTypes[i], magicTypes[rndIx]);
                magicTypes.Remove(magicTypes[rndIx]);
            }

            int storeSeedPos = Data.RandomSeed.Instance.GetSeedPosition; //To aviod disrupting the seed for old saves

            //Create random blueprints for magic crafting
            List<Gadgets.GoodsType> bombIngrediences = new List<Gadgets.GoodsType>(MagicIngrediences);

            Data.Gadgets.BluePrint[] bombs = new Data.Gadgets.BluePrint[]
            {
                Data.Gadgets.BluePrint.FireBomb,
                Data.Gadgets.BluePrint.PoisionBomb,
                Data.Gadgets.BluePrint.EvilBomb,
                Data.Gadgets.BluePrint.LightningBomb,
                Data.Gadgets.BluePrint.FluffyBomb,
            };

            foreach (Data.Gadgets.BluePrint bp in bombs)
            {
                const int NumBombItems = 3;
                 Data.Gadgets.BlueprintIngrediens ingrediences = new Data.Gadgets.BlueprintIngrediens(
                     GameObjects.Gadgets.GadgetLib.GadgetIcon(GameObjects.Gadgets.GadgetLib.BluePrintToGoodstype(bp)),
                     bp.ToString(), new List<Data.Gadgets.Ingredient>());

                for (int i = 0; i < NumBombItems; ++i)
                {
                    ingrediences.Requierd.Add(new Data.Gadgets.Ingredient("Ingredience " + TextLib.IndexToString(i),
                        new List<Gadgets.GoodsType> { Data.RandomSeed.Instance.RandomListMemeberRemove(bombIngrediences) }, 1));
                }

                lib.DictionaryAddOrReplace(Data.Gadgets.BluePrintLib.BlueprintIngrediensLib, bp, ingrediences);
            }

            GenerateStaffBP(Data.Gadgets.BluePrint.FireStaff, MagicElement.Fire, bombIngrediences);
            GenerateStaffBP(Data.Gadgets.BluePrint.PoisionStaff, MagicElement.Poision, bombIngrediences);
            GenerateStaffBP(Data.Gadgets.BluePrint.EvilStaff, MagicElement.Evil, bombIngrediences);
            GenerateStaffBP(Data.Gadgets.BluePrint.LightningStaff, MagicElement.Lightning, bombIngrediences);

            
            List<Gadgets.GoodsType> ringIngrediences = new List<Gadgets.GoodsType>(MagicIngrediences);
            Gadgets.GoodsType[] ringMetals = new Gadgets.GoodsType[]
            {
                Gadgets.GoodsType.Silver,
                Gadgets.GoodsType.Gold,
                Gadgets.GoodsType.Mithril,
            };

            for (Magic.MagicRingSkill skill = Magic.MagicRingSkill.NO_SKILL + 1; skill < Magic.MagicRingSkill.NUM; ++skill)
            {
                MagicElement magicRingSkillElement = MagicRingSkillElement(skill);
                Gadgets.GoodsType gem;
                int numGems;
                if (magicRingSkillElement == MagicElement.NoMagic)
                {
                    gem = Data.RandomSeed.Instance.RandomListMemeber(GemTypes);
                    numGems = 1;
                }
                else
                {
                    gem = lib.DictionaryKeyFromValue(GemToMagic, magicRingSkillElement);
                    numGems = 2;
                }
                List<Data.Gadgets.Ingredient> ingrediences = new List<Data.Gadgets.Ingredient>
                    {
                        new Data.Gadgets.Ingredient("Metal", new List<Gadgets.GoodsType>{ Data.RandomSeed.Instance.RandomListMemeber(ringMetals) }, 1),
                        new Data.Gadgets.Ingredient("Gem", new List<Gadgets.GoodsType>{ gem }, numGems),
                    };
                if (magicRingSkillElement == MagicElement.NoMagic)
                {
                    ingrediences.Add(new Data.Gadgets.Ingredient("Item", new List<Gadgets.GoodsType> { Data.RandomSeed.Instance.RandomListMemeberRemove(ringIngrediences) }, 2));
                }

                lib.DictionaryAddOrReplace(
                    Data.Gadgets.BluePrintLib.BlueprintIngrediensLib, 
                    Data.Gadgets.BluePrint.RingReservationStart + (int)skill, 
                    //Ingrediences
                    new Data.Gadgets.BlueprintIngrediens(GameObjects.Gadgets.Jevelery.MagicRingSkillIcon(skill), "Magic ring " + ((int)skill).ToString(),
                        ingrediences)
                    );
            }


            Data.RandomSeed.Instance.SetSeedPosition(storeSeedPos);
        }

        static void GenerateStaffBP(Data.Gadgets.BluePrint bp, MagicElement magic, List<Gadgets.GoodsType> bombIngrediences)
        {
            Data.Gadgets.BlueprintIngrediens ingrediences = new Data.Gadgets.BlueprintIngrediens(
                      SpriteName.WeaponStaff,
                     bp.ToString(), new List<Data.Gadgets.Ingredient>());

            const int NumStaffItems = 2;
            for (int i = 0; i < NumStaffItems; ++i)
            {
                ingrediences.Requierd.Add(new Data.Gadgets.Ingredient("Ingredience " + TextLib.IndexToString(i),
                    new List<Gadgets.GoodsType> { Data.RandomSeed.Instance.RandomListMemeberRemove(bombIngrediences) }, 2));
            }
            ingrediences.Requierd.Add(new Data.Gadgets.Ingredient("Staff",
                    new List<Gadgets.GoodsType> { Gadgets.GoodsType.Stick }, 4));


            Gadgets.GoodsType gem = lib.DictionaryKeyFromValue(GemToMagic, magic);
            ingrediences.Requierd.Add(new Data.Gadgets.Ingredient("Stones",
                    new List<Gadgets.GoodsType> { gem }, 3));

            lib.DictionaryAddOrReplace(Data.Gadgets.BluePrintLib.BlueprintIngrediensLib, bp, ingrediences);
        }

        public static void DebugFacts(DataLib.DocFile file)
        {
            foreach (KeyValuePair<Gadgets.GoodsType, MagicElement> kv in GemToMagic)
            {
                file.addText(kv.Key.ToString() + " to " + kv.Value.ToString(), TextStyle.Body);
            }
        }
        public static SpriteName Icon(MagicElement type)
        {
            switch (type)
            {
                default:
                    return SpriteName.NO_IMAGE;
                case MagicElement.Evil:
                    return SpriteName.LFIconEvilEnchant;
                case MagicElement.Fire:
                    return SpriteName.LFIconFireEnchant;
                case MagicElement.Lightning:
                    return SpriteName.LFIconLightningEnchant;
                case MagicElement.Poision:
                    return SpriteName.LFIconPoisionEnchant;
            }
        }


        static MagicElement MagicRingSkillElement(MagicRingSkill skill)
        {
            if (skill == MagicRingSkill.Evil_aura ||
                skill == MagicRingSkill.Evil_boost ||
                skill == MagicRingSkill.Evil_touch ||
                skill == MagicRingSkill.Projectile_evil_burst)
            {
                return MagicElement.Evil;
            }
            if (skill == MagicRingSkill.Projectile_fire_burst ||
                skill == MagicRingSkill.Fire_boost)
            {
                return MagicElement.Fire;
            }
            if (skill == MagicRingSkill.Lighting_boost ||
               skill == MagicRingSkill.Projectile_lightning_burst)
            {
                return MagicElement.Lightning;
            }
            if (skill == MagicRingSkill.Poision_boost ||
               skill == MagicRingSkill.Projectile_poision_burst)
            {
                return MagicElement.Poision;
            }
            return MagicElement.NoMagic;
        }
    }
    enum MagicElement
    {
        NoMagic,
        Fire,
        Poision,
        Evil,
        Lightning,
        //Stunn,
        NUM,
        
    }

    enum MagicUnderType
    {
        WiseLadyAttack,
        NUM_NON,
    }

    enum MagicRingSkill
    {
        NO_SKILL,
        Recylcling_bowman, //percent chance that the arrows will reappear after use
        Magic_boost,
        Fire_boost,
        Poision_boost,
        Evil_boost,
        Lighting_boost,
        First_swing,//-First blood, första slaget blir extra kraffullt, tar en minut att ladda up, en för båge
        First_string, //Push_force,//-större pushback
        Evil_touch,//-evil touch, knuffas gör skada
        Evil_aura,//-evil aura, skadar alla i närheten
        Javelin_master,//-stärka effekten av kastvapen
        Light_hand,//-snabbare vapen cooldown
        //-Stunn, slag eller pil
        Butcher,//-butcher, större chans att få högkvalité skinn
        Apple_lover,//-apple lover, öka hälsan av att äta äpple
        Hobbit_skill,//-hobbit ring, all mat ger mer hälsa
        //-kunna stunna med slag istället för att skada
        Elven_warrior,//-Elven warrior, träsvärd skadar som järn
        //-ring för marje magi, som sprider sig från pilen vid träff
        Projectile_fire_burst,
        Projectile_poision_burst,
        Projectile_evil_burst,
        Projectile_lightning_burst,
        Paladins, //-paladin, svärd kan skjuta om dom är magiska
        Spear_rush,//-springa me spjut
        Axe_master, //NY kan springa och svinga yxa
        Barbarian_swing,//-ringar som förstärker push
        Ring_of_protection,//-20% mer health
        Librarian,//-större radie och längre tid boost på scrolls
        Healer,
        NUM,

    }
}
