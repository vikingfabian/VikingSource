using System;
using System.IO;

using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
//using Microsoft.Xna.Framework.Storage;å
using Microsoft.Xna.Framework.Media;
using VikingEngine.Engine;
using VikingEngine.Graphics;
using System.Threading;

namespace VikingEngine.Engine
{
    static class LoadContent
    {
        public static string SteamVersion = "-"; 
        public static bool BaseContentLoaded = false;
        public static ContentManager Content;
        public static Texture2D[] Textures = new Texture2D[(int)LoadedTexture.NUM];
        static SpriteFont[] Fonts = new SpriteFont[(int)LoadedFont.NUM_NON];
        static SoundEffect[] SoundEffects = new SoundEffect[(int)LoadedSound.NUM];

        static Model[] Models = new Model[(int)LoadedMesh.NUM];//Dictionary<LoadedMesh, Model> modelList = new Dictionary<LoadedMesh, Model>();
        static Effect[] effectList = new Effect[(int)LoadedEffect.NUM_NoEffect];

        public const string TexturePath = "Texture\\";
        public const string ModelPath = "Model\\";

        //static SpriteFont regular, bold, console;
        static FontLanguage currentFontLanguage = FontLanguage.NONE;
        //static SpriteFont chinese_regular, chinese_bold, chinese_console;

        public static Color[] GetTextureData(LoadedTexture texture, Rectangle pixels)
        {
            int numPixels = pixels.Width * pixels.Height;
            Color[] retrievedColor = new Color[numPixels];

            Textures[(int)texture].GetData<Color>(0, pixels, retrievedColor, 0, numPixels);
            
            return retrievedColor;
        }

        public static void Init(ContentManager inContent)
        {
            Content = inContent;
            
            //Load fonts
            Fonts = new SpriteFont[(int)LoadedFont.NUM_NON];

            //regular = Content.Load<SpriteFont>("Font\\Regular");
            //bold = Content.Load<SpriteFont>("Font\\Bold");
            //console = Content.Load<SpriteFont>("Font\\Console");

            

            setFontLanguage(FontLanguage.Western);

            Engine.Screen.RegularFontSize = MeasureString("XXjj", LoadedFont.Regular, out _).Y;
            //Engine.Screen.RefreshUiSize();

            Textures[0] = Content.Load<Texture2D>(TexturePath + "noimage");
            Textures[(int)LoadedTexture.TargetColor0] = Content.Load<Texture2D>(TexturePath + "noimage");
            Textures[(int)LoadedTexture.WhiteArea] = Content.Load<Texture2D>(TexturePath + "whitearea256");
            effectList[(int)LoadedEffect.ParticleEffect] = LoadShader(LoadedEffect.ParticleEffect.ToString());
            BaseContentLoaded = true;
        }

        public static void setFontLanguage(FontLanguage fontLanguage)
        {
            if (currentFontLanguage != fontLanguage)
            {
                currentFontLanguage = fontLanguage;
                switch (fontLanguage)
                {
                    case FontLanguage.Western:
                        var regular = Content.Load<SpriteFont>("Font\\Regular");
                        var bold = Content.Load<SpriteFont>("Font\\Bold");
                        var console = Content.Load<SpriteFont>("Font\\Console");

                        Fonts[(int)LoadedFont.Regular] = regular;
                        Fonts[(int)LoadedFont.Bold] = bold;
                        Fonts[(int)LoadedFont.Console] = console;
                        break;
                    case FontLanguage.Chinese:
                        var chinese_regular = Content.Load<SpriteFont>("Font\\ChineseRegular");
                        var chinese_bold = Content.Load<SpriteFont>("Font\\ChineseBold");
                        var chinese_console = Content.Load<SpriteFont>("Font\\ChineseConsole");

                        Fonts[(int)LoadedFont.Regular] = chinese_regular;
                        Fonts[(int)LoadedFont.Bold] = chinese_bold;
                        Fonts[(int)LoadedFont.Console] = chinese_console;
                        break;

                    case FontLanguage.Japanese:
                        var japanese_regular = Content.Load<SpriteFont>("Font\\JapaneseRegular");
                        var japanese_bold = Content.Load<SpriteFont>("Font\\JapaneseBold");
                        var japanese_console = Content.Load<SpriteFont>("Font\\JapaneseConsole");

                        Fonts[(int)LoadedFont.Regular] = japanese_regular;
                        Fonts[(int)LoadedFont.Bold] = japanese_bold;
                        Fonts[(int)LoadedFont.Console] = japanese_console;
                        break;
                }
            }
        }
       
        public static void LoadMesh(LoadedMesh name, string dir)
        {
            Models[(int)name] = Content.Load<Model>(dir);
        }
        
        public static void LoadTexture(LoadedTexture pointer, string path)
        {
            Textures[(int)pointer] = Content.Load<Texture2D>(path);
        }
        public static Texture2D LoadTexture(string path)
        {
            return Content.Load<Texture2D>(path);
        }

        public static void ClearTexture(LoadedTexture txt)
        {
            Textures[(int)txt] = null;
        }

        public static void LoadSteamVersion()
        {
            var versionFile = DataLib.SaveLoad.LoadTextFile(Engine.LoadContent.Content.RootDirectory + "\\Version Number.txt");
            if (versionFile != null && versionFile.Count > 0)
            {
                SteamVersion = versionFile[0];
                //PlatformSettings.SteamVersion = "Version " + versionFile[0];
            }
        }

       public static void LoadTextures(List<LoadedTexture> loadThese)
       {
            foreach (LoadedTexture loadThis in loadThese)
            {
                Textures[(int)loadThis] = Content.Load<Texture2D>("Texture//" + loadThis.ToString());
            }
        }


        public static Vector2 MeasureString(string text, LoadedFont font, out bool error)
        {
            if (text == null) 
            { 
                error = true;
                return Vector2.One; 
            }

            try
            {
                error = false;
                return Fonts[(int)font].MeasureString(text);
            }
            catch
            {
                error = true;
                return new Vector2(100, 10);
            }
        }
        /// <summary>
        /// replaces non-standard symbol and/or foreign character with '*' 
        /// </summary>
        public static string CheckCharsSafety(string text, LoadedFont font)
        {
            if (font == LoadedFont.NUM_NON)
            {
                return text;
            }

            string result = TextLib.EmptyString;
            SpriteFont sf = Fonts[(int)font];
            foreach (char c in text)
            {
                if (sf.Characters.Contains(c))
                {
                    result += c;
                }
                else
                {
                    const char Replace = '*';
                    result += Replace;
                }
            }
            return result;            
        }

        public static void LoadSound(LoadedSound sound, string dir)
        {
            SoundEffects[(int)sound] = Content.Load<SoundEffect>(dir);
        }
        
        public static void SetTextureFromTarget(Texture2D texture, LoadedTexture name)
        {
            Textures[(int)name] = texture;
        }
        public static void SetTargetTexture(Texture2D texture)
        {
            Textures[(int)LoadedTexture.TargetColor0] = texture;
        }

        public static Texture2D Texture(LoadedTexture texture)
        { return Textures[(int)texture]; }

        public static SpriteFont Font(LoadedFont font)
        { return Fonts[(int)font]; }

        public static Model Mesh(LoadedMesh mesh)
        { 
            return Models[(int)mesh]; 
        }

        public static Effect Effect(LoadedEffect e)
        {
            return effectList[(int)e];
        }

        public static SoundEffect Sound(LoadedSound sound)
        { return SoundEffects[(int)sound]; }

        public static Effect LoadShader(string name)
        { 
            return Content.Load<Effect>("Shaders\\" + name);
        }
    }
    
}
namespace VikingEngine
{
    public enum LoadedMesh
    {
        NO_MESH = 0,
        cube_repeating,
        plane,
        cylinder_closed,
        sphere,
        SelectSquareDotted,
        SelectSquareSolid,
        SelectCircleDotted,
        SelectCircleSolid,
        SelectCircleThick,
        //rts_armybanner,
        //rts_spearman,
        //rts_seawarrior,
        //rts_ballista,
        //rts_cavalry,

        //rts_spearman_ship,
        //rts_seawarrior_ship,
        //rts_ballista_ship,
        //rts_cavalry_ship,

        //rts_armyicon,
        //rts_shipicon,
        //rts_citybanner,
        //rts_cityicon1,
        //rts_cityicon2,
        //rts_cityicon3,
        move_arrow,
        NUM
    };
    public enum LoadedTexture
    {
        NO_TEXTURE = 0,
        LF_TargetSheet,
        BlockTextures,
        SpriteSheet,
        TargetColor0,
        ptrace,
        WhiteArea,
        square_particle,
        ccg_piece_particle,
        realistic_particle,


        BirdJoustBG,
        cmdTiles,
        ccgTiles,
        NUM
    };

    public enum FontLanguage
    { 
        NONE,
        Western,
        Chinese,
        Japanese,
    }
}