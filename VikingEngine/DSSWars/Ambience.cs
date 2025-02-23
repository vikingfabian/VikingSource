using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.Sound;

namespace VikingEngine.DSSWars
{
    /// <summary>
    /// Manages all background ambient sounds
    /// </summary>
    class Ambience
    {
        static readonly string AmbienceDir = SoundLib.SoundDir + DataStream.FilePath.Dir + "ambience" + DataStream.FilePath.Dir;
        static readonly string WindDir = AmbienceDir + "wind" + DataStream.FilePath.Dir;
        static readonly string WindMidDir = WindDir + "mid" + DataStream.FilePath.Dir;
        static readonly string WindFarDir = WindDir + "far out" + DataStream.FilePath.Dir;

        static readonly LoopingSoundData[] Wind_mid = new LoopingSoundData[]
            {
                new LoopingSoundData(WindMidDir + "wind_hot_desert_low_loop", 0.08f),
            };

        static readonly LoopingSoundData[] Wind_farout = new LoopingSoundData[]
            {
                new LoopingSoundData(WindFarDir + "wind_cold_blizzard_low_loop", 0.1f),
            };

        LoopingSound currentWindMid;
        LoopingSound currentWindFarOut;

        double volumeCurveTime = 0;
        const float FarNearFadeSpeed_PerSec = 1f;
        float farOutFade = 1f;
        //float volumeCurve = 1;

        public Ambience()
        { }

        public void update()
        {
            volumeCurveTime += Ref.DeltaTimeSec * Ref.rnd.Double() * 0.2;
            float volumeCurve = 1f + (float)(Math.Sin(volumeCurveTime) * 0.3);

            float goalFade;
            switch (Map.MapDetailLayerManager.CameraIndexToView[0].current.type)
            {
                case Map.MapDetailLayerType.UnitDetail1:
                    goalFade = 0;
                    break;
                case Map.MapDetailLayerType.TerrainOverview2:
                    goalFade = 0.5f;
                    break;
                default:
                    goalFade = 1f;
                    break;
            }
            farOutFade = Bound.Set(farOutFade - FarNearFadeSpeed_PerSec * lib.ToLeftRight(farOutFade-goalFade) * Ref.DeltaGameTimeSec, 0f, 1f);

            currentWindMid.setVolume(volumeCurve * (1f - farOutFade));
            currentWindFarOut.setVolume(volumeCurve * farOutFade);

        }

        public void contentLoad()
        {
            currentWindMid = new LoopingSound();
            currentWindMid.Load(arraylib.RandomListMember(Wind_mid));

            currentWindFarOut = new LoopingSound();
            currentWindFarOut.Load(arraylib.RandomListMember(Wind_farout));
        }

        public void gameStart()
        {
            currentWindMid.Play();
            currentWindFarOut.Play();
        }
        public void gameEnd()
        {

        }
    }
}
