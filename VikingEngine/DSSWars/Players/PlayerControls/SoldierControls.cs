using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DebugExtensions;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.Players.Command;
using VikingEngine.LootFest.Players;
using VikingEngine.Physics;
using VikingEngine.ToGG.ToggEngine.QueAction;
using static VikingEngine.PJ.Bagatelle.BagatellePlayState;

namespace VikingEngine.DSSWars.Players.PlayerControls
{
    class SoldierControls
    {
        List<SoldierGroup> groups;
        public SoldierControls(List<SoldierGroup> groups)
        {
            this.groups = groups;
        }

        public void mapExecute(LocalPlayer player)
        {
            var pos = WP.SubtileToWorldPosXZgroundY_Centered(player.gameControls.mapControls.subTilePosition);
            SoldierGroup target = null;
            if (player.gameControls.mapControls.armyMayAttackHoverObj())
            {
                target = player.gameControls.mapControls.hover.obj.GetSoldierGroup();
                new AttackHereAnimation(target, player.playerData.view.ScreenIndex);
            }
            else
            {
                new MoveHereAnimation(pos);
            }
            calculateGroupOrder(player, pos, target);
            //if (target == null)
            //{
            //    new MoveHereAnimation(pos);
            //}
            //else
            //{ 
            //    new AttackHereAnimation(target, player.playerData.view.ScreenIndex);
            //}

            //if (player.gameControls.mapControls.armyMayAttackHoverObj())
            //{

            //    foreach (SoldierGroup group in groups)
            //    {
            //        new AttackCommand(group, target, false);
            //    }
            //    new AttackHereAnimation(target, player.playerData.view.ScreenIndex);
            //}
            //else
            //{

            //    foreach (SoldierGroup group in groups)
            //    {
            //        new MoveCommand(group, pos, false);

            //        if (group.InGuardPost())
            //        {
            //            new GuardPostTransform(group, -1, false);
            //        }

            //        if (player.gameControls.mapControls.hover.subTile.selectTileResult == SelectTileResult.Wall)
            //        {
            //            var enterCommand = new EnterPostCommand(group, player.gameControls.mapControls.hover.subTile.subTilePos, true);
            //            enterCommand.claimPost(group, player.gameControls.mapControls.hover.subTile.city, player.gameControls.mapControls.hover.subTile.city.defenceIxFromPosId(enterCommand.id));
            //        }
            //    }
            //    new MoveHereAnimation(pos);
            //}
        }

        void calculateGroupOrder(LocalPlayer player, Vector3 goalPos, SoldierGroup target)
        {
            List<SoldierGroup> groups_sp = groups.ToList();

            if (groups_sp.Count > 0)
            {
                bool wall = target == null && player.gameControls.mapControls.hover.subTile.selectTileResult == SelectTileResult.Wall;
                IntVector2 subTile = player.gameControls.mapControls.hover.subTile.subTilePos;
                var city = player.gameControls.mapControls.hover.subTile.city;

                Task.Run(() =>
                {
                    try
                    {
                        Vector3 center = Vector3.Zero;
                        float groupRotation = 0;
                        bool allSameDir = true;
                        float firstRotation = groups_sp.First().rotation.radians;
                        foreach (SoldierGroup group in groups_sp)
                        {
                            center += group.position;
                            groupRotation += group.rotation.radians;
                            if (allSameDir && group.rotation.AngleDifference(firstRotation) > MathExt.TauOver4)
                            {
                                allSameDir = false;
                            }
                        }
                        center /= groups_sp.Count;
                        groupRotation /= groups_sp.Count;
                        //If the soldiers all look in the same general direction, rotate the formation, otherwise just move
                        Vector2 goalDir = VectorExt.V3XZtoV2(goalPos - center);
                        Vector2 goalCenter = VectorExt.V3XZtoV2(goalPos);
                        Rotation1D goalRot = Rotation1D.FromDirection(goalDir);
                        Vector2 goalDirNorm = goalDir;
                        goalDirNorm.Normalize();

                        float rotateGroupOffsets = 0;
                        bool mirror = false;
                        if (allSameDir)
                        {
                            rotateGroupOffsets = -goalRot.AngleDifference(groupRotation);
                            mirror = Math.Abs(rotateGroupOffsets) > MathExt.TauOver4;
                        }

                        Span<GroupCommandPlacement> groupPlacements = stackalloc GroupCommandPlacement[groups_sp.Count];
                        for (int i = 0; i < groups_sp.Count; i++)
                        {
                            GroupCommandPlacement placement = new GroupCommandPlacement(groups_sp[i].position, groups_sp[i].GroupMoveBoundRadius(), center, goalPos);

                            if (allSameDir)
                            {
                                placement.rotateGroup(goalDirNorm, rotateGroupOffsets, mirror);
                            }

                            placement.finalize();

                            groupPlacements[i] = placement;
                        }



                        //float r = DssVar.SoldierGroup_Spacing_Radius * 1f;
                        bool collision = true;
                        int loopMaxCount = 20;
                        if (groups_sp.Count > 1)
                        {
                            //Pull all groups outward from center until they don't collide anymore
                            while (collision && loopMaxCount > 0)
                            {
                                collision = false;
                                for (int group1Ix = 0; group1Ix < groups_sp.Count - 1; ++group1Ix)
                                {
                                    for (int group2Ix = group1Ix + 1; group2Ix < groups_sp.Count; ++group2Ix)
                                    {
                                        var group1 = groupPlacements[group1Ix];
                                        var group2 = groupPlacements[group2Ix];

                                        if (PhysicsLib2D.CirkleIntersect(group1.currentPlacement, group1.radius,
                                             group2.currentPlacement, group2.radius, out float intersect))
                                        {
                                            collision = true;

                                            //On collision move the group furthest away from its offset
                                            int moveIx;
                                            int otherIx;
                                            if (group1.distanceToGroupOffset() > group2.distanceToGroupOffset())
                                            {
                                                moveIx = group1Ix;
                                                otherIx = group2Ix;
                                            }
                                            else
                                            {
                                                moveIx = group2Ix;
                                                otherIx = group1Ix;
                                            }

                                            ref var moveGroup = ref groupPlacements[moveIx];
                                            moveGroup.moveOnCollision(goalCenter, intersect, groupPlacements[otherIx].currentPlacement);
                                        }
                                    }
                                }
                                --loopMaxCount;
                            }
                        }


                        for (int i = 0; i < groups_sp.Count; ++i)
                        {
                            var place = groupPlacements[i];
                            var group = groups_sp[i];

                            if (target == null)
                            {
                                new MoveCommand(group, VectorExt.V2toV3XZ(place.currentPlacement), goalRot.radians, false);
                            }
                            else
                            {
                                new AttackCommand(group, place.currentPlacement - goalCenter, target, false);
                            }

                            if (group.IsGuardGroup())
                            {
                                if (group.InGuardPost())
                                {
                                    new GuardPostTransform(group, -1, false);
                                }

                                if (wall)
                                {
                                    if (EnterPostCommand.tryClaimPost(group, city, subTile))
                                    {
                                        var enterCommand = new EnterPostCommand(group, subTile, true);
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        BlueScreen.ThreadException = ex;
                    }

                    
                });
            }
        }
    }

    struct GroupCommandPlacement
    {
         
        bool isCenter;
        Vector2 groupOffset;
        float groupOffsetLenght;
        Vector2 groupOffsetNorm;
        public Vector2 currentPlacement;
        public float radius;

        Vector2 goalPlusOffset;
        bool pastOffset;
        public GroupCommandPlacement(Vector3 position, float radius, Vector3 center, Vector3 goalPos)
        {
            this.radius = radius;
            currentPlacement = VectorExt.V3XZtoV2(goalPos);

            if (position == center)
            { 
                isCenter = true;
            }
            else
            {
                isCenter = false;
                groupOffset = VectorExt.V3XZtoV2(position - center);                
            }
        }

        public void rotateGroup(Vector2 goalDirNorm, float rotate, bool mirror)
        {
            if (!isCenter)
            {
                if (mirror)
                {
                    groupOffset = -VectorExt.Reflect(groupOffset, goalDirNorm);
                }

                groupOffset = VectorExt.RotateVector(groupOffset, rotate);
                
                //How do I mirror the offset if the angle is above 90 degrees? Mirror with the goalDir vector as center line
            }
        }

        public float distanceToGroupOffset()
        { 
            return (currentPlacement - goalPlusOffset).Length();
        }

        public void moveOnCollision(Vector2 goalCenter, float intersect, Vector2 otherObjectPos)
        {
            //Check if pushed past group offset
            if (!pastOffset)
            {
                pastOffset = (goalCenter - currentPlacement).Length() > groupOffsetLenght * 1.3f;             
            }

            intersect = lib.SmallestValue(Math.Abs(intersect), DssVar.SoldierGroup_Spacing_Radius);
            intersect += DssVar.SoldierGroup_Spacing * 0.05f;
            if (pastOffset && otherObjectPos != currentPlacement)
            {
                //Push away from the other object
                Vector2 diff = otherObjectPos - currentPlacement;
                diff.Normalize();
                currentPlacement += intersect * -diff;
            }
            else
            { 
                //Go towards offset
                currentPlacement += intersect * groupOffsetNorm;
            }
        }

        public void finalize()
        {
            if (groupOffset.X != 0 || groupOffset.Y != 0)
            {
                groupOffsetNorm = VectorExt.Normalize(groupOffset, out groupOffsetLenght);
            }
            else
            {
                groupOffsetNorm = Vector2.UnitX;
            }
            goalPlusOffset = currentPlacement + groupOffset;
        }
    }
}
