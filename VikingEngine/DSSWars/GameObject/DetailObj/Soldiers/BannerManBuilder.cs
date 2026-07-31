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
    class BannerManBuilder : ConscriptedSoldierBuilder
    {
        public BannerManBuilder():base() 
        {
            unitBuildType = UnitBuildType.BannerMan;
        }
        public override AbsSoldierUnit CreateUnit(bool bannerman)
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

        protected override DetailUnitModel initModel(bool bannerman)
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
            banner = new Banner(soldier.pfaction.GetFaction(), soldier.soldierData.modelScale, (int)soldier.group.soldierConscript.conscript.training);
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
            banner?.DeleteMe();
            banner = null;
        }

        public override void onNewModel(VoxelModelName name, VoxelModel master, AbsSoldierUnit unit)
        {
            base.onNewModel(name, master, unit);
            banner.onNewModel_asynch(name, master);
        }
    }

    class Banner : AbsModelAttachment_Batched
    {
        public Banner(Faction faction, float soldierScale, int skill)
        {
            model = faction.AutoLoadModelInstance_batched(
               modelName(), soldierScale * 2f / 1.76f);
            model.Frame = skill;
            diff = new Vector3(0.17f, 0, 0.12f) * soldierScale / 1.76f;
        }
        protected override VoxelModelName modelName()
        {
            return VoxelModelName.banner;
        }

        
    }

    
}
