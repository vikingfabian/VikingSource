using VikingEngine.ToGG.Data.Property;

namespace VikingEngine.ToGG.Commander.UnitsData
{
    abstract class CmdUnitData : AbsUnitData
    {
        protected const int OneMan = 1;
        protected const int InfantryHealth = 4;
        protected const int CavalryHealth = 3;
        protected const int ChariotHealth = 2;
        protected const int StandardRangedAttacks = 2;

        protected const int OrcInfantryHealth = InfantryHealth -1;
        protected const int OrcCavalryHealth = CavalryHealth -1;

        //public UnitType type;
        public UnitMainType mainType; //döp till cathegory
        public UnitUnderType underType;

        public string name;

        public bool canBackStab, canCounterAttack;
        public UnitProgress progress;

        public CmdUnitData()
        { }

        public CmdUnitData(string name, UnitMainType mainType, UnitUnderType underType, SpriteName image)
        {
            oneManArmy = true;
            //armySetupType = -1;
            this.name = name;
            this.modelSettings.image = image;
            this.mainType = mainType;
            this.underType = underType;
            this.canBackStab = wep.meleeStrength > 0;
            this.canCounterAttack = wep.meleeStrength > 0;

        }

        public CmdUnitData(string name, UnitMainType mainType, UnitUnderType underType,
            bool readyForRetail, SpriteName image,
            int move, int ccAttacks, int rangedAttacks, int fireRange, int health, bool OneManArmy)
        {
            //armySetupType = -1;
            this.name = name;
            this.readyForRetail = readyForRetail;
            this.move = move;
            this.wep.meleeStrength = ccAttacks;
            this.wep.projectileStrength = rangedAttacks;
            this.wep.projectileRange = fireRange;
            this.modelSettings.image = image;
            modelSettings.modelScale = 0.7f;
            this.startHealth = health;
            //this.properties = properties;

            this.mainType = mainType;
            this.underType = underType;
            this.oneManArmy = OneManArmy;
            this.canBackStab = ccAttacks > 0;
            this.canCounterAttack = ccAttacks > 0;

        }

        public override float armorValue()
        {
            return 0;
        }

        public override WeaponStats WeaponStats => wep;

        abstract public UnitType Type { get; }

        public override string Name => name;
    }
}
