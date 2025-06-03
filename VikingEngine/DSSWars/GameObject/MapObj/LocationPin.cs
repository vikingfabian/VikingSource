using Microsoft.Xna.Framework;
using System;
using VikingEngine.DSSWars.Data;
using VikingEngine.DSSWars.Display;
using VikingEngine.DSSWars.Players;
using VikingEngine.HUD.RichBox;
using VikingEngine.HUD.RichBox.Artistic;

namespace VikingEngine.DSSWars.GameObject
{
    class LocationPin: AbsMapObject
    {
        ObjectName name = new ObjectName();
        Graphics.AbsVoxelObj overviewModel;
        BoundingSphere bound;

        public LocationPin(LocalPlayer player, Vector3 position)
        { 
            this.position = position;
            tilePos = WP.ToTilePos(position);
            faction= player.faction;
            createOverViewModel();
            inRender_overviewLayer = true;          
        }

        public LocationPin(LocalPlayer player, System.IO.BinaryReader r, int subVersion)
        {
            faction = player.faction;
            readGameState(r, subVersion);
        }

        public void basicInit()
        {
            bound = new BoundingSphere(position, 0.3f);
            name.setDefault("Pin " + parentArrayIndex.ToString());
        }

        public void update()
        {
            updateDetailLevel();
        }

        public override void toHud(ObjectHudArgs args)
        {
            base.toHud(args);
            
            args.content.newLine();
            args.content.Add(new ArtButton(RbButtonStyle.Primary, new System.Collections.Generic.List<AbsRichBoxMember>{
               new RbText(  DssRef.lang.Hud_Delete) }, new RbAction1Arg<int>(args.player.deletePin, parentArrayIndex)));
               
        }

        public void writeGameState(System.IO.BinaryWriter w)
        {
            name.write(w);
            WP.WritePosXZPercentU16(w, position);
        }


        void readGameState(System.IO.BinaryReader r, int subVersion)
        {
            name.read(r, subVersion);
            //if (!name.custom)
            //{
            //    name.name = Data.NameGenerator.ArmyName(id);
            //}

            if (subVersion < 62)
            {
                WP.readPosXZ_old(r, out position, out tilePos);
            }
            else
            {
                WP.ReadPosXZPercentU16(r, out position, out tilePos);
            }
        }

        void createOverViewModel()
        {
            overviewModel?.DeleteMe();

            overviewModel = faction.AutoLoadModelInstance(
               LootFest.VoxelModelName.wars_flag, 1f, false);
            overviewModel.AddToRender(DrawGame.TerrainLayer);
            overviewModel.position = position;
        }
        public override void asynchCullingUpdate(float time, bool bStateA)
        {
            //if (inRender_detailLayer)
            //{
            //    lib.DoNothing();
            //}
            DssRef.state.culling.InRender_Asynch(ref enterRender_overviewLayer_async, ref enterRender_detailLayer_async, bStateA, tilePos, faction.player.GetLocalPlayer().playerData.localPlayerIndex);
        }

        protected override void setInRenderState()
        {
            if (inRender_overviewLayer)
            {
                if (overviewModel == null)
                {
                    createOverViewModel();
                }
            }
            else
            {
                if (overviewModel != null)
                {
                    overviewModel.DeleteMe();
                    overviewModel = null;
                }
            }
        }

        public override void DeleteMe(DeleteReason reason, bool removeFromParent)
        {
            base.DeleteMe(reason, removeFromParent);
            overviewModel?.DeleteMe();
        }

        override public bool rayCollision(Ray ray)
        {
            if (inRender_overviewLayer)
            {
                float? distance = ray.Intersects(bound);
                return distance.HasValue;
            }

            return false;
        }

        public override bool aliveAndBelongTo(Faction faction)
        {
            return base.aliveAndBelongTo(faction);
        }

        public override bool defeatedBy(Faction attacker)
        {
            throw new NotImplementedException();
        }

        public override bool aliveAndBelongTo(int faction)
        {
            throw new NotImplementedException();
        }
        public override void OnNewOwner()
        {
            throw new NotImplementedException();
        }

        public override GameObjectType gameobjectType()
        {
           return GameObjectType.LocationPin;
        }

        protected override void NameEditEvent(string result, object tag)
        {
            name.setCustom(result);
        }

        public override string TypeName()
        {
            return ".Location Pin";
        }

        public override string Name(out bool mayEdit)
        {
            mayEdit = true;
            return name.name;
        }
    }
}
