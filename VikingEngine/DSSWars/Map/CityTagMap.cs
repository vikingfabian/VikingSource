using Microsoft.Xna.Framework;
using System.Collections.Generic;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.Players;
using VikingEngine.Input;
using VikingEngine.LootFest.Players;

namespace VikingEngine.DSSWars.Map
{
    class CityTagMap
    {
        List<CityTag> cityTags;
        LocalPlayer player;
        SpottedArrayCounter<City> citiesC;
        public CityTagMap(LocalPlayer player)
        {
            this.player = player;
            citiesC = player.faction.cities.counter();
            cityTags = new List<CityTag>(8);
        }

        public void update()
        {
            int tagIndex = 0;

            if (DssRef.storage.viewTagsOnMap)
            {
                citiesC.Reset();
                while (citiesC.Next())
                {
                    if (citiesC.sel.tagBack != Data.CityTagBack.NONE)
                    {

                        if (cityTags.Count <= tagIndex)
                        {
                            cityTags.Add(new CityTag());
                        }

                        cityTags[tagIndex].update(player, citiesC.sel);
                        tagIndex++;
                    }
                }
            }
            while (cityTags.Count > tagIndex)
            {
                arraylib.PullLastMember(cityTags).DeleteMe();
            }
        }

        public void DeleteMe()
        {
            foreach (CityTag cityTag in cityTags)
            {
                cityTag.DeleteMe();
            }
        }
    }

    class CityTag
    {
        public Graphics.Image icon = null;
        public Graphics.Image bg = null;
        //Vector2 offset;

        public CityTag()
        {
            bg = new Graphics.ImageAdvanced(SpriteName.NO_IMAGE, Vector2.Zero, Engine.Screen.IconSizeV2 * 0.8f, HudLib.DiplomacyDisplayLayer +1, true);
            icon = new Graphics.ImageAdvanced(SpriteName.NO_IMAGE, Vector2.Zero, Engine.Screen.IconSizeV2 * 0.65f, HudLib.DiplomacyDisplayLayer, true);
            //offset = (bg.size - icon.size) * 0.5f;
        }

        public void update(LocalPlayer player, City city)
        {
            Vector3 wp = city.position;
            wp.X += 0.2f;
            wp.Z += 0.2f;

            bg.position = player.playerData.view.From3DToScreenPos(wp) + bg.HalfSize;


            bool viewBounds = DssRef.state.localPlayers.Count > 1;
            bool visible = true;
            if (viewBounds)
            {
                visible = player.playerData.view.DrawAreaF.IntersectPoint(bg.position);
            }

            if ((Mouse.Position - bg.position).Length() < Engine.Screen.IconSize)
            { 
                visible = false;
            }

            if (visible)
            {
                bg.Visible = true;
                bg.SetSpriteName(Data.CityTag.BackSprite(city.tagBack));
                if (city.tagArt != Data.CityTagArt.None)
                {
                    icon.position = bg.position;
                    icon.Visible = true;
                    icon.SetSpriteName(Data.CityTag.ArtSprite(city.tagArt));
                }
                else
                {
                    icon.Visible = false;
                }
            }
            else
            {
                bg.Visible = false;
                icon.Visible = false;
            }
        }

        public void DeleteMe()
        { 
            bg.DeleteMe();
            icon.DeleteMe();
        }
    }
}
