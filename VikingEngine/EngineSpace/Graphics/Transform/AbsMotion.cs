using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.Graphics
{
    abstract class AbsMotion : AbsUpdateable
    {
        public event Action OnComplete;

        protected MotionType motionType = MotionType.NON;
        protected Vector3 Stepping = Vector3.Zero;
        //protected int TimeStep = 0;
        //protected int AccumulatedTime = 0;
        bool haveReversed = false;
        protected float timeLeft=0;
        protected float timeLeftSave = 0;
        protected MotionRepeate repeate = MotionRepeate.NO_REPEAT;
        protected Vector3 originalPose;
      
        abstract public bool In3d{ get; }
        abstract protected bool inRenderList { get; }
        public bool checkInRenderList = true;
        
        public override void Time_Update(float time)
        {
            if (checkInRenderList && !inRenderList)
            {
                DeleteMe();
            }

            if (!IsDeleted)
            {
                timeLeft -= time;
                if (timeLeft <= 0)
                {
                    switch (repeate)
                    { 
                        case MotionRepeate.Loop:
                            timeLeft = timeLeftSave;
                            break;
                        case MotionRepeate.NO_REPEAT:
                            UpdateMotion(time + timeLeft);
                            if (OnComplete != null)
                            {
                                OnComplete();
                                OnComplete = null;
                            }
                            DeleteMe();
                            break;
                        case MotionRepeate.BackNForwardLoop:
                            reverseMotion();
                            break;
                        case MotionRepeate.BackNForwardOnce:
                            if (haveReversed)
                            {
                                resetOriginalPosition();
                                if (OnComplete != null)
                                {
                                    OnComplete();
                                    OnComplete = null;
                                }
                                DeleteMe();
                            }
                            else
                            {
                                reverseMotion();
                            }
                            break;
                    }
                    
                    //Delete this object
                    
                }
                else
                { UpdateMotion(time); }
            }
           
        }

        void reverseMotion()
        {
            haveReversed = true;
            timeLeft = timeLeftSave;
            Stepping.X = -Stepping.X;
            Stepping.Y = -Stepping.Y;
            Stepping.Z = -Stepping.Z;
        }
        public AbsMotion(MotionType type,
            Vector3 valueChange, MotionRepeate repeateType,
            float timeMS, bool addToUpdateList)
            :base(addToUpdateList)
        {
            //IsDeleted = false;
            motionType = type;
            //imageInMotion = objImage;
            repeate = repeateType;
            //if (addToUpdateList && IsDeleted)
            //{
            //   //state.listImageMotions.Add(this);
            //    AddToUpdateList(true);
            //}

            timeLeft = timeMS;
            timeLeftSave = timeLeft;
            Stepping.X = valueChange.X / timeMS;
            Stepping.Y = valueChange.Y / timeMS;
            Stepping.Z = valueChange.Z / timeMS;
        }

        abstract protected void resetOriginalPosition();

        abstract public AbsMotion CloneMe(AbsDraw newTargetObj, bool addToUpdate);

        abstract protected bool UpdateMotion(float time);


        public void ignoreImageRender()
        {
            checkInRenderList = false;
        }
        //public override UpdateType uType
        //{
        //    get
        //    {
        //        return UpdateType.Motion;
        //    }
        //}
        

        public void startDelay(Engine.GameState state, int delayMS)
        {
            Timer.UpdateTrigger trigger = new VikingEngine.Timer.UpdateTrigger(delayMS, this, true);
        }

        public override string ToString()
        {
            return "Motion" + (In3d ? "3d" : "2d") + "{" + motionType.ToString() + "}";
        }
    
    }
}
