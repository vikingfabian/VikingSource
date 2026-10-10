using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.Core.BlackBolts.Data;
using VikingEngine.DSSWars;
using VikingEngine.HUD.RichBox;

namespace VikingEngine.Core.BlackBolts.Mission
{
    

    abstract class AbsMissionSetup
    {
        public RunStatistics runStatistics;

        public IntVector2 mapSize = new IntVector2(25, 20);
        public string missionName;
        public List<ToolSetupComponent> componentList;
        public List<IOTemplate> ioUnits;

        public AbsMissionSetup()
        {
            BlackRef.missionSetup = this;
        }

        public void LoadMissionMap()
        {
            if (!IsSandbox)
            {
                BlackRef.storage.Load(false);
                for(int i = 0; i < BlackRef.storage.restorePoint.Count; ++i)
                {
                    var m = BlackRef.storage.restorePoint[i];
                    m.locked = true;
                    BlackRef.storage.restorePoint[i] = m;
                }
            }
        }

        protected void mission1units()
        {
            {
                IOTemplate template = new IOTemplate() { name = "Heater", tilesize = new IntVector2(1, 1) };
                template.ports.Add(new IO_port(true, new Map.MapPlacement(IntVector2.Zero, Dir4.S),
                    ResourceType.Any, 1));
                template.ports.Add(new IO_port(false, new Map.MapPlacement(IntVector2.Zero, Dir4.N),
                    ResourceType.Heat, 1));
                template.model = BlackRef.models.voxelModels[LootFest.VoxelModelName.bb_onetile];
                template.frame = 7;
                template.scale = 1.6f;
                ioUnits.Add(template);
            }
            {
                IOTemplate template = new IOTemplate() { name = "Chicken seperator", tilesize = new IntVector2(2, 1) };
                template.ports.Add(new IO_port(true, new Map.MapPlacement(IntVector2.Zero, Dir4.W),
                    ResourceType.Chicken, 1));
                template.ports.Add(new IO_port(false, new Map.MapPlacement(IntVector2.Right, Dir4.N),
                    ResourceType.Feather, 2));
                template.ports.Add(new IO_port(false, new Map.MapPlacement(IntVector2.Right, Dir4.S),
                    ResourceType.Flesh, 1));
                ioUnits.Add(template);
            }
            {
                IOTemplate template = new IOTemplate() { name = "Duplicator", tilesize = new IntVector2(2, 1) };
                template.ports.Add(new IO_port(true, new Map.MapPlacement(IntVector2.Zero, Dir4.W),
                    ResourceType.Any, 1));
                template.ports.Add(new IO_port(false, new Map.MapPlacement(IntVector2.Right, Dir4.N),
                    ResourceType.Any, 1));
                template.ports.Add(new IO_port(false, new Map.MapPlacement(IntVector2.Right, Dir4.E),
                    ResourceType.Poop, 2));
                template.ports.Add(new IO_port(false, new Map.MapPlacement(IntVector2.Right, Dir4.S),
                    ResourceType.Any, 1));
                ioUnits.Add(template);
            }
            {
                IOTemplate template = new IOTemplate() { name = "Night eggs", tilesize = new IntVector2(2, 1) };
                template.ports.Add(new IO_port(true, new Map.MapPlacement(IntVector2.Zero, Dir4.N),
                    ResourceType.Magic_crystal, 1));
                template.ports.Add(new IO_port(true, new Map.MapPlacement(IntVector2.Zero, Dir4.W),
                    ResourceType.Void_cube, 2));
                template.ports.Add(new IO_port(true, new Map.MapPlacement(IntVector2.Zero, Dir4.S),
                    ResourceType.Feather, 1));

                template.ports.Add(new IO_port(false, new Map.MapPlacement(IntVector2.Right, Dir4.E),
                    ResourceType.Night_egg, 1));
                ioUnits.Add(template);
            }
            {
                IOTemplate template = new IOTemplate() { name = "Dragon eggs", tilesize = new IntVector2(2, 1) };
                template.ports.Add(new IO_port(true, new Map.MapPlacement(IntVector2.Zero, Dir4.N),
                    ResourceType.Fire_crystal, 2));
                template.ports.Add(new IO_port(true, new Map.MapPlacement(IntVector2.Zero, Dir4.W),
                    ResourceType.Grilled_meat, 1));
                template.ports.Add(new IO_port(true, new Map.MapPlacement(IntVector2.Zero, Dir4.S),
                    ResourceType.Feather, 1));

                template.ports.Add(new IO_port(false, new Map.MapPlacement(IntVector2.Right, Dir4.E),
                    ResourceType.Dragon_egg, 1));

                ioUnits.Add(template);
            }

            foreach (var io in ioUnits)
            {
                io.buildModel();
                io.buildId();
            }
        }

        virtual public void ToHud(RichBoxContent content)
        {
#if DEBUG
            content.text("Workers used: " + runStatistics.workersUsed.Count.ToString());
            content.text("Box deliver: " + runStatistics.itemDelivered[(int)ResourceType.Box].ToString());
            content.text("Night demon kills: " + runStatistics.killsBy[(int)FactoryObjectType.NightDemon].ToString());
            content.text("Dragon kills: " + runStatistics.killsBy[(int)FactoryObjectType.Dragon].ToString());
            content.text("Black knight kills: " + runStatistics.killsBy[(int)FactoryObjectType.BlackKnight].ToString());
            content.text("White knight deaths: " + runStatistics.destroyedCount[(int)FactoryObjectType.WhiteKnight].ToString());
            content.text("Poop stomp: " + runStatistics.poopStomps.ToString());
#endif
        }

        virtual public bool IsSandbox => false;

        protected SpriteName SuccessIcon(bool success)
        {
            return success ? HudLib.AvailableIcon : HudLib.NotAvailableIcon;
        }

        
    }

    class TutorialSetup : AbsMissionSetup
    {
        const int DeliverCount = 3;

        public TutorialSetup()
             :base()
        {
            missionName = "tutorial";

            componentList = new List<ToolSetupComponent>
            {
                new ToolSetupComponent( FactoryObjectType.Spin_plate),
                new ToolSetupComponent( FactoryObjectType.Belt),
                new ToolSetupComponent( FactoryObjectType.Table),
                new ToolSetupComponent( FactoryObjectType.Floor_drop),
            };

            ioUnits = new List<IOTemplate>(0);
        }
        public override void ToHud(RichBoxContent content)
        {
            bool boxSuccess = runStatistics.itemDelivered[(int)ResourceType.Box] >= DeliverCount;

            content.h1("Mission", Color.Yellow);
            content.icontext(SuccessIcon(boxSuccess), $"Deliver {DeliverCount} boxes");

            content.newParagraph();
            content.h1("Bonus objective", Color.Yellow);
            content.icontext(SuccessIcon(runStatistics.workersUsed.Count > 1), $"Use both workers");

            if (boxSuccess)
            {
                BlackRef.playScene.player.runExecuter.OnSuccess();
            }
        }
    }
    class WhiteKnightSetup : AbsMissionSetup
    {
        public WhiteKnightSetup()
             : base()
        {
            missionName = "whiteknight";

            componentList = new List<ToolSetupComponent>
            {
                new ToolSetupComponent( FactoryObjectType.Worker),
                new ToolSetupComponent( FactoryObjectType.Spin_plate),
                new ToolSetupComponent( FactoryObjectType.Belt),
                new ToolSetupComponent( FactoryObjectType.Table),
                new ToolSetupComponent( FactoryObjectType.Floor_drop),
                new ToolSetupComponent( FactoryObjectType.Stone_pillar),
            };

            ioUnits = new List<IOTemplate>(8);
            mission1units();
        }

        public override void ToHud(RichBoxContent content)
        {
            const int WhiteKnightKills = 10;

            bool missionSuccess = runStatistics.destroyedCount[(int)FactoryObjectType.WhiteKnight] >= WhiteKnightKills;
            bool nightDemonKill = runStatistics.killsBy[(int)FactoryObjectType.NightDemon] > 0;
            bool dragonKill = runStatistics.killsBy[(int)FactoryObjectType.Dragon] > 0;
            bool blackknightKill = runStatistics.killsBy[(int)FactoryObjectType.BlackKnight] > 0;
            bool noPoopSuccess = runStatistics.poopStomps <= 0;

            content.h1("Mission", Color.Yellow);
            content.icontext(SuccessIcon(missionSuccess), $"Kill {WhiteKnightKills} white knights");

            content.newParagraph();
            content.h1("Bonus objective", Color.Yellow);
            content.icontext(SuccessIcon(nightDemonKill), $"Get a Night Demon kill");
            content.icontext(SuccessIcon(dragonKill), $"Get a Dragon kill");
            content.icontext(SuccessIcon(blackknightKill), $"Get a Black Knight kill");
            content.icontext(SuccessIcon(noPoopSuccess), $"Don't step in shit");

            if (missionSuccess)
            {
                BlackRef.playScene.player.runExecuter.OnSuccess();
            }
        }
    }

    class SandboxSetup : AbsMissionSetup
    {
        public SandboxSetup()
            :base()
        {
            missionName = "sandbox";
            componentList = new List<ToolSetupComponent>((int)FactoryObjectType.NUM_NONE);

            for (FactoryObjectType fobj = 0; fobj < FactoryObjectType.NUM_NONE; fobj++)
            {
                if (FactoryObjectLib.Get(fobj).debugLevel > ObjectDebugLevel.Incomplete)
                {
                    componentList.Add(new ToolSetupComponent(fobj));
                }
            }

            ioUnits = new List<IOTemplate>(8);
            mission1units();

            
        }
        public override bool IsSandbox => true;
    }

    enum MissionType
    { 
        Sandbox,
        Tutorial,
        WhiteKnight,
    }
}
