using Steamworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Players.Profile;
using VikingEngine.Graphics;
using VikingEngine.LootFest;
using VikingEngine.LootFest.Map.HDvoxel;
using VikingEngine.Voxels;

namespace VikingEngine.DSSWars
{
    class FactionModelBuilder : Voxels.ModelBuilder
    {
        static readonly IntVector3 TroopBannerStart = new IntVector3(4, 44, 2);
        static readonly IntVector3 WavingFlagStart = new IntVector3(4, 44, 3);
        static readonly IntVector3 WavingFlagStart_LargeFlag = new IntVector3(7, 39, 3);
        static readonly IntVector3 HorseBannerStart = new IntVector3(3, 50, 0);
        static readonly IntVector3 CityBannerStart = new IntVector3(6, 44, 0);
        static readonly IntVector3 ArmyBannerStart = new IntVector3(1, 0, 1);
        static readonly IntVector3 ArmyStandStart = new IntVector3(8, 32, 8);
        static readonly IntVector3 PinStart = new IntVector3(8, 22, 8);
        static readonly IntVector3 ArmyShipStart = new IntVector3(8, 25, 8);
        static readonly IntVector3 CityIconStart = new IntVector3(3, 2, 3);

        public Graphics.AbsVoxelObj buildModel(Faction faction, VoxelModelName name, VoxelObjGridDataAnimHD grid)
        {
            if (faction == null || faction.player == null || faction.player.profile.flag == null)
            {
                return DssRef.models.ErrorModel();//empty
            }
            //this.faction = faction;
            //this.name = name;

            VoxelObjGridDataAnimHD copy = grid.Clone();
            copy.ReplaceMaterial(faction.player.profile.flag.GetColorReplaceTable());


            switch (name)
            {
                case VoxelModelName.banner:
                    addFlagTexture(faction, copy, TroopBannerStart, true, null, 1);
                    addFlagTexture(faction, copy, TroopBannerStart, true, null, 2);
                    addFlagTexture(faction, copy, TroopBannerStart, true, null, 3);
                    break;
                case VoxelModelName.wars_flag:
                    addFlagTexture(faction, copy, WavingFlagStart, true, null, 0);
                    addFlagTexture(faction, copy, WavingFlagStart, true, null, 1);

                    addFlagTexture(faction, copy, WavingFlagStart, true, null, 3);
                    addFlagTexture(faction, copy, WavingFlagStart, true, null, 4);
                    addFlagTexture(faction, copy, WavingFlagStart_LargeFlag, true, null, 5);

                    addFlagTexture(faction, copy, WavingFlagStart, true, null, 8);
                    break;
                case VoxelModelName.horsebanner:
                    addFlagTexture(faction, copy, HorseBannerStart, true, null);
                    break;
                case VoxelModelName.citybanner:
                    addFlagTexture(faction, copy, CityBannerStart, true, null);
                    break;
                case VoxelModelName.armystand:
                    addFlagTexture(faction, copy, ArmyStandStart, true, null, 0);
                    addFlagTexture(faction, copy, ArmyShipStart, true, null, 1);
                    break;
                case VoxelModelName.pin:
                    ushort replace =  BlockHD.ToBlockValue(faction.player.profile.flag.col0_Main, BlockHD.DefaultMaterial);
                    addFlagTexture(faction, copy, PinStart, true, replace, 0);
                    addFlagTexture(faction, copy, PinStart, true, replace, 1);
                    addFlagTexture(faction, copy, PinStart, true, replace, 2);

                    break;
                case VoxelModelName.armystand_detail:
                    addFlagTexture(faction, copy, ArmyStandStart, true, null, 0);
                    break;
                case VoxelModelName.armybanner:
                    addFlagTexture(faction, copy, ArmyBannerStart, false, null);
                    break;
                case VoxelModelName.cityicon:
                    addFlagTexture(faction, copy, CityIconStart, false, null);
                    break;
            }

            var centerAdjust = grid.Frames[0].BottomCenterAdj();

            buildVerticeDataHD_ColorNormal(copy.Frames, centerAdjust);

            Graphics.VoxelModel model = modelFromVertices();

            if (name == VoxelModelName.wars_flag)
            {
                model.Effect = FlagWaveEffect.GetFlagSingletonSafe();
                model.customShader = true;
            }

            return model;
        }

        void addFlagTexture(Faction faction, VoxelObjGridDataAnimHD grid, IntVector3 start, bool standing, ushort? replace, int frame = 0)
        {

            Span<ushort> blockColors = stackalloc ushort[9];
            faction.player.profile.flag.FillBlockColors(blockColors);

            var gridData = grid.Frames[frame];

            var flagLoop = faction.player.profile.flag.flagDesign.LoopInstance();


            while (flagLoop.Next())
            {
                byte colId = faction.player.profile.flag.flagDesign.Get(flagLoop.Position);
                ushort blockCol = blockColors[colId];

                IntVector3 gridPos = start;
                gridPos.X += flagLoop.Position.X;
                if (standing)
                {
                    gridPos.Y -= flagLoop.Position.Y;
                }
                else
                {
                    gridPos.Z += flagLoop.Position.Y;
                }

                if (!replace.HasValue || gridData.Get(gridPos)== replace.Value)
                {
                    gridData.Set(gridPos, blockCol);
                }
            }
        }
        
    }
}
