using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.Players;
using VikingEngine.Graphics;
using VikingEngine.LootFest.Players;
using VikingEngine.ToGG.HeroQuest.Data.Condition;

namespace VikingEngine.DSSWars.Build
{
    class BuildSelectGuiCollection : List<BuildSelectGui>
    {
        public int useCount = 0;

        public BuildSelectGuiCollection()
            :base(64)
        { }

        public void Create(LocalPlayer player, IntVector2 subTilePos, bool canAct, int usesBuildQue, City city)
        {
            BuildSelectGui buildSelectGui;

            if (useCount < Count)
            {
                buildSelectGui = this[useCount++];
            }
            else
            {
                Mesh model = SelectedSubTile.CreateOutlineModel(player, false);
                model.Visible = true;

                buildSelectGui = new BuildSelectGui()
                {
                    model = model
                };
                Add(buildSelectGui);
            }

            buildSelectGui.position = subTilePos;
            buildSelectGui.mayBuild = canAct;
            if (canAct)
            {
                buildSelectGui.model.Color = Color.White;
            }
            else
            {
                buildSelectGui.model.Color = HudLib.NotAvailableColor;
            }
            buildSelectGui.usesBuildQue = usesBuildQue;
            buildSelectGui.City = city;
            buildSelectGui.model.Visible = true;
            buildSelectGui.model.position = WP.SubtileToWorldPosXZgroundY_Centered(subTilePos);

        }

        public void deleteSelection()
        {
            for (int i = 0; i < useCount;++i)//each (var sel in this)
            {
                this[i].Hide();
            }

            if (Count > useCount && Count > 65)
            {
                for (int i = 0; i < 4; ++i)
                {
                    int ix = Count - 1;
                    this[ix].model.DeleteMe();
                    RemoveAt(ix);
                }
            }

            useCount = 0;
        }
    }

    class BuildSelectGui
    {
        public IntVector2 position;
        public bool mayBuild;
        public Mesh model;
        public int usesBuildQue;
        public City City;

        public void Hide()
        {
            City = null;
            model.Visible = false;
           // position = IntVector2.NegativeOne;
        }

        
    }
}
