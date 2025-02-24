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

        static readonly string BattleDir = AmbienceDir + "battle" + DataStream.FilePath.Dir;

        static readonly string MelodyDir = AmbienceDir + "melody" + DataStream.FilePath.Dir;

        static readonly string MelodyNorthDir = MelodyDir + "north" + DataStream.FilePath.Dir;
        static readonly string MelodySouthDir = MelodyDir + "south" + DataStream.FilePath.Dir;
        static readonly string MelodyWarsDir = MelodyDir + "wars" + DataStream.FilePath.Dir;

        static readonly string WindDir = AmbienceDir + "wind" + DataStream.FilePath.Dir;
        static readonly string WindColdDir = WindDir + "cold" + DataStream.FilePath.Dir;
        static readonly string WindHotDir = WindDir + "hot" + DataStream.FilePath.Dir;
        static readonly string WindMidDir = WindDir + "mid" + DataStream.FilePath.Dir;
        static readonly string WindSeaDir = WindDir + "sea" + DataStream.FilePath.Dir;
        static readonly string WindFarDir = WindDir + "far out" + DataStream.FilePath.Dir;

        static readonly LoopingSoundData[] MelodyNorth = new LoopingSoundData[]
            {
                new LoopingSoundData(MelodyNorthDir + "drkfnt_amb_v2_winter_loop", 0.08f),
                new LoopingSoundData(MelodyNorthDir + "drkfnt_amb_v5_way_loop", 0.08f),
                new LoopingSoundData(MelodyNorthDir + "scifi_amb_v1_cold_loop", 0.08f),
                new LoopingSoundData(MelodyNorthDir + "space_amb_v1_theme4_loop", 0.08f),
                new LoopingSoundData(MelodyNorthDir + "surreal_amb_forest_loop", 0.08f),
                new LoopingSoundData(MelodyNorthDir + "surreal_amb_frozen_loop", 0.08f),
                new LoopingSoundData(MelodyNorthDir + "surreal_amb_infinity2_loop", 0.08f),
                new LoopingSoundData(MelodyNorthDir + "surreal_amb_ruins_loop", 0.08f),
                new LoopingSoundData(MelodyNorthDir + "surreal_amb_winds_loop", 0.08f),
            };

        static readonly LoopingSoundData[] MelodySouth = new LoopingSoundData[]
            {
                new LoopingSoundData(MelodySouthDir + "drkfnt_amb_v1_theme1_loop", 0.08f),
                new LoopingSoundData(MelodySouthDir + "drkfnt_amb_v4_flute_loop", 0.08f),
                new LoopingSoundData(MelodySouthDir + "surreal_amb_disturbing1_loop", 0.08f),
                new LoopingSoundData(MelodySouthDir + "surreal_amb_disturbing2_loop", 0.08f),
            };

        static readonly LoopingSoundData[] MelodyWars = new LoopingSoundData[]
            {
                new LoopingSoundData(MelodyWarsDir + "drkfnt_amb_v2_flute1_loop", 0.08f),
                new LoopingSoundData(MelodyWarsDir + "drkfnt_amb_v2_horn2_loop", 0.08f),
                new LoopingSoundData(MelodyWarsDir + "drkfnt_amb_v3_cursed_loop", 0.08f),
                new LoopingSoundData(MelodyWarsDir + "drkfnt_amb_v3_underworld_loop", 0.08f),
                new LoopingSoundData(MelodyWarsDir + "drkfnt_amb_v5_horn_loop", 0.08f),
                new LoopingSoundData(MelodyWarsDir + "drkfnt_amb_v6_valhalla_loop", 0.08f),
                new LoopingSoundData(MelodyWarsDir + "horror_amb_v1_melodic3_loop", 0.08f),
                new LoopingSoundData(MelodyWarsDir + "horror_amb_v1_theme7_loop", 0.08f),
                new LoopingSoundData(MelodyWarsDir + "horror_amb_v1_theme8_loop", 0.08f),
                new LoopingSoundData(MelodyWarsDir + "scifi_amb_v1_theme5_loop", 0.08f),
                new LoopingSoundData(MelodyWarsDir + "scifi_amb_v1_theme6_loop", 0.08f),
                new LoopingSoundData(MelodyWarsDir + "space_amb_v1_interstellar3_loop", 0.08f),
            };

        static readonly LoopingSoundData[] MelodyGeneral = new LoopingSoundData[]
            {
                new LoopingSoundData(MelodyDir + "drkfnt_amb_v1_flute_loop", 0.08f),
                new LoopingSoundData(MelodyDir + "drkfnt_amb_v1_melodic1_loop", 0.08f),
                new LoopingSoundData(MelodyDir + "drkfnt_amb_v1_melodic2_loop", 0.08f),
                new LoopingSoundData(MelodyDir + "drkfnt_amb_v1_melodic3_loop", 0.08f),
                new LoopingSoundData(MelodyDir + "drkfnt_amb_v1_melodic4_loop", 0.08f),
                new LoopingSoundData(MelodyDir + "drkfnt_amb_v1_theme2_loop", 0.08f),
                new LoopingSoundData(MelodyDir + "drkfnt_amb_v1_theme3_loop", 0.08f),
                new LoopingSoundData(MelodyDir + "drkfnt_amb_v1_theme4_loop", 0.08f),
                new LoopingSoundData(MelodyDir + "drkfnt_amb_v1_theme5_loop", 0.08f),
                new LoopingSoundData(MelodyDir + "drkfnt_amb_v1_theme7_loop", 0.08f),
                new LoopingSoundData(MelodyDir + "drkfnt_amb_v1_theme8_loop", 0.08f),
                new LoopingSoundData(MelodyDir + "drkfnt_amb_v1_theme9_loop", 0.08f),
                new LoopingSoundData(MelodyDir + "drkfnt_amb_v1_wind1_loop", 0.08f),
                new LoopingSoundData(MelodyDir + "drkfnt_amb_v1_wind2_loop", 0.08f),
                new LoopingSoundData(MelodyDir + "drkfnt_amb_v2_darkness1_loop", 0.08f),
                new LoopingSoundData(MelodyDir + "drkfnt_amb_v2_darkness2_loop", 0.08f),
                new LoopingSoundData(MelodyDir + "drkfnt_amb_v2_destiny_loop", 0.08f),
                new LoopingSoundData(MelodyDir + "drkfnt_amb_v2_forest2_loop", 0.08f),
                new LoopingSoundData(MelodyDir + "drkfnt_amb_v3_calm_loop", 0.08f),
                new LoopingSoundData(MelodyDir + "drkfnt_amb_v3_flute_loop", 0.08f),
                new LoopingSoundData(MelodyDir + "drkfnt_amb_v3_silence_loop", 0.08f),
                new LoopingSoundData(MelodyDir + "drkfnt_amb_v4_horn_loop", 0.08f),
                new LoopingSoundData(MelodyDir + "drkfnt_amb_v4_strings_loop", 0.08f),
                new LoopingSoundData(MelodyDir + "drkfnt_amb_v5_darkness2_loop", 0.08f),
                new LoopingSoundData(MelodyDir + "drkfnt_amb_v5_darkness2_loop", 0.08f),
                new LoopingSoundData(MelodyDir + "drkfnt_amb_v6_fort_loop", 0.08f),
                new LoopingSoundData(MelodyDir + "drkfnt_amb_v6_mystic_loop", 0.08f),
                new LoopingSoundData(MelodyDir + "scifi_amb_v1_infinity_loop", 0.08f),
                new LoopingSoundData(MelodyDir + "space_amb_v1_interstellar1_loop", 0.08f),
                new LoopingSoundData(MelodyDir + "space_amb_v1_interstellar2_loop", 0.08f),
                new LoopingSoundData(MelodyDir + "space_amb_v1_low1_loop", 0.08f),
                new LoopingSoundData(MelodyDir + "space_amb_v1_low2_loop", 0.08f),
                new LoopingSoundData(MelodyDir + "space_amb_v1_moon2_loop", 0.08f),
                new LoopingSoundData(MelodyDir + "space_amb_v1_theme5_loop", 0.08f),
            };


        static readonly LoopingSoundData[] Battle = new LoopingSoundData[]
           {
                new LoopingSoundData(BattleDir + "music_guitar_120bpm_loop_theme_02", 0.08f),
                new LoopingSoundData(BattleDir + "music_percussion_120bpm_loop_theme_01", 0.08f),
                new LoopingSoundData(BattleDir + "music_percussion_120bpm_loop_theme_03", 0.08f),
                new LoopingSoundData(BattleDir + "music_strings_120bpm_loop_theme_02", 0.08f),
                new LoopingSoundData(BattleDir + "music_strings_120bpm_loop_theme_03", 0.08f),
                new LoopingSoundData(BattleDir + "music_strings_120bpm_loop_theme_05", 0.08f),
                new LoopingSoundData(BattleDir + "strings_120bpm_loop_theme_02", 0.08f),
                new LoopingSoundData(BattleDir + "strings_120bpm_loop_theme_03", 0.08f),
                new LoopingSoundData(BattleDir + "strings_120bpm_loop_theme_04", 0.08f),
                new LoopingSoundData(BattleDir + "strings_120bpm_loop_theme_08", 0.08f),
                new LoopingSoundData(BattleDir + "strings_120bpm_loop_theme_09", 0.08f),
                new LoopingSoundData(BattleDir + "strings_120bpm_loop_theme_12", 0.08f),
                new LoopingSoundData(BattleDir + "strings_120bpm_loop_theme_13", 0.08f),
                new LoopingSoundData(BattleDir + "strings_120bpm_loop_theme_15", 0.08f),
                new LoopingSoundData(BattleDir + "strings_120bpm_loop_theme_17", 0.08f),
                new LoopingSoundData(BattleDir + "strings_120bpm_loop_theme_18", 0.08f),
                new LoopingSoundData(BattleDir + "strings_120bpm_loop_theme_20", 0.08f),
           };

        static readonly LoopingSoundData[] WindCold = new LoopingSoundData[]
            {
                new LoopingSoundData(WindColdDir + "wind_cold_arctic_01_loop", 0.08f),
                new LoopingSoundData(WindColdDir + "wind_cold_frost_med_loop", 0.08f),
                new LoopingSoundData(WindColdDir + "wind_cold_winter_01_loop", 0.08f),
                new LoopingSoundData(WindColdDir + "wind_outside_plaine_med_loop", 0.08f),
            };

        static readonly LoopingSoundData[] WindHot = new LoopingSoundData[]
            {
                new LoopingSoundData(WindHotDir + "wind_hot_dry_low_loop", 0.08f),
                new LoopingSoundData(WindHotDir + "wind_hot_dune_med_loop", 0.08f),
                new LoopingSoundData(WindHotDir + "wind_hot_summer_loop", 0.08f),

            };

        static readonly LoopingSoundData[] WindSea = new LoopingSoundData[]
           {
                new LoopingSoundData(WindSeaDir + "wind_outside_lake_01_loop", 0.08f),
                new LoopingSoundData(WindSeaDir + "wind_outside_lake_02_loop", 0.08f),
                new LoopingSoundData(WindSeaDir + "wind_outside_mountain_low_loop", 0.08f),
                new LoopingSoundData(WindSeaDir + "wind_outside_seaside_low_loop", 0.08f),

           };

        static readonly LoopingSoundData[] WindMid = new LoopingSoundData[]
           {
                new LoopingSoundData(WindMidDir + "wind_hot_desert_low_loop", 0.08f),
                new LoopingSoundData(WindMidDir + "wind_hot_dune_low_loop", 0.08f),
                new LoopingSoundData(WindMidDir + "wind_outside_forest_low_loop", 0.08f),
                new LoopingSoundData(WindMidDir + "wind_outside_forest_med_loop", 0.08f),
                new LoopingSoundData(WindMidDir + "wind_outside_meadow_low_loop", 0.08f),
                new LoopingSoundData(WindMidDir + "wind_outside_plaine_low_loop", 0.08f),
           };        

        static readonly LoopingSoundData[] Wind_farout = new LoopingSoundData[]
            {
                new LoopingSoundData(WindFarDir + "wind_cold_blizzard_low_loop", 0.1f),
                new LoopingSoundData(WindFarDir + "wind_cold_blizzard_med_loop", 0.1f),
                new LoopingSoundData(WindFarDir + "wind_hot_dune_high_loop", 0.1f),
                new LoopingSoundData(WindFarDir + "wind_outside_field_med_loop", 0.1f),
                new LoopingSoundData(WindFarDir + "wind_outside_mountain_high_loop", 0.1f),
                new LoopingSoundData(WindFarDir + "wind_outside_valley_low_loop", 0.1f),
            };

        bool currentPlayingMelody = false;
        LoopingSound currentNearSound, nextNearSound, loadingNearSound;
        LoopingSound currentFarSound, nextFarSound, loadingFarSound;

        double volumeCurveTime = 0;
        const float FarNearFadeSpeed_PerSec = 1f;
        const float NewSoundFadeSpeed_PerSec = 0.25f;
        float farOutFade = 1f;
        float newNearFade = 0;
        Time nextNearSoundLoad = new Time(2f, TimeUnit.Seconds);
        Time nextFarSoundLoad = new Time(5, TimeUnit.Minutes);

        SoundLoadingState nearLoadingState = SoundLoadingState.None;
        public Ambience()
        { }

        public void update()
        {
            volumeCurveTime += Ref.DeltaTimeSec * Ref.rnd.Double() * 0.2;
            float volumeCurve = 1f + (float)(Math.Sin(volumeCurveTime) * 0.3);

            float goalFade;
            var detailLayer = Map.MapDetailLayerManager.CameraIndexToView[0];
            switch (detailLayer.current.type)
            {
                case Map.MapDetailLayerType.UnitDetail1:
                    goalFade = 0;
                    break;
                case Map.MapDetailLayerType.TerrainOverview2:
                    goalFade = 0.4f;
                    break;
                default:
                    goalFade = 0.8f;
                    break;
            }
            farOutFade = Bound.Set(farOutFade - FarNearFadeSpeed_PerSec * lib.ToLeftRight(farOutFade - goalFade) * Ref.DeltaGameTimeSec, 0f, 1f);

            float farVolRaise = 1f + detailLayer.PercZoom() * 0.3f;
            volumeCurve *= farVolRaise;

            float nearSoundLevel = volumeCurve * (1f - farOutFade);
            float farSoundLevel = volumeCurve * farOutFade;
            currentNearSound.setVolume(nearSoundLevel * (1f - newNearFade));
            if (newNearFade > 0)
            {
                nextNearSound.setVolume(nearSoundLevel * newNearFade);
            }
            currentFarSound.setVolume(farSoundLevel);

            switch (nearLoadingState)
            {
                case SoundLoadingState.None:
                    {
                        if (nextNearSoundLoad.CountDown())
                        {
                            nearLoadingState = SoundLoadingState.Loading;
                            new Timer.AsynchActionTrigger(loadNextNearSound_async, true);
                        }
                    }
                    break;
                case SoundLoadingState.Complete:
                    {
                        nextNearSound = loadingNearSound;
                        nextNearSound.setVolume(0f);
                        nextNearSound.Play();

                        loadingNearSound = null;

                        currentPlayingMelody = !currentPlayingMelody;
                        if (currentPlayingMelody)
                        {
                            //Melody is shorter
                            nextNearSoundLoad = new Time(Ref.rnd.Float(5, 30), TimeUnit.Seconds);
                        }
                        else
                        {
                            nextNearSoundLoad = new Time(Ref.rnd.Float(5, 20)/*Ref.rnd.Float(0.5f, 2f)*/, TimeUnit.Seconds);
                        }
                        newNearFade = 0;
                        nearLoadingState = SoundLoadingState.FadeIn;
                    }
                    break;
                case SoundLoadingState.FadeIn:
                    newNearFade += Ref.DeltaTimeSec * NewSoundFadeSpeed_PerSec;
                    if (newNearFade >= 1)
                    {
                        newNearFade = 0;
                        currentNearSound.Stop();
                        currentNearSound = nextNearSound;
                        nearLoadingState = SoundLoadingState.None;
                    }
                    break;
            } 
        }

        public void contentLoad()
        {
            currentNearSound = new LoopingSound();
            currentNearSound.Load(arraylib.RandomListMember(WindMid));

            currentFarSound = new LoopingSound();
            currentFarSound.Load(arraylib.RandomListMember(Wind_farout));
        }

        void loadNextNearSound_async()
        { 
            bool melody = !currentPlayingMelody;
            LoopingSoundData[] list;
            if (melody)
            {
                list = MelodyGeneral;
            }
            else
            {
                list = WindMid;
            }

            loadingNearSound = new LoopingSound();
            loadingNearSound.Load(arraylib.RandomListMember(list));
            nearLoadingState = SoundLoadingState.Complete;
        }

        public void gameStart()
        {
            currentNearSound.Play();
            currentFarSound.Play();
        }
        public void gameEnd()
        {

        }

        enum SoundLoadingState
        { 
            None,
            Loading,
            Complete,
            FadeIn,
        }
    }
}
