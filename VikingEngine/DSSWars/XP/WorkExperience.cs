using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.DSSWars.XP
{
    struct WorkExperience: IComparable<WorkExperience>
    {
        public static readonly WorkExperience Empty = new WorkExperience();
        public byte xp;

        public ExperienceLevel Level()
        {
            return XpLib.ToLevel(xp);
        }
        public void setLevel(int toLevel)
        {
            xp = (byte)(toLevel * DssConst.WorkXpToLevel);
        }
        public void write(System.IO.BinaryWriter w)
        {
            w.Write(xp);
        }
        public void read(System.IO.BinaryReader r)
        {
            xp = r.ReadByte();
        }
        public int CompareTo(WorkExperience value)
        {
            return xp - value.xp;
        }

        public bool InBound(int xpRequired, int maxXp)
        {
            return xp >= xpRequired && xp < maxXp;
        }
    }
    enum WorkExperienceType
    {        
        Farm,
        AnimalCare,
        HouseBuilding,
        WoodWork,
        StoneCutter,
        Mining,
        Transport,
        Cook,
        Fletcher,
        Smelting,
        CastMetal,
        CraftMetal,
        CraftArmor,
        CraftFuel,
        Chemistry,
        NUM_NONE,
    }
}
