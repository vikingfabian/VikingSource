using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Drawing;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.Players;
using VikingEngine.DSSWars.Players.Profile;
using VikingEngine.Graphics;
using VikingEngine.Input;
using VikingEngine.LootFest.Players;

namespace VikingEngine.DSSWars.Interface
{
    class CityTagMap
    {
        List<CityTagMapMember> cityTags;
        LocalPlayer player;
        SpottedArrayCounter<Army> armiesC;
        public CityTagMap(LocalPlayer player)
        {
            this.player = player;
            armiesC = player.faction.armies.counter();
            cityTags = new List<CityTagMapMember>(8);
        }

        public void update()
        {
            int tagIndex = 0;

            if (player.cityHudSettings.ViewAnyOnMap())
            {
                SpottedPointerArrayCounter citiesC = new SpottedPointerArrayCounter();
                while (citiesC.Next(ref player.faction.cities, DssRef.world.cities, out City citySel))
                {
                    if (citySel.Tag.backType != Data.CityTagBack.NONE &&
                        citySel.inRender_overviewLayer)
                    {

                        if (cityTags.Count <= tagIndex)
                        {
                            cityTags.Add(new CityTagMapMember());
                        }

                        cityTags[tagIndex].update(player, citySel, player.cityHudSettings);
                        tagIndex++;
                    }
                }
            }

            if (player.armyHudSettings.ViewAnyOnMap())
            {
                armiesC.Reset();
                while (armiesC.Next())
                {
                    if (armiesC.sel.Tag.backType != Data.CityTagBack.NONE &&
                        armiesC.sel.inRender_overviewLayer)
                    {

                        if (cityTags.Count <= tagIndex)
                        {
                            cityTags.Add(new CityTagMapMember());
                        }

                        cityTags[tagIndex].update(player, armiesC.sel, player.armyHudSettings);
                        tagIndex++;
                    }
                }
            }

            if (player.pinHudSettings.viewTagsOnMap)
            {
                foreach (var p in DssRef.state.localPlayers)
                {
                    playerPins(p);
                }

                var remoteC = DssRef.state.remotePlayers.counter();
                while (remoteC.Next())
                {
                    playerPins(remoteC.sel);
                }

                void playerPins(AbsHumanPlayer p)
                {
                    var pinsC = p.pins.counter();
                    while (pinsC.Next())
                    {
                        if (pinsC.sel.Tag.backType != Data.CityTagBack.NONE &&
                            pinsC.sel.inRender_overviewLayer)
                        {
                            if (cityTags.Count <= tagIndex)
                            {
                                cityTags.Add(new CityTagMapMember());
                            }

                            cityTags[tagIndex].update(player, pinsC.sel, player.pinHudSettings);
                            tagIndex++;
                        }
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
            foreach (CityTagMapMember cityTag in cityTags)
            {
                cityTag.DeleteMe();
            }
        }
    }

    class CityTagMapMember
    {
        const float BgScale = 0.8f;
        static readonly float BgHalfScale = BgScale * 0.5f;

        //Vector2 bgSize;

        public List<Image> icons = new List<Image>(4);
        //public Graphics.Image icon = null;
        public Image bg = null;

        bool isVisible = true;
        
        //Vector2 offset;

        public CityTagMapMember()
        {

        }

        public void update(LocalPlayer player, AbsMapObject mapObj, ObjectHudSettings hudSettings)
        {
            Vector2 position;
            bool visible = true;

            Vector3 wp = mapObj.position;
            
            wp.X += 0.02f;
            wp.Z += -0.2f;

            position = player.playerData.view.From3DToScreenPos(wp) + Engine.Screen.IconSizeV2  * BgHalfScale;

            bool viewBounds = DssRef.state.localPlayers.Count > 1;
            
            if (viewBounds)
            {
                visible = player.playerData.view.DrawAreaF.IntersectPoint(position);
            }

            if ((player.gameControls.map.pointerPos() - position).Length() < Engine.Screen.IconSize)
            { 
                visible = false;
            }
            
            if (visible)
            {
                int iconIndex = 0;

                if (hudSettings.viewTagsOnMap)
                {
                    mapObj.tagSprites(out SpriteName back, out SpriteName art);

                    if (bg == null)
                    {
                        bg = new Image(SpriteName.NO_IMAGE, Vector2.Zero, Engine.Screen.IconSizeV2 * BgScale, HudLib.DiplomacyDisplayLayer + 1, true);
                        bg.Opacity = 0.7f;
                    }

                    bg.position = nextPosition();
                    bg.Visible = true;
                    bg.SetSpriteName(back);

                    if (art != SpriteName.NO_IMAGE)
                    {
                        var icon = nextIcon();
                        icon.position = bg.position;
                        icon.Visible = true;
                        icon.SetSpriteName(art);
                    }
                }

                if (hudSettings.viewLowFoodOnMap && mapObj.lowFood())
                {
                    var icon = nextIcon();
                    icon.position = nextPosition();                    
                    icon.SetSpriteName( SpriteName.WarsResource_FoodEmpty);
                }

                if (hudSettings.viewIdleWorkOnMap && mapObj.GetCity().WorkerStats_IdleCount >= 10)
                {
                    var icon = nextIcon();
                    icon.position = nextPosition();
                    icon.SetSpriteName(SpriteName.WarsIcon_WorkQueueIdle);
                }

                if (hudSettings.viewStuckBuildOrdersOnMap && mapObj.GetCity().WorkerStats_StuckBuildings >= 2)
                {
                    var icon = nextIcon();
                    icon.position = nextPosition();
                    icon.SetSpriteName(SpriteName.WarsConstructBuildingIcon);
                }

                for (int i = iconIndex; i < icons.Count; ++i)
                {
                    icons[i].Visible = false;
                }

                Vector2 nextPosition()
                {
                    var result = position;
                    position.X += Engine.Screen.IconSize * 0.8f;
                    return result;
                }

                Image nextIcon()
                {
                    int index = iconIndex++;

                    if (index >= icons.Count)
                    {
                        var icon = new Image(SpriteName.NO_IMAGE, Vector2.Zero, Engine.Screen.IconSizeV2 * 0.65f, HudLib.DiplomacyDisplayLayer, true);
                        icons.Add(icon);
                        return icon;
                    }
                    icons[index].Visible = true;
                    return icons[index];
                }

            }
            else
            {
                if (isVisible != visible)
                {
                    if (bg != null)
                    {
                        bg.Visible = false;
                    }
                    foreach (var m in icons)
                    {
                        m.Visible = false;
                    }
                }
            }

            isVisible = visible;
           
        }



        public void DeleteMe()
        { 
            bg?.DeleteMe();
            foreach (var m in icons)
            {
                m.DeleteMe();
            }
        }
    }
}
