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
    class BannerManProfile : ConscriptedSoldierProfile
    {
        public BannerManProfile():base() 
        {
            unitType = UnitType.BannerMan;
        }
        public override AbsSoldierUnit CreateUnit()
        {
            return new BannerMan();
        }
    }

    //class BannerManData : AbsSoldierProfile
    //{
    //    public BannerManData()
    //    {
    //        unitType = UnitType.BannerMan;

    //        modelScale =DssConst.Men_StandardModelScale * 1f;
    //        boundRadius = DssVar.StandardBoundRadius;

    //        walkingSpeed = DssConst.Men_StandardWalkingSpeed;
    //        rotationSpeed = StandardRotatingSpeed;

    //        //basehealth = 50;
    //        canAttackCharacters = false;

    //        data.modelName = LootFest.VoxelModelName.war_bannerman;
    //    }

    //    public override AbsDetailUnit CreateUnit()
    //    {
    //        return new BannerMan();
    //    }
    //}

    class BannerMan : BaseSoldier
    {        
        //banner flag coord: 1, 44, 2 (Y är vänt)
        public BannerMan()
            : base()
        { }

        protected override DetailUnitModel initModel()
        {
            updateGroudY(true);
            return new BannerManModel(this);
        }
    }

    class BannerManModel : SoldierUnitAdvancedModel
    {
        Banner banner;

        public BannerManModel(AbsSoldierUnit soldier)
            : base(soldier)
        {
            banner = new Banner(soldier.GetFaction(), soldier.Profile().modelScale, (int)soldier.group.soldierConscript.conscript.training);
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
        public Banner(Faction faction, float soldierScale, int skill)
        {
            model = faction.AutoLoadModelInstance(
               modelName(), soldierScale * 2f, true);
            model.Frame = skill;
            diff = new Vector3(0.17f, 0, 0.12f) * soldierScale;
        }

        protected override VoxelModelName modelName()
        {
            return VoxelModelName.banner;
        }
    }

    
}
