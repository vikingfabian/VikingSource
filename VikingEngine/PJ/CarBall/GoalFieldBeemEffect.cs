using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.PJ.CarBall
{
    class GoalFieldBeemEffect : AbsUpdateable
    {
        List2<Graphics.Image> images = new List2<Graphics.Image>(64);
        VectorRect beamArea;
        int dir;
        float startOpaciy = 0.8f;

        public GoalFieldBeemEffect(bool leftSideGoal)
            : base(true)
        {
            beamArea = cballRef.state.field.area;
            beamArea.Width = Convert.ToInt32(cballRef.state.field.area.Height * 0.06f);

            if (leftSideGoal)
            {
                dir = 1;
            }
            else
            {
                dir = -1;
                beamArea.SetRight(cballRef.state.field.area.Right, false);
            }

            nextBeam();
        }

        void nextBeam()
        {
            //const int AddBeamCount = 2;
            if (startOpaciy > 0)
            {
                if (dir > 0)
                {
                    if (beamArea.Right > cballRef.state.field.area.Right)
                    {
                        return;
                    }
                }
                else
                {
                    if (beamArea.X < cballRef.state.field.area.X)
                    {
                        return;
                    }
                }

                Graphics.Image image = new Graphics.Image(SpriteName.WhiteArea,
                    beamArea.Position, beamArea.Size, cballLib.LayerFieldEffects);
                image.Opacity = startOpaciy;//0.6f;

                images.Add(image);
                beamArea.X += beamArea.Width * dir;
                startOpaciy -= 0.04f;//f;
            }
        }

        public override void Time_Update(float time_ms)
        {
            if (Ref.TimePassed16ms)
            {
                if (images.Count > 0)
                {
                    images.loopBegin(true);
                    //var loop = new ForListLoop<Graphics.Image>(images);
                    while (images.loopNext())
                    {
                        images.sel.Opacity *= 0.8f;
                        if (images.sel.Opacity < 0.001f)
                        {
                            images.sel.DeleteMe();
                            images.loopRemove();
                        }
                    }

                    nextBeam();
                }
                else
                {
                    DeleteMe();
                }
            }    
        }
    }
}
