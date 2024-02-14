using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.ToGG.HeroQuest.Display
{
    class StaminaStepGUI : Graphics.ImageGroup
    {
        public StaminaStepGUI(IntVector2 fromPos, Unit unit, List<IntVector2> availableNormalWalkTiles)
            : base(8)
        {
            Vector3 Scale = new Vector3(0.3f);
            //Graphics.TextureEffect tex = new Graphics.TextureEffect(Graphics.TextureEffectType.Flat, SpriteName.cmdStaminaStep);
            
            foreach (var m in IntVector2.Dir8Array)
            {
                IntVector2 toPos = m + fromPos;
                if (toggRef.board.MovementRestriction(toPos, unit) != ToggEngine.Map.MovementRestrictionType.Impassable &&
                    !availableNormalWalkTiles.Contains(toPos) &&
                    !unit.movelines.Contains(toPos))
                {
                    Graphics.Mesh dot = new Graphics.Mesh(LoadedMesh.plane, 
                        toggRef.board.toWorldPos_Center(toPos, AvailableTilesGUI.PlaneY), Scale,
                        Graphics.TextureEffectType.Flat, SpriteName.cmdStaminaStep, Color.White);
                        //tex, Scale);
                    Add(dot);
                }
            }
        }
    }
}
