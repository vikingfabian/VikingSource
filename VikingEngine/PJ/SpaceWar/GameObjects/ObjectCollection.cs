using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.PJ.SpaceWar.GameObjects
{
    class ObjectCollection
    {
        public List<ShopSquare> shops;

        public SpottedArray<AbsGameObject> gameObjects;
        public SpottedArrayCounter<AbsGameObject> goCounter;
        public SpottedArrayCounter<AbsGameObject> gameObjectsAsynchCounter;

        public ObjectCollection()
        {
            gameObjects = new SpottedArray<AbsGameObject>(2048);
            goCounter = new SpottedArrayCounter<AbsGameObject>(gameObjects);
            gameObjectsAsynchCounter = new SpottedArrayCounter<AbsGameObject>(gameObjects);

            shops = new List<ShopSquare>(16);
        }

        public void Add(AbsGameObject obj)
        {
            gameObjects.Add(obj);
        }
    }
}
