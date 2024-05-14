using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.DSSWars.Data;
using VikingEngine.DSSWars.Map;
using VikingEngine.EngineSpace;
using VikingEngine.Graphics;
using VikingEngine.HUD.RichBox;
using VikingEngine.LootFest;
using VikingEngine.ToGG;
using VikingEngine.ToGG.HeroQuest.Data.Condition;

namespace VikingEngine.DSSWars.GameObject
{
    class CityDetailData : AbsDetailUnitData
    {
        public const float ShortRangeAttack = 1.5f;
        public const float LongRangeAttack = 3.01f;
        public CityDetailData()
        {
            mainAttack = AttackType.Arrow;
            secondaryAttack = AttackType.Ballista;
            attackDamage = AbsSoldierData.DefaultHealth;
            attackDamageSea = attackDamage;
            secondaryAttackDamage = attackDamage / 2;
            attackRange = LongRangeAttack;//1f;
            targetSpotRange = StandardTargetSpotRange;
        }

        public override AbsDetailUnit CreateUnit()
        {
            throw new NotImplementedException();
        }
    }

    class CityDetail : AbsDetailUnit
    {
        const int WorkersPerHut = 30;
        const int WorkerHutsPerTile = 4;
        const int WorkerHutsPerTile_MaxLevel = WorkerHutsPerTile* HutMaxLevel;
        public const int WorkersPerTile = WorkersPerHut * WorkerHutsPerTile * HutMaxLevel;
        public const int HutMaxLevel = 2;
        City city;
        const int GuardMaxHealth = 80;
        int guardHealth = GuardMaxHealth;
        float nextRespawn = 0;
        CityDetailData data;
        public SoldierGroup inBattle = null;

        //int workerModelsActiveCount = 0;
        //List<WorkerData> workers= new List<WorkerData>();
        int totalWorkerHutAndLevelCount = 0;
        //public WorkersModels workersModels = null;
        int storedAttacks = 0;
        public CityDetail(City city)
        {
            data = new CityDetailData();
            this.city = city;
            position = city.position;
            
            tilePos = city.tilePos;
            
            health = 10000;
            radius = 0.7f;

            refreshWorkerSubtiles();
            //attack = new AttackAnimation(this);
            //onNewOwner();
        }

        public void writeGameState(System.IO.BinaryWriter w)
        {
            
        }
        public void readGameState(System.IO.BinaryReader r, int subversion, ObjectPointerCollection pointers)
        {
            
        }

        protected override DetailUnitModel initModel()
        {
            return new CityModel(city);
        }

        public void refreshModel()
        {
            if (model != null)
            {
                model.DeleteMe();
                model = initModel();
            }
        }

        public void setDetailLevel(bool inRender)
        {
            if (inRender)
            {
                if (model == null)
                {
                    model = initModel();

                    //updateWorkerModels();
                }
            }  
            else
            {
                if (model != null)
                {
                    model.DeleteMe();
                    model = null;

                    //workersModels.DeleteMe();
                    //workersModels = null;
                }
            }
        }

        public void refreshWorkerSubtiles()
        {
            int goalDisplayCount = WorkersToModelsCount(city.workForceMax);
            if (goalDisplayCount > totalWorkerHutAndLevelCount)
            {
                Task.Factory.StartNew(() =>
                {
                    ForXYEdgeLoop edgeLoop = new ForXYEdgeLoop(Rectangle2.FromCenterTileAndRadius(city.tilePos, 1));
                    edgeLoop.RandomPosition(true);

                    int maxLoops = 10000;

                    while (goalDisplayCount > totalWorkerHutAndLevelCount)
                    {
                        if (edgeLoop.Next())
                        {
                            
                            Tile t;
                            if (DssRef.world.tileGrid.TryGet(edgeLoop.Position, out t) &&
                                    t.IsLand() && t.CityIndex == city.parentArrayIndex)
                            {
                                const int SubStartTrialCount = 4;
                                IntVector2 topLeft = WP.ToSubTilePos_TopLeft(edgeLoop.Position);

                                for (int trialIx = 0; trialIx < SubStartTrialCount; ++trialIx)
                                {
                                    IntVector2 subPos = topLeft;
                                    subPos.X += Ref.rnd.Int(WorldData.TileSubDivitions);
                                    subPos.Y += Ref.rnd.Int(WorldData.TileSubDivitions);


                                    if (Build.BuildLib.TryAutoBuild(subPos, TerrainMainType.Building, (int)TerrainBuildingType.WorkerHut))
                                    {
                                        ++totalWorkerHutAndLevelCount;

                                        //Place farm curlutures
                                        const int CulturesPerFarm = 2;
                                        int cultureCount = 0;

                                        ForXYEdgeLoop farmLoop = new ForXYEdgeLoop(Rectangle2.FromCenterTileAndRadius(subPos, 1));
                                        farmLoop.RandomPosition(true);

                                        while (cultureCount < CulturesPerFarm)
                                        {
                                            while (farmLoop.Next())
                                            {
                                                if (Build.BuildLib.TryAutoBuild(farmLoop.Position, TerrainMainType.Foil, (int)TerrainSubFoilType.FarmCulture))
                                                {
                                                    ++cultureCount;
                                                    if (cultureCount >= CulturesPerFarm)
                                                    {
                                                        break;
                                                    }
                                                }
                                            }

                                            farmLoop.ExpandRadius();
                                            farmLoop.RandomPosition(true);

                                            if (--maxLoops < 0)
                                            {
                                                return;
                                            }
                                        }

                                        if (goalDisplayCount <= totalWorkerHutAndLevelCount)
                                        {
                                            return;
                                        }
                                    }
                                }
                            }

                        }
                        else
                        {
                            edgeLoop.ExpandRadius();
                            edgeLoop.RandomPosition(true);
                        }


                        if (--maxLoops < 0)
                        {   
                            return;
                        }
                    }
                });



            }
        }

        //public void updateWorkerModels()
        //{
        //    if (model != null)
        //    {
        //        int goalDisplayCount = WorkersToModelsCount(city.workForce.max);
        //        PcgRandom rnd = new PcgRandom(DssRef.world.TileSeed(city.tilePos.X, city.tilePos.Y));

        //        if (goalDisplayCount > totalWorkerHutAndLevelCount)
        //        {
        //            //find available tile
                    
        //            ForXYEdgeLoop edgeLoop = new ForXYEdgeLoop(Rectangle2.FromCenterTileAndRadius(city.tilePos, 1));
        //            edgeLoop.Next();
        //            //edgeLoop.RandomPosition(rnd);

        //            while (goalDisplayCount > totalWorkerHutAndLevelCount)
        //            {
        //                Tile t;
        //                if (DssRef.world.tileGrid.TryGet(edgeLoop.Position, out t) &&
        //                        t.IsLand() && t.CityIndex == city.index &&
        //                    (
        //                        t.tileContent == TileContent.NONE || 
        //                        (t.tileContent == TileContent.WorkerHut && t.WorkerCount < WorkerHutsPerTile_MaxLevel))
        //                    )
        //                {
        //                    if (t.WorkerCount < WorkerHutsPerTile)
        //                    {
        //                        workers.Add(new WorkerData()
        //                        {
        //                            tile = edgeLoop.Position,
        //                            tilePlacementIndex = t.WorkerCount,
        //                            //inUse = false,
        //                        });
        //                    }
        //                    else
        //                    {
        //                        for (int i = workers.Count - 1; i >= 0; i--)
        //                        {
        //                            if (workers[i].tile == edgeLoop.Position && workers[i].level==1)
        //                            {
        //                                ++workers[i].level;
        //                                break;
        //                            }
        //                        }
        //                    }

        //                    t.tileContent = TileContent.WorkerHut;
        //                    ++t.WorkerCount;
        //                    ++totalWorkerHutAndLevelCount;
        //                }
        //                else
        //                {
        //                    bool inLoop = edgeLoop.Next();
        //                    if (!inLoop)
        //                    {
        //                        edgeLoop.ExpandRadius();
        //                    }
        //                }
        //            }
        //        }

        //        //if (workersModels == null)
        //        //{
        //        //    workersModels = new WorkersModels();
        //        //}
        //        workersModels.Refresh(city, workers, rnd);
        //    }
        //}

       
        static int WorkersToModelsCount(int workers)
        {
            return (int)Math.Floor(workers / (double)WorkersPerHut);
        }

        public void onNewOwner()
        {
            //if (bannerModel != null)
            //{
            //    bannerModel.DeleteMe();
            //}

            //bannerModel = city.faction.AutoLoadModelInstance(
            //   LootFest.VoxelModelName.citybanner, 0.8f);
            //bannerModel.AddToRender(DrawGame.UnitDetailLayer);
            //bannerModel.position = city.WorldPosition();

            if (model != null)
            {
                model.DeleteMe();
                model = initModel();
            }
        }

        public override void takeDamage(int damageAmount, Rotation1D attackDir, Faction damageFaction, bool fullUpdate)
        {
            //base.takeDamage(damage);
            int totalDamage = Bound.Min(damageAmount / 2, 1);
            guardHealth -= totalDamage;

            if (guardHealth <= 0)
            {
                guardHealth = GuardMaxHealth;
                city.guardCount--;
                city.damages.add(1.75, city.workForce.value * 0.75);

                if (city.guardCount <= 0)
                {
                    city.guardCount = 0;
                }
            }
        }

        public void oneSecondUpdate()
        {
            nextRespawn += 1f;

            float respawnTime = 10f / (city.maxGuardSize / AbsSoldierData.GroupDefaultCount);
            if (nextRespawn >= respawnTime)
            {
                nextRespawn -= respawnTime;
                city.respawnGuard();
            }
        }

        public override void update(float time, bool fullUpdate)
        {
            if (city.debugTagged)
            {
                lib.DoNothing();
            }

            if (IsAttacking)
            {   
                updateAttack(time);
            }
            else
            {
                if (city.debugTagged)
                {
                    lib.DoNothing();
                }

                SoldierGroup attackingGroup = null;
                
                refreshAttackTarget();

                int guards = city.guardCount;
                var attackTarget_sp = attackTarget;
                if (attackTarget_sp != null && guards > 0)
                {
                    float dist = spaceBetweenUnits(attackTarget_sp);
                    if (dist <= data.attackRange)
                    {
                        bool shortDist = dist < CityDetailData.ShortRangeAttack;
                        data.attackTimePlusCoolDown = 50000f / guards;

                        if (shortDist)
                        {
                            int attacks = Ref.rnd.Int(5, 10) + Bound.Max(storedAttacks, 5);

                            int hits = startMultiAttack(fullUpdate, attackTarget_sp, shortDist, attacks, true);
                            storedAttacks = attacks - hits;
                        }
                        else
                        {
                            startAttack(fullUpdate, attackTarget_sp, shortDist, true);
                        }
                        attackingGroup = attackTarget_sp.group;

                        attackTarget = null;
                    }
                    
                }
                inBattle = attackingGroup;
            }
        }

        public override Vector3 projectileStartPos()
        {
            Vector3 pos = position;
            pos.X += Ref.rnd.Plus_MinusF(0.3f);
            pos.Z += Ref.rnd.Plus_MinusF(0.3f);

            return pos;

        }
        public void asynchFindBattleTarget()
        {
            AbsDetailUnit closestOpponent = null;
            float closestOpponentDistance = float.MaxValue;

            var opponentGroups = DssRef.world.unitCollAreaGrid.collectOpponentGroups(GetFaction(), tilePos);

            foreach (var m in opponentGroups)
            {
                var soldiers = m.soldiers.counter();
                while (soldiers.Next())
                {
                    if (soldiers.sel.Alive_IncomingDamageIncluded())
                    {
                        //float distance = spaceBetweenUnits(soldiers.sel);

                        closestTargetCheck(soldiers.sel, //distance,
                            ref closestOpponent, ref closestOpponentDistance);
                    }
                }
            }

            this.nextAttackTarget = closestOpponent;
            refreshAttackTarget();
        }

        public override void asynchUpdate()
        {
            //asynchUpdate_closestTarget();
        }

        public override bool defeatedBy(Faction attacker)
        {
            return this.GetFaction() == attacker;
        }

        public override bool aliveAndBelongTo(Faction faction)
        {
            return this.GetFaction() == faction;
        }

        public override bool IsShipType()
        {
            return false;
        }

        public override bool IsStructure() 
        { return true; }

        public override bool IsSingleTarget()
        {
            return true;
        }

        public override bool IsSoldierUnit()
        {
            return false;
        }

        override public Faction GetFaction()
        {
            return city.faction;
        }

        public override AbsMapObject RelatedMapObject()
        {
            return city;
        }

        protected override AbsMapObject ParentMapObject()
        {
            return city;
        }

        public override string ToString()
        {
            return city.TypeName() + " -detail obj";
        }

        public override AbsDetailUnitData Data()
        {
            return data;
        }

        public override GameObjectType gameobjectType()
        {
            return GameObjectType.City;
        }

        public override UnitType DetailUnitType() { return UnitType.City; }
    }

    class CityModel : DetailUnitModel
    {
        const VoxelModelName BannerModelName = VoxelModelName.citybanner;
        Graphics.AbsVoxelObj bannerModel;

        public CityModel(City city)
            : base()
        {
            VoxelModelName detailmodelName;

            switch (city.CityType)
            {
                case CityType.Small:
                    detailmodelName = VoxelModelName.war_town1;
                    break;
                case CityType.Large:
                    detailmodelName = VoxelModelName.war_town2;
                    break;
                default:
                    detailmodelName = VoxelModelName.war_town3;
                    break;
                case CityType.Factory:
                    detailmodelName = VoxelModelName.war_town_factory;
                    break;
            }

            this.bound = new Physics.RectangleBound(city.WorldPositionXZ(), new Vector2(0.5f));

            //model = DssRef.models.ModelInstance(detailmodelName, 1f, false);
            //model.AddToRender(DrawGame.UnitDetailLayer);
            //model.position = city.position;

            bannerModel = city.faction.AutoLoadModelInstance(
               BannerModelName, 0.6f);
            bannerModel.AddToRender(DrawGame.UnitDetailLayer);
            bannerModel.position = city.position;
        }

        public override void onNewModel(VoxelModelName name, VoxelModel master, AbsDetailUnit unit)
        {
            DSSWars.Faction.SetNewMaster(name, BannerModelName, bannerModel, master);
        }

        //protected override void updateAnimation(AbsSoldier soldier)
        //{
        //}

        public override void DeleteMe()
        {
            //base.DeleteMe();
            bannerModel.DeleteMe();
        }
    }

    class WorkerData
    {
        public IntVector2 tile;
        public int tilePlacementIndex;
        public int level = 1;
        //public bool inUse;
    }

    class WorkersModels
    {
        const float Scale = 0.14f;
        const float RndOffset = Scale;


        static readonly Vector2[] tilePlacements = {
            new Vector2(-0.25f,-0.25f),
            new Vector2( 0.25f,-0.25f),
            new Vector2(-0.25f,0.25f),
            new Vector2(0.25f,0.25f),
        };
        List< Graphics.AbsVoxelObj> modelList = new List<AbsVoxelObj>(64);

        public void Refresh(City city, List<WorkerData> workers, PcgRandom rnd) 
        {
            while (modelList.Count < workers.Count) 
            {
                var model = DssRef.models.ModelInstance(VoxelModelName.war_workerhut, Scale, false);
                model.AddToRender(DrawGame.UnitDetailLayer);
                model.position = VectorExt.AddXZ(WP.ToMapPos(workers[modelList.Count].tile), tilePlacements[workers[modelList.Count].tilePlacementIndex]);
                model.position.X += rnd.Plus_MinusF(RndOffset);
                model.position.Z += rnd.Plus_MinusF(RndOffset);

                model.position.Y = DssRef.world.SubTileHeight(model.position);

                WP.Rotation1DToQuaterion(model, rnd.Float(0.17f));

                modelList.Add(model);
            }

            for (int i = 0; i < workers.Count; i++)
            {
                int frame = city.workHutStyle;
                if (workers[i].level > 1)
                {
                    frame += 2;
                }
                modelList[i].Frame = frame;
            }
        }

        public void DeleteMe()
        {
            foreach (var m in modelList)
            {
                m.DeleteMe();
            }
        }
    }
}
