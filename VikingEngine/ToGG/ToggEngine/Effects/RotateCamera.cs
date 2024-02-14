using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace VikingEngine.ToGG
{
    //class RotateCamera : AbsUpdateable
    //{
    //    Rotation1D angle, goalAngle;

    //    public RotateCamera(bool firstPlayerView)
    //        :base(false)
    //    {
    //        angle = new Rotation1D(Ref.draw.Camera.TiltX);
    //        goalAngle = firstPlayerView ? Rotation1D.D90 : Rotation1D.D270;

    //        if (angle != goalAngle)
    //        {
    //            AddToUpdateList();
    //        }
    //    }

    //    public override void Time_Update(float time)
    //    {
    //        float add = time * 0.006f;
    //        if (Math.Abs(angle.AngleDifference(goalAngle.Radians)) <= add)
    //        {
    //            angle = goalAngle;
    //            DeleteMe();
    //        }
    //        else
    //        {
    //            angle.Add(add);
    //        }

    //        Ref.draw.Camera.TiltX = angle.Radians;

    //        Unit.UnitsRotation = RotationQuarterion.Identity;
    //        Unit.UnitsRotation.RotateWorldX(MathHelper.Pi - angle.Radians - MathHelper.PiOver2);
    //        foreach (Unit u in cmdRef.units)
    //        {
    //            u.UpdateRotation();
    //        }
    //    }
    //}
}
