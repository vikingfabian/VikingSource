using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.ToGG.HeroQuest;
using VikingEngine.ToGG.ToggEngine.GO;

namespace VikingEngine.ToGG.ToggEngine.Data
{
    abstract class AbsBoardArea
    {
        abstract public bool pathFindBestTargetCount(AbsUnit activeUnit,
            out List<IntVector2> targetSquares, out List<AttackTarget> attackTargets, out WalkingPath path);
    }

    class AttackCone: AbsBoardArea
    {
        public float startRadius, endRadius, length;

        public AttackCone(float startRadius, float endRadius, float length)
        {
            this.startRadius = startRadius;
            this.endRadius = endRadius;
            this.length = length;
        }

        public override bool pathFindBestTargetCount(AbsUnit activeUnit,
             out List<IntVector2> targetSquares, out List<AttackTarget> attackTargets, out WalkingPath path)
        {
            throw new NotImplementedException();
        }

        public string description()
        {
            const string Format = "#.0";
            return "Cone area, length " + length.ToString(Format);
        }
    }

    class AttackBlast: AbsBoardArea
    {
        public int radius;
        public AttackBlast(int radius = 1)
        {
            this.radius = radius;
        }
        public override bool pathFindBestTargetCount(AbsUnit activeUnit,
              out List<IntVector2> targetSquares, out List<AttackTarget> attackTargets, out WalkingPath path)
        {
            throw new NotImplementedException();
        }

        public string Name => "Blast" + radius.ToString();

        public string Desc => "Attack will include all units within a radius of " + radius.ToString();
    }

    
}
