using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.Graphics;

namespace VikingEngine.DSSWars.Players.Command
{
    abstract class AbsCommandAnimation : AbsUpdateable
    {
        protected List<Mesh> models;

        public AbsCommandAnimation()
        :base(true){ }

        public override void DeleteMe()
        {
            base.DeleteMe();
            foreach (var model in models)
            {
                model.DeleteMe();
            }
        }
    }

    class AttackHereAnimation : AbsCommandAnimation
    {
        const float FlashTime = 100;

        int flashes = 3;
        float timeLeft= FlashTime;
        bool visible = true;

        public AttackHereAnimation(SoldierGroup target, int playerIndex)
            : base()
        {
            var soldiers_sp = target.soldiers;
            if (soldiers_sp != null)
            {
                var soldiersC = soldiers_sp.counter();
                models = new List<Mesh>(soldiersC.array.Count);

                while (soldiersC.Next())
                {
                    soldiersC.sel.selectionFramePlacement(out var pos, out var scale);
                    var model = new Mesh( LoadedMesh.SelectCircleSolid, pos, scale,
                        TextureEffectType.Flat, SpriteName.WhiteArea, Color.Red, false);
                    model.AddToRender(DrawGame.UnitDetailLayer);
                    model.setVisibleCamera(playerIndex);
                    models.Add(model);
                }
            }
        }

        public override void Time_Update(float time_ms)
        {
            timeLeft -= time_ms;
            if (timeLeft <= 0)
            {
                visible = !visible;

                foreach (var model in models)
                { 
                    model.Visible = visible;
                }

                timeLeft = FlashTime;

                if (--flashes <= 0)
                {
                    DeleteMe();
                }
            }
        }
    }

    class MoveHereAnimation : AbsCommandAnimation
    {
        const int CirkleCount = 2;
        
        float timePassed = 0;
        float startScale;
        float scaleDown;
        const float ScaleDownTimeMs = 230;
        public MoveHereAnimation(Vector3 wp)
            :base()
        {
            startScale = WorldData.SubTileWidth * 2.5f;
            scaleDown = startScale / ScaleDownTimeMs;

            //wp.Y += WP.GroundY(wp) - 0.0001f;

            models = new List<Mesh>(CirkleCount);

            for (int i = 0; i < CirkleCount; i++)
            {
                Mesh model = new Mesh( LoadedMesh.SelectCircleSolid, wp, new Vector3(startScale), TextureEffectType.Flat, SpriteName.WhiteArea, Color.White);
                model.Opacity = 0;
                models.Add(model);
            }
        }

        public override void Time_Update(float time_ms)
        {
            timePassed += time_ms;
            int updateCount = models.Count;
            if (timePassed < 120)
            {
                updateCount -= 1;
            }

            int completeCount = 0;
            for (int i = 0; i < updateCount; i++)
            {
                models[i].Opacity += 0.004f * time_ms;
                if (models[i].scale.X > scaleDown)
                {
                    models[i].Scale1D -= scaleDown * time_ms;
                }
                else
                { 
                    completeCount++;
                }
            }

            if (completeCount >= models.Count)
            {
                DeleteMe();
            }
        }

        
    }
}
