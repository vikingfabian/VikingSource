using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Display.Translation;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.Map;
using VikingEngine.DSSWars.Resource;
using VikingEngine.DSSWars.Work;

namespace VikingEngine.DSSWars.Build
{
    class BuildOption
    {   
        public BuildAndExpandType buildType;
        public CraftBlueprint blueprint;
        public TerrainMainType mainType;
        public int subType;
        public SpriteName sprite;
        public bool uniqueBuilding = false;

        public BuildOption(BuildAndExpandType buildType, TerrainMainType mainType, int subType, SpriteName sprite, CraftBlueprint blueprint)
        {
            this.sprite = sprite;
            this.buildType = buildType;
            this.blueprint = blueprint;
            this.mainType = mainType;
            this.subType = subType;
            //this.experienceType = experienceType;

            BuildLib.BuildOptions[(int)buildType] = this;
        }
        public WorkExperienceType experienceType() 
        {
            return blueprint.experienceType;
        }
        public string Label()
        {
            return LangLib.TerrainName(mainType, subType);
        }
        public string Description()
        {
            switch (mainType)
            {
                case TerrainMainType.Building:
                    return LangLib.BuildingDescription((TerrainBuildingType)subType);
                case TerrainMainType.Foil:
                    return DssRef.lang.BuildingType_Farm_Description;
                case TerrainMainType.Decor:
                    return DssRef.lang.BuildingType_Decor_Description;
            }

            return TextLib.Error;
        }

        public void destroy_async(City city, IntVector2 subPos)
        {
            switch (mainType)
            {
                case TerrainMainType.Building:
                    {
                        switch ((TerrainBuildingType)subType)
                        {
                            
                            case TerrainBuildingType.WorkerHut:
                                city.onWorkHutBuild(false);
                                break;

                            case TerrainBuildingType.SoldierBarracks:
                            case TerrainBuildingType.ArcherBarracks:
                            case TerrainBuildingType.WarmashineBarracks:
                            case TerrainBuildingType.KnightsBarracks:
                            case TerrainBuildingType.GunBarracks:
                                city.destroyBarracks(subPos);
                                break;

                            case TerrainBuildingType.Recruitment:
                            case TerrainBuildingType.Postal:
                                city.destroyDelivery(subPos);
                                break;
                        }
                    }
                    break;
            }
        }

        public void execute_async(City city, IntVector2 subPos, ref SubTile subTile)
        {
            subTile.SetType(mainType, subType, 1);

            switch (mainType)
            {
                case TerrainMainType.Building:
                    {
                        switch ((TerrainBuildingType)subType)
                        {
                            case TerrainBuildingType.Logistics:
                                if (city.CanBuildLogistics(2))
                                {
                                    subTile.terrainAmount = 2;
                                }
                                city.buildingLevel_logistics = subTile.terrainAmount;
                                break;

                            case TerrainBuildingType.WorkerHut:
                                city.onWorkHutBuild(true);
                                break;

                            case TerrainBuildingType.SoldierBarracks:
                                Ref.update.AddSyncAction(new SyncAction2Arg<IntVector2, Conscript.BarracksType>(city.addBarracks, subPos, Conscript.BarracksType.Soldier));
                                break;
                            case TerrainBuildingType.ArcherBarracks:
                                Ref.update.AddSyncAction(new SyncAction2Arg<IntVector2, Conscript.BarracksType>(city.addBarracks, subPos, Conscript.BarracksType.Archer));
                                break;
                            case TerrainBuildingType.WarmashineBarracks:
                                Ref.update.AddSyncAction(new SyncAction2Arg<IntVector2, Conscript.BarracksType>(city.addBarracks, subPos, Conscript.BarracksType.Warmashine));
                                break;
                            case TerrainBuildingType.KnightsBarracks:
                                Ref.update.AddSyncAction(new SyncAction2Arg<IntVector2, Conscript.BarracksType>(city.addBarracks, subPos, Conscript.BarracksType.Knight));
                                break;
                            case TerrainBuildingType.GunBarracks:
                                Ref.update.AddSyncAction(new SyncAction2Arg<IntVector2, Conscript.BarracksType>(city.addBarracks, subPos, Conscript.BarracksType.Gun));
                                break;
                            case TerrainBuildingType.CannonBarracks:
                                Ref.update.AddSyncAction(new SyncAction2Arg<IntVector2, Conscript.BarracksType>(city.addBarracks, subPos, Conscript.BarracksType.Cannon));
                                break;

                            case TerrainBuildingType.Postal:
                                Ref.update.AddSyncAction(new SyncAction2Arg<IntVector2, bool>(city.addDelivery, subPos, false));
                                break;

                            case TerrainBuildingType.Recruitment:
                                Ref.update.AddSyncAction(new SyncAction2Arg<IntVector2, bool>(city.addDelivery, subPos, true));
                                break;
                        }
                    }
                    break;

                case TerrainMainType.Decor:
                    bool statue = false;
                    switch ((TerrainDecorType)subType)
                    {
                        case TerrainDecorType.Statue_ThePlayer:
                            statue = true;
                            break;
                    }

                    if (city.faction.player.IsPlayer())
                    {
                        city.faction.player.GetLocalPlayer().statistics.onDecorBuild_async(statue);
                    }
                    break;
            }

            blueprint.payResources(city);
        }
    }
}
