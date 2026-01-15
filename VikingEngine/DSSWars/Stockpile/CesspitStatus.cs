using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VikingEngine.DebugExtensions;
using VikingEngine.DSSWars.Delivery;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.Interface;
using VikingEngine.DSSWars.Players;
using VikingEngine.DSSWars.Resource;
using VikingEngine.DSSWars.Stockpile;
using VikingEngine.EngineSpace;
using VikingEngine.HUD.RichBox;
using VikingEngine.HUD.RichBox.Artistic;
using VikingEngine.LootFest.GO.Gadgets;
using VikingEngine.PJ.Joust;
using VikingEngine.ToGG.MoonFall;

namespace VikingEngine.DSSWars.Stockpile
{
    struct CesspitStatus
    {
        public int idAndPosition;
        public ItemResourceType type;

        public void writeGameState(System.IO.BinaryWriter w)
        {
            w.Write(idAndPosition);
            w.Write((byte)type);
        }

        public void readGameState(System.IO.BinaryReader r, int subVersion)
        {
            idAndPosition = r.ReadInt32();
            type = (ItemResourceType)r.ReadByte();
        }
    }

}

namespace VikingEngine.DSSWars.GameObject
{
   
    partial class City
    {
        public int selectedCessPit = -1;
        public StructList<CesspitStatus> cesspits = new StructList<CesspitStatus>(0);

        public void cesspitToHud(LocalPlayer player, RichBoxContent content)
        {
            if (cesspits.Count > 0)
            {
                lock (cesspits.array)
                {
                    if (cesspits.InBound(selectedCessPit))
                    {
                        CesspitStatus currentStatus = cesspits.array[selectedCessPit];
                        //selected view

                        HudLib.buildingMenuTitle(content, SpriteName.NO_IMAGE, DssRef.todoLang.BuildingType_Cesspit, currentStatus.idAndPosition,
                            selectedCessPit, cesspits.Count,
                            () => { selectedCessPit = -1; },
                            (int next) => {
                                selectedCessPit = Bound.SetRollover(selectedCessPit + next, 0, cesspits.Count - 1);
                            });

                        content.newParagraph();
                        option(ItemResourceType.NONE);

                        for (ResourceGroupType group = 0; group < ResourceGroupType.NUM; group++)
                        {
                            content.newParagraph();
                            var list = ResourceLib.ResourceGroupList(group);
                            foreach (var item in list)
                            {
                                option(item);
                            }
                        }

                        void option(ItemResourceType item)
                        {
                            IconName.Item(item, out var icon, out _);
                            content.Add(new ArtOption(item == currentStatus.type,
                                new List<AbsRichBoxMember> { new RbImage(icon) },
                                new RbAction1Arg<ItemResourceType>((ItemResourceType item) =>
                                {
                                    lock (cesspits.array)
                                    {
                                        if (cesspits.InBound(selectedCessPit))
                                        {
                                            cesspits.array[selectedCessPit].type = item;
                                            refreshResourceCesspits();
                                        }
                                    }
                                }, item), 
                                new RbTooltip(ResourceLib.FullResourceInfo, new ResourceInfoTag(null, this, item))));
                        }
                    }
                    else
                    {
                        bool hasAnySelected = false;
                        //list all
                        content.h2(".Select building", HudLib.TitleColor_Action);

                        for (int i = 0; i < cesspits.Count; ++i)
                        {
                            ItemResourceType item = cesspits.array[i].type;
                            hasAnySelected |= item != ItemResourceType.NONE;
                            IconName.Item(item, out var icon, out var name);

                            content.newLine();
                            content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> {
                                new RbImage(icon), new RbSpace(), new RbText(name),
                            }, new RbAction1Arg<int>((int selectIndex) => { selectedCessPit = selectIndex; }, i),
                            null)
                            { fillWidth = true });
                        }

                        content.newParagraph();
                        HudLib.copyPaste(content, player, new RbAction(() =>
                            {
                                player.cesspitsCopy.Clear();
                                player.cesspitsCopy.AddRange(cesspits.array);
                            }),
                            new RbAction(() =>
                            {
                                lock (cesspits.array)
                                {
                                    int count = Math.Min(cesspits.Count, player.cesspitsCopy.Count);
                                    for (int i = 0; i < count; ++i)
                                    {
                                        cesspits.array[i].type = player.cesspitsCopy[i].type;
                                    }
                                }
                                refreshResourceCesspits();
                            }),
                            hasAnySelected, player.cesspitsCopy.Count > 0);

                        content.newLine();
                        content.Add(new ArtButton(RbButtonStyle.Secondary, new List<AbsRichBoxMember> {
                            new RbText(DssRef.lang.FlagEditor_ClearAll)
                            },
                            new RbAction(() =>
                            {
                                for (int i = 0; i < cesspits.Count; ++i)
                                {
                                    cesspits.array[i].type = ItemResourceType.NONE;
                                }
                                refreshResourceCesspits();
                            })));
                        
                    }                
                }
            }
            else
            {
                //No cesspits
                content.text(DssRef.lang.Hud_EmptyList, HudLib.InfoYellow_Light);
                content.newParagraph();
                content.h2(DssRef.lang.Hud_PurchaseTitle_Requirement, HudLib.TitleColor_Label);
                content.newLine();
                content.Add(new RbImage(SpriteName.MissingImage));
                content.space();
                content.Add(new RbText(DssRef.todoLang.BuildingType_Cesspit));
            }

            content.newLine();
            content.text(".Destroy selected resurces that reach stockpile limit.", HudLib.InfoYellow_Light);
            content.text(string.Format(".Convert {0}% to {1}", 10, DssRef.lang.Resource_TypeName_Fuel), HudLib.InfoYellow_Light);

        }

        void refreshResourceCesspits()
        {
            Task.Run(() =>
            {
                try
                {
                    if (cesspits.array != null)
                    {
                        Span<bool> span = stackalloc bool[EntityComponent.CityResoureIndex.COUNT];

                        lock (cesspits.array)
                        {
                            foreach (var cp in cesspits.array)
                            {
                                if (cp.type != ItemResourceType.NONE)
                                {
                                    span[Resource.ItemPropertyColl.Get(cp.type).cityResourceIndex] = true;
                                }
                            }
                        }

                        for (int i = 0; i < EntityComponent.CityResoureIndex.COUNT; ++i)
                        {
                            DssRef.world.cityResouces[resourceComponentStartIndex + i].hasCesspit = span[i];
                        }
                    }
                }
                catch (Exception ex)
                { 
                    BlueScreen.ThreadException = ex;
                }
            });
        }

        public void addCesspit(IntVector2 subPos)
        {
            CesspitStatus status = new CesspitStatus()
            {
                idAndPosition = conv.IntVector2ToInt(subPos),
                type = ItemResourceType.NONE,
            };

            if (cesspits.array == null)
            {
                cesspits.Init(4);
            }

            lock (cesspits.array)
            {
                cesspits.Add(status);
            }
        }

        public void destroyCesspit(IntVector2 subPos)
        {
            lock (cesspits.array)
            {
                int index = deliveryIxFromSubTile(subPos);
                
                cesspits.RemoveAt(index);
            }
        }


        public int cesspitIxFromSubTile(IntVector2 subTilePos)
        {
            int id = conv.IntVector2ToInt(subTilePos);
            for (int i = 0; i < cesspits.Count; ++i)
            {
                if (cesspits.array[i].idAndPosition == id)
                {
                    return i;
                }
            }

            return -1;
        }

        public void itemCesspitClick(LocalPlayer player, ItemResourceType item)
        {
            for (int i = 0; i < cesspits.Count; ++i)
            {
                if (cesspits.array[i].type == item)
                {
                    selectedCessPit = i;
                    player.cityTab = MenuTab.CessPit;
                    return;
                }
            }
        }
    }
}
