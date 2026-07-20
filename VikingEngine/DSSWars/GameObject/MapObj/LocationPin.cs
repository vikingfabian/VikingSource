using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using VikingEngine.DSSWars.Data;
using VikingEngine.DSSWars.GameObject.ObjectPointer;
using VikingEngine.DSSWars.Interface;
using VikingEngine.DSSWars.Players;
using VikingEngine.DSSWars.Presentation;
using VikingEngine.HUD.RichBox;
using VikingEngine.HUD.RichBox.Artistic;
using VikingEngine.LootFest.Players;
using VikingEngine.Network;

namespace VikingEngine.DSSWars.GameObject
{
    class LocationPin: AbsMapObject
    {
        const int ModelVariants = 3;

        Graphics.AbsVoxelObj overviewModel;
        BoundingSphere bound;
        int modelVariant = 0;
        public PingMessage pingMessage = PingMessage.None;
        public NetInteractLevel netInteractLevel = NetInteractLevel.Hidden;
        

        public LocationPin(RemotePlayer player)
        {
            IsNetHosted = false;
            pfaction = player.pfaction;
        }

        public LocationPin(AbsHumanPlayer player, Vector3 position)
        { 
            this.position = position;
            pfaction = player.pfaction; 
            createOverViewModel();
            inRender_overviewLayer = true;          
        }

        public LocationPin(AbsHumanPlayer player, System.IO.BinaryReader r, int subVersion)
        {
            pfaction = player.pfaction;
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

        void PinPresentationHud(ObjectHudArgs args, bool tooltip)
        {
            nameToHud(args.content, !tooltip);

            args.content.Add(new RbBeginTitle(tooltip ? 2 : 1));
            if (!tagToHud(args.content))
            {
                var faction = pfaction.GetFaction();
                if (faction != null)
                {
                    args.content.Add(faction.FlagTextureToHud());
                }
            }
            args.content.space(0.5f);
            args.content.Add(new RbImage(SpriteName.WarsLocationPin));
            args.content.space(0.5f);
            args.content.Add(new RbText(DssRef.todoLang.ObjectType_LocationPin, tooltip ? HudLib.TitleColor_TypeName : HudLib.TitleColor_Head));

            args.content.space(1);

            IndexToHud(args.content);

            args.content.newLine();
            ownerToHud(args, !tooltip);
        }

        public override void toHud(ObjectHudArgs args)
        {
            PinPresentationHud(args, false);

            int tabSel = 0;

            var tabs = new List<ArtTabMember>((int)MenuTab.NUM_NONE);

            List<MenuTab> availableTabs = new List<MenuTab> { MenuTab.Info, MenuTab.Tag };
            for (int i = 0; i < availableTabs.Count; ++i)
            {
                var text = new RbText(LangLib.Tab(availableTabs[i], out string description, out _));
                text.overrideColor = HudLib.RbSettings.tabSelected.Color;

                tabs.Add(new ArtTabMember(new List<AbsRichBoxMember>
                            {
                                text
                            }, null));

                if (availableTabs[i] == args.player.pinTab)
                {
                    tabSel = i;
                }
            }

            bool viewControllerTabs = args.player.gameControls.tabFocusColor(Players.PlayerControls.ControllerTabFocus.ArmyMenu, out Color focusColor);
            if (viewControllerTabs && args.player.gameControls.input.Controller_TabLeft.IsActive)
            {
                args.content.Add(new RbImage(args.player.gameControls.input.Controller_TabLeft.Icon) { color = focusColor });
                args.content.space(0.5f);
            }
            var tabGroup = new ArtTabgroup(tabs, tabSel, args.player.pinTabClick);
            if (viewControllerTabs && args.player.gameControls.input.Controller_TabRight.IsActive)
            {
                tabGroup.endAttach = new List<AbsRichBoxMember> { new RbSpace(0.5f), new RbImage(args.player.gameControls.input.Controller_TabRight.Icon) { color = focusColor } };
            }

            args.content.Add(tabGroup);
            //content.newParagraph();
            //content.newLine();
            switch (args.player.pinTab)
            {
                case MenuTab.Info:
                    infoHud(args);
                    break;
               
                case MenuTab.Tag:
                    //tagsToMenu(content);
                    TagLib.TagsToMenu(args.content, args.player, this);
                    break;


            }
        }
        public void infoHud(ObjectHudArgs args)
        {
            args.content.newParagraph();
            HudLib.Label(args.content, SpriteName.NO_IMAGE, DssRef.todoLang.Message);
            args.content.newLine();
            for (PingMessage message = 0; message < PingMessage.NUM; message++)
            {
                args.content.Add(new ArtOption(message == pingMessage, new System.Collections.Generic.List<AbsRichBoxMember> { new RbText(message.ToString()) },
                    new RbAction1Arg<PingMessage>((PingMessage selected) =>
                    {
                        pingMessage = selected;
                    }, message)));
            }

            if (DssRef.DlcSupporter.owned)
            {
                args.content.newParagraph();
                HudLib.Label(args.content, SpriteName.NO_IMAGE, DssRef.todoLang.Hud_ModelType);
                args.content.newLine();

                int exendModel = ModelVariants;
                if (DssRef.FromGloryToGoo.owned)
                {
                    exendModel += 1;
                }

                for (int i = 0; i < exendModel; i++)
                {
                    args.content.Add(new ArtOption(i == modelVariant, new System.Collections.Generic.List<AbsRichBoxMember> { new RbText(TextLib.IndexToString(i)) },
                        new RbAction1Arg<int>((int selected) =>
                        {
                            modelVariant = selected;
                            if (overviewModel != null)
                            {
                                overviewModel.Frame = selected;
                            }
                        }, i)));
                }
            }

            if (Ref.netSession.InMultiplayerSession)
            {
                args.content.newParagraph();
                HudLib.Label(args.content, SpriteName.NO_IMAGE, DssRef.todoLang.ObjectType_LocationPin_Share);
                args.content.newLine();

                if (netInteractLevel == NetInteractLevel.Hidden)
                {
                    interactLevelButton(DssRef.todoLang.Group_Everyone, NetInteractLevel.Public);
                }
                else
                {
                    interactLevelButton(DssRef.todoLang.Hud_Hide, NetInteractLevel.Hidden);
                }

                void interactLevelButton(string label, NetInteractLevel level)
                {
                    args.content.Add(new ArtButton(RbButtonStyle.Primary, new System.Collections.Generic.List<AbsRichBoxMember>
                    {  new RbText(label)}, new RbAction1Arg<NetInteractLevel>(setInteractLevel, level, level == NetInteractLevel.Hidden? RbSoundType.Back : RbSoundType.Ping)));
                }
            }
            args.content.Add(new RbSeperationLine());
            args.content.newParagraph();
            args.content.Add(new ArtButton(RbButtonStyle.Primary, new System.Collections.Generic.List<AbsRichBoxMember>{
               new RbText(  DssRef.lang.Hud_Delete) }, new RbAction1Arg<int>(args.player.deletePin, myIndex)));

            args.content.newLine();
            args.content.Add(new ArtButton(RbButtonStyle.Primary, new System.Collections.Generic.List<AbsRichBoxMember>{
               new RbText(  DssRef.todoLang.Hud_DeleteAll) }, new RbAction1Arg<DeleteReason>(args.player.clearPins, DeleteReason.Disband)));

        }

        public void setInteractLevel(NetInteractLevel selected)
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
        }

        public override void toTooltip(ObjectHudArgs args)
        {
            PinPresentationHud(args, true);

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
            //v116
            w.Write((byte)modelVariant);
            Tag.write(w);
        }


        public void readGameState(System.IO.BinaryReader r, int subVersion)
        {
            name.read(r, subVersion);
            
            WP.ReadPosXZPercentU16(r, out position, out tilePos);

            if (subVersion >= 115)
            {
                var hbytes = new TwoHalfByte();
                hbytes.read(r);
                pingMessage = (PingMessage)hbytes.Value1;
                netInteractLevel = (NetInteractLevel)hbytes.Value2;
            }
            if (subVersion >= 116)
            {
                modelVariant = r.ReadByte();
                Tag.read(r, subVersion);
            }
        }

        public void createOverViewModel()
        {
            var f = pfaction.GetFaction();
            if (f != null && Net_IsVisible())
            {
                tilePos = WP.ToTilePos(position);
                this.position.Y = DssRef.world.tileGrid.Get(tilePos).ModelGroundY() + 0.05f;
                bound = new BoundingSphere(position, 0.3f);

                overviewModel?.DeleteMe();

                overviewModel = f.AutoLoadModelInstance(
                   LootFest.VoxelModelName.pin, 1f, false);
                overviewModel.Frame = modelVariant;
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
            DssRef.state.culling.InRender_Asynch(ref enterRender_overviewLayer_async, ref enterRender_detailLayer_async, bStateA, tilePos, IsNetHosted ? pfaction.GetPlayer().GetLocalPlayer().playerData.localPlayerIndex : 0);
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

        public override bool defeatedBy(PFaction attackerFaction)
        {
            throw new NotImplementedException();
        }

        public override void OnNewOwner(Faction newFaction, bool convert, ConvertReason convertReason)
        {
            throw new NotImplementedException();
        }

        public override GameObjectType gameobjectType()
        {
           return GameObjectType.LocationPin;
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

        public override bool IsArmy()
        {
            return false;
        }
        public override bool IsCity()
        {
            return false;
        }
        public override bool aliveAndBelongTo(PFaction faction)
        {
            throw new NotImplementedException();
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
