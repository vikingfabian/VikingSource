using Microsoft.Xna.Framework;
using System.Collections.Generic;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.Players;
using VikingEngine.Graphics;
using VikingEngine.LootFest.Players;

namespace VikingEngine.DSSWars.Communication
{
    class DiplomacyMapSelection
    {
        Graphics.Image seletionbox;
        bool selection;

        List<Mesh> citySelections = new List<Mesh>(16);

        public DiplomacyMapSelection(bool selection)
        {
            this.selection = selection;
            seletionbox = new Graphics.Image(SpriteName.WarsRelationFlagOutline, Vector2.Zero, Vector2.One, HudLib.DiplomacyDisplayLayer + 3);
            seletionbox.Visible = false;
            if (!selection)
            {
                seletionbox.Color = ColorExt.FromAlpha(0.9f);
            }
        }

        public void updateSelectBox(LocalPlayer player, RelationFlag relation)
        {
            if (relation != null)
            {
                if (relation.bg != null)
                {
                    var hoverArea = relation.bg.RealArea();

                    hoverArea.AddRadius(4);
                    seletionbox.Area = hoverArea;
                    seletionbox.Visible = relation.bg != null && relation.bg.Visible;

                    int cityCount = 0;
                    var faction = DssRef.world.faction(relation.faction);
                    if (faction != null)
                    {
                        
                        SpottedPointerArrayCounter citiesC = new SpottedPointerArrayCounter();
                        while (citiesC.Next(ref faction.cities, DssRef.world.cities, out City city))
                        {
                            if (cityCount >= citySelections.Count) 
                            {
                                Mesh cirkleMesh = new Mesh(LoadedMesh.plane, 
                                    Vector3.Zero, new Vector3(2.4f), TextureEffectType.Flat,
                                    selection? SpriteName.WarsOverviewCitySelect : SpriteName.WarsOverviewCityHover, 
                                    Color.White, false);
                                cirkleMesh.AddToRender(DrawGame.FarLayer);
                                citySelections.Add(cirkleMesh);
                            }
                            citySelections[cityCount].Visible = true;
                            citySelections[cityCount].Position = new Vector3(city.position.X - 0.0f, 0.02f, city.position.Z - 0.0f);

                            cityCount++;
                        }
                    }
                }
            }
            else
            {
                Hide();
            }
        }

        public void Hide()
        {
            seletionbox.Visible = false;
            foreach (Mesh image in citySelections)
            {
                image.Visible = false;
            }
        }

        public void DeleteMe()
        {
            seletionbox.DeleteMe();
            foreach (Mesh image in citySelections)
            {
                image.DeleteMe();
            }
        }
    }
}
