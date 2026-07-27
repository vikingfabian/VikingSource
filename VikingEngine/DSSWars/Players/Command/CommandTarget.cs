using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.LootFest.Players;

namespace VikingEngine.DSSWars.Players.Command
{
    /// <summary>
    /// Start a marker that can place a command like "build here" for a soldier unit
    /// </summary>
    abstract class AbsCommandTarget
    {
        public bool available = true;

        public AbsCommandTarget(LocalPlayer player)
        {
            player.gameControls.commandTarget?.DeleteMe();

            player.gameControls.commandTarget = this;
        }

        /// <returns>complete</returns>
        virtual public bool update(LocalPlayer player) 
        {
            if (player.gameControls.input.cancelDownEvent())
            {
                return true;
            }
            else if (player.gameControls.input.mouseSelect.DownEvent)
            {
                if (available)
                {
                    OnClick(player, out bool complete);
                    SoundLib.ordermove.Play();
                    return complete;
                }
                else
                {
                    SoundLib.wrong.Play();
                }
               
            }
            
            return false;
        }

        abstract protected void OnClick(LocalPlayer player, out bool complete);

        virtual public void DeleteMe()
        { }
    }

    class SettlerCommandTarget : AbsCommandTarget
    {
        SoldierGroup soldierGroup;
        Graphics.Mesh model;
        public SettlerCommandTarget(LocalPlayer player, SoldierGroup soldierGroup) 
            :base(player)
        { 
            this.soldierGroup = soldierGroup;
            player.gameControls.mapSelect(soldierGroup);
            player.gameControls.map.SetTargetZoom(Map.MapDetailLayerType.UnitDetail1);

            model = SelectedSubTile.CreateOutlineModel(player, true);
            model.Visible = true;            
        }

        public override bool update(LocalPlayer player)
        {
            model.position = WP.SubtileToWorldPosXZgroundY_Centered(player.gameControls.map.hover.subTile.subTilePos);
            available = false;

            if (DssRef.world.tileGrid.TryGet(WP.SubtileToTilePos(player.gameControls.map.hover.subTile.subTilePos), out var tile))
            {
                available = tile.City().cityType == CityType.UnClaimed;
            }

            model.Color = available ? Color.White : HudLib.NotAvailableColor;

            return base.update(player);
        }

        protected override void OnClick(LocalPlayer player, out bool complete)
        {
            if (!soldierGroup.isDeleted)
            {
                OrderSettler(soldierGroup, player.gameControls.map.hover.subTile.subTilePos);
            }
            complete = true;
        }

        public static void OrderSettler(SoldierGroup soldierGroup, IntVector2 subTilePos)
        {
            new MoveCommand(soldierGroup, WP.SubtileToWorldPosXZ(subTilePos), float.MinValue, false);
            new ClaimCityGommand(soldierGroup, subTilePos, true);
        }

        public override void DeleteMe()
        {
            base.DeleteMe();
            model.DeleteMe();
        }
    }
}
