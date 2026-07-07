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

namespace VikingEngine.DSSWars.Interface.HudPinUi
{
    //HUD Pins
    

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
            Remove(city.myIndex);
        }

        public void writeGameState(System.IO.BinaryWriter w)
        {
            w.Write((ushort)Count);
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
            if (Ref.netSession.InMultiplayerSession)
            {
                AllHumansLoop allHumans = new AllHumansLoop();
                while (allHumans.Next(out _))
                { 
                    RichBoxContent buttoncontent = new RichBoxContent();
                    allHumans.sel.addNetGamerIconsToHud(buttoncontent, true);
                    RbImage voiceIcon = new RbImage(allHumans.sel.voiceState);
                    
                    allHumans.sel.voiceIcon = voiceIcon;
                    buttoncontent.Add(voiceIcon);

                    bool enabled;
                    RbButtonStyle style;
                    if (allHumans.sel.IsLocal)
                    {
                        enabled = false;
                        style = RbButtonStyle.HoverArea;
                    }
                    else
                    { 
                        enabled = true;
                        style = allHumans.sel.HasSupportDLC() ? RbButtonStyle.GoldOutline : RbButtonStyle.Outline;
                    }
                    content.Add(new ArtButton(style, buttoncontent, 
                        new RbAction1Arg<AbsHumanPlayer>((AbsHumanPlayer selected)=>
                        { 
                            player.gameControls.clearSelection();
                            player.hud.objMenu.netSessionDisplay.selectedPlayer = selected.GetRemotePlayer();
                            player.hud.needRefresh = true;
                        }, allHumans.sel), 
                        new RbTooltip((RichBoxContent content, object tag) => {

                            DssRef.state.LocalHost().gameControls.input.VoiceChat.ToRichContent(content);
                            content.space();
                            content.Add(new RbText(".Voice chat", HudLib.TitleColor_Action));

                            content.newParagraph();
                            AbsHumanPlayer player = (AbsHumanPlayer)tag;
                            player.addNetGamerToHud(content, true, true);
                            
                            }, allHumans.sel), enabled));
                }
            }

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
                        Remove(id.cityIndex);
                    }
                }
            }
            else if (add)
            {
                pins = new CityHudPin() { id.hudPin };
                Add(id.cityIndex, pins);
            }

        }
    }
}
