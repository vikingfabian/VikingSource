using Microsoft.CodeAnalysis.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Map;
using VikingEngine.DSSWars.Map.Settings;
using VikingEngine.Engine;
using VikingEngine.Sound;

namespace VikingEngine.DSSWars
{
    /// <summary>
    /// Manages all background ambient sounds
    /// </summary>
    class Ambience
    {
        //const bool BlockAmbience = true;

        public static readonly string AmbienceDir = SoundLib.SoundDir + DataStream.FilePath.Dir + "ambience" + DataStream.FilePath.Dir;

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
                new LoopingSoundData(BattleDir + "music_guitar_120bpm_loop_theme_02", 0.3f),
                new LoopingSoundData(BattleDir + "music_percussion_120bpm_loop_theme_01", 0.3f),
                new LoopingSoundData(BattleDir + "music_percussion_120bpm_loop_theme_03", 0.3f),
                new LoopingSoundData(BattleDir + "music_strings_120bpm_loop_theme_02", 0.3f),
                new LoopingSoundData(BattleDir + "music_strings_120bpm_loop_theme_03", 0.3f),
                new LoopingSoundData(BattleDir + "music_strings_120bpm_loop_theme_05", 0.3f),
                new LoopingSoundData(BattleDir + "strings_120bpm_loop_theme_02", 0.3f),
                new LoopingSoundData(BattleDir + "strings_120bpm_loop_theme_03", 0.3f),
                new LoopingSoundData(BattleDir + "strings_120bpm_loop_theme_04", 0.3f),
                new LoopingSoundData(BattleDir + "strings_120bpm_loop_theme_08", 0.3f),
                new LoopingSoundData(BattleDir + "strings_120bpm_loop_theme_09", 0.3f),
                new LoopingSoundData(BattleDir + "strings_120bpm_loop_theme_12", 0.3f),
                new LoopingSoundData(BattleDir + "strings_120bpm_loop_theme_13", 0.3f),
                new LoopingSoundData(BattleDir + "strings_120bpm_loop_theme_15", 0.3f),
                new LoopingSoundData(BattleDir + "strings_120bpm_loop_theme_17", 0.3f),
                new LoopingSoundData(BattleDir + "strings_120bpm_loop_theme_18", 0.3f),
                new LoopingSoundData(BattleDir + "strings_120bpm_loop_theme_20", 0.3f),
           };

        static readonly LoopingSoundData[] WindCold = new LoopingSoundData[]
            {
                new LoopingSoundData(WindColdDir + "wind_cold_arctic_01_loop", 0.12f),
                new LoopingSoundData(WindColdDir + "wind_cold_frost_med_loop", 0.12f),
                new LoopingSoundData(WindColdDir + "wind_cold_winter_01_loop", 0.12f),
                new LoopingSoundData(WindColdDir + "wind_outside_plaine_med_loop", 0.12f),
            };

        static readonly LoopingSoundData[] WindHot = new LoopingSoundData[]
            {
                new LoopingSoundData(WindHotDir + "wind_hot_dry_low_loop", 0.12f),
                new LoopingSoundData(WindHotDir + "wind_hot_dune_med_loop", 0.12f),
                new LoopingSoundData(WindHotDir + "wind_hot_summer_loop", 0.12f),

            };

        static readonly LoopingSoundData[] WindSea = new LoopingSoundData[]
           {
                new LoopingSoundData(WindSeaDir + "sea sound1", 0.12f),
                new LoopingSoundData(WindSeaDir + "sea sound2", 0.12f),
                new LoopingSoundData(WindSeaDir + "sea sound3", 0.12f),

           };

        static readonly LoopingSoundData[] WindMid = new LoopingSoundData[]
           {
                new LoopingSoundData(WindMidDir + "wind_hot_desert_low_loop", 0.12f),
                new LoopingSoundData(WindMidDir + "wind_hot_dune_low_loop", 0.12f),
                new LoopingSoundData(WindMidDir + "wind_outside_forest_low_loop", 0.12f),
                new LoopingSoundData(WindMidDir + "wind_outside_forest_med_loop", 0.12f),
                new LoopingSoundData(WindMidDir + "wind_outside_meadow_low_loop", 0.12f),
                new LoopingSoundData(WindMidDir + "wind_outside_plaine_low_loop", 0.12f),
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


        AmbientSoundLoop nearLoop, farLoop, seaLoop, battleLoop;

        double volumeCurveTime = 0;
        const float FarNearFadeSpeed_PerSec = 1f;
        const float MaxSeaLevel = 1.5f;
        float farOutFade = 1f;
        float deepSeaFade = 0f;
        float battleFade = 0f;
        float musicReduceFade = 1f;
        public Ambience()
        {
            
            nearLoop = new AmbientSoundLoop();
            farLoop = new AmbientSoundLoop();
            seaLoop = new AmbientSoundLoop();
            battleLoop = new AmbientSoundLoop();
        }
        public void contentLoad()
        {
            
            nearLoop.contentLoad(WindMid, new IntervalF(5, 20),
                MelodyGeneral, new IntervalF(5, 20));

            farLoop.contentLoad(Wind_farout, new IntervalF(5, 10) * TimeExt.MinuteInSeconds,
                null, IntervalF.Zero);

            seaLoop.contentLoad(WindSea, new IntervalF(10, 20) * TimeExt.MinuteInSeconds,
                null, IntervalF.Zero);

            battleLoop.contentLoad(Battle, new IntervalF(40, 80) * TimeExt.MinuteInSeconds,
                null, IntervalF.Zero);
        }
        public void update_async()
        {
            
            IntVector2 tileCenter = WP.ToTilePos( DssRef.state.culling.players[0].MapCenter);
            Tile onTile = DssRef.world.tileGrid.Get(tileCenter);                      

            volumeCurveTime += Ref.DeltaTimeSec * Ref.peRnd.Float() * 0.2;
            float volumeCurve = 1f + (float)(Math.Sin(volumeCurveTime) * 0.3);

            float goalFade;
            var detailLayer = Map.MapLayerManager.CameraIndexToView[0];
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

            if (Ref.music != null)
            {
                musicReduceFade = Bound.Set(musicReduceFade - FarNearFadeSpeed_PerSec * lib.BoolToLeftRight(Ref.music.IsPlayingNowOrSoon()) * Ref.DeltaGameTimeSec, 0f, 1f);
            }

            bool playerLookingAtBattle = detailLayer.current.type == MapDetailLayerType.UnitDetail1 &&
                DssRef.world.unitCollAreaGrid.PlayerInBattle(tileCenter, DssRef.state.LocalHost().faction.myIndex);
            bool hadBattleSound = battleFade > 0;
            battleFade = Bound.Set(battleFade + FarNearFadeSpeed_PerSec * lib.BoolToLeftRight(playerLookingAtBattle) * Ref.DeltaGameTimeSec, 0f, 1f);
            var battleFadeTotal = battleFade * musicReduceFade * Ref.gamesett.BattleMelodyVol();
            if (hadBattleSound && battleFade <= 0)
            {
                //Change battle melody after a battle
                battleLoop.reduceChangeSoundTimer();
            }
            float battleReduce = 1f - battleFadeTotal;

            int deepSeaSoundLevelDir = lib.BoolToLeftRight(onTile.heightLevel <= Height.DeepWaterHeight);
            deepSeaFade = Bound.Set(deepSeaFade + FarNearFadeSpeed_PerSec * deepSeaSoundLevelDir * Ref.DeltaGameTimeSec, 0f, MaxSeaLevel);
            float seaSoundReduce = 1f - deepSeaFade * 0.5f;


            float farVolRaise = 1f + detailLayer.PercZoom() * 0.3f;
            volumeCurve *= farVolRaise;

            float nearSoundLevel = volumeCurve * (1f - farOutFade) * seaSoundReduce * battleReduce * musicReduceFade;
            float farSoundLevel = volumeCurve * farOutFade * seaSoundReduce;

            nearLoop.update(nearSoundLevel, out bool nearNeedSoundBiom);
            if (nearNeedSoundBiom)
            {
                LoopingSoundData[] wind, melody;
                switch (onTile.biom)
                { 
                    case BiomType.YellowDry:
                    case BiomType.RedDry:
                        wind = WindHot;
                        melody = MelodySouth;
                        break;
                    case BiomType.Frozen:
                        wind = WindCold;
                        melody = MelodyNorth;
                        break;
                    default:
                        wind = WindMid;
                        melody = MelodyGeneral;
                        break;
                }

                float dangerLevel = 0f;
                float warLevel = DssRef.state.localPlayers[0].opposingSizePerc;

                if (warLevel >= 2)
                {
                    dangerLevel = 1f;
                    melody = MelodyWars;
                }
                else if (warLevel >= 1)
                {
                    dangerLevel = 0.5f;
                }
                else if (warLevel >= 0.5f)
                {
                    dangerLevel = 0.2f;
                }

                var windTime = new IntervalF(5, 20) + 10 * (1 - dangerLevel);
                var melodyTime = new IntervalF(5, 20) + 20 * dangerLevel;

                nearLoop.SetBiom(wind, windTime, melody, melodyTime);
                nearLoop.SoundBiomReady();
            }
            farLoop.update(farSoundLevel * musicReduceFade, out bool farNeedSoundBiom);
            if (farNeedSoundBiom)
            {
                farLoop.SoundBiomReady();
            }
            seaLoop.update(deepSeaFade * musicReduceFade, out bool seaNeedSoundBiom);
            if (seaNeedSoundBiom)
            {
                seaLoop.SoundBiomReady();
            }
            battleLoop.update(battleFadeTotal * musicReduceFade, out bool battleNeedSoundBiom);
            if (battleNeedSoundBiom)
            {
                battleLoop.SoundBiomReady();
            }
        }

        public void gameStart()
        {
            //nearLoop.Play();
            //farLoop.Play();
            //seaLoop.Play();
            //battleLoop.Play();
        }
        public void gameEnd()
        {
            
            nearLoop.stop();
            farLoop.stop();
            seaLoop.stop();
            battleLoop.stop();
        }        
    }

    class AmbientSoundLoop
    {
        const float NewSoundFadeSpeed_PerSec = 0.25f;
        LoopingSoundData[] soundList, melodyList;


        bool currentPlayingMelody = false;
        LoopingSound currentSound, nextSound, loadingSound;

        float newSoundFade = 0;
        IntervalF playTime_sound_sec, playTime_melody_sec;
        Time nextNearSoundLoad = new Time(2f, TimeUnit.Seconds);
        SoundLoadingState loadingState = SoundLoadingState.None;

        public void contentLoad(LoopingSoundData[] soundList, IntervalF playTime_sound,
            LoopingSoundData[] melodyList, IntervalF playTime_melody)
        {
            this.soundList = soundList;
            this.melodyList = melodyList;

            this.playTime_sound_sec = playTime_sound;
            this.playTime_melody_sec = playTime_melody;

            nextNearSoundLoad = new Time(playTime_sound_sec.PeRandom(), TimeUnit.Seconds);

            currentSound = new LoopingSound();
            currentSound.Load(arraylib.RandomListMember(soundList));
        }

        public void Play()
        {
            currentSound.setVolume(0);
            currentSound.Play();
        }

        public void update(float volume, out bool needSoundBiom)
        {
            currentSound.setVolume(volume * (1f - newSoundFade));
            if (newSoundFade > 0)
            {
                nextSound.setVolume(volume * newSoundFade);
            }

            switch (loadingState)
            {
                case SoundLoadingState.None:
                    {
                        if (nextNearSoundLoad.CountDown())
                        {
                            loadingState = SoundLoadingState.UpdateSoundBiom;
                        }
                    }
                    break;

                case SoundLoadingState.Complete:
                    {
                        nextSound = loadingSound;
                        nextSound.setVolume(0f);
                        nextSound.Play();

                        loadingSound = null;

                        currentPlayingMelody = !currentPlayingMelody;
                        if (currentPlayingMelody && melodyList != null)
                        {
                            //Melody is shorter
                            nextNearSoundLoad = new Time(playTime_melody_sec.PeRandom(), TimeUnit.Seconds);
                        }
                        else
                        {
                            nextNearSoundLoad = new Time(playTime_sound_sec.PeRandom(), TimeUnit.Seconds);
                        }

                        //Longer melodies when there is more danger

                        newSoundFade = 0;
                        loadingState = SoundLoadingState.FadeIn;
                    }
                    break;
                case SoundLoadingState.FadeIn:
                    newSoundFade += Ref.DeltaTimeSec * NewSoundFadeSpeed_PerSec;
                    if (newSoundFade >= 1)
                    {
                        newSoundFade = 0;
                        currentSound.StopAndUnload();
                        currentSound = nextSound;
                        loadingState = SoundLoadingState.None;
                    }
                    break;
            }

            needSoundBiom = loadingState == SoundLoadingState.UpdateSoundBiom;
        }

        public void reduceChangeSoundTimer()
        {
            nextNearSoundLoad.MilliSeconds *= 0.1f;
        }

        public void SetBiom(LoopingSoundData[] soundList, IntervalF playTime_sound,
            LoopingSoundData[] melodyList, IntervalF playTime_melody)
        {
            this.soundList = soundList;
            this.melodyList = melodyList;

            this.playTime_sound_sec = playTime_sound;
            this.playTime_melody_sec = playTime_melody;
        }

        public void SoundBiomReady()
        {
            loadingState = SoundLoadingState.Loading;
            Ref.update.AddSyncAction(new SyncAction(() =>
            {
                new Timer.AsynchActionTrigger(loadNextNearSound_async, true);
            }));
        }

        void loadNextNearSound_async()
        {
            bool melody = !currentPlayingMelody;
            LoopingSoundData[] list;
            if (melody && melodyList != null)
            {
                list = melodyList;
            }
            else
            {
                list = soundList;
                melody = false;
            }

            loadingSound = new LoopingSound();
            loadingSound.Load(arraylib.RandomListMember(list));
            loadingState = SoundLoadingState.Complete;
        }

        public void stop()
        {
            currentSound?.StopAndUnload();
            nextSound?.StopAndUnload();
        }
        //public void stopAndUnload()
        //{
            
        //}
    }

    enum SoundLoadingState
    {
        None,
        UpdateSoundBiom,
        Loading,
        Complete,
        FadeIn,
    }
}
