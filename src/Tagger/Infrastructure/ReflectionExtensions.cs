using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace Tagger.Infrastructure
{
    static class ReflectionExtensions
    {
        public static PropertyBuilder BuildProperty(this TypeBuilder typeBuilder, string name, Type type, IEnumerable<Type> interfaces)
        {
            var fromInterface = interfaces.ContainsProperty(name);
            // Field
            var fieldBuilder = typeBuilder.DefineField("_" + name, type, FieldAttributes.Private);
            // Property
            var propBuilder = typeBuilder.DefineProperty(name, PropertyAttributes.HasDefault, type, null);
            // Getter
            var getterBuilder = typeBuilder.DefineMethod(
                    string.Concat("get_", name),
                    MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig |
                    MethodAttributes.Virtual,
                    type,
                    Type.EmptyTypes);
            var getterIL = getterBuilder.GetILGenerator();
            getterIL.Emit(OpCodes.Ldarg_0);
            getterIL.Emit(OpCodes.Ldfld, fieldBuilder);
            getterIL.Emit(OpCodes.Ret);
            if (fromInterface)
            {
                typeBuilder.DefineMethodOverride(getterBuilder, interfaces.FindGetter(name));
            }
            //Setter
            var setterBuilder = typeBuilder.DefineMethod(
                string.Concat("set_", name),
                MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig |
                MethodAttributes.Virtual,
                null,
                new[] { type });
            var setterIL = setterBuilder.GetILGenerator();
            setterIL.Emit(OpCodes.Ldarg_0);
            setterIL.Emit(OpCodes.Ldarg_1);
            setterIL.Emit(OpCodes.Stfld, fieldBuilder);
            setterIL.Emit(OpCodes.Ret);
            if (fromInterface)
            {
                typeBuilder.DefineMethodOverride(setterBuilder, interfaces.FindSetter(name));
            }

            propBuilder.SetGetMethod(getterBuilder);
            propBuilder.SetSetMethod(setterBuilder);
            return propBuilder;
        }

        public static bool ContainsProperty(this IEnumerable<Type> types, string propertyName)
        {
            var allNames = (from p in types.SelectMany(t => t.GetProperties())
                           select p.Name).Distinct();
            return allNames.Any(n => n.Equals(propertyName));
        }

        public static MethodInfo FindGetter(this IEnumerable<Type> types, string propertyName)
        {
            var allMethods =
                from m in types.SelectMany(t => t.GetMethods())
                select m;
            return allMethods.Single(m => m.Name.Equals(string.Concat("get_", propertyName)));
        }

        public static MethodInfo FindSetter(this IEnumerable<Type> types, string propertyName)
        {
            var allMethods =
                from m in types.SelectMany(t => t.GetMethods())
                select m;
            return allMethods.Single(m => m.Name.Equals(string.Concat("set_", propertyName)));
        }
    }
}