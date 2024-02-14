using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace VikingEngine.HUD
{
    interface IFieldReflector<T>
    {
        T Get();
        void Set(T x);
        void DeleteMe();
    }

    struct ReflectedStructInClassField<T> : IFieldReflector<T> 
    {
        /* Fields */
        FieldInfo modified;
        FieldInfo wrapped;
        object wrapper;

        /* Constructors */
        public ReflectedStructInClassField(object wrapper, FieldInfo modified, FieldInfo wrapped)
        {
            this.modified = modified;
            this.wrapped = wrapped;
            this.wrapper = wrapper;
        }

        /* Novelty Methods */
        public T Get()
        {
            //TypedReference r = TypedReference.MakeTypedReference(wrapper, new FieldInfo[] { wrapped });
            //return (T)modified.GetValueDirect(r);
            throw new NotImplementedException();
        }

        public void Set(T x)
        {
            //TypedReference r = TypedReference.MakeTypedReference(wrapper, new FieldInfo[] { wrapped });
            //modified.SetValueDirect(r, x);
        }

        public void DeleteMe()
        { }
    }

    struct ReflectedClassField<T> : IFieldReflector<T>
    {
        /* Fields */
        FieldInfo fi;
        object instance;

        /* Constructors */
        public ReflectedClassField(object instance, FieldInfo fi)
        {
            this.instance = instance;
            this.fi = fi;
        }

        /* Novelty Methods */
        public T Get()
        {
            return (T)fi.GetValue(instance);
        }

        public void Set(T x)
        {
            fi.SetValue(instance, x);
        }

        public void DeleteMe()
        {
            instance = null;
        }
    }

    class GuiTweakableAttribute : Attribute
    {
        public GuiTweakableAttribute()
        { }
    }

    class GuiSliderAttribute : GuiTweakableAttribute
    {
        public IntervalF interval;

        public GuiSliderAttribute(float minValue, float maxValue)
        {
            interval = new IntervalF(minValue, maxValue);
        }
    }

    class GuiCheckboxAttribute : GuiTweakableAttribute
    {
        public GuiCheckboxAttribute()
        { }
    }

    class GuiLabelAttribute : GuiTweakableAttribute
    {
        public GuiLabelAttribute()
        { }
    }

    static class GuiAutoReflection
    {
        public static void StructInstance(Type modify, object wrapperClass, FieldInfo structField, bool attributeLess, GuiLayout layout)
        {
            //if (modify.IsValueType)
            //    return;
            //var fields = modify.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            //foreach (var field in fields)
            //{
            //    TypedReference r = TypedReference.MakeTypedReference(wrapperClass, new FieldInfo[] { structField });
            //    object o = field.GetValueDirect(r);

            //    if (o != null)
            //    {
            //        if (attributeLess)
            //        {
            //            // Attribute-less version. Not necessarily useful, more likely to cause unexpected problems. Use at own risk.
            //            if (o != null)
            //            {
            //                string name = field.FieldType.Name.ToString() + ": " + field.Name;
            //                if (o is float)
            //                {
            //                    IFieldReflector<float> mirror = new ReflectedStructInClassField<float>(wrapperClass, field, structField);
            //                    new GuiInstanceFloatSlider(name, mirror, new IntervalF(float.MinValue, float.MaxValue), false, layout);
            //                }
            //                else if (o is int)
            //                {
            //                    IFieldReflector<int> mirror = new ReflectedStructInClassField<int>(wrapperClass, field, structField);
            //                    new GuiInstanceIntSlider(name, mirror, new IntervalF(int.MinValue, int.MaxValue - 1000), false, layout);
            //                }
            //                else if (o is bool)
            //                {
            //                    IFieldReflector<bool> mirror = new ReflectedStructInClassField<bool>(wrapperClass, field, structField);
            //                    new GuiInstanceCheckbox(name, null, mirror, layout);
            //                }
            //                else if (o is string)
            //                {
            //                    IFieldReflector<string> mirror = new ReflectedStructInClassField<string>(wrapperClass, field, structField);
            //                    new GuiInstanceLabel(mirror, layout);
            //                }
            //            }
            //        }
            //        else
            //        {
            //            object[] attributes = field.GetCustomAttributes(typeof(GuiTweakableAttribute), true);
            //            if (attributes != null && attributes.Length > 0)
            //            {
            //                GuiTweakableAttribute attr = (GuiTweakableAttribute)attributes[0];
            //                if (attr is GuiSliderAttribute)
            //                {
            //                    GuiSliderAttribute slider = (GuiSliderAttribute)attr;
            //                    if (o is float)
            //                    {
            //                        IFieldReflector<float> fgsp = new ReflectedStructInClassField<float>(wrapperClass, field, structField);
            //                        new GuiInstanceFloatSlider(field.Name, fgsp, slider.interval, false, layout);
            //                    }
            //                    else if (o is int)
            //                    {
            //                        IFieldReflector<int> fgsp = new ReflectedStructInClassField<int>(wrapperClass, field, structField);
            //                        new GuiInstanceIntSlider(field.Name, fgsp, slider.interval, false, layout);
            //                    }
            //                }
            //                else if (attr is GuiCheckboxAttribute)
            //                {
            //                    GuiCheckboxAttribute checkbox = (GuiCheckboxAttribute)attr;
            //                    if (o is bool)
            //                    {
            //                        IFieldReflector<bool> mirror = new ReflectedStructInClassField<bool>(wrapperClass, field, structField);
            //                        new GuiInstanceCheckbox(field.Name, null, mirror, layout);
            //                    }
            //                }
            //                else if (attr is GuiLabelAttribute)
            //                {
            //                    GuiLabelAttribute checkbox = (GuiLabelAttribute)attr;
            //                    if (o is string)
            //                    {
            //                        IFieldReflector<string> mirror = new ReflectedStructInClassField<string>(wrapperClass, field, structField);
            //                        new GuiInstanceLabel(mirror, layout);
            //                    }
            //                }
            //            }
            //        }
            //    }
            //}
        }

        public static void ClassInstance(object instance, bool attributeLess, GuiLayout layout)
        {
            new GuiLabel(instance.GetType().Name + " " + instance.GetHashCode().ToString(), layout);
            
            var fields = instance.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (var field in fields)
            {
                object o = field.GetValue(instance);

                if (o != null)
                {
                    if (attributeLess)
                    {
                        // Attribute-less version. Not necessarily useful, more likely to cause unexpected problems. Use at own risk.
                        if (o != null)
                        {
                            string name = field.FieldType.Name.ToString() + ": " + field.Name;
                            if (o is float)
                            {
                                IFieldReflector<float> mirror = new ReflectedClassField<float>(instance, field);
                                new GuiInstanceFloatSlider(name, mirror, new IntervalF(float.MinValue, float.MaxValue), false, layout);
                            }
                            else if (o is int)
                            {
                                IFieldReflector<int> mirror = new ReflectedClassField<int>(instance, field);
                                new GuiInstanceIntSlider(name, mirror, new IntervalF(int.MinValue, int.MaxValue - 1000), false, layout);
                            }
                            else if (o is bool)
                            {
                                IFieldReflector<bool> mirror = new ReflectedClassField<bool>(instance, field);
                                new GuiInstanceCheckbox(name, null, mirror, layout);
                            }
                            else if (o is string)
                            {
                                IFieldReflector<string> mirror = new ReflectedClassField<string>(instance, field);
                                new GuiInstanceLabel(mirror, layout);
                            }
                            else
                            {
                                //if (o.GetType().IsClass)
                                //{
                                //    new GuiTextButton(name, null, new GuiAction4Arg<object, string, bool, Gui>(PushClassInstanceLayoutNoReturn, o, name, attributeLess, layout.gui), true, layout);
                                //}
                                //else
                                    StructInstance(o.GetType(), instance, field, attributeLess, layout);
                            }
                        }
                    }
                    else
                    {
                    //    var attributes = field.GetCustomAttributes(typeof(GuiTweakableAttribute), true);
                    //    if (attributes != null && attributes.Length > 0)
                    //    {
                    //        GuiTweakableAttribute attr = (GuiTweakableAttribute)attributes[0];
                    //        if (attr is GuiSliderAttribute)
                    //        {
                    //            GuiSliderAttribute slider = (GuiSliderAttribute)attr;
                    //            if (o is float)
                    //            {
                    //                IFieldReflector<float> mirror = new ReflectedClassField<float>(instance, field);
                    //                new GuiInstanceFloatSlider(field.Name, mirror, slider.interval, false, layout);
                    //            }
                    //            else if (o is int)
                    //            {
                    //                IFieldReflector<int> mirror = new ReflectedClassField<int>(instance, field);
                    //                new GuiInstanceIntSlider(field.Name, mirror, slider.interval, false, layout);
                    //            }
                    //        }
                    //        else if (attr is GuiCheckboxAttribute)
                    //        {
                    //            GuiCheckboxAttribute checkbox = (GuiCheckboxAttribute)attr;
                    //            if (o is bool)
                    //            {
                    //                IFieldReflector<bool> mirror = new ReflectedClassField<bool>(instance, field);
                    //                new GuiInstanceCheckbox(field.Name, null, mirror, layout);
                    //            }
                    //        }
                    //        else if (attr is GuiLabelAttribute)
                    //        {
                    //            GuiLabelAttribute checkbox = (GuiLabelAttribute)attr;
                    //            if (o is string)
                    //            {
                    //                IFieldReflector<string> mirror = new ReflectedClassField<string>(instance, field);
                    //                new GuiInstanceLabel(mirror, layout);
                    //            }
                    //        }
                    //        else
                    //        {
                    //            //if (o.GetType().IsClass)
                    //            //{
                    //            //    string name = field.FieldType.Name.ToString() + " " + field.Name;
                    //            //    new GuiTextButton(name, null, new GuiAction4Arg<object, string, bool, Gui>(PushClassInstanceLayoutNoReturn, o, name, attributeLess, layout.gui), true, layout);
                    //            //}
                    //            //else
                    //                StructInstance(o.GetType(), instance, field, attributeLess, layout);
                    //        }
                    //    }
                    }
                }
            }

            var methods = instance.GetType().GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            foreach (var method in methods)
            {
                if (method.ReturnType == typeof(void))
                {
                    var parameters = method.GetParameters();
                    if (parameters.Length == 0)
                    {
                        new GuiTextButton(method.Name, "", new GuiAction2ArgNoReturn<object, object[]>(method.Invoke, instance, null), true, layout);
                    }
                }
            }
        }

        public static GuiLayout PushClassInstanceLayout(object instance, string name, bool attributeLess, Gui menu)
        {
            GuiLayout layout = new GuiLayout(name, menu);
            {
                ClassInstance(instance, attributeLess, layout);
            }
            layout.End();

            return layout;
        }

        public static void PushClassInstanceLayoutNoReturn(object instance, string name, bool attributeLess, Gui menu)
        {
            PushClassInstanceLayout(instance, name, attributeLess, menu);
        }

        public static void GuiButtonOpenClassInstance(object instance, string name, bool attributeLess, GuiLayout layout)
        {
            new GuiTextButton(name, null, new GuiAction4Arg<object, string, bool, Gui>(PushClassInstanceLayoutNoReturn, instance, name, attributeLess, layout.gui), true, layout);
        }
    }
}
