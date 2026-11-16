using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.Core.DSSWars.Map.MapData;
using VikingEngine.DSSWars.Build;
using VikingEngine.DSSWars.Data;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.GameObject.ObjectPointer;
using VikingEngine.DSSWars.Interface;
using VikingEngine.DSSWars.Map;
using VikingEngine.DSSWars.Map.MapLib;
using VikingEngine.DSSWars.Resource;
using VikingEngine.DSSWars.Work;
using VikingEngine.Graphics;
using VikingEngine.HUD.RichBox;
using VikingEngine.LootFest;
using VikingEngine.ToGG.MoonFall;

namespace VikingEngine.DSSWars.Players.Orders
{
    abstract class AbsBuildOrder : AbsOrder
    {
        public PMapObject pcity;
        protected IntVector2 subTile;
        protected VoxelModelInstance model;

        protected void createModel(int frame, int playerIx)
        {
            
            model = DssRef.models.ModelInstance_drawbatch(LootFest.VoxelModelName.buildarea, Map.MapData.MapTile1_1.ModelScale * 1.4f);
            model.Frame = frame;
            model.setVisibleCamera(playerIx);
        }

        override public void refreshYpos()
        { 
            model.position = WP.SubtileToWorldPosXZgroundY_Centered(subTile);
        }

        public override bool IsBuildOnSubTile(IntVector2 subTile)
        {
            return this.subTile == subTile;
        }

        public override bool IsConflictingOrder(AbsOrder other)
        {
            return other.IsBuildOnSubTile(subTile);
        }

        public override void DeleteMe()
        {
            model.preRemoveFromDrawBatch();
            base.DeleteMe();
        }

        public override void cullingUpdate(bool bStateA, int playerIx)
        {
            if (model != null)
            {
                model.Visible = DssRef.state.culling.InRender_Asynch(playerIx, bStateA, ref subTile);
            }
        }
    }

    class BuildOrder : AbsBuildOrder
    {
        public GroundType? terraform;
        public BuildAndExpandType buildingType;
        Mesh icon;
        public bool upgrade;

        public BuildOrder()
        { }
        public BuildOrder(int priority, bool bLocalPlayer, City city, IntVector2 subTile, GroundType? terraForm, BuildAndExpandType buildingType, bool upgrade)
        {
            this.upgrade = upgrade;
            baseInit(priority);
            this.pcity = city.mapObjPointer();
            this.subTile = subTile;
            this.terraform = terraForm;
            this.buildingType = buildingType;
            this.upgrade = upgrade;
        }

        public override void onAdd(int playerIx)
        {
            createModel(0, playerIx);

            SpriteName sprite;
            if (buildingType == BuildAndExpandType.TERRAFORM)
            {
                IconName.GroundType(terraform.Value, out sprite, out var name);
            }
            else
            {
                sprite = Build.BuildLib.BuildOptions[(int)buildingType].icon;
            }

            icon = new Mesh(LoadedMesh.plane, Vector3.Zero, model.scale * 9.6f, TextureEffectType.Flat, sprite, Color.White, false);
#if DEBUG
            icon.DebugName = "BuildOrder icon";
#endif
            icon.Opacity = 0.8f;
            icon.Rotation = DssLib.FaceForwardRotation;
            icon.AddToRender(DrawGame.NearLayer);
            refreshYpos();
        }

        public override void refreshYpos()
        {
            base.refreshYpos();
            Vector3 iconPos = model.position;
            iconPos.Y += model.scale.Y * 6f;
            iconPos.Z += model.scale.Y * 0.3f;
            icon.position = iconPos;
        }

        public override RichBoxContent ToHud()
        {           
            RichBoxContent content = new RichBoxContent();
            if (buildingType != BuildAndExpandType.TERRAFORM)
            {
                content.h2(SpriteName.WarsConstructBuildingIcon, upgrade ? DssRef.lang.Upgrade_Order : DssRef.lang.Build_Order, HudLib.TitleColor_Head);
                BuildLib.BuildOptions[(int)buildingType].blueprint.toMenu(content, pcity.GetCity(), upgrade);

                content.newLine();
            }
            HudLib.Label(content, DssRef.lang.Work_OrderPrioTitle);
            content.space();
            content.Add(new RbText(priority.ToString()));

            if (terraform.HasValue)
            {
                Tooltip.TerraformToHud(content, pcity.GetCity(), DssRef.world.subTileGrid.Get(subTile).groundType, terraform.Value);
            }
            BuildControls.buildTooltip_YouOwn(pcity.GetCity(), content, terraform, buildingType);
            

            return content;
        }

        override public void writeGameState(System.IO.BinaryWriter w)
        {
            base.writeGameState(w);
            
            pcity.writeCity(w);
            WP.writeSubTilePos(w, subTile);
            w.Write((byte)buildingType);
        }
        override public bool readGameState(int playerIx, System.IO.BinaryReader r, int subversion, ObjectPointerCollection pointers)
        {
            base.readGameState(playerIx, r, subversion, pointers);

            pcity.readCity(r);
            
            subTile = WP.readSubTilePos(r);
            
            buildingType = (BuildAndExpandType)r.ReadByte();

            if (pcity.TryGetCity(out _))
            {
                onAdd(playerIx);
                return true;
            }
            return false;
            
        }

        override public void DeleteMe()
        { 
            base.DeleteMe();
            
            icon.DeleteMe();
        }
        public override BuildOrder GetBuild()
        {
            return this;
        }
        
        public override bool BuildQueue(City city)
        {
            if (this.pcity.objectIndex == city.myIndex && orderStatus != OrderStatus.Complete)
            {
                return true;
            }
            return false;
        }

        public WorkQueMember createWorkQue(/*out CraftBlueprint blueprint*/)
        {
            WorkQueMember result;
            if (buildingType == BuildAndExpandType.TERRAFORM)
            {
                //blueprint = GroundPropertiesLib.Get(terraform.Value).TerraformToCost;
                result = new WorkQueMember( WorkType.Terraform, (int)terraform.Value, 0, WorkPriorityType.buildOrders, subTile,
                    priority, 0, 0)
                { orderId = id };
            }
            else
            {
                int type = (int)buildingType;
                //blueprint = BuildLib.BuildOptions[type].blueprint;
                result = new WorkQueMember(upgrade ? WorkType.Upgrade : WorkType.Build, type, 0, WorkPriorityType.buildOrders, subTile,
                    priority, 0, 0)
                { orderId = id };
            }
            return result;
        }

        public override bool refreshAvailable(PFaction faction)
        {
            return pcity.pfaction == faction;
        }

        override public OrderType GetWorkType(City city)
        {
            if (this.pcity.objectIndex == city.myIndex)
            {
                return OrderType.Build;
            }
            return OrderType.NONE;
        }

        public override OrderType Type()
        {
            return OrderType.Build;
        }
    }

    enum OrderStatus
    { 
        Waiting,
        Started,
        Complete,
    }
}
