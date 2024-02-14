#if PCGAME
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Valve.Steamworks;

namespace VikingEngine.SteamWrapping
{
    [StructLayout(LayoutKind.Sequential)]
    class CCallbackBaseVTable
    {
        /* Delegates */
        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        public delegate void RunCallbackDelegate(IntPtr thisPtr, IntPtr pvParam);
        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        public delegate void RunCallResultDelegate(IntPtr thisPtr, IntPtr pvParam, [MarshalAs(UnmanagedType.I1)] bool bIOFailure, ulong hAPICall);
        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        public delegate int GetCallbackSizeBytesDelegate(IntPtr thisPtr);

        /* Fields */
        [NonSerialized]
        [MarshalAs(UnmanagedType.FunctionPtr)]
        public RunCallResultDelegate runCallResult;
        [NonSerialized]
        [MarshalAs(UnmanagedType.FunctionPtr)]
        public RunCallbackDelegate runCallback;
        [NonSerialized]
        [MarshalAs(UnmanagedType.FunctionPtr)]
        public GetCallbackSizeBytesDelegate getCallbackSize;

        /* Constructors */
        public CCallbackBaseVTable(RunCallbackDelegate cbDel, RunCallResultDelegate crDel, GetCallbackSizeBytesDelegate getCbSizeDel)
        {
            runCallback = cbDel;
            runCallResult = crDel;
            getCallbackSize = getCbSizeDel;
        }
    }

    public static class CallbackDispatcher
    {
        // We catch exceptions inside callbacks and reroute them here.
        // For some reason throwing an exception causes RunCallbacks() to break otherwise.
        public static void ExceptionHandler(Exception e)
        {
            Debug.LogError(e.Message);
        }
    }

    class CallbackIdentityAttribute : Attribute
    {
        public int Identity { get; set; }
        public CallbackIdentityAttribute(int callbackNumber)
        {
            Identity = callbackNumber;
        }
    }

    class CallbackIdentities
    {
        public static int GetCallbackIdentity(Type callbackStruct)
        {
            foreach (CallbackIdentityAttribute a in callbackStruct.GetCustomAttributes(typeof(CallbackIdentityAttribute), false))
            {
                return a.Identity;
            }

            throw new Exception("Callback number not found for struct " + callbackStruct.ToString());
            //See [CallbackIdentity(...)] in steam_api_interop.cs
        }
    }

    class SteamCallback<T>
    {
        /* Readonly */
        readonly int structureSize = Marshal.SizeOf(typeof(T));
        
        /* Delegates */
        public delegate void DispatchDelegate(T param);

        /* Events */
        event DispatchDelegate function;

        /* Fields */
        CCallbackBaseVTable vTable;
        IntPtr vTablePointer;
        CCallbackBase callback;
        GCHandle callbackPointer;

        bool isGameServer;

        /* Constructors */
        public SteamCallback(DispatchDelegate func, bool isGameServer)
        {
            this.isGameServer = isGameServer;

            // The callback structure needs a virtual table with three members.
            // A virtual table is in reality a pointer to a structure of function pointers.
            // I've mirrored Steam's layout in the class instanced here.
            vTable = new CCallbackBaseVTable(OnRunCallback, null, null);
            // We need to create a pointer to the structure and marshal it so that native code can use it.
            // This is done by allocating a global handle to (some) memory structure the size of our class.
            vTablePointer = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(CCallbackBaseVTable)));
            // Then we marshal our structure into that global memory.
            Marshal.StructureToPtr(vTable, vTablePointer, false);

            // Now we can create the callback structure
            callback = new CCallbackBase()
            {
                m_vTable = vTablePointer, // and set the pointer to the virtual table
                m_nCallbackFlags = 0,
                m_iCallback = CallbackIdentities.GetCallbackIdentity(typeof(T))
            };

            // Then we make sure that the garbage collector does not move our callback object
            // around in memory to speed up allocation of other stuff. Thus we can finally
            // submit the pointer to the adress of our callback struct to the SteamAPI as
            // the callback object without worrying that the adress becomes invalid.
            callbackPointer = GCHandle.Alloc(callback, GCHandleType.Pinned);

            if (func == null)
                throw new Exception("Callback function must not be null.");

            if ((callback.m_nCallbackFlags & CCallbackBase.k_ECallbackFlagsRegistered) == CCallbackBase.k_ECallbackFlagsRegistered)
            {
                Unregister();
            }

            if (isGameServer)
                callback.m_nCallbackFlags |= CCallbackBase.k_ECallbackFlagsGameServer;

            function = func;

            // k_ECallbackFlagsRegistered is set by SteamAPI.RegisterCallback.
            SteamAPI.RegisterCallback(callbackPointer.AddrOfPinnedObject(), CallbackIdentities.GetCallbackIdentity(typeof(T)));
        }

        /* Destructors */
        ~SteamCallback()
        {
            Unregister();

			if (vTablePointer != IntPtr.Zero)
            {
				Marshal.FreeHGlobal(vTablePointer);
			}

			if (callbackPointer.IsAllocated)
            {
				callbackPointer.Free();
			}
		}

        /* Novelty methods */        
        public void Unregister()
        {
            // k_ECallbackFlagsRegistered is removed by SteamAPI.UnregisterCallback.
            SteamAPI.UnregisterCallback(callbackPointer.AddrOfPinnedObject());
        }

        private void OnRunCallback(IntPtr thisPtr, IntPtr pvParam)
        {
			try
            {
                function((T)Marshal.PtrToStructure(pvParam, typeof(T)));
			}
			catch (Exception e)
            {
				CallbackDispatcher.ExceptionHandler(e);
			}
		}

    }

    class SteamCallResult<T>
    {
        /* Constants */
        const ulong STEAMAPICALLHANDLE_INVALID = 0;

        /* Readonly */
        readonly int structureSize = Marshal.SizeOf(typeof(T));

        /* Delegates */
        public delegate void DispatchDelegate(T value, bool ioFailure);

        /* Events */
        event DispatchDelegate function;

        /* Fields */
        CCallbackBaseVTable vTable;
        IntPtr vTablePointer;
        CCallbackBase callback;
        GCHandle callbackPointer;
        ulong steamAPICallHandle = STEAMAPICALLHANDLE_INVALID;

        /* Constructors */
        public SteamCallResult(DispatchDelegate func)
        {
            function = func;

            // To understand this, see the comments in the SteamCallback<T> class constructor
            vTable = new CCallbackBaseVTable(OnRunCallback, OnRunCallResult, OnGetCallbackSizeBytes);
            vTablePointer = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(CCallbackBaseVTable)));
            Marshal.StructureToPtr(vTable, vTablePointer, false);

            callback = new CCallbackBase()
            {
                m_vTable = vTablePointer,
                m_nCallbackFlags = 0,
                m_iCallback = CallbackIdentities.GetCallbackIdentity(typeof(T))
            };
            callbackPointer = GCHandle.Alloc(callback, GCHandleType.Pinned);
        }

        /* Destructors */
        ~SteamCallResult()
        {
            Cancel();

            if (vTablePointer != IntPtr.Zero)
            {
                Marshal.FreeHGlobal(vTablePointer);
            }

            if (callbackPointer.IsAllocated)
            {
                callbackPointer.Free();
            }
        }

        /* Novelty methods */
        public void Set(ulong hAPICall)
        {
            // Unlike the official SDK we let the user assign a single function during creation,
            // and allow them to skip having to do so every time that they call .Set()

            if (function == null)
            {
                throw new Exception("CallResult function was null, you must either set it in the CallResult Constructor or in Set()");
            }

            if (steamAPICallHandle != STEAMAPICALLHANDLE_INVALID)
            {
                SteamAPI.UnregisterCallResult(callbackPointer.AddrOfPinnedObject(), steamAPICallHandle);
            }

            steamAPICallHandle = hAPICall;

            if (hAPICall != STEAMAPICALLHANDLE_INVALID)
            {
                SteamAPI.RegisterCallResult(callbackPointer.AddrOfPinnedObject(), hAPICall);
            }
        }

        public void Set(ulong hAPICall, DispatchDelegate func)
        {
            if (func != null)
            {
                function = func;
            }

            if (function == null)
            {
                throw new Exception("CallResult function was null, you must either set it in the CallResult Constructor or in Set()");
            }

            if (steamAPICallHandle != STEAMAPICALLHANDLE_INVALID)
            {
                SteamAPI.UnregisterCallResult(callbackPointer.AddrOfPinnedObject(), steamAPICallHandle);
            }

            steamAPICallHandle = hAPICall;

            if (hAPICall != STEAMAPICALLHANDLE_INVALID)
            {
                SteamAPI.RegisterCallResult(callbackPointer.AddrOfPinnedObject(), hAPICall);
            }
        }

        public bool IsActive()
        {
            return steamAPICallHandle != STEAMAPICALLHANDLE_INVALID;
        }

        public void Cancel()
        {
            if (steamAPICallHandle != STEAMAPICALLHANDLE_INVALID)
            {
                SteamAPI.UnregisterCallResult(callbackPointer.AddrOfPinnedObject(), steamAPICallHandle);
                steamAPICallHandle = STEAMAPICALLHANDLE_INVALID;
            }
        }

        // Shouldn't ever get called here, but this is what C++ Steamworks does.
        private void OnRunCallback(IntPtr thisptr, IntPtr pvParam)
        {
            steamAPICallHandle = STEAMAPICALLHANDLE_INVALID; // Caller unregisters for us+
            try
            {
                function((T)Marshal.PtrToStructure(pvParam, typeof(T)), false);
            }
            catch (Exception e)
            {
                CallbackDispatcher.ExceptionHandler(e);
            }
        }

        private void OnRunCallResult(IntPtr thisptr, IntPtr pvParam, bool bFailed, ulong hSteamAPICall)
        {
            if (hSteamAPICall == steamAPICallHandle)
            {
                //try
                //{
                    function((T)Marshal.PtrToStructure(pvParam, typeof(T)), bFailed);
                //}
                //catch (Exception e)
                //{
                //    CallbackDispatcher.ExceptionHandler(e);
                //}

                // The official SDK sets steamAPICallHandle to invalid before calling the callresult function,
                // this doesn't let us access .Handle from within the function though.
                if (hSteamAPICall == steamAPICallHandle) // Ensure that m_hAPICall has not been changed in m_Func
                {
                    steamAPICallHandle = STEAMAPICALLHANDLE_INVALID; // Caller unregisters for us
                }
            }
        }

        private int OnGetCallbackSizeBytes(IntPtr thisptr)
        {
            return structureSize;
        }
    }
}
#endif