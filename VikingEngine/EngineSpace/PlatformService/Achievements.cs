using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.HUD;

namespace VikingEngine.PlatformService
{
    static class Achievements
    {
        const bool Debug_ViewOnScreen = true;
        public static List<string> Unlocked = new List<string>(32);//bool[] Unlocked = new bool[32]; 

        public static void Set(ref AchievementSettings achievement)
        {
            VikingEngine.Graphics.Text2 debugtext = null;

            if (PlatformSettings.DevBuild)
            {
                Debug.Log("ACHIEVEMENT: " + achievement.DebugName);
                if (Debug_ViewOnScreen)
                {
                    debugtext = new VikingEngine.Graphics.Text2(
                        achievement.DebugName, LoadedFont.Bold,
                        Engine.Screen.Area.PercentToPosition(new Vector2(0.5f, 0.3f)),
                        Engine.Screen.IconSize,
                        Color.Blue, ImageLayers.AbsoluteTopLayer);
                    debugtext.OrigoAtCenter();

                    new Timer.Terminator(4000, debugtext);
                }
            }

            if (!achievement.IsUnlocked)
            {
                achievement.IsUnlocked = true;

                if (TextLib.IsEmpty(achievement.id))
                {
                    if (debugtext != null)
                    {
                        debugtext.TextString += " - ERR empty ID";
                    }
                    else if (PlatformSettings.DevBuild)
                    {
                        throw new Exception("Missing Id: " + achievement.DebugName);
                    }
                }
                else
                {
#if XBOX
                    Ref.xbox.achievements.Unlock(achievement.id);
#endif
#if PCGAME
                    if (Ref.steam.isInitialized)
                    {
                        Ref.steam.Achievements.SetAchievement(achievement.id);
                    }
#endif
                }
            }
        }

        public static void ToMenu(List<AchievementSettings> achievements, HUD.Gui menu)
        {
            GuiLayout layout = new GuiLayout("Debug Achievements", menu);
            {
                foreach (var m in achievements)
                {
                    new GuiTextButton(m.DebugName + " #" + m.id.ToString() + "(" + m.IsUnlocked.ToString() + ")",
                        "Click to unlock", m.Unlock, false, layout);
                }
            }
            layout.End();
        }
    }
    
    struct AchievementSettings
    {
        //static int NextIndex;

        public string DebugName;
        public string id;
        //int index;

        public AchievementSettings(string DebugName, string steamId, string xboxId, FeatureStage featureStage)
        {
            if (PlatformSettings.DebugLevel == BuildDebugLevel.Release &&
                featureStage != FeatureStage.Tested_2)
            {
                throw new Exception("Untested Achievement: " + DebugName);
            }

            this.DebugName = DebugName;
#if XBOX
            this.id = xboxId;
#else
            this.id = steamId;
#endif
            //index = NextIndex++;
        }

        public void Unlock()
        {
            Achievements.Set(ref this);
        }

        public bool IsUnlocked
        {
            get { return Achievements.Unlocked.Contains(id); }
            set
            {
                Achievements.Unlocked.Add(id);
            }
        }        
    }
}
