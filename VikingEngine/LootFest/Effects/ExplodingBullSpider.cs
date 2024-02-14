using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingEngine.Graphics;
using Microsoft.Xna.Framework;
using VikingEngine.LootFest.GO.Characters.Monster3;

namespace VikingEngine.LootFest.Effects
{
    class ExplodingBullSpider : AbsInGameUpdateable
    {
        const float ScaleUpTime = 120;

        bool scaleUp = true;
        Time idleTime = new Time(1f, TimeUnit.Seconds);
        Timer.Basic scaleTimer = new Timer.Basic(ScaleUpTime, true);
        //Time lifeTime = new Time(ScaleUpTime * 3f, TimeUnit.Milliseconds);
        AbsVoxelObj image;

        int scaleUpCount = 0;
        float scaleUpSpeed;

        bool localMember;

        public ExplodingBullSpider(VikingEngine.LootFest.GO.Characters.Monster3.BullSpider spider)
            :base(true)
        {
            this.image = spider.image;
            scaleUpSpeed = image.Scale1D * 0.4f / ScaleUpTime;
            localMember = spider.NetworkLocalMember;
        }

        public override void Time_Update(float time_ms)
        {
            if (idleTime.TimeOut)
            {
                image.Scale1D += scaleUpSpeed * time_ms * lib.BoolToLeftRight(scaleUp);

                if (scaleTimer.Update())
                {
                    if (scaleUp)
                    {
                        if (++scaleUpCount >= 3)
                        {
                            //Blow up
                            image.DeleteMe();


                            int numDummies = 20;
                            int numBlocks = 20;
                            if (Engine.Update.IsRunningSlow || Ref.gamesett.DetailLevel == 0)
                            {
                                numBlocks = 0;
                            }
                            else if (Ref.gamesett.DetailLevel == 2)
                            {
                                numBlocks += numDummies;
                                numDummies = 0;
                            }
                            Vector3 pos = image.position;
                            pos.Y += image.scale.Y * 8f;
                            float scale = lib.SmallestValue(image.scale.X * 1.6f, 0.5f);
                            for (int i = 0; i < numBlocks; i++)
                            {
                                new Effects.BouncingBlock2(pos, BullSpider.DamageColorsLvl1.GetRandom(), scale);
                            }
                            for (int i = 0; i < numDummies; i++)
                            {
                                new Effects.BouncingBlock2Dummie(pos, BullSpider.DamageColorsLvl1.GetRandom(), scale);
                            }


                            if (localMember)
                            {

                                int spawnCount = Ref.rnd.Int(3, 5);
                                Rotation1D dir = Rotation1D.Random();
                                

                                for (int i = 0; i < spawnCount; ++i)
                                {
                                    dir.Add(MathHelper.TwoPi / spawnCount);

                                    var spider = new Spider(new GO.GoArgs(image.position + VectorExt.V2toV3XZ(dir.Direction(1.4f), 1.5f)));
                                    spider.bullSpiderSpawn(dir);
                                }
                            }

                            this.DeleteMe();
                        }
                    }

                    scaleUp = !scaleUp;
                }

            }
            else
            {
                if (idleTime.CountDown())
                {
                    new Effects.DamageFlash(image, ScaleUpTime * 5f);//idleTime.MilliSeconds + 
                }
            }
        }

       


    }
}
