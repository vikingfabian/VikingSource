using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Authentication;
using System.Text;
using VikingEngine.Core.Wand.Equipment;
using VikingEngine.LootFest.GO.EnvironmentObj;
using VikingEngine.PJ.SpaceWar.SpaceShip;

namespace VikingEngine.Core.Wand.Data
{
    class MagicFormulaLib
    {
        public static MagicFormula Heal = new MagicFormula("Heal", MagicTrigger.OnComplete, MagicTrigger_ObjectType.None, MagicEffect.Heal_user, 1);
        public static MagicFormula HealTarget = new MagicFormula("Heal target", MagicTrigger.ObjectHit, MagicTrigger_ObjectType.Creature, MagicEffect.Heal_target, 2);
        public static MagicFormula SmallDamage = new MagicFormula("Small damage", MagicTrigger.ObjectHit, MagicTrigger_ObjectType.Creature, MagicEffect.Damage, 1);
        public static MagicFormula MediumDamage = new MagicFormula("Medium damage", MagicTrigger.ObjectHit, MagicTrigger_ObjectType.Creature, MagicEffect.Damage, 2);
        public static MagicFormula MassiveDamage = new MagicFormula("Massive damage", MagicTrigger.ObjectHit, MagicTrigger_ObjectType.Creature, MagicEffect.Damage, 5);
        public static MagicFormula Backfire = new MagicFormula("Backfire", MagicTrigger.OnComplete, MagicTrigger_ObjectType.None, MagicEffect.Back_fire, 1);
        public static MagicFormula Leecher = new MagicFormula("Leecher", MagicTrigger.ObjectHit, MagicTrigger_ObjectType.Creature, MagicEffect.Leech, 2);
        public static MagicFormula Executer = new MagicFormula("Executer", MagicTrigger.ObjectHit, MagicTrigger_ObjectType.Creature, MagicEffect.Execute_damaged, MagicFormula.NoTier);
        public static MagicFormula Explosion = new MagicFormula("Explosion", MagicTrigger.OnComplete, MagicTrigger_ObjectType.None, MagicEffect.Explode, 1);
        public static MagicFormula BigExplosion = new MagicFormula("Big Explosion", MagicTrigger.OnComplete, MagicTrigger_ObjectType.None, MagicEffect.Explode, 3);
        public static MagicFormula Teleport = new MagicFormula("Teleport", MagicTrigger.OnComplete, MagicTrigger_ObjectType.None, MagicEffect.Teleport_user, MagicFormula.NoTier);

        public static MagicFormula ThinWallSmasher = new MagicFormula("Thin wall smasher", MagicTrigger.ObjectHit, MagicTrigger_ObjectType.Thin_Wall, MagicEffect.Open_wall, 1);
        public static MagicFormula Thin2WallSmasher = new MagicFormula("Thin wall smasher II", MagicTrigger.ObjectHit, MagicTrigger_ObjectType.Thin_Wall, MagicEffect.Open_wall, 2);
        public static MagicFormula WallSmasher = new MagicFormula("Wall smasher", MagicTrigger.ObjectHit, MagicTrigger_ObjectType.Any_Wall, MagicEffect.Open_wall, 1);

        public static MagicFormula ExplosiveBlood = new MagicFormula("Explosive blood", MagicTrigger.SpendFluid, MagicTrigger_ObjectType.Blood, MagicEffect.Explode, 1);
        public static MagicFormula SuckBlood = new MagicFormula("Suck blood", MagicTrigger.SpendFluid, MagicTrigger_ObjectType.Blood, MagicEffect.Heal_user, 1);
        public static MagicFormula BloodMagic = new MagicFormula("Blood magic", MagicTrigger.SpendFluid, MagicTrigger_ObjectType.Blood, MagicEffect.Magic_up, 1);
        public static MagicFormula BloodRevival = new MagicFormula("Blood revival", MagicTrigger.SpendFluid, MagicTrigger_ObjectType.Blood, MagicEffect.Heal_target, 1);
        public static MagicFormula EndOnCreature = new MagicFormula("End on creature", MagicTrigger.ObjectHit, MagicTrigger_ObjectType.Creature, MagicEffect.End, MagicFormula.NoTier);
        public static MagicFormula Cloner = new MagicFormula("Cloner", MagicTrigger.ObjectHit, MagicTrigger_ObjectType.Any, MagicEffect.Clone, 1);
        public static MagicFormula Upper = new MagicFormula("Upper", MagicTrigger.ObjectHit, MagicTrigger_ObjectType.Any, MagicEffect.Level_up, 1);

        public static MagicFormula BloodActivate = new MagicFormula("Blood activate", MagicTrigger.SpendFluid, MagicTrigger_ObjectType.Blood, MagicEffect.Activate, 1);
        public static MagicFormula CreatureActivate = new MagicFormula("Creature activate", MagicTrigger.PassObject, MagicTrigger_ObjectType.Creature, MagicEffect.Activate, 1);
        public static MagicFormula ThinWallActivate = new MagicFormula("Wall activate", MagicTrigger.PassObject, MagicTrigger_ObjectType.Thin_Wall, MagicEffect.Activate, 1);
        public static MagicFormula NorthActivate = new MagicFormula("North fire", MagicTrigger.Fire_north, MagicTrigger_ObjectType.None, MagicEffect.Activate, 1);
        public static MagicFormula SouthActivate = new MagicFormula("South fire", MagicTrigger.Fire_south, MagicTrigger_ObjectType.None, MagicEffect.Activate, 1);
        //List<MagicFormula> activators;

        //List<MagicFormula> collection;
        FormulaCollection[] collections;

        public const int MaxSpells = 6;

        public MagicFormulaLib()
        {
            collections = new FormulaCollection[(int)FormulaCollectionType.NUM];
            for (int i = 0; i < collections.Length; ++i)
            {
                collections[i] = new FormulaCollection((FormulaCollectionType)i);
            }

            //activators = new List<MagicFormula>
            //{
            //    BloodActivate,
            //    CreatureActivate,
            //    ThinWallActivate,
            //    NorthActivate,
            //    SouthActivate,
            //};

            //collection = new List<MagicFormula>
            //{
            //    Heal,
            //    HealTarget,
            //    SmallDamage,
            //    MediumDamage,
            //    MassiveDamage,
            //    Backfire,
            //    Leecher,
            //    Executer,
            //    ThinWallSmasher,
            //    WallSmasher,
            //    Explosion,
            //    BigExplosion,
            //    Teleport,
            //    ExplosiveBlood,
            //    SuckBlood,
            //    //BloodMagic,
            //    BloodRevival,
            //    EndOnCreature,
            //    Cloner,
            //    Upper,
            //};
        }

        public List2<MagicFormula> CreateMagicEffectList(MagicWandType wandType)
        {
            List2<MagicFormula> formulas = new List2<MagicFormula>(MaxSpells);
            switch (wandType)
            {
                case MagicWandType.Friendly1:
                    formulas.Add(SmallDamage);
                    formulas[0].isFound = true;
                    AddToFormulas(1, FormulaCollectionType.Friendly, ref formulas);
                    break;

                case MagicWandType.Friendly2:
                    formulas.Add(MediumDamage);
                    formulas[0].isFound = true;
                    AddToFormulas(1, FormulaCollectionType.Friendly, ref formulas);
                    break;

                default:
                    if (Ref.rnd.Chance(0.25))
                    {
                        AddToFormulas(1, FormulaCollectionType.Activators, ref formulas);
                    }

                    if (Ref.rnd.Chance(0.1))
                    {
                        formulas.Add(BloodMagic);
                    }
                    AddToFormulas(3 - formulas.Count, FormulaCollectionType.All, ref formulas);
                    break;
            }

            return formulas;
        }

        public void AddToFormulas(int add, FormulaCollectionType fromCollection, ref List2<MagicFormula> formulas)
        {
            int goalCount = formulas.Count + add;
            int maxLoops = 100;

            FormulaCollection collection = collections[(int)fromCollection];

            while (formulas.Count < goalCount && maxLoops > 0)
            {
                --maxLoops;
                var form = collection.PeekNext();//arraylib.RandomListMember(collection);

                bool canBeUsed = true;

                for (int i = 0; i < formulas.Count; ++i)
                {
                    if (!canBeCombined(form, formulas.Array[i]))
                    {
                        canBeUsed = false;
                        break;
                    }
                }

                if (canBeUsed)
                {
                    formulas.Add(form);
                    collection.PullSelected();
                }
            }
        }

        //public void AddToEffectList(ref List2<MagicFormula> list)
        //{
        //    AddToCount( list.Count + 1, ref list);
        //}

        bool canBeCombined(MagicFormula form1, MagicFormula form2)
        {
            return form1.effect != form2.effect;
        }
    }

    class FormulaCollection
    {
        public FormulaCollection(FormulaCollectionType collectionType)
        {
            this.collectionType = collectionType;
        }
        FormulaCollectionType collectionType;
        List2<MagicFormula> collection = new List2<MagicFormula>(16);

        public MagicFormula PullNext()
        {
            var result = PeekNext();
            PullSelected();
            return result;
        }
        public MagicFormula PeekNext()
        {
            if (collection.Count <= 2)
            {
                refill();
            }

            collection.SelectRandom();
            return collection.Sel();
        }

        public void PullSelected()
        {
            collection.RemoveAtSwapBack(collection.selIndex);
        }

        void refill()
        {
            switch (collectionType)
            {
                case FormulaCollectionType.Activators:

                    add(MagicFormulaLib.BloodActivate, 2);
                    add(MagicFormulaLib.CreatureActivate, 2);
                    add(MagicFormulaLib.ThinWallActivate, 2);
                    add(MagicFormulaLib.NorthActivate, 2);
                    add(MagicFormulaLib.SouthActivate, 2);

                    break;

                case FormulaCollectionType.All:

                    add(MagicFormulaLib.Heal, 2);
                    add(MagicFormulaLib.HealTarget, 2);
                    add(MagicFormulaLib.SmallDamage, 4);
                    add(MagicFormulaLib.MediumDamage, 2);
                    add(MagicFormulaLib.MassiveDamage, 2);
                    add(MagicFormulaLib.Backfire, 2);
                    add(MagicFormulaLib.Leecher, 2);
                    add(MagicFormulaLib.Executer, 2);
                    add(MagicFormulaLib.ThinWallSmasher, 2);
                    add(MagicFormulaLib.WallSmasher, 2);
                    add(MagicFormulaLib.Explosion, 2);
                    add(MagicFormulaLib.BigExplosion, 2);
                    add(MagicFormulaLib.Teleport, 2);
                    add(MagicFormulaLib.ExplosiveBlood, 2);
                    add(MagicFormulaLib.SuckBlood, 2);
                    add(MagicFormulaLib.BloodMagic, 2);
                    add(MagicFormulaLib.BloodRevival, 2);
                    add(MagicFormulaLib.EndOnCreature, 2);
                    add(MagicFormulaLib.Cloner, 2);
                    add(MagicFormulaLib.Upper, 2);

                    break;

                case FormulaCollectionType.Friendly:

                    add(MagicFormulaLib.Heal, 2);
                    add(MagicFormulaLib.SmallDamage, 2);
                    add(MagicFormulaLib.MediumDamage, 2);
                    add(MagicFormulaLib.MassiveDamage, 2);
                    add(MagicFormulaLib.Leecher, 2);
                    add(MagicFormulaLib.Executer, 2);
                    add(MagicFormulaLib.ThinWallSmasher, 2);
                    add(MagicFormulaLib.WallSmasher, 2);
                    
                    break;
            }

            void add(MagicFormula formula, int commoness)
            {
                for (int i = 0; i < commoness; ++i)
                {
                    collection.Add(formula);
                }
            }
        }

       
    }

    enum FormulaCollectionType
    { 
        Activators,
        All,
        Friendly,
        NUM
    }
}
