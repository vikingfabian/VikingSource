using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.DSSWars.GameObject
{
    class ShipUnitAdvancedModel : AbsDetailUnitAdvancedModel
    {
        Graphics.AbsVoxelObj captain, leftcrew, rightcrew;
        Vector3 captainPosDiff, leftCrewPosDiff, rightCrewPosDiff;
        public ShipUnitAdvancedModel()
        { }

        public ShipUnitAdvancedModel(AbsSoldierUnit soldier)
            : base(soldier)
        {
            captainPosDiff = soldier.data.modelScale * soldier.data.captainPosDiff;//new Vector3(-0.05f, 0.18f, -.27f);

            leftCrewPosDiff = soldier.data.modelScale * soldier.data.leftCrewPosDiff;//new Vector3(-0.076f, 0.12f, 0.065f);
            rightCrewPosDiff = leftCrewPosDiff;
            rightCrewPosDiff.X = -rightCrewPosDiff.X;

            float crewScale = DssConst.Men_StandardModelScale * 1.6f;

            captain = soldier.group.army.faction.AutoLoadModelInstance(
                LootFest.VoxelModelName.wars_captain, DssConst.Men_StandardModelScale * 0.7f, true);

            leftcrew = soldier.group.army.faction.AutoLoadModelInstance(
                LootFest.VoxelModelName.wars_shipcrew, crewScale, true);

            rightcrew = soldier.group.army.faction.AutoLoadModelInstance(
                LootFest.VoxelModelName.wars_shipcrew, crewScale, true);
        }

        public override void update(AbsSoldierUnit soldier)
        {
            base.update(soldier);
            model.position.Y -= 0.02f;
            WP.Rotation1DToQuaterion(model, soldier.rotation.Radians);

            captain.Rotation = model.Rotation;
            captain.position = model.Rotation.TranslateAlongAxis(
                captainPosDiff, model.position);

            leftcrew.Rotation = model.Rotation;
            leftcrew.position = model.Rotation.TranslateAlongAxis(
                leftCrewPosDiff, model.position);

            rightcrew.Rotation = model.Rotation;
            rightcrew.position = model.Rotation.TranslateAlongAxis(
                rightCrewPosDiff, model.position);

        }

        public override void displayHealth(float percHealth)
        {
            int viewCount = (int)Math.Ceiling(12 * percHealth);
            int left = viewCount / 2;
            int right = viewCount-left;

            leftcrew.Frame = 6 - left;
            rightcrew.Frame = 6 - right;
        }

        public override void DeleteMe()
        {
            base.DeleteMe();
            captain.DeleteMe();
            leftcrew.DeleteMe();
            rightcrew.DeleteMe();
        }

    }

}
