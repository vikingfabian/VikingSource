using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.Core.BlackBolts.Data;
using VikingEngine.Core.BlackBolts.Interface;
using VikingEngine.Core.BlackBolts.Map;
using VikingEngine.Core.BlackBolts.Render;
using VikingEngine.Graphics;

namespace VikingEngine.Core.BlackBolts.GO
{
    abstract class AbsGameObject
    {
        public PlaceObjectData placementData;

        public MapPlacement currentPos;
        public MapPlacement nextPos;
        public bool hasBeltMove = false;
        public VoxelModelInstance model;
        public ObjectPointer pointer;

        public IntVector2 tilesize = IntVector2.One;

        public bool lockItem = false;
        public ObjectPointer pResource = ObjectPointer.Empty;        

        abstract public FactoryObjectType FactoryObjectType { get; }

        virtual public Vector3 ResourceOffset() { return Vector3.Zero; }

        public AbsGameObject(PlaceObjectData placementData)
        { 
            this.placementData = placementData;
            currentPos = placementData.mapPlacement;
        }

        virtual public void ItemHandle(out bool mayPick, out bool mayDrop)
        {
            mayPick = false;
            mayDrop = false;
        }

        public ObjectPointer HandoverItem(ObjectPointer newResource)
        {
            if (newResource.hasValue || pResource.hasValue)
            {
                ObjectPointer returnItem = pResource;
                pResource = newResource;
                RefreshResourcePos();
                OnResourceChanged();
                return returnItem;
            }
            else
            {
                OnResourceChanged();
                return ObjectPointer.Empty;
            }
        }

        virtual public void OnResourceChanged()
        { }

        virtual public void RefreshResourcePos()
        {
            if (pResource.hasValue)
            {
                var resource = pResource.GetSolidResource();
                resource.model.position = model.position + ResourceOffset();
            }
        }
        virtual public void TweenUpdate(bool beltMove, float tween)
        {
            if (!beltMove || hasBeltMove)
            {
                if (currentPos.direction != nextPos.direction)
                {
                    float from = WP.DirToAngle(currentPos.direction);
                    float to = WP.DirToAngle(nextPos.direction);
                    var diff = Rotation1D.AngleDifference(from, to);
                    //if (to < from && to == 0)
                    //{
                    //    to += MathExt.Tau;
                    //}
                    //else if (to - from > MathExt.TauOver2)
                    //{
                    //    to -= MathExt.Tau;
                    //}

                    WP.Rotation1DToQuaterion(model, from * (1 - tween) + to * tween);
                }

                {
                    Vector3 from = WP.TileToWp(currentPos.tilePos);
                    from.Y = currentPos.groundY;
                    Vector3 to = WP.TileToWp(nextPos.tilePos);
                    to.Y = nextPos.groundY;
                    model.position = from * (1 - tween) + to * tween;
                }
            }
        }

        virtual public bool RefreshUiDisplay(IOdisplay display)
        {
            return false;
        }

        virtual public void DeleteMe()
        {
            model.DeleteMe();

            if (pResource.hasValue)
            {
                var resource = pResource.GetSolidResource();
                resource.DeleteMe();
            }
        }

    }


}
