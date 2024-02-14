using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.PJ.CarBall
{
    class Gamer
    {
        public GamerData gamerdata;
        public bool leftTeam;
        public Car car;
        public Goalie goalie = null;
        public int goals = 0;

        public Gamer(GamerData gamerData)
        {
            this.leftTeam = gamerData.LeftTeam;
            this.gamerdata = gamerData;

            car = new Car(this);
        }

        public void update(int myUpdateIndex, int updatePart)
        {
            if (goalie != null)
            {
                if (updatePart == 0)
                {
                    if (goalie.update())
                    {
                        car.leaveGoalie(goalie);
                        new Timer.TimedAction0ArgTrigger(field.resetGolieSpots, 400);

                        goalie = null;
                        field.goalie = null;
                    }
                }
            }
            else
            {
                car.update(myUpdateIndex, updatePart);
            }
        }

        public void updateIntro()
        {
            car.updateIntro();
        }

        public bool HasActiveCar()
        {
            return car.Alive && car.image.car.Visible;
        }

        public Vector2 Center()
        {
            return car.image.position;
        }

        public FieldHalf field
        {
            get { return leftTeam ? cballRef.state.field.leftField : cballRef.state.field.rightField; }
        }
    }
}
