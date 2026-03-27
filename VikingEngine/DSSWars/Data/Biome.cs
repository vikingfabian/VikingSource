using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Players;
using VikingEngine.DSSWars.Presentation;
using VikingEngine.HUD.RichBox;

namespace VikingEngine.DSSWars.Data
{
    static class Biome
    {
        public static void biomeToHud(CityBiome biome, LocalPlayer player, RichBoxContent content, bool interactive)
        {
            content.newLine();
            content.Add(new RbImage(SpriteName.WarsMapFilterTerrain));
            content.hspace();
            content.Add(new RbText(TextLib.LabelColon(DssRef.todoLang.CityBiome_Title), HudLib.TitleColor_Label));
            content.space();
            content.Add(new RbText(LangLib.Biome(biome)));
            content.space();
            HudLib.InfoButton(content, new RbTooltip(biomeTooltip, biome));
        }

        static void biomeTooltip(RichBoxContent content, object tag)
        {
            CityBiome biome = (CityBiome)tag;
            content.text(DssRef.todoLang.CityBiome_Description);

            switch (biome)
            {
                case CityBiome.Frozen:
                    content.newParagraph();
                    content.h2(DssRef.todoLang.CityBiome_Frozen, HudLib.TitleColor_Head2);

                    content.newLine();
                    HudLib.BulletPoint(content);
                    content.Add(new RbImage(SpriteName.WarsBuild_FoodStorage));
                    content.hspace();
                    content.Add(new RbText(DssRef.todoLang.Bonus_FoodStorage));

                    content.text(string.Format(DssRef.lang.Hud_ChangeFactor, "200%"));

                    content.newLine();
                    HudLib.BulletPoint(content);
                    content.Add(new RbImage(SpriteName.WarsResource_SkinAndLinen));
                    content.hspace();
                    content.Add(new RbText(DssRef.todoLang.Bonus_IncreaseSkin));

                    content.text(string.Format(DssRef.lang.Hud_ChangeFactor, "+50%"));

                    break;

                case CityBiome.Mountain:
                    content.newParagraph();
                    content.h2(DssRef.todoLang.CityBiome_Mountain, HudLib.TitleColor_Head2);

                    content.newLine();
                    HudLib.BulletPoint(content);
                    content.Add(new RbImage(SpriteName.WarsResource_WildPig));
                    content.hspace();
                    content.Add(new RbText(string.Format(DssRef.todoLang.CityCulture_Production, DssRef.todoLang.Resource_TypeName_WildHog)));
                    break;
            }
        }
        
    }

    enum CityBiome
    {
        Default_Fields,
        Frozen,// (Extra food storage, extra skin production) -storage done
        Forest,
        Mountain, //(Hog breeding)
        Desolate,
        Desert, //(Elephant breeding, drying meat, drying salt)
        NUM_NONE,
    }
}
