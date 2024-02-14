using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.Graphics;

namespace VikingEngine.LootFest.GO.EnvironmentObj
{
    //class FireCannon : AbsUpdateObj
    //{
    //    //I framtiden kanske ha en box som kan tas sönder
    //    Vector3 position;
    //    const float FireRate = 4000;
    //    TimeCounter fireTime = new TimeCounter(FireRate);

    //    public FireCannon(Map.WorldPosition wp, Facing4Dir fireDir)
    //        :base()
    //    {
    //        position = wp.ToV3();
    //        position.Y += 1;
    //        position.X += Editor.VoxelObjDataLoader.StandardHalfBlockWitdh;
    //        position.Z += Editor.VoxelObjDataLoader.StandardHalfBlockWitdh;

    //        switch (fireDir)
    //        {
    //            case Facing4Dir.SOUTH:
    //                rotation = Rotation1D.D180;
    //                break;
    //        }
            
    //    }

    //    public override void Time_LasyUpdate(ref float time)
    //    {
    //        base.Time_LasyUpdate(ref time);

    //        if (fireTime.TimeUpdate(time))
    //        {
    //            new Weapons.FireBall(position, rotation);
    //        }
    //    }

    //    public override Vector3 Position
    //    {
    //        get
    //        {
    //            return position;
    //        }
    //        set
    //        {
    //            position = value;
    //        }
    //    }
    //    public override float X
    //    {
    //        get { return position.X; }
    //        set { position.X = value; }
    //    }
    //    public override float Y
    //    {
    //        get { return position.Y; }
    //        set { position.Y = value; }
    //    }
    //    public override float Z
    //    {
    //        get { return position.Z; }
    //        set { position.Z = value; }
    //    }
    //    public override Vector2 PlanePos
    //    {
    //        get
    //        {
    //            return VectorExt.V3XZtoV2(position);
    //        }
    //        set
    //        {
    //            position = new Vector2toV3(value, position.Y);
    //        }
    //    }

    //    public override ObjectType Type
    //    {
    //        get { return ObjectType.InteractionObj; }
    //    }
    //    public override GameObjectType Type
    //    {
    //        get { throw new NotImplementedException(); }
    //    }


    //}
}
