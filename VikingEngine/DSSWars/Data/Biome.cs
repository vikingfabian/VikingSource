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
            content.Add(new RbText(TextLib.LabelColon(DssRef.todoLang.CityBiome_Title), HudLib.TitleColor_Label));
            content.space();
            content.Add(new RbText(LangLib.Biome(biome)));
            content.space();
            HudLib.InfoButton(content, new RbTooltip_Text(DssRef.todoLang.CityBiome_Description));
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
    }
}
