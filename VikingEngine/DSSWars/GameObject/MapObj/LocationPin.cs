using Microsoft.Xna.Framework;
using System;
using VikingEngine.DSSWars.Data;
using VikingEngine.DSSWars.Interface;
using VikingEngine.DSSWars.Players;
using VikingEngine.HUD.RichBox;
using VikingEngine.HUD.RichBox.Artistic;
using VikingEngine.Network;

namespace VikingEngine.DSSWars.GameObject
{
    class LocationPin: AbsMapObject
    {
        ObjectName name = new ObjectName();
        Graphics.AbsVoxelObj overviewModel;
        BoundingSphere bound;
        public PingMessage pingMessage = PingMessage.None;
        public NetInteractLevel netInteractLevel = NetInteractLevel.Hidden;
        

        public LocationPin(RemotePlayer player)
        {
            IsNetHosted = false;
            factionIndex = player.faction.myIndex;
        }

        public LocationPin(AbsHumanPlayer player, Vector3 position)
        { 
            this.position = position;
            
            factionIndex = player.faction.myIndex;
            createOverViewModel();
            inRender_overviewLayer = true;          
        }

        public LocationPin(AbsHumanPlayer player, System.IO.BinaryReader r, int subVersion)
        {
            factionIndex = player.faction.myIndex;
            readGameState(r, subVersion);
        }


        public void basicInit()
        {
            
            tilePos = WP.ToTilePos(position);

            
            name.setDefault("Pin " + myIndex.ToString());

            inRender_overviewLayer = false;
        }

        public void update()
        {
            updateDetailLevel();
        }

        public void Hide()
        {
            netInteractLevel = NetInteractLevel.Hidden;
            if (overviewModel != null)
            {
                overviewModel.DeleteMe();
                overviewModel = null;
            }
        }

        public override void toHud(ObjectHudArgs args)
        {
            base.toHud(args);

            args.content.newParagraph();
            HudLib.Label(args.content, SpriteName.NO_IMAGE, ".Message");
            args.content.newLine();
            for (PingMessage message = 0; message < PingMessage.NUM; message++)
            {
                args.content.Add(new ArtOption(message == pingMessage, new System.Collections.Generic.List<AbsRichBoxMember> { new RbText(message.ToString()) },
                    new RbAction1Arg<PingMessage>((PingMessage selected) =>
                    {
                        pingMessage = selected;
                    }, message)));
            }

            if (Ref.netSession.InMultiplayerSession)
            {
                args.content.newParagraph();
                HudLib.Label(args.content, SpriteName.NO_IMAGE, ".Share and ping");
                args.content.newLine();

                if (netInteractLevel == NetInteractLevel.Hidden)
                {
                    interactLevelButton("Team", NetInteractLevel.Team);
                    interactLevelButton("Everyone", NetInteractLevel.Public);
                }
                else
                {
                    interactLevelButton("Hide", NetInteractLevel.Hidden);
                }

                void interactLevelButton(string label, NetInteractLevel level)
                {
                    args.content.Add(new ArtButton(RbButtonStyle.Primary, new System.Collections.Generic.List<AbsRichBoxMember>
                    {  new RbText(label)}, new RbAction1Arg<NetInteractLevel>((NetInteractLevel selected) =>
                    {
                        netInteractLevel = selected;
                        if (netInteractLevel > NetInteractLevel.Hidden)
                        {
                            var w = Ref.netSession.BeginWritingPacket(PacketType.DssPing, PacketReliability.Reliable);
                            w.Write((ushort)myIndex);
                            writeGameState(w);
                        }
                        else
                        {
                            var w = Ref.netSession.BeginWritingPacket(PacketType.DssPinHide, PacketReliability.Reliable);
                            w.Write((ushort)myIndex);
                        }
                    }, level, level == NetInteractLevel.Hidden? RbSoundType.Back : RbSoundType.Ping)));
                }
                //for (NetInteractLevel level = 0; level < NetInteractLevel.NUM; level++)
                //{ 
                    
                //}
            }
            args.content.Add(new RbSeperationLine());
            args.content.newParagraph();
            args.content.Add(new ArtButton(RbButtonStyle.Primary, new System.Collections.Generic.List<AbsRichBoxMember>{
               new RbText(  DssRef.lang.Hud_Delete) }, new RbAction1Arg<int>(args.player.deletePin, myIndex)));

            args.content.newLine();
            args.content.Add(new ArtButton(RbButtonStyle.Primary, new System.Collections.Generic.List<AbsRichBoxMember>{
               new RbText(  ".Delete all") }, new RbAction1Arg<DeleteReason>(args.player.clearPins, DeleteReason.Disband)));

        }

        public override void toTooltip(ObjectHudArgs args)
        {
            base.toTooltip(args);
            var remote = GetFaction()?.player.GetRemotePlayer();
            if (remote != null)
            {
                args.content.newLine();
                remote.addNetGamerToHud(args.content, true, false);
            }

            if (pingMessage != PingMessage.None)
            {
                args.content.text(pingMessage.ToString(), HudLib.InfoYellow_Light);
            }
        }

        public void writeGameState(System.IO.BinaryWriter w)
        {
            name.write(w);
            WP.WritePosXZPercentU16(w, position);
            //v115
            new TwoHalfByte((byte)pingMessage, (byte)netInteractLevel).write(w);

        }


        public void readGameState(System.IO.BinaryReader r, int subVersion)
        {
            name.read(r, subVersion);
            
            WP.ReadPosXZPercentU16(r, out position, out tilePos);

            if (subVersion >= 115)
            {
                var hbytes = new TwoHalfByte();
                hbytes.read(r);
                pingMessage = (PingMessage) hbytes.Value1;
                netInteractLevel = (NetInteractLevel)hbytes.Value2;
            }
        }

        public void createOverViewModel()
        {
            var f = GetFaction();
            if (f != null && Net_IsVisible())
            {
                tilePos = WP.ToTilePos(position);
                this.position.Y = DssRef.world.tileGrid.Get(tilePos).ModelGroundY() + 0.05f;
                bound = new BoundingSphere(position, 0.3f);

                overviewModel?.DeleteMe();

                overviewModel = f.AutoLoadModelInstance(
                   LootFest.VoxelModelName.wars_flag, 1f, false);
                overviewModel.AddToRender(DrawGame.MidLayer);
                overviewModel.position = position;
            }
        }

        public bool Net_IsVisible()
        {
            return IsNetHosted || netInteractLevel != NetInteractLevel.Hidden;
        }

        public override void selectionFrame(LocalPlayer player, bool hover, Selection selection)
        {
            selection.groupModels_terrian.OneFrameModel(position, new Vector3( 0.6f), hover, false);
        }

        public override void asynchCullingUpdate(float time, bool bStateA)
        {
            //if (inRender_detailLayer)
            //{
            //    lib.DoNothing();
            //}
            DssRef.state.culling.InRender_Asynch(ref enterRender_overviewLayer_async, ref enterRender_detailLayer_async, bStateA, tilePos, IsNetHosted ? GetPlayer().GetLocalPlayer().playerData.localPlayerIndex : 0);
        }

        public override void setInRenderState()
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

            if (reason != DeleteReason.LostHost)
            {
                var w = Ref.netSession.BeginWritingPacket(PacketType.DssPinDelete, PacketReliability.Reliable);
                w.Write((ushort)myIndex);
            }
        }

        override public bool rayCollision(Ray ray)
        {
            if (overviewModel != null)
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

        public override bool defeatedBy(int attackerFaction)
        {
            throw new NotImplementedException();
        }

        public override bool aliveAndBelongTo(int faction)
        {
            throw new NotImplementedException();
        }
        public override void OnNewOwner(Faction newFaction, bool convert)
        {
            throw new NotImplementedException();
        }

        public override GameObjectType gameobjectType()
        {
           return GameObjectType.LocationPin;
        }

        public override void NameEditEvent(string result, object tag)
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

    enum PingMessage
    {
        None,
        Look,
        GoHere,
        Attack, 
        Defend, 
        Help,
        Delivery, 
        
        NUM
    }
}
