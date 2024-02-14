using VikingEngine.EngineSpace.Maths;
using VikingEngine.LootFest.GO;
using VikingEngine.LootFest.GO.Characters;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.Graphics
{
    /* Interfaces */
    interface IFunction<T>
    {
        T Evaluate();
    }

    /// <summary>
    /// Constant Function. Name abbreviated since struct is very frequently used.
    /// </summary>
    struct ConFunc<T> : IFunction<T>
    {
        /* Fields */
        T value;

        /* Constructors */
        public ConFunc(T value)
        {
            this.value = value;
        }

        /* Interface Methods */
        public T Evaluate()
        {
            return value;
        }
    }

    /// <summary>
    /// Delegate Function. Name abbreviated since struct is very frequently used.
    /// </summary>
    struct DelFunc<T> : IFunction<T>
    {
        /* Delegates */
        public delegate T Function();

        /* Fields */
        Function func;

        /* Constructors */
        public DelFunc(Function func)
        {
            this.func = func;
        }

        /* Interface Methods */
        public T Evaluate()
        {
            return func();
        }
    }

    /// <summary>
    /// Delegate Function. Name abbreviated since struct is very frequently used.
    /// </summary>
    struct DelFunc1Arg<T, A1> : IFunction<T>
    {
        /* Delegates */
        public delegate T Function(A1 arg);

        /* Fields */
        Function func;
        A1 arg;

        /* Constructors */
        public DelFunc1Arg(Function func, A1 arg)
        {
            this.func = func;
            this.arg = arg;
        }

        /* Interface Methods */
        public T Evaluate()
        {
            return func(arg);
        }
    }

    interface ITerminationCriterion
    {
        float Completeness { get; }
        bool IsMet(float time_ms);
    }

    struct OnceCriterion : ITerminationCriterion
    {
        /* Properties */
        public float Completeness { get { return 1; } }

        /* Fields */
        bool done;

        /* Interface Methods */
        public bool IsMet(float time_ms)
        {
            if (!done)
            {
                done = true;
                return false;
            }
            return true;
        }
    }

    struct TimerCriterion : ITerminationCriterion
    {
        /* Constants */
        const float MILLISECONDS_TO_SECONDS = 0.001f;

        /* Properties */
        public float Completeness { get { return timer / duration; } }

        /* Fields */
        public float duration;
        public float timer;

        /* Constructors */
        public TimerCriterion(float duration)
        {
            this.duration = duration;
            timer = 0;
        }

        /* Interface Methods */
        public bool IsMet(float time_ms)
        {
            timer += time_ms * MILLISECONDS_TO_SECONDS;
            return timer >= duration;
        }
    }

    struct EventCriterion : ITerminationCriterion
    {
        /* Properties */
        public float Completeness { get { return 1; } }

        /* Fields */
        IFunction<bool> criterion;

        /* Constructors */
        public EventCriterion(DelFunc<bool>.Function func)
        {
            this.criterion = new DelFunc<bool>(func);
        }

        /* Interface Methods */
        public bool IsMet(float time_ms) { return criterion.Evaluate(); }
    }

    struct EventCriterion1Arg<A1> : ITerminationCriterion
    {
        /* Properties */
        public float Completeness { get { return 1; } }

        /* Fields */
        DelFunc1Arg<bool, A1> criterion;

        /* Constructors */
        public EventCriterion1Arg(DelFunc1Arg<bool, A1>.Function func, A1 arg)
        {
            criterion = new DelFunc1Arg<bool,A1>(func, arg);
        }

        /* Interface Methods */
        public bool IsMet(float time_ms) { return criterion.Evaluate(); }
    }

    /* Classes */
    //abstract class AbsCinematicCmd
    //{
    //    /* Events */
    //    public event Action OnComplete;

    //    /* Fields */
    //    protected ITerminationCriterion criterion;

    //    /* Constructors */
    //    public AbsCinematicCmd(ITerminationCriterion criterion)
    //    {
    //        this.criterion = criterion;
    //    }

    //    /* Novelty Methods */
    //    public virtual void Init(AbsCamera camera) { }

    //    /// <summary>
    //    /// Returns true when done
    //    /// </summary>
    //    public bool Update(float time_ms, AbsCamera camera)
    //    {
    //        if (criterion.IsMet(time_ms))
    //            return true;
            
    //        Execute(time_ms, camera);
    //        return false;
    //    }

    //    public abstract void Execute(float time_ms, AbsCamera camera);

    //    public virtual void Destroy()
    //    {
    //        if (OnComplete != null)
    //            OnComplete();
    //    }
    //}

    //class CamCmd_Offset : AbsCinematicCmd
    //{
    //    /* Fields */
    //    IFunction<Vector3> target;
    //    Vector3 start;

    //    /* Constructors */
    //    public CamCmd_Offset(IFunction<Vector3> target, ITerminationCriterion criterion)
    //        : base(criterion)
    //    {
    //        this.target = target;
    //    }

    //    public CamCmd_Offset(Vector3 target, ITerminationCriterion criterion)
    //        : this(new ConFunc<Vector3>(target), criterion)
    //    { }

    //    public CamCmd_Offset(DelFunc<Vector3>.Function target, ITerminationCriterion criterion)
    //        : this(new DelFunc<Vector3>(target), criterion)
    //    { }

    //    /* Family Methods */
    //    public override void Init(AbsCamera camera)
    //    {
    //        start = camera.positionOffset;
    //    }

    //    public override void Execute(float time_ms, AbsCamera camera)
    //    {
    //        Vector3 target = this.target.Evaluate();
    //        camera.positionOffset = Mgth.Cerp(start, target, criterion.Completeness);
    //    }
    //}

    //class CamCmd_Zoom : AbsCinematicCmd
    //{
    //    /* Fields */
    //    IFunction<float> target;
    //    float start;

    //    /* Constructors */
    //    public CamCmd_Zoom(IFunction<float> target, ITerminationCriterion criterion)
    //        : base(criterion)
    //    {
    //        this.target = target;
    //    }

    //    public CamCmd_Zoom(float target, ITerminationCriterion criterion)
    //        : this(new ConFunc<float>(target), criterion)
    //    { }

    //    public CamCmd_Zoom(DelFunc<float>.Function target, ITerminationCriterion criterion)
    //        : this(new DelFunc<float>(target), criterion)
    //    { }

    //    /* Family Methods */
    //    public override void Init(AbsCamera camera)
    //    {
    //        start = camera.CurrentZoom;
    //    }

    //    public override void Execute(float time_ms, AbsCamera camera)
    //    {
    //        float target = this.target.Evaluate();
    //        camera.CurrentZoom = Mgth.Cerp(start, target, criterion.Completeness);
    //    }
    //}

    //class CamCmd_YawRot : AbsCinematicCmd
    //{
    //    /* Static */
    //    public static float DirToAngle(Vector2 vec) { return (float)Math.Atan2(-vec.Y, -vec.X); }
    //    public static float DirToAngle(Vector3 vec) { return (float)Math.Atan2(-vec.Z, -vec.X); }
    //    public static float DirToAngle(Dir4 dir) { Vector2 vec = lib.Dir4ToIntVec2(dir, 1).Vec; return (float)Math.Atan2(-vec.Y, -vec.X); }

    //    /* Fields */
    //    IFunction<float> target;
    //    float start;

    //    /* Constructors */
    //    public CamCmd_YawRot(IFunction<float> target, ITerminationCriterion criterion)
    //        : base(criterion)
    //    {
    //        this.target = target;
    //    }

    //    public CamCmd_YawRot(float target, ITerminationCriterion criterion)
    //        : this(new ConFunc<float>(target), criterion)
    //    { }

    //    public CamCmd_YawRot(DelFunc<float>.Function target, ITerminationCriterion criterion)
    //        : this(new DelFunc<float>(target), criterion)
    //    { }

    //    /* Family Methods */
    //    public override void Init(AbsCamera camera)
    //    {
    //        this.start = camera.TiltX;
    //        float end = target.Evaluate();
    //        if (end - start > MathHelper.Pi)
    //        {
    //            start += MathHelper.TwoPi;
    //        }
    //        else if (end - start < -MathHelper.Pi)
    //        {
    //            start -= MathHelper.TwoPi;
    //        }
    //    }

    //    public override void Execute(float time_ms, AbsCamera camera)
    //    {
    //        float target = this.target.Evaluate();
    //        camera.TiltX = Mgth.Cerp(start, target, criterion.Completeness);
    //    }
    //}

    //class CamCmd_PitchRot : AbsCinematicCmd
    //{
    //    /* Constants */
    //    public const float AngleFromTop = 0;
    //    public const float Angle45DegAbove = MathHelper.PiOver4;
    //    public const float AngleFromSide = MathHelper.PiOver2;

    //    /* Fields */
    //    IFunction<float> target;
    //    float start;

    //    /* Constructors */
    //    public CamCmd_PitchRot(IFunction<float> target, ITerminationCriterion criterion)
    //        : base(criterion)
    //    {
    //        this.target = target;
    //    }

    //    public CamCmd_PitchRot(float target, ITerminationCriterion criterion)
    //        : this(new ConFunc<float>(target), criterion)
    //    { }

    //    public CamCmd_PitchRot(DelFunc<float>.Function target, ITerminationCriterion criterion)
    //        : this(new DelFunc<float>(target), criterion)
    //    { }

    //    /* Family Methods */
    //    public override void Init(AbsCamera camera)
    //    {
    //        this.start = camera.TiltY;
    //    }

    //    public override void Execute(float time_ms, AbsCamera camera)
    //    {
    //        float target = this.target.Evaluate();
    //        camera.TiltY = Mgth.Cerp(start, target, criterion.Completeness);
    //    }
    //}

    //class CamCmd_UsePosFromRot : AbsCinematicCmd
    //{
    //    /* Fields */
    //    IFunction<bool> target;

    //    /* Constructors */
    //    public CamCmd_UsePosFromRot(IFunction<bool> target)
    //        : base(new OnceCriterion())
    //    {
    //        this.target = target;
    //    }

    //    public CamCmd_UsePosFromRot(bool target)
    //        : this(new ConFunc<bool>(target))
    //    { }

    //    public CamCmd_UsePosFromRot(DelFunc<bool>.Function target)
    //        : this(new DelFunc<bool>(target))
    //    { }

    //    /* Family Methods */
    //    public override void Execute(float time_ms, AbsCamera camera)
    //    {
    //        camera.UsePositionFromRotation = target.Evaluate();
    //    }
    //}

    //class CamCmd_FOV : AbsCinematicCmd
    //{
    //    /* Static Readonly */
    //    public static readonly float MaxFOV = AbsCamera.ActualFOVBounds.Max;

    //    /* Fields */
    //    IFunction<float> target;
    //    float start;

    //    /* Constructors */
    //    public CamCmd_FOV(IFunction<float> target, ITerminationCriterion criterion)
    //        : base(criterion)
    //    {
    //        this.target = target;
    //    }

    //    public CamCmd_FOV(float target, ITerminationCriterion criterion)
    //        : this(new ConFunc<float>(target), criterion)
    //    { }

    //    public CamCmd_FOV(DelFunc<float>.Function target, ITerminationCriterion criterion)
    //        : this(new DelFunc<float>(target), criterion)
    //    { }

    //    /* Family Methods */
    //    public override void Init(AbsCamera camera)
    //    {
    //        start = camera.FieldOfView;
    //    }

    //    public override void Execute(float time_ms, AbsCamera camera)
    //    {
    //        float target = this.target.Evaluate();
    //        camera.FieldOfView = Mgth.Cerp(start, target, criterion.Completeness);
    //    }
    //}

    //class CamCmd_MoveCam : AbsCinematicCmd
    //{
    //    /* Fields */
    //    IFunction<Vector3> target;
    //    Vector3 start;

    //    /* Constructors */
    //    public CamCmd_MoveCam(IFunction<Vector3> target, ITerminationCriterion criterion)
    //        : base(criterion)
    //    {
    //        this.target = target;
    //    }

    //    public CamCmd_MoveCam(Vector3 target, ITerminationCriterion criterion)
    //        : this(new ConFunc<Vector3>(target), criterion)
    //    { }

    //    public CamCmd_MoveCam(DelFunc<Vector3>.Function target, ITerminationCriterion criterion)
    //        : this(new DelFunc<Vector3>(target), criterion)
    //    { }

    //    /* Family Methods */
    //    public override void Init(AbsCamera camera)
    //    {
    //        this.start = camera.Position;
    //    }

    //    public override void Execute(float time_ms, AbsCamera camera)
    //    {
    //        Vector3 target = this.target.Evaluate();
    //        camera.Position = Mgth.Cerp(start, target, criterion.Completeness);
    //        camera.LookTarget = camera.GoalLookTarget;
    //    }
    //}

    //class CamCmd_Group : AbsCinematicCmd
    //{
    //    /* Fields */
    //    AbsCinematicCmd[] commands;

    //    /* Constructors */
    //    public CamCmd_Group(params AbsCinematicCmd[] commands)
    //        : base(null)
    //    {
    //        this.commands = commands;
    //        criterion = new EventCriterion(GroupIsDone);
    //    }

    //    /* Family Methods */
    //    public override void Init(AbsCamera camera)
    //    {
    //        foreach (AbsCinematicCmd command in commands)
    //        {
    //            command.Init(camera);
    //        }
    //        base.Init(camera);
    //    }

    //    public override void Execute(float time_ms, AbsCamera camera)
    //    {
    //        for (int i = 0; i != commands.Length; ++i)
    //        {
    //            AbsCinematicCmd command = commands[i];
    //            if (command != null)
    //            {
    //                if (command.Update(time_ms, camera))
    //                {
    //                    commands[i].Destroy();
    //                    commands[i] = null;
    //                }
    //            }
    //        }
    //    }

    //    public override void Destroy()
    //    {
    //        commands = null;
    //        base.Destroy();
    //    }

    //    /* Novelty Methods */
    //    bool GroupIsDone()
    //    {
    //        foreach (AbsCinematicCmd command in commands)
    //        {
    //            if (command != null)
    //                return false;
    //        }
    //        return true;
    //    }
    //}

    //class CharCmd_Move : AbsCinematicCmd
    //{
    //    /* Fields */
    //    AbsCharacter character;
    //    IFunction<Vector3> target;

    //    /* Constructors */
    //    /// <param name="criterion">If null, criterion is set to be met when character reaches target XZ.</param>
    //    public CharCmd_Move(AbsCharacter character, IFunction<Vector3> target, ITerminationCriterion criterion)
    //        : base(criterion)
    //    {
    //        this.character = character;
    //        this.target = target;
    //        if (this.criterion == null)
    //            this.criterion = ReachedTargetCriterion();
    //    }

    //    /// <param name="criterion">If null, criterion is set to be met when character reaches target XZ.</param>
    //    public CharCmd_Move(AbsCharacter character, Vector3 target, ITerminationCriterion criterion)
    //        : this(character, new ConFunc<Vector3>(target), criterion)
    //    { }

    //    /// <param name="criterion">If null, criterion is set to be met when character reaches target XZ.</param>
    //    public CharCmd_Move(AbsCharacter character, DelFunc<Vector3>.Function target, ITerminationCriterion criterion)
    //        : this(character, new DelFunc<Vector3>(target), criterion)
    //    { }

    //    /* Family Methods */
    //    public override void Execute(float time_ms, AbsCamera camera)
    //    {
    //        Vector3 target = this.target.Evaluate();
    //        Vector3 pos = character.Position;
    //        Vector3 dir = target - pos;
    //        dir.Y = 0;
    //        float len = dir.Length();
    //        float speed = (time_ms * 0.001f);
    //        if (len > speed * Ref.DeltaTimeMs)
    //        {
    //            dir /= len;
    //            character.Velocity.Value = dir * speed;
    //        }
    //        else
    //        {
    //            character.Position = target;
    //        }
    //        lib.DoNothing();
    //    }

    //    public override void Destroy()
    //    {
    //        base.Destroy();
    //        character = null;
    //        target = null;
    //    }

    //    /* Novelty Methods */
    //    bool HasReachedTarget()
    //    {
    //        return (character.Position - target.Evaluate()).PlaneXZLengthSq() < 0.0001f;
    //    }

    //    ITerminationCriterion ReachedTargetCriterion()
    //    {
    //        return new EventCriterion(HasReachedTarget);
    //    }
    //}

    //class ActionCmd_2arg<A1, A2> : AbsCinematicCmd
    //{
    //    /* Fields */
    //    Action<A1, A2> action;
    //    A1 arg1;
    //    A2 arg2;

    //    /* Constructors */
    //    public ActionCmd_2arg(Action<A1, A2> action, A1 arg1, A2 arg2, ITerminationCriterion criterion)
    //        : base(criterion)
    //    {
    //        this.action = action;
    //        this.arg1 = arg1;
    //        this.arg2 = arg2;
    //    }

    //    /* Family Methods */
    //    public override void Execute(float time_ms, AbsCamera camera)
    //    {
    //        action(arg1, arg2);
    //    }

    //    public override void Destroy()
    //    {
    //        base.Destroy();
    //        action = null;
    //        arg1 = default(A1);
    //        arg2 = default(A2);
    //    }
    //}

    //class CinematicScriptHandler
    //{
    //    /* Fields */
    //    AbsCamera camera;
    //    Queue<AbsCinematicCmd> commands;
    //    AbsCinematicCmd currentCommand;

    //    /* Constructors */
    //    public CinematicScriptHandler(AbsCamera camera)
    //    {
    //        this.camera = camera;
    //        commands = new Queue<AbsCinematicCmd>();
    //        currentCommand = null;
    //    }

    //    /* Family Methods */
    //    public void Time_Update(float time_ms)
    //    {
    //        if (currentCommand == null)
    //        {
    //            if (commands.Count > 0)
    //            {
    //                currentCommand = commands.Dequeue();
    //                currentCommand.Init(camera);
    //            }
    //            else
    //                return;
    //        }

    //        if (currentCommand.Update(time_ms, camera))
    //        {
    //            currentCommand.Destroy();
    //            currentCommand = null;
    //        }
    //    }

    //    /* Novelty Methods */
    //    public void Enqueue(AbsCinematicCmd command)
    //    {
    //        commands.Enqueue(command);
    //    }

    //    public void Enqueue(params AbsCinematicCmd[] commands)
    //    {
    //        this.commands.Enqueue(new CamCmd_Group(commands));
    //    }

    //    public void ClearQueue()
    //    {
    //        while (commands.Count > 0)
    //        {
    //            currentCommand = commands.Dequeue();
    //            currentCommand.Destroy();
    //            currentCommand = null;
    //        }
    //    }
    //}
}
