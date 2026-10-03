using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.Core.BlackBolts.GO;

namespace VikingEngine.Core.BlackBolts.Player
{
    class ToolShop
    {
        public Dir4 toolDir = Dir4.S;
        public GameObjectType selectedObjectType = GameObjectType.Worker;

        public void selectTool(GameObjectType objectType)
        {
            selectedObjectType = objectType;
            checkToolDir();
        }

        public void checkToolDir()
        {
            if (selectedObjectType == GameObjectType.Spin_plate)
            {
                if (toolDir != Dir4.W && toolDir != Dir4.E)
                {
                    rotate();
                }
            }
        }
        public void RotateTool()
        {
            rotate();
            checkToolDir();
        }

        void rotate()
        {
            toolDir++;
            if (toolDir >= Dir4.NUM_NON)
            {
                toolDir = Dir4.N;
            }
        }
    }
}
