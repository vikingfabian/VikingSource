using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace VikingEngine.EngineSpace.Reflection
{
    class DelayedCtor
    {
        /* Fields*/
        ConstructorInfo ctor;
        object[] defaultArguments;

        /* Constructors */
        public DelayedCtor(Type returnType, params Type[] parameterTypes)
        {
            //ctor = returnType.GetConstructor(BindingFlags.Public | BindingFlags.Instance, Type.DefaultBinder, parameterTypes, null);
            //if (ctor == null)
            //{
            //    throw new NotImplementedException("Could not find a constructor matching the specified parameter list");
            //}
        }

        /* Methods */
        public void FindCtorFromSignature(Type returnType, params Type[] parameterTypes)
        {
            //ctor = returnType.GetConstructor(BindingFlags.Public | BindingFlags.Instance, Type.DefaultBinder, parameterTypes, null);
            //if (ctor == null)
            //{
            //    throw new NotImplementedException("Could not find a constructor matching the specified parameter list");
            //}
        }
        public void SetDefaultArguments(params object[] defaultArguments)
        {
            this.defaultArguments = defaultArguments;
        }
        public object Invoke(params object[] parameters)
        {            
            if (ctor != null)
            {
                if (defaultArguments != null)
                {
                    if (defaultArguments.Length != parameters.Length)
                    {
                        throw new IndexOutOfRangeException("Default argument count must match the parameter count passed to constructor.");
                    }
                    for (int i = 0; i < parameters.Length; ++i)
                    {
                        if (defaultArguments[i] != null)
                        {
                            parameters[i] = defaultArguments[i];
                        }
                    }
                }

                return ctor.Invoke(parameters);
            }
            throw new NullReferenceException("You must set the constructor signature first. Try calling FindCtorFromSignature");
        }
        public T Invoke<T>(params object[] parameters)
        {
            if (ctor != null)
            {
                object result = ctor.Invoke(parameters);
                if (result is T)
                    return (T)result;
                throw new NotImplementedException("Generic must match constructor type");
            }
            throw new NullReferenceException("You must set the constructor signature first. Try calling FindCtorFromSignature");
        }

        private void ReplaceWithDefaults(object[] parameters)
        {
            if (defaultArguments != null)
            {
                if (defaultArguments.Length != parameters.Length)
                {
                    throw new IndexOutOfRangeException("Default argument count must match the parameter count passed to constructor.");
                }
                for (int i = 0; i < parameters.Length; ++i)
                {
                    if (parameters[i] == null)
                    {
                        if (defaultArguments[i] != null)
                        {
                            parameters[i] = defaultArguments[i];
                        }
                    }
                }
            }
        }
    }
}
