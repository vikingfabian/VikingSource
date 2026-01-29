using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Data;
using VikingEngine.HUD.RichBox;
using VikingEngine.HUD.RichBox.Artistic;
using VikingEngine.LootFest.Players;

namespace VikingEngine.DSSWars.Players.Profile
{
    class ObjectHudSettings
    {
        public bool viewTagsOnMap = true;
        public bool viewLowFoodOnMap = false;
        public bool viewIdleWorkOnMap = false;
        public bool viewStuckBuildOrdersOnMap = false;

        public bool ViewAnyOnMap()
        {
            return viewTagsOnMap || viewLowFoodOnMap || viewIdleWorkOnMap || viewStuckBuildOrdersOnMap;
        }

        public bool viewTagsOnMapProperty(object tag, bool set, bool value)
        {
            if (set)
            {
                viewTagsOnMap = value;
            }
            return viewTagsOnMap;
        }

        public bool viewLowFoodOnMapProperty(object tag, bool set, bool value)
        {
            if (set)
            {
                viewLowFoodOnMap = value;
            }
            return viewLowFoodOnMap;
        }

        public bool viewIdleWorkOnMapProperty(object tag, bool set, bool value)
        {
            if (set)
            {
                viewIdleWorkOnMap = value;
            }
            return viewIdleWorkOnMap;
        }

        public bool viewStuckBuildOrdersOnMapProperty(object tag, bool set, bool value)
        {
            if (set)
            {
                viewStuckBuildOrdersOnMap = value;
            }
            return viewStuckBuildOrdersOnMap;
        }
        public void write(BinaryWriter w)
        {
            w.Write(viewTagsOnMap);
            w.Write(viewLowFoodOnMap);
            w.Write(viewIdleWorkOnMap);
            w.Write(viewStuckBuildOrdersOnMap);
        }
        public void read(BinaryReader r, int subversion)
        {
            viewTagsOnMap = r.ReadBoolean();
            viewLowFoodOnMap = r.ReadBoolean();
            viewIdleWorkOnMap = r.ReadBoolean();
            viewStuckBuildOrdersOnMap = r.ReadBoolean();
        }

        public void toHud(RichBoxContent content, bool city, bool casual)
        {
            content.Add(new ArtCheckbox(new List<AbsRichBoxMember> {
                new RbImage( SpriteName.warsFolder_carton), new RbSpace(), new RbText(DssRef.lang.MenuTab_Tag) }, viewTagsOnMapProperty));

            if (!casual)
            {
                content.newLine();
                content.Add(new ArtCheckbox(new List<AbsRichBoxMember> {
                new RbImage( SpriteName.WarsResource_FoodEmpty), new RbSpace(), new RbText(DssRef.lang.Message_OutOfFood_Title) }, viewLowFoodOnMapProperty));

                if (city)
                {
                    content.newLine();
                    content.Add(new ArtCheckbox(new List<AbsRichBoxMember> {
                    new RbImage( SpriteName.WarsIcon_WorkQueueIdle), new RbSpace(), new RbText(DssRef.lang.WorkQueue_IdleWorkers) }, viewIdleWorkOnMapProperty));
                        content.newLine();
                        content.Add(new ArtCheckbox(new List<AbsRichBoxMember> {
                    new RbImage( SpriteName.WarsConstructBuildingIcon), new RbSpace(), new RbText(DssRef.lang.ObjectUi_StuckBuildOrders) }, viewStuckBuildOrdersOnMapProperty));
                }
            }
        }

        public ObjectHudSettings()
        {             
        }
    }
}
