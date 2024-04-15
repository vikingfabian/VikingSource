using Microsoft.Xna.Framework;

using VikingEngine.HUD.RichBox;

namespace VikingEngine.DSSWars
{
    static class HudLib
    {
        public const string OneDecimalFormat = "{0:0.0}";
        public const float HeadDisplayBgOpacity = 0.9f;
        public static float HeadDisplayWidth, HeadDisplayEdge;

        public const ImageLayers StoryContentLayer = ImageLayers.Lay1_Front;
        public const ImageLayers StoryBgLayer = ImageLayers.Lay1_Back;

        public const ImageLayers CutContentLayer = ImageLayers.Lay2;
        public const ImageLayers CutSceneBgLayer = ImageLayers.Lay3;

        public const ImageLayers HeadDisplayContentLayer = ImageLayers.Lay6;
        public const ImageLayers HeadDisplayLayer = ImageLayers.Lay7;

        public const ImageLayers DiplomacyDisplayLayer = ImageLayers.Lay8;

        public static RichBoxSettings RbSettings;

        public static RichboxGuiSettings richboxGui;
        public static RichboxGuiSettings cutsceneGui;

        public static void Init()
        {
            const float TextToIconSz = 1.2f;

            RbSettings = new HUD.RichBox.RichBoxSettings(
                new TextFormat(LoadedFont.Regular, Engine.Screen.TextBreadHeight, Color.White, ColorExt.Empty),
                new TextFormat(LoadedFont.Regular, Engine.Screen.TextBreadHeight, Color.Black, Color.CornflowerBlue),
            Engine.Screen.TextBreadHeight * TextToIconSz, 1.1f);
            RbSettings.head1.Font = LoadedFont.Bold;
            RbSettings.head1.Color = Color.LightGray;

            HeadDisplayWidth = Engine.Screen.IconSize * 6;
            HeadDisplayEdge = Engine.Screen.BorderWidth;

            richboxGui = new RichboxGuiSettings()
            {
                bgCol = Color.Black,
                bgAlpha = HeadDisplayBgOpacity,
                edgeWidth = HeadDisplayEdge,
                width = HeadDisplayWidth,
                contentLayer= HeadDisplayContentLayer,
                bglayer = HeadDisplayLayer,
                RbSettings= RbSettings,
            };

            cutsceneGui = new RichboxGuiSettings()
            {
                bgCol = Color.Black,
                bgAlpha = 0.5f,
                edgeWidth = HeadDisplayEdge,
                width = HeadDisplayWidth,
                contentLayer = CutContentLayer -2,
                bglayer = CutSceneBgLayer -2,
                RbSettings = RbSettings,
            };
        }

        public static void ResourceCost(RichBoxContent content, GameObject.Resource.ResourceType resource, int needResource, int hasResource)
        {
            SpriteName icon = GameObject.Resource.ResourceLib.PayIcon(resource);

            if (icon != SpriteName.NO_IMAGE)
            {
                content.Add(new RichBoxImage(icon));
            }

            string text = string.Format(DssRef.lang.Hud_Purchase_ResourceCostOfAvailable,
                GameObject.Resource.ResourceLib.Name(resource), TextLib.LargeNumber(needResource), TextLib.LargeNumber(hasResource));

            content.Add( new RichBoxText(text, ResourceCostColor(hasResource >= needResource)));
        }

        public static void Upkeep(RichBoxContent content, int value, bool increase)
        {
            string text = DssRef.lang.Hud_Upkeep;
            if (increase) 
            {
                text += " +";
            }
            else
            {
                text += ": ";
            }
            content.icontext(SpriteName.rtsUpkeepTime, text + TextLib.LargeNumber(value));  
        }
        public static Color ResourceCostColor(bool hasEnough)
        { 
            return hasEnough ? Color.LightGreen : Color.Red;
        }

        public static SpriteName CheckImage(bool value)
        { 
            return value? SpriteName.cmdHudCheckOn : SpriteName.cmdHudCheckOff;
        }
        
    }
}
