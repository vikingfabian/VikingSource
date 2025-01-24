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

        protected void createModel(int frame)
        {
            Debug.CrashIfThreaded();
            model = DssRef.models.ModelInstance(LootFest.VoxelModelName.buildarea, true, WorldData.SubTileWidth * 1.4f, true, true, false);
            model.Frame = frame;
            //model.AddToRender(DrawGame.UnitDetailLayer);
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
            //model.DeleteMe();
            DssRef.models.recycle(ref model, true);
            base.DeleteMe();
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

            //if (bLocalPlayer)
            //{
            //    init();
            //}
        }

        public override void onAdd()
        {

            createModel(0);

             Vector3 iconPos = model.position;
            iconPos.Y += model.scale.Y * 6f;
            iconPos.Z += model.scale.Y * 0.15f;

            icon = new Mesh(LoadedMesh.plane, iconPos, model.scale * 9.6f, TextureEffectType.Flat, Build.BuildLib.BuildOptions[(int)buildingType].sprite, Color.White, false);
            icon.Opacity = 0.8f;
            icon.Rotation = DssLib.FaceForwardRotation;
            icon.AddToRender(DrawGame.UnitDetailLayer);
        }

        public override RichBoxContent ToHud()
        {
            RichBoxContent content = new RichBoxContent();
            content.h2(upgrade? DssRef.todoLang.Upgrade_Order : DssRef.lang.Build_Order);
            BuildLib.BuildOptions[(int)buildingType].blueprint.toMenu(content, city, upgrade);

            return content;
        }

        override public void writeGameState(System.IO.BinaryWriter w)
        {
            base.writeGameState(w);

            w.Write((ushort)city.parentArrayIndex);
            subTile.write(w);
            w.Write((byte)buildingType);
        }
        override public void readGameState(System.IO.BinaryReader r, int subversion, ObjectPointerCollection pointers)
        {
            base.readGameState(r, subversion, pointers);

            city = DssRef.world.cities[r.ReadUInt16()];
            subTile.read(r);
            buildingType = (BuildAndExpandType)r.ReadByte();

            onAdd();
            //init();
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
            return city.faction == faction;
        }

        override public OrderType GetWorkType(City city)
        {
            if (this.city == city)
            {
                return OrderType.Build;
            }
            return OrderType.NONE;
        }
    }

    enum OrderStatus
    { 
        Waiting,
        Started,
        Complete,
    }
}
