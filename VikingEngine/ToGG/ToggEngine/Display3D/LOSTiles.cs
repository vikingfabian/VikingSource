using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.ToGG
{
    /// <summary>
    /// Visualize all tiles that are in Line Of Sight
    /// </summary>
    class LOSTiles : AbsQuedTasks//QueAndSynch
    {
        Graphics.ImageGroupParent3D models;
        bool complete = false;
        IntVector2 position;
        bool removed = false;

        LOSTiles previous;

        public LOSTiles(IntVector2 position, LOSTiles previous)
            :base( QuedTasksType.QueAndSynch)
        {
            this.previous = previous;
            models = new Graphics.ImageGroupParent3D();
            this.position = position;
            //start();
            beginAutoTasksRun();
        }

        //protected override bool quedEvent()
        //{
        protected override void runQuedAsynchTask()
        {
        //var tex = new Graphics.TextureEffect(Graphics.TextureEffectType.Flat, SpriteName.lineofsightEye);
        //var centerTex = new Graphics.TextureEffect(Graphics.TextureEffectType.Flat, SpriteName.lineofsightEyeCenter);

        IntVector2 block;
            ForXYLoop loop = new ForXYLoop(toggRef.board.tileGrid.Size);
            while (loop.Next())
            {
                if (toggRef.board.InLineOfSight(position, loop.Position, out block, null, toggRef.UnitsBlockLOS, false))
                {
                    bool center = position == loop.Position;
                    Graphics.Mesh model = new Graphics.Mesh(LoadedMesh.plane, toggLib.ToV3(loop.Position, 0.0f), new Vector3(center ? 0.4f : 0.2f),
                        Graphics.TextureEffectType.Flat, center ? SpriteName.lineofsightEyeCenter : SpriteName.lineofsightEye, Color.White, false); 
                       
                    models.Add(model);
                }
            }

            //return true;
        }

        //protected override void SynchedEvent()//public override void Time_Update(float time)
        // {
        public override void runSyncAction()
        {
            if (!removed)
            {
                previous?.DeleteModels();
                previous = null;

                Ref.draw.CurrentRenderLayer = Draw.Draw3dOverlay_Layer;
                models.AddAllToDraw();
                complete = true;

                Ref.draw.CurrentRenderLayer = 0;
            }
        }
        
        public void Transparentsy(float t)
        {
            if (complete)
            {
                if (t > 0f)
                {
                    if (!models.VisibleGroup)
                        models.SetVisible(true);
                    models.SetTransparentsy(t * 0.8f);
                }
                else
                {
                    if (models.VisibleGroup)
                        models.SetVisible(false);
                }
            }
        }

        public void DeleteModels()
        {
            previous?.DeleteModels();
            removed = true;
            if (complete)
            {
                Ref.draw.CurrentRenderLayer = Draw.Draw3dOverlay_Layer;
                models.DeleteAll();
                Ref.draw.CurrentRenderLayer = 0;
            }
        }
    }
}
