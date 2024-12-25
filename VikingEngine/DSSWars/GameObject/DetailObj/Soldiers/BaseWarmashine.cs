using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using VikingEngine.DSSWars.GameObject.DetailObj.Warmashines;
using VikingEngine.Graphics;
using VikingEngine.LootFest;
using VikingEngine.ToGG.MoonFall.GO;

namespace VikingEngine.DSSWars.GameObject
{
    

    class WarmashineProfile : ConscriptedSoldierProfile
    {
        public const float BallistaRange = 3;
        public WarmashineProfile() 
            :base()
        {
            unitType = UnitType.ConscriptWarmashine;
            
            boundRadius = DssVar.StandardBoundRadius * 2.2f;

            //walkingSpeed = DssConst.Men_StandardWalkingSpeed * 0.6f;
            //ArmySpeedBonusLand = -0.5;
            rotationSpeed = StandardRotatingSpeed * 0.04f;
            
            targetSpotRange = BallistaRange + StandardTargetSpotRange;
            
            maxAttackAngle = 0.07f;
            goldCost = MathExt.MultiplyInt(0.9, DssLib.GroupDefaultCost);
            

            hasBannerMan = false;
            
            description = DssRef.lang.UnitType_Description_Ballista;

            
        }
        public override AbsSoldierUnit CreateUnit()
        {
            
            return new BaseWarmashine();
        }
    }

    class BaseWarmashine : BaseSoldier
    {
        public BaseWarmashine()
            : base()
        { }

        protected override DetailUnitModel initModel()
        {
            updateGroudY(true);
            switch (group.soldierConscript.conscript.weapon)
            {
                default: return new CannonModel(this);

                case Resource.ItemResourceType.Ballista:
                    return new DetailObj.Warmashines.BallistaModel(this);
                case Resource.ItemResourceType.Manuballista:
                    return new DetailObj.Warmashines.ManuballistaModel(this);
                case Resource.ItemResourceType.Catapult:
                    return new DetailObj.Warmashines.CatapultModel(this);
                case Resource.ItemResourceType.SiegeCannonBronze:
                    return new DetailObj.Warmashines.SiegeCannonBronzeModel(this);
                case Resource.ItemResourceType.SiegeCannonIron:
                    return new DetailObj.Warmashines.HaubitzModel(this);

                    //default: throw new NotImplementedException();
            }

        }
    }

   

    class WarmashineWorkerCollection
    {
        List<WarmashineWorker> members = new List<WarmashineWorker>(2);

        public void Add(Faction player, float xDiff, float zDiff)
        {
            members.Add(new WarmashineWorker(player, new Vector3(xDiff, 0, zDiff)));
        }

        public void update(AbsSoldierUnit parent)
        {
            foreach (var m in members)
            {
                m.update(parent);
            }
        }

        public void onNewModel_asynch(LootFest.VoxelModelName name, Graphics.VoxelModel master)
        {
            if (name == DssLib.WorkerModel)
            {
                foreach (var m in members)
                {
                    m.onNewModel_asynch(name, master);
                }
            }
        }

        public void DeleteMe()
        {
            foreach (var m in members)
            {
                m.DeleteMe();
            }
        }
    }

    class WarmashineWorker
    {
        WalkingAnimation walkingAnimation = WalkingAnimation.WorkerWalking;
        Graphics.AbsVoxelObj model;
        Vector3 diff;

        public WarmashineWorker(Faction faction, Vector3 diff)
        {
            this.diff = diff;
            
            model = faction.AutoLoadModelInstance(
               DssLib.WorkerModel, DssConst.Men_StandardModelScale * 0.9f, true);
        }

        public void onNewModel_asynch(LootFest.VoxelModelName name, Graphics.VoxelModel master)
        {
            DSSWars.Faction.SetNewMaster(name, DssLib.WorkerModel, model, master);
        }

        public void update(AbsSoldierUnit parent)
        {
            if (parent.state.walking || parent.state.rotating)
            {
                float move = parent.walkingSpeedWithModifiers(Ref.DeltaTimeMs);
                walkingAnimation.update(move, model);
            }
            else
            {
                model.Frame = 0;
            }
            var pModel = parent.model;
            if (pModel != null)
            {
                model.Rotation = pModel.model.Rotation;
                model.position = pModel.model.Rotation.TranslateAlongAxis(diff, parent.position);
            }
        }

        public void DeleteMe()
        {
            model.DeleteMe();
        }
    }
}
