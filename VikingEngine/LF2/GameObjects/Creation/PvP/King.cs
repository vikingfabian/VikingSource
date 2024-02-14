using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.Graphics;

namespace VikingEngine.LootFest.Creation.PvP
{
#if CMODE
    //class King : GameObjects.Characters.AbsEnemy
    //{
    //    static readonly RangeF ScaleRange = new RangeF(0.2f, 1f);
    //    static readonly RangeF PosAdd = new RangeF(0.2f, Map.WorldPosition.ChunkWith * 0.3f);
    //    bool walkingMode = false;
    //    float modeTimeLeft = 0;
    //    //CritterType critterType;
    //    bool blue;
    //    const float StartHealth = 10;
    //    Timer.Basic addKing = new Timer.Basic(1000);

    //    public King(Map.WorldPosition pos, bool blue)
    //        : base()
    //    {
    //        basicInit(blue);
    //        pos.UpdateWorldGridPos();
    //        image.Position = pos.WorldGridPos.Vec;
    //        image.Position.Y = 4;
    //        image.Position.X += PosAdd.GetRandom();
    //        image.Position.Z += PosAdd.GetRandom();
    //        NextMode();
    //        //critterType = type;
    //        health = StartHealth;
    //        NetworkShare();
    //    }
    //    public King(System.IO.BinaryReader packetReader)
    //        : base(packetReader)
    //    {
    //        health = StartHealth;
    //        basicInit(packetReader.ReadBoolean());

    //        //if (LfRef.gamestate.FatKing != null)
    //        //{
    //        //    LfRef.gamestate.FatKing.SetClientKing(this, blue);
    //        //}
    //    }
    //    //public override void ClientTimeUpdate(float time, List<AbsUpdateObj> args.localMembersCounter, List<AbsUpdateObj> active)
    //    //{
    //    //    base.ClientTimeUpdate(time, args.localMembersCounter, active);
    //    //    if (addKing.Update(time))
    //    //    {
    //    //        if (LfRef.gamestate.FatKing != null)
    //    //        {
    //    //            LfRef.gamestate.FatKing.SetClientKing(this, blue);
    //    //        }
    //    //    }
    //    //}
    //    public override void ObjToNetPacket(System.IO.BinaryWriter writer)
    //    {
    //        base.ObjToNetPacket(writer);
    //        writer.Write(blue);
    //    }
    //    static readonly Graphics.AnimationsSettings AnimationsSettings = new Graphics.AnimationsSettings(5, 0.8f);
    //    void basicInit(bool blue)
    //    {
    //        this.blue = blue;
    //        //this.image = new Graphics.VoxelModelInstance(
    //        //        LootfestLib.Images.StandardAnimatedVoxelObjects[VoxelModelName.fatzombie],
    //        //    AnimationsSettings);
    //        Graphics.VoxelObjAnimated org = Editor.VoxelObjDataLoader.GetVoxelObjAnimWithMReplace(VoxelModelName.fatzombie,
    //            Data.MaterialType.ZombieSkin, 
    //            FatKing.TeamColor(blue));
    //        org.DeleteMe();
    //        image = new Graphics.VoxelModelInstance(org, AnimationsSettings);
    //        image.Scale = lib.V3(0.4f);
    //        CollisionBound = GameObjects.LootFest.AbsObjBound.QuickBoundingBox(6 * 0.4f);
    //    }

    //    static readonly RangeF WalkingModeTime = new RangeF(400, 1000);
    //    static readonly RangeF WaitingModeTime = new RangeF(400, 3000);

    //    void NextMode()
    //    {
    //        walkingMode = !walkingMode;
    //        if (walkingMode)
    //        {
    //            Rotation = Rotation1D.Random;
    //            Speed.Set(rotation, 0.007f);
    //            modeTimeLeft = WalkingModeTime.GetRandom();
    //        }
    //        else
    //        {
    //            Speed.SetZeroPlaneSpeed();
    //            modeTimeLeft = WaitingModeTime.GetRandom();
    //            //if (!hostedMember && LfRef.gamestate.FatKing != null)
    //            //{
    //            //    LfRef.gamestate.FatKing.SetClientKing(this, blue);
    //            //}
    //        }
    //    }
    //    public override void Time_LasyUpdate(ref float time)
    //    {
    //        base.Time_LasyUpdate(ref time);
    //        modeTimeLeft -= time;
    //        if (modeTimeLeft <= 0)
    //        {
    //            NextMode();
    //        }
    //    }

    //    //protected override void handleHeroColl(GameObjects.Characters.AbsCharacter hero)
    //    //{
    //    //    //do nothing
    //    //    //base.handleHeroColl(hero);
    //    //}
    //    //protected override void handleHeroColl(GameObjects.Characters.Hero hero)
    //    //{
    //    //    //do nothing
    //    //}
    //    //public override void HandleObsticleColl3D(LootFest.CollisionIntersection3D collData)
    //    //{

    //    //    NextMode();
    //    //}
    //    //protected override void DeathEvent(bool local, WeaponAttack.DamageData damage)
    //    //{
    //    //    LfRef.gamestate.KingDied(blue);
    //    //    base.DeathEvent(local, damage);
    //    //}

    //    public override Data.Characters.EnemyType EnemyType
    //    {
    //        get { return Data.Characters.EnemyType.King; }
    //    }
    //    public override GameObjects.Characters.CharacterUtype CharacterType
    //    {
    //        get { return GameObjects.Characters.CharacterUtype.Enemy; }
    //    }
    //}
#endif
}