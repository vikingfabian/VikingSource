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
    //class CityDetailProfile : AbsDetailUnitProfile
    //{
    //    public const float ShortRangeAttack = 1.5f;
    //    public const float LongRangeAttack = 3.01f;
    //    public CityDetailProfile()
    //    {
    //        mainAttack = AttackType.Arrow;
    //        secondaryAttack = AttackType.Ballista;
    //        attackDamage = DssConst.Soldier_DefaultHealth;
    //        attackDamageSea = attackDamage;
    //        secondaryAttackDamage = attackDamage / 2;
    //        attackRange = LongRangeAttack;
    //        targetSpotRange = StandardTargetSpotRange;
    //    }

    //    public override AbsDetailUnit CreateUnit()
    //    {
    //        throw new NotImplementedException();
    //    }
    //}

    class CityDetail : AbsDetailUnit
    {
        public const float ShortRangeAttack = 1.5f;
        public const float LongRangeAttack = 3.01f;
        public const int WorkersPerHut = 30;
        const int WorkerHutsPerTile = 4;
        const int WorkerHutsPerTile_MaxLevel = WorkerHutsPerTile* HutMaxLevel;
        public const int WorkersPerTile = WorkersPerHut * WorkerHutsPerTile * HutMaxLevel;
        public const int HutMaxLevel = 2;
        City city;
        const int GuardMaxHealth = 160;
        int guardHealth = GuardMaxHealth;
        float nextRespawn = 0;
        //CityDetailData data;
        public SoldierGroup inBattle = null;

        //int workerModelsActiveCount = 0;
        //List<WorkerData> workers= new List<WorkerData>();
        int totalWorkerHutAndLevelCount = 0;
        //public WorkersModels workersModels = null;
        int storedAttacks = 0;
        public CityDetail(City city, bool newGame)
        {
            //data = new CityDetailProfile();
            this.city = city;
            position = city.position;
            
            tilePos = city.tilePos;
            //this.bound = new Physics.RectangleBound(city.WorldPositionXZ(), new Vector2(0.5f));

            health = 10000;
            radius = 0.7f;

            soldierData.mainAttack = AttackType.Arrow;
            soldierData.secondaryAttack = AttackType.Ballista;
            soldierData.attackDamage = DssConst.Soldier_DefaultHealth;
            soldierData.attackDamageSea = soldierData.attackDamage;
            soldierData.secondaryAttackDamage = soldierData.attackDamage / 2;
            soldierData.attackRange = LongRangeAttack;
            //targetSpotRange = StandardTargetSpotRange;

            if (newGame)
            {
                refreshWorkerSubtiles();
            }
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
                            
                            if (DssRef.world.tileGrid.TryGet(edgeLoop.Position, out Tile t) &&
                                    t.IsLand() && t.CityIndex == city.parentArrayIndex)
                            {
                                const int SubStartTrialCount = 4;
                                IntVector2 topLeft = WP.ToSubTilePos_TopLeft(edgeLoop.Position);

                                for (int trialIx = 0; trialIx < SubStartTrialCount; ++trialIx)
                                {
                                    IntVector2 subPos = topLeft;
                                    subPos.X += Ref.rnd.Int(1, WorldData.TileSubDivitions -1);
                                    subPos.Y += Ref.rnd.Int(1, WorldData.TileSubDivitions -1);


                                    if (Build.BuildLib.TryAutoBuild(subPos, TerrainMainType.Building, (int)TerrainBuildingType.WorkerHut, 1))
                                    {
                                        ++totalWorkerHutAndLevelCount;

                                        //Place farm curlutures
                                        const int CulturesPerFarm = 8;
                                        int cultureCount = 0;

                                        ForXYEdgeLoop farmLoop = new ForXYEdgeLoop(Rectangle2.FromCenterTileAndRadius(subPos, 1));
                                        farmLoop.RandomPosition(true);
                                        

                                        while (cultureCount < CulturesPerFarm)
                                        {
                                            while (farmLoop.Next())
                                            {
                                                TerrainMainType terrain;
                                                int sub;
                                                int maxAmount;
                                                if (Ref.rnd.Chance(0.75))
                                                {
                                                    terrain = TerrainMainType.Foil;
                                                    sub = (int)TerrainSubFoilType.WheatFarm;
                                                    maxAmount = TerrainContent.FarmCulture_MaxSize;
                                                }
                                                else
                                                {
                                                    terrain = TerrainMainType.Building;
                                                    if (Ref.rnd.Chance(0.4))
                                                    {
                                                        sub = (int)TerrainBuildingType.PigPen;
                                                        maxAmount = TerrainContent.PigMaxSize;
                                                    }
                                                    else
                                                    {
                                                        sub = (int)TerrainBuildingType.HenPen;
                                                        maxAmount = TerrainContent.HenMaxSize;
                                                    }
                                                }
                                                
                                                if (Build.BuildLib.TryAutoBuild(farmLoop.Position, terrain, sub, Ref.rnd.Int(1, maxAmount)))
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

        static int WorkersToModelsCount(int workers)
        {
            return (int)Math.Floor(workers / (double)WorkersPerHut);
        }

        public void onNewOwner()
        {
            if (model != null)
            {
                model.DeleteMe();
                model = initModel();
            }
        }

        public override void takeDamage(int damageAmount, AbsDetailUnit meleeAttacker, Rotation1D attackDir, Faction damageFaction, bool fullUpdate)
        {
            //base.takeDamage(damage);
            int totalDamage = Bound.Min(damageAmount / 2, 1);
            guardHealth -= totalDamage;

            if (guardHealth <= 0)
            {
                guardHealth = GuardMaxHealth;
                city.guardCount--;
                city.damages.add(1.75, city.MaxDamages());

                if (city.guardCount <= 0)
                {
                    city.guardCount = 0;
                }
            }
        }

        public void oneSecondUpdate()
        {
            nextRespawn += 1f;

            const int respawnTime = 5;
            //float respawnTime = 10f / (city.maxGuardSize / DssConst.SoldierGroup_DefaultCount);
            if (nextRespawn >= respawnTime)
            {
                nextRespawn = 0;
                city.respawnGuard();
            }
        }

        float nextArrowCooldown;

        public override void update(float time, bool fullUpdate)
        {
            if (city.debugTagged)
            {
                lib.DoNothing();
            }

            if (nextArrowCooldown > 0)//IsAttacking)
            {
                //updateAttack(time);
                nextArrowCooldown -= time;
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
                    if (dist <= DssRef.profile.city.data.attackRange)
                    {
                        bool shortDist = dist < CityDetailProfile.ShortRangeAttack;
                        nextArrowCooldown = 50000f / guards;

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
        public void asynchFindBattleTarget(List<GameObject.SoldierGroup> groups)
        {
            //if (city.debugTagged)
            //{
            //    lib.DoNothing();
            //}
            AbsDetailUnit closestOpponent = null;
            float closestOpponentDistance = float.MaxValue;

            //DssRef.world.unitCollAreaGrid.collectOpponentGroups(GetFaction(), tilePos, out List<GameObject.SoldierGroup> groups, out List<City> cities);

            foreach (var m in groups)
            {
                var soldiers_SP = m.soldiers;
                if (soldiers_SP != null)
                {
                    var soldiersC = soldiers_SP.counter();
                    while (soldiersC.Next())
                    {
                        if (soldiersC.sel.Alive_IncomingDamageIncluded())
                        {
                            //float distance = spaceBetweenUnits(soldiers.sel);

                            closestTargetCheck(soldiersC.sel, //distance,
                                ref closestOpponent, ref closestOpponentDistance);
                        }
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

        public override bool aliveAndBelongTo(int faction)
        {
            return this.GetFaction().parentArrayIndex == faction;
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

        public override AbsDetailUnitProfile Profile()
        {
            return DssRef.profile.city;
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

    //class WorkerData
    //{
    //    public IntVector2 tile;
    //    public int tilePlacementIndex;
    //    public int level = 1;
    //    //public bool inUse;
    //}

    //class WorkersModels
    //{
    //    const float Scale = 0.14f;
    //    const float RndOffset = Scale;


    //    static readonly Vector2[] tilePlacements = {
    //        new Vector2(-0.25f,-0.25f),
    //        new Vector2( 0.25f,-0.25f),
    //        new Vector2(-0.25f,0.25f),
    //        new Vector2(0.25f,0.25f),
    //    };
    //    List< Graphics.AbsVoxelObj> modelList = new List<AbsVoxelObj>(64);

    //    public void Refresh(City city, List<WorkerData> workers, PcgRandom rnd) 
    //    {
    //        while (modelList.Count < workers.Count) 
    //        {
    //            var model = DssRef.models.ModelInstance(VoxelModelName.war_workerhut, Scale, false);
    //            model.AddToRender(DrawGame.UnitDetailLayer);
    //            model.position = VectorExt.AddXZ(WP.ToMapPos(workers[modelList.Count].tile), tilePlacements[workers[modelList.Count].tilePlacementIndex]);
    //            model.position.X += rnd.Plus_MinusF(RndOffset);
    //            model.position.Z += rnd.Plus_MinusF(RndOffset);

    //            model.position.Y = DssRef.world.SubTileHeight(model.position);

    //            WP.Rotation1DToQuaterion(model, rnd.Float(0.17f));

    //            modelList.Add(model);
    //        }

    //        for (int i = 0; i < workers.Count; i++)
    //        {
    //            int frame = city.workHutStyle;
    //            if (workers[i].level > 1)
    //            {
    //                frame += 2;
    //            }
    //            modelList[i].Frame = frame;
    //        }
    //    }

    //    public void DeleteMe()
    //    {
    //        foreach (var m in modelList)
    //        {
    //            m.DeleteMe();
    //        }
    //    }
    //}
}
