using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using VikingEngine.ToGG.ToggEngine.GO;

namespace VikingEngine.ToGG
{
    /// <summary>
    /// View tiles that are available for attacks and movement for the unit you hover
    /// </summary>
    class UnitMoveAndAttackGUI : AbsQuedTasks
    {
        

        Graphics.ImageGroup images;
        AvailableMovement movement = null;
        AttackTargetCollection targets = null;
        HeroQuest.PetTarget petTargets = null;
        AbsUnit unit;
        IntVector2 pos;
        bool targetTerrain;
        bool isMoving = false;
        bool isRemoved = false;

        public UnitMoveAndAttackGUI(AbsUnit unit, IntVector2 pos, bool targetTerrain, bool isMoving)
            :base(QuedTasksType.QueAndSynch)
        {
            if (Input.Mouse.IsButtonDown(MouseButton.Left))
            {
                lib.DoNothing();
            }
            this.targetTerrain = targetTerrain;
            this.unit = unit;
            this.isMoving = isMoving;
            this.pos = pos;
            
            if (isMoving)
            {
                pos = unit.movelines.CurrentSquarePos();
            }
            
            beginAutoTasksRun();
        }
        
        protected override void runQuedAsynchTask()
        {
            if (targetTerrain)
            {
                collectTerrainTargets();
            }
            else
            {
                collectUnitTargets();
            }
        }

        void collectTerrainTargets()
        {
            targets = new AttackTargetCollection(unit, pos, false, true);
        }

        void collectUnitTargets()
        {
            if (!isMoving)
            {
                movement = new AvailableMovement(null, unit, true, true);
            }
            
            targets = new AttackTargetCollection(unit, pos, true);
            
            if (unit.HasProperty(UnitPropertyType.Pet))
            {
                petTargets = new HeroQuest.PetTarget((HeroQuest.Unit)unit);
                petTargets.collectAllTargets(pos);
            }
        }
        
        public override void runSyncAction()
        {
            if (!isRemoved)
            {
                Clear();
               
                images = new Graphics.ImageGroup();
                
                const float MoveDotHeight = AvailableTilesGUI.PlaneY + PublicConstants.LayerMinDiff3D;

                bool opponentGuiStyle = unit.IsScenarioOpponent();
                SpriteName sprite = opponentGuiStyle ? SpriteName.cmdUnitMoveGui_EnemySmall : SpriteName.cmdUnitMoveGui_Small;

                if (movement != null)
                {
                    foreach (var move in movement.available)
                    {
                        if (move != pos)
                        {
                            Graphics.Mesh moveDot = new Graphics.Mesh(LoadedMesh.plane, new Vector3(move.X, MoveDotHeight, move.Y), new Vector3(0.14f),
                                Graphics.TextureEffectType.Flat,
                                 sprite, 
                                ColorExt.FromAlpha(0.6f));
                            images.Add(moveDot);
                        }
                    }
                }
                
                foreach (var t in targets.targets.list)
                {
                    CreateTargetDot(t, opponentGuiStyle);
                }

                if (petTargets != null && petTargets.targets != null)
                {
                    foreach (var m in petTargets.targets)
                    {
                        Vector3 wp = toggRef.board.toWorldPos_Center(m.squarePos, ModelLayers.UnitMoveAndAttackGUI);
                        wp.Z += ModelLayers.UnitMoveAndAttackGUI * 0.2f;

                        Graphics.Mesh tDot = new Graphics.Mesh(LoadedMesh.plane, wp, new Vector3(0.3f),
                            Graphics.TextureEffectType.Flat, SpriteName.cmdPetTargetGui, Color.White);

                        tDot.Rotation = toggLib.PlaneTowardsCam;
                        images.Add(tDot);
                    }
                }

                toggRef.gamestate.hoverUnitMoveAndAttackDots = this;
            }            
        }

        public static void Clear()
        {
            if (toggRef.gamestate.hoverUnitMoveAndAttackDots != null)
            {
                toggRef.gamestate.hoverUnitMoveAndAttackDots.DeleteImages();
            }
        }

        void CreateTargetDot(AttackTarget target, bool opponentGuiStyle)
        {
            Vector3 wp = toggRef.board.toWorldPos_adjustForUnitZ(target.position, ModelLayers.UnitMoveAndAttackGUI);

            Vector3 targetDir;
            if (pos != target.position)
            {
                targetDir = toggRef.board.toWorldPos_Center(pos, ModelLayers.UnitMoveAndAttackGUI) - wp;
            }
            else
            {
                targetDir = new Vector3(-1f, 0, -1f);
            }
            targetDir = VectorExt.SetLength(targetDir, 0.4f);
            wp.Z += ModelLayers.UnitMoveAndAttackGUI * 0.2f;

            Vector2 dir2 = new Vector2(targetDir.X, targetDir.Z);
            wp += toggLib.MoveInCamFacePlane(dir2);

            SpriteName sprite;
            SpriteName arrowSprite;
            if (opponentGuiStyle)
            {
                arrowSprite = SpriteName.cmdUnitGuiArrow_Enemy;
                if (target.attackType.IsMelee)
                {
                    sprite = SpriteName.cmdUnitMeleeGui_EnemySmall;
                }
                else
                {
                    sprite = SpriteName.cmdUnitRangedGui_EnemySmall;
                }
            }
            else
            {
                arrowSprite = SpriteName.cmdUnitGuiArrow;
                if (target.attackType.IsMelee)
                {
                    sprite = SpriteName.cmdUnitMeleeGui_Small;
                }
                else
                {
                    sprite = SpriteName.cmdUnitRangedGui_Small;
                }
            }

            Graphics.Mesh tDot = new Graphics.Mesh(LoadedMesh.plane, wp, new Vector3(0.14f),
                Graphics.TextureEffectType.Flat,
                 sprite, 
                 Color.White);

            tDot.Position += toggLib.UpVector * 0.01f;
            tDot.Rotation = toggLib.PlaneTowardsCam;
            
            Graphics.Mesh arrow = new Graphics.Mesh(LoadedMesh.plane, wp, new Vector3(0.4f),
                Graphics.TextureEffectType.Flat, arrowSprite, Color.White);
            arrow.Rotation = toggLib.PlaneTowardsCamWithRotation(lib.V2ToAngle(dir2) + MathExt.TauOver2);

            images.Add(tDot); images.Add(arrow);
        }

        public void DeleteImages()
        {
            isRemoved = true;
            if (images != null)
            {
                images.DeleteAll();
            }
        }
    }
}
