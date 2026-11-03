using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingEngine.DSSWars;
using VikingEngine.DSSWars.Build;
using VikingEngine.DSSWars.Data;
using VikingEngine.DSSWars.EntityComponent;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.Interface;
using VikingEngine.DSSWars.Resource;
using VikingEngine.DSSWars.Work;
using VikingEngine.HUD.RichBox;
using VikingEngine.HUD.RichBox.Artistic;
using VikingEngine.HUD.RichMenu;

namespace VikingEngine.Core.DSSWars.Data
{
    struct ResourceProfile
    {
        public ItemResourceType resourceType;
        public GroupedResource resource;
        
        public bool priority, stockpile, requestDelivery;
    }

    struct BuidingProfile
    {
        public BuildAndExpandType buildingType;
        public int count;
        public bool accumulate;
        public byte buildPriority;
    }

    struct WorkProfile
    {
        public WorkPriorityType workType;
        public WorkPriority  workPriority;
    }

    class CityProfile
    {
        Guid_8char id;
        public bool favorite;
        public string name;
        public MapObjectTag tag;
        public List<ResourceProfile> resourceProfiles;
        public List<BuidingProfile> buildProfiles;
        public List<WorkProfile> workProfiles;

        public CityProfile()
        { 
            id = Guid_8char.NewId();
            name = "City profile_" + id.ToString();
        }
    }

    class CityProfileStorage : List<CityProfile>
    {
        public const string ProfilesMenu = "city_profiles";
        const string EditProfileMenu = "edit_profile";

        public int currentlyEditingProfileIndex = -1;
        public CityProfileStorage()
            :base(8)
        {  
        }

        public void ToHud(City city, RichMenu menu, RichBoxContent content)
        {
            HudLib.returnButton(content, menu, true, menu.menuStack.Clear);
            content.h1("City Profile", HudLib.TitleColor_Head);

            switch (menu.menuStack.Last())
            {
                default:
                    if (Count == 0)
                    {
                        content.text("No city profiles available.");
                    }
                    else
                    {
                        for (int i = 0; i < Count; i++)
                        {
                            CityProfile profile = this[i];
                            content.h2(profile.name);
                            content.text("Favorite: " + profile.favorite);
                            content.text("Tag: " + profile.tag.backType.ToString());
                            //content.text("Resources:");
                            //for (int j = 0; j < profile.resourceProfiles.Count; j++)
                            //{
                            //    ResourceProfile resProfile = profile.resourceProfiles[j];
                            //    content.text("- " + resProfile.resourceType.ToString() + ": " + resProfile.resource.amount + ", Priority: " + resProfile.priority + ", Stockpile: " + resProfile.stockpile + ", Request Delivery: " + resProfile.requestDelivery);
                            //}
                            //content.text("Buildings:");
                            //for (int j = 0; j < profile.buildProfiles.Count; j++)
                            //{
                            //    BuidingProfile buildProfile = profile.buildProfiles[j];
                            //    content.text("- " + buildProfile.buildingType.ToString() + ": Count: " + buildProfile.count + ", Accumulate: " + buildProfile.accumulate + ", Build Priority: " + buildProfile.buildPriority);
                            //}
                            //content.text("Work Priorities:");
                            //for (int j = 0; j < profile.workProfiles.Count; j++)
                            //{
                            //    WorkProfile workProfile = profile.workProfiles[j];
                            //    content.text("- " + workProfile.workType.ToString() + ": Priority: " + workProfile.workPriority.ToString());
                            //}
                        }
                    }

                    content.newParagraph();
                    content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> { new RbText("Add City Profile") }, new RbAction(() => {
                        currentlyEditingProfileIndex = Count;
                        this.Add(new CityProfile());
                        menu.menuStack.Add(EditProfileMenu);
                    })));
                    break;
                case EditProfileMenu:
                    {
                        CityProfile profile = this[currentlyEditingProfileIndex];
                        //EDIT
                        content.h2(profile.name);
                    }
                    break;

            }
        }
    }
}
