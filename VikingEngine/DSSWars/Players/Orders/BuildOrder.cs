using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Build;
using VikingEngine.DSSWars.Data;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.Map;
using VikingEngine.DSSWars.Resource;
using VikingEngine.DSSWars.Work;
using VikingEngine.Graphics;
using VikingEngine.HUD.RichBox;
using VikingEngine.ToGG.MoonFall;

namespace VikingEngine.DSSWars.Players.Orders
{
    abstract class AbsBuildOrder : AbsOrder
    {
        protected City city;
        protected IntVector2 subTile;
        protected VoxelModelInstance model;

        protected void createModel(int frame, int playerIx)
        {
            Debug.CrashIfThreaded();
            model = DssRef.models.ModelInstance_drawbatch(LootFest.VoxelModelName.buildarea, WorldData.SubTileWidth * 1.4f);
            model.Frame = frame;
            model.position = WP.SubtileToWorldPosXZgroundY_Centered(subTile);
            model.setVisibleCamera(playerIx);
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
            //DssRef.models.recycle(ref model, true);
            model.preRemoveFromDrawBatch();
            base.DeleteMe();
        }

        public override void cullingUpdate(bool bStateA, int playerIx)
        {
            IntVector2 tilepos = WP.SubtileToTilePos(subTile);
            model.Visible = DssRef.state.culling.InRender_Asynch(playerIx, bStateA, ref tilepos);
        }
    }

    class BuildOrder : AbsBuildOrder
    {
        
        public BuildAndExpandType buildingType;
        Mesh icon;
        public bool upgrade;

        public BuildOrder()
        { }
        public BuildOrder(int priority, bool bLocalPlayer, City city, IntVector2 subTile, BuildAndExpandType buildingType, bool upgrade)
        {
            this.upgrade = upgrade;
            baseInit(priority);
            this.city = city;
            this.subTile = subTile;
            this.buildingType = buildingType;
            this.upgrade = upgrade;
        }



        public override void onAdd(int playerIx)
        {

            createModel(0, playerIx);

            Vector3 iconPos = model.position;
            iconPos.Y += model.scale.Y * 6f;
            iconPos.Z += model.scale.Y * 0.15f;

            icon = new Mesh(LoadedMesh.plane, iconPos, model.scale * 9.6f, TextureEffectType.Flat, Build.BuildLib.BuildOptions[(int)buildingType].sprite, Color.White, false);
#if DEBUG
            icon.DebugName = "BuildOrder icon";
#endif
            icon.Opacity = 0.8f;
            icon.Rotation = DssLib.FaceForwardRotation;
            icon.AddToRender(DrawGame.UnitDetailLayer);
        }

        public override RichBoxContent ToHud()
        {
            RichBoxContent content = new RichBoxContent();
            content.h2(upgrade? DssRef.lang.Upgrade_Order : DssRef.lang.Build_Order);
            BuildLib.BuildOptions[(int)buildingType].blueprint.toMenu(content, city, upgrade);

            return content;
        }

        override public void writeGameState(System.IO.BinaryWriter w)
        {
            base.writeGameState(w);

            w.Write((ushort)city.myIndex);
            subTile.write(w);
            w.Write((byte)buildingType);
        }
        override public void readGameState(int playerIx, System.IO.BinaryReader r, int subversion, ObjectPointerCollection pointers)
        {
            base.readGameState(playerIx, r, subversion, pointers);

            city = DssRef.world.cities[r.ReadUInt16()];
            subTile.read(r);
            buildingType = (BuildAndExpandType)r.ReadByte();

            onAdd(playerIx);
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
            if (this.city == city && orderStatus != OrderStatus.Complete)
            {
                return true;
            }
            return false;
        }

        public WorkQueMember createWorkQue(out CraftBlueprint blueprint)
        {
            int type = (int)buildingType;
            blueprint = BuildLib.BuildOptions[type].blueprint;
            var result = new WorkQueMember(upgrade? WorkType.Upgrade : WorkType.Build, type, 0, subTile, priority, 0, 0);
            result.orderId = id;
            return result;
        }

      

        public override bool refreshAvailable(Faction faction)
        {
            return city.factionIndex == faction.myIndex;
        }

        override public OrderType GetWorkType(City city)
        {
            if (this.city == city)
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
