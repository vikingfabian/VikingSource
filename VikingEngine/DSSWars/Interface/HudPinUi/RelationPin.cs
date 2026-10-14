using Microsoft.Xna.Framework;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.DSSWars.GameObject.ObjectPointer;
using VikingEngine.DSSWars.Players;
using VikingEngine.HUD.RichBox;
using VikingEngine.HUD.RichBox.Artistic;
using VikingEngine.PJ;

namespace VikingEngine.DSSWars.Interface.HudPinUi
{
    partial class HudPinManager
    {
        public HashSet<PFaction> relationPins = new HashSet<PFaction>(4);

        protected void relationsToHUD(LocalPlayer player, RichBoxContent content)
        {
            lock (relationPins)
            {
                PFaction remove = PFaction.Empty;
                foreach (var pin in relationPins)
                {
                    if (pin.TryGetFaction(out var faction)) {
                        var relation = DssRef.world.diplomacy.GetRelation(player.pfaction, pin);
                        if (!DisplayRelation(relation.Relation))
                        {
                            remove = pin;
                        }

                        RichBoxContent buttoncontent = new RichBoxContent();
                        IconName.Relation(relation.Relation, out SpriteName opprelIcon, out string opprelName);
                        buttoncontent.Add(faction.FlagTextureToHud());
                        buttoncontent.Add(new RbImage(opprelIcon));
                        buttoncontent.hspace();
                        buttoncontent.Add(new RbText(opprelName, Color.White));
                        buttoncontent.space();
                        int sec = Convert.ToInt32(relation.RelationEnd_GameTimeSec.Seconds);
                        buttoncontent.Add(new RbText(string.Format(DssRef.lang.Diplomacy_TruceTimeLength, sec), HudLib.NotAvailableColor));

                        content.Add(new RbButton(buttoncontent,
                          new RbAction1Arg<Faction>((Faction selected) =>
                          {
                             
                          }, faction),
                          new RbTooltip((RichBoxContent content, object tag) => {
                              PFaction pFaction = (PFaction)tag;
                              var relation = DssRef.world.diplomacy.GetRelation(player.pfaction, pFaction);
                              DiplomacyDisplay.FactionRelationDisplay(pFaction.GetFaction(), relation.Relation, content, true);
                          }, pin), true, HudPin.BgCol));
                    } }

                if (remove.HasValue())
                {
                    relationPins.Remove(remove);
                }
            }

           
        }

        public static bool DisplayRelation(RelationType relation)
        { 
            return relation == RelationType.RelationTypeN2_Truce || relation == RelationType.RelationTypeN3_Mobilization;
        }
    }
}
