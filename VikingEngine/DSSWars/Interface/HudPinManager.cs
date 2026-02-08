using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Data;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.Players;
using VikingEngine.DSSWars.Presentation;
using VikingEngine.DSSWars.Resource;
using VikingEngine.DSSWars.XP;
using VikingEngine.HUD.RichBox;
using VikingEngine.HUD.RichBox.Artistic;
using VikingEngine.LootFest.Players;
using VikingEngine.ToGG.MoonFall;

namespace VikingEngine.DSSWars.Interface
{
    //HUD Pins
    enum HudPinType
    {
        Resource,
        TechnologyTree,
        WorkerXp,
    }

    struct HudPin
    {
        static readonly Color BgCol = new Color(0, 0, 0.1f, 0.4f);
        public static readonly HudPin Empty = new HudPin();

        public HudPinType type;
        public int id;

        public void writeGameState(System.IO.BinaryWriter w)
        {
            w.Write((byte)id);
            w.Write((byte)type);
        }
        public void readGameState(System.IO.BinaryReader r, int subversion)
        {
            id = r.ReadByte();
            if (subversion >= 88)
            { 
                type = (HudPinType)r.ReadByte();
            }
        }

        public HudPin(ItemResourceType itemResource)
        {
            type = HudPinType.Resource;
            id = (int)itemResource;
        }

        public HudPin(TechnologyTreeType techType)
        {
            type = HudPinType.TechnologyTree;
            id = (int)techType;
        }

        public HudPin(WorkExperienceType experienceType)
        {
            type = HudPinType.WorkerXp;
            id = (int)experienceType;
        }

        public void toHud(RichBoxContent content, City city)
        {
            switch (type)
            {
                case HudPinType.Resource:
                    var item = (ItemResourceType)id;
                    var resourceCount = city.GetGroupedResource(item);
                    content.Add(new RbButton(
                        new List<AbsRichBoxMember> { 
                            new RbImage(ResourceLib.Icon(item)),
                            new RbSpace(0.5f), 
                            new RbText(resourceCount.amount.ToString(), Color.White)
                        }, null, new RbTooltip(ResourceLib.FullResourceInfo, new ResourceInfoTag(null,city, item)), true, BgCol));
                    break;
                case HudPinType.TechnologyTree:
                    var techType = (TechnologyTreeType)id;
                    LangLib.Technology(techType, out SpriteName techicon, out string techname);
                    ResearchProgress progress = city.technology.progress(techType, out int goal);

                    List<AbsRichBoxMember> buttonContent = new List<AbsRichBoxMember>(3);
                    if (progress.points >= goal)
                    {
                        buttonContent.Add(new RbImage(SpriteName.WarsTechnology_Unlocked));
                    }
                    else
                    {
                        buttonContent.Add(new RbImage(SpriteName.WarsTechnology_Locked));
                    }
                    buttonContent.Add(new RbImage(techicon));
                    buttonContent.Add(new RbSpace(0.5f));
                    if (progress.points < goal)
                    {
                        buttonContent.Add(new RbText($"{progress.points}/{goal}", Color.White));
                    }
                    content.Add(new RbButton(
                        buttonContent, null, 
                        new RbTooltip_Text(string.Format( DssRef.lang.Language_ItemCountPresentation, DssRef.lang.Technology_Title, techname)), 
                        true, BgCol));
                    break;

                case HudPinType.WorkerXp:
                    WorkExperienceType experienceType = (WorkExperienceType)id;
                    LangLib.ExperienceType(experienceType, out string xpName, out SpriteName xpIcon);
                    WorkExperienceLevels levels = city.cityExperienceLevels.Get(experienceType);

                    content.Add(new RbButton(
                        new List<AbsRichBoxMember> {
                            new RbImage(xpIcon),
                            new RbSpace(0.5f),
                            new RbImage(LangLib.ExperienceLevelIcon( levels.Max())),
                        }, null, new RbTooltip_Text(xpName), true, BgCol));
                    
                    break;
        }}

        public bool Equals(HudPin other)
        {
            return type == other.type && id == other.id;
        }
    }

    struct CityHudPinId
    {
        public int cityIndex;
        public HudPin hudPin;

        public CityHudPinId(int cityIndex, HudPin hudPin)
        {
            this.cityIndex = cityIndex;
            this.hudPin = hudPin;
        }
    }

    class CityHudPin : List<HudPin>
    {
        public void writeGameState(System.IO.BinaryWriter w)
        {
            w.Write((byte)this.Count);
            foreach (var pin in this)
            {
                pin.writeGameState(w);
            }
        }
        public void readGameState(System.IO.BinaryReader r, int subversion)
        {
            var count = r.ReadByte();
            for (int i = 0; i < count; i++)
            {
                HudPin pin = new HudPin();
                pin.readGameState(r, subversion);
                Add(pin);
            }
        }

        public bool TryGet(HudPin id)
        {
            foreach (var pin in this)
            {
                if (pin.Equals(id))
                {
                    return true;
                }
            } 
            return false;
        }

        public bool TryRemove(HudPin id)
        {
            for (int i = 0; i < Count; ++i)
            {
                if (this[i].Equals(id))
                {
                    RemoveAt(i);
                    return true;
                }
            }
            return false;
        }
    }

    class HudPinManager : Dictionary<int, CityHudPin>
    {
        public HudPinManager() :
            base(8)
        { }

        public void toggleButton(RichBoxContent content, CityHudPinId pinId)
        {
            bool onHud = TryGet(pinId);
            content.Add(new ArtToggle(onHud, new List<AbsRichBoxMember> {
                    new RbImage(SpriteName.HudPinIcon, 1f, onHud? Color.White : Color.Gray) }, new RbAction(() => { Set(pinId, !onHud); }),
                    new RbTooltip_Text(DssRef.lang.HudPins)));
        }

        public void clear(City city)
        {
            this.Remove(city.myIndex);
        }

        public void writeGameState(System.IO.BinaryWriter w)
        {
            w.Write((ushort)this.Count);
            foreach (var kv in this)
            {
                w.Write((ushort)kv.Key);
                kv.Value.writeGameState(w);
            }
        }

        public void readGameState(System.IO.BinaryReader r, int subversion)
        {
            int count = r.ReadUInt16();
            for (int i = 0; i < count; i++)
            {
                int city = r.ReadUInt16();

                CityHudPin pins = new CityHudPin();
                pins.readGameState(r, subversion);
                Add(city, pins);
            }
        }


        public void toHUD(LocalPlayer player, RichBoxContent content)
        { 
            foreach (var kv in this)
            {
                City city = DssRef.world.cities[kv.Key];
                if (city.ToPinHud(new ObjectHudArgs(content, player, false))) 
                {
                    foreach (var pin in kv.Value)
                    { 
                        pin.toHud(content, city);
                    }
                    content.space(2);
                }
            }
        }

        public bool isPinnedProperty(object tag, bool set, bool value)
        {
            CityHudPinId id = (CityHudPinId)tag;
            if (set)
            {
                Set(id, value);
            }
            return TryGet(id);
        }

        public bool TryGet(CityHudPinId id)
        {
            if (TryGetValue(id.cityIndex, out CityHudPin pins))
            {
                return pins.TryGet(id.hudPin);
            }
            return false;
        }

        public void Set(CityHudPinId id, bool add)
        {
            if (TryGetValue(id.cityIndex, out CityHudPin pins))
            {
                if (add)
                {
                    if (!pins.TryGet(id.hudPin))
                    {
                        pins.Add(id.hudPin);
                    }
                }
                else
                {
                    pins.TryRemove(id.hudPin);
                    if (pins.Count == 0)
                    { 
                        this.Remove(id.cityIndex);
                    }
                }
            }
            else if (add)
            {
                pins = new CityHudPin() { id.hudPin };
                this.Add(id.cityIndex, pins);
            }

        }
    }
}
