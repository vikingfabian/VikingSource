using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.Graphics;
using VikingEngine.LootFest;

namespace VikingEngine.DSSWars.GameObject
{
    class BannerManData : AbsSoldierData
    {
        public BannerManData()
        {
            unitType = UnitType.BannerMan;

            modelScale = StandardModelScale * 1f;
            boundRadius = StandardBoundRadius;

            walkingSpeed = StandardWalkingSpeed;
            rotationSpeed = StandardRotatingSpeed;

            basehealth = 50;
            canAttackCharacters = false;

            modelName = LootFest.VoxelModelName.war_bannerman;
        }

        public override AbsDetailUnit CreateUnit()
        {
            return new BannerMan();
        }
    }

    class BannerMan : BaseSoldier
    {        
        //banner flag coord: 1, 44, 2 (Y är vänt)
        public BannerMan()
            : base()
        { }

        protected override DetailUnitModel initModel()
        {
            return new BannerManModel(this);
        }
    }

    class BannerManModel : SoldierUnitAdvancedModel
    {
        Banner banner;

        public BannerManModel(AbsSoldierUnit soldier)
            : base(soldier)
        {
            banner = new Banner(soldier.GetFaction(), soldier.data.modelScale);
        }

        //protected override void updateShipAnimation(AbsSoldierUnit soldier)
        //{
        //    base.updateShipAnimation(soldier);
        //    banner.update(soldier);
        //}
        protected override void updateAnimation(AbsSoldierUnit soldier)
        {
            base.updateAnimation(soldier);
            banner.update(soldier);
        }

        public override void DeleteMe()
        {
            base.DeleteMe();
            banner.DeleteMe();
        }

        public override void onNewModel(VoxelModelName name, VoxelModel master, AbsDetailUnit unit)
        {
            base.onNewModel(name, master, unit);
            banner.onNewModel_asynch(name, master);
        }
    }

    class Banner : AbsModelAttachment
    {
        public Banner(Faction faction, float soldierScale)
        {
            model = faction.AutoLoadModelInstance(
               modelName(), soldierScale * 2f, true);
            diff = new Vector3(0.17f, 0, 0.12f) * soldierScale;
        }

        protected override VoxelModelName modelName()
        {
            return VoxelModelName.banner;
        }
    }

    
}
