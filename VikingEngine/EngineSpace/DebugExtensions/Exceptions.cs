using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine
{
    abstract class AbsVikingException : System.Exception
    {
        public int MessageValue;

        public AbsVikingException(string message, int MessageValue)
            :base(message)
        { }

        abstract public ExceptionType Type { get; }
    }
    

    /// <summary>
    /// A endless loop have been triggered
    /// </summary>
    class EndlessLoopException : AbsVikingException
    {
        public EndlessLoopException(string position, int MessageValue = -1)
            : base("Endless loop, " + position, MessageValue)
        { }

        override public ExceptionType Type { get { return ExceptionType.EndlessLoop; } }
    }

    class OutsideBoundsException : AbsVikingException
    {
        public OutsideBoundsException(string position, int MessageValue = -1)
            : base("Outside bounds, " + position, MessageValue)
        { }

        override public ExceptionType Type { get { return ExceptionType.OutsideBounds; } }
    }

    class CorruptFileException : AbsVikingException
    {
        public CorruptFileException(string position, int MessageValue = -1)
            : base("Corrupt file, " + position, MessageValue)
        { }

        override public ExceptionType Type { get { return ExceptionType.CorruptFile; } }
    }

    class HostDataLostException : AbsVikingException
    {
        public HostDataLostException(string position, int MessageValue = -1)
            : base("Network host data lost, " + position, MessageValue)
        { }

        override public ExceptionType Type { get { return ExceptionType.HostDataLost; } }
    }

    class HostClientMixUpException : AbsVikingException
    {
        public HostClientMixUpException(string position, int MessageValue = -1)
            : base("Host permissions is mixed up with clients, " + position, MessageValue)
        { }

        override public ExceptionType Type { get { return ExceptionType.HostClientMixUp; } }
    }

    class GeneratingDublicateException : AbsVikingException
    {
        public GeneratingDublicateException(string position, int MessageValue = -1)
            : base("object is already generated, " + position, MessageValue)
        { }

        override public ExceptionType Type { get { return ExceptionType.GeneratingDublicate; } }
    }

    class LoadingItemException : AbsVikingException
    {
        public LoadingItemException(string item, int MessageValue = -1)
            : base("Loaded item is corrupt, " + item, MessageValue)
        { }

        override public ExceptionType Type { get { return ExceptionType.LoadingItem; } }
    }

    class MissingReadException : AbsVikingException
    {
        public MissingReadException(string obj, int MessageValue = -1)
            : base("Written object has no read, " + obj, MessageValue)
        { }

        override public ExceptionType Type { get { return ExceptionType.MissingRead; } }
    }

    class EmptyValueException : AbsVikingException
    {
        public EmptyValueException(string obj, int MessageValue = -1)
            : base("Empty value, " + obj, MessageValue)
        { }

        override public ExceptionType Type { get { return ExceptionType.EmptyValue; } }
    }

    class NetworkWriteReadSynchException : AbsVikingException
    {
        public NetworkWriteReadSynchException(string obj, int MessageValue = -1)
            : base("Network write and read out of synch, " + obj, MessageValue)
        { }

        override public ExceptionType Type { get { return ExceptionType.NetworkWriteReadSynch; } }
    }

    class NetworkException : AbsVikingException
    {
        public NetworkException(string obj, int MessageValue = -1)
            : base("Network exception, " + obj, MessageValue)
        { }

        override public ExceptionType Type { get { return ExceptionType.Network; } }
    }

    //EXTENSIONS
    class ExceptionExt : AbsVikingException
    {
        public ExceptionExt(string obj, int MessageValue = -1)
            : base(obj, MessageValue)
        { }

        override public ExceptionType Type { get { return ExceptionType.ExceptionExt; } }
    }

    class NullReferenceExceptionExt : AbsVikingException
    {
        public NullReferenceExceptionExt(string obj, int MessageValue = -1)
            : base("Null Refence: " + obj, MessageValue)
        { }

        override public ExceptionType Type { get { return ExceptionType.NullReferenceExceptionExt; } }
    }

    class IndexOutOfRangeExceptionExt : AbsVikingException
    {
        public IndexOutOfRangeExceptionExt(string obj, int MessageValue = -1)
            : base("Index out of range: " + obj, MessageValue)
        { }

        override public ExceptionType Type { get { return ExceptionType.IndexOutOfRangeExceptionExt; } }
    }

    class NotImplementedExceptionExt : AbsVikingException
    {
        public NotImplementedExceptionExt(string obj, int MessageValue = -1)
            : base("Not impemented: " + obj, MessageValue)
        { }

        override public ExceptionType Type { get { return ExceptionType.NotImplementedExceptionExt; } }
    }

    class TestException : AbsVikingException
    {
        public TestException()
            : base("Test crash", -1)
        { }

        override public ExceptionType Type { get { return ExceptionType.TestException; } }
    }

    enum ExceptionType : byte
    {
        Other,
        NullRef,
        NotImplemented,
        IndexOutOfRange,


        //My Engine
        VikingEngineException_START,

        EndlessLoop,
        OutsideBounds,
        CorruptFile,
        HostDataLost,
        HostClientMixUp,
        GeneratingDublicate,
        LoadingItem,
        MissingRead,
        EmptyValue,
        NetworkWriteReadSynch,
        
        ExceptionExt,
        NullReferenceExceptionExt,
        IndexOutOfRangeExceptionExt,
        NotImplementedExceptionExt,
        TestException,

        Network,
    }
}
