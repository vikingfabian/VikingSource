using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.Network;

namespace VikingEngine.DSSWars.Map.Generate
{
    struct MapSettingsStorage
    {
        public static readonly MapSettingsStorage Default = new MapSettingsStorage()
        {
            seed = 0,
        };

        public bool customSeed;
        public ushort seed;

        public void write(System.IO.BinaryWriter w)
        {
            w.Write(customSeed);
            w.Write(seed);
        }

        public void read(System.IO.BinaryReader r, int version)
        {
            if (version >= 23)
            {
                customSeed = r.ReadBoolean();
                seed = r.ReadUInt16();
            }
        }
    }

    class MapGenerateSettings
    {
        public bool useGenerate = false;

        public float LandChainMinRadius = 2;
        public float LandChainMaxRadius = 30;
        public IntervalF linkPosDiffRange = new IntervalF(0.5f, 3);
        public Range landSpotSzRange = new Range(2, 24);
        public IntervalF startRadiusRange;
        public Range chainLengthRange = new Range(2, 20);

        //public Range chainLengthRange2 = new Range(20, 150);

        public float BuildChainsCount_per100Tiles = 0.1f; //Per 100 tiles 
        public float DigChainsCount_per100Tiles = 0.07f; //Per 100 tiles 

        public int repeatBuildDigCount = 3;
        public MapStartAs StartAs = MapStartAs.Water;

        public bool bCustomSize = false;
        public IntVector2 customMapSize = new IntVector2(WorldData.CustomMapSize_Min);
        public bool cleanUpSingleTiles = false;
        public bool factionsOnMap = true;

        public float percentageUnclaimed = 0.25f;

        public MapSettingsStorage storage;

        public MapGenerateSettings()
        {
            storage = DssRef.storage.mapSettings;
            startRadiusRange = new IntervalF(LandChainMinRadius, LandChainMaxRadius * 0.5f);
        }

        public bool CustomSizeProperty(object tag, bool set, bool value)
        {
            if (set)
            {
                bCustomSize = value;
                (value ? SoundLib.click : SoundLib.back).Play();
            }
            return bCustomSize;
        }

        public bool CleanUpProperty(object tag, bool set, bool value)
        {
            if (set)
            {
                cleanUpSingleTiles = value;
                (value ? SoundLib.click : SoundLib.back).Play();
            }
            return cleanUpSingleTiles;
        }

        public int MapXProperty(object tag, bool set, int value)
        {
            if (set)
            {
                customMapSize.X = value;
            }
            return customMapSize.X;
        }
        public int MapYProperty(object tag, bool set, int value)
        {
            if (set)
            {
                customMapSize.Y = value;
            }
            return customMapSize.Y;
        }

        public void setCustomSize(IntVector2 customMapSize)
        { 
            this.customMapSize = customMapSize;
            
            bCustomSize = true;
        }

    }
}
