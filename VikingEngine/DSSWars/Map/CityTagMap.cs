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
        SpottedArrayCounter<Army> armiesC;
        public CityTagMap(LocalPlayer player)
        {
            this.player = player;
            citiesC = player.faction.cities.counter();
            armiesC = player.faction.armies.counter();
            cityTags = new List<CityTag>(8);
        }

        public void update()
        {
            int tagIndex = 0;

            if (player.viewCityTagsOnMap)
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

            if (player.viewArmyTagsOnMap)
            {
                armiesC.Reset();
                while (armiesC.Next())
                {
                    if (armiesC.sel.tagBack != Data.CityTagBack.NONE)
                    {

                        if (cityTags.Count <= tagIndex)
                        {
                            cityTags.Add(new CityTag());
                        }

                        cityTags[tagIndex].update(player, armiesC.sel);
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
            bg.Opacity = 0.7f;
            icon = new Graphics.ImageAdvanced(SpriteName.NO_IMAGE, Vector2.Zero, Engine.Screen.IconSizeV2 * 0.65f, HudLib.DiplomacyDisplayLayer, true);
            //offset = (bg.size - icon.size) * 0.5f;
        }

        public void update(LocalPlayer player, AbsMapObject mapObj)
        {
            Vector3 wp = mapObj.position;
            if (mapObj.gameobjectType() == GameObjectType.Army)
            {

            }
            else
            {
                wp.X += 0.2f;
                wp.Z += 0.2f;
            }

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
                mapObj.tagSprites(out SpriteName back, out SpriteName art);
                    

                bg.Visible = true;
                bg.SetSpriteName(back);
                if (art != SpriteName.NO_IMAGE)
                {
                    icon.position = bg.position;
                    icon.Visible = true;
                    icon.SetSpriteName(art);
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
