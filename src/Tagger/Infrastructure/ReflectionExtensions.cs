using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Tagger.Infrastructure
{
    static class ReflectionExtensions
    {
        public static PropertyBuilder BuildProperty(this TypeBuilder typeBuilder, string name, Type type)
        {
            // Field
            var fieldBuilder = typeBuilder.DefineField("_" + name, type, FieldAttributes.Private);
            // Property
            var propBuilder = typeBuilder.DefineProperty(name, PropertyAttributes.HasDefault, type, null);
            // Getter
            var getterBuilder = typeBuilder.DefineMethod(
                string.Concat("get_", name),
                MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig,
                type,
                Type.EmptyTypes);
            var getterIL = getterBuilder.GetILGenerator();
            getterIL.Emit(OpCodes.Ldarg_0);
            getterIL.Emit(OpCodes.Ldfld, fieldBuilder);
            getterIL.Emit(OpCodes.Ret);
            //Setter
            var setterBuilder = typeBuilder.DefineMethod(
                string.Concat("set_", name),
                MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig,
                null,
                new[] { type });
            var setterIL = setterBuilder.GetILGenerator();
            setterIL.Emit(OpCodes.Ldarg_0);
            setterIL.Emit(OpCodes.Ldarg_1);
            setterIL.Emit(OpCodes.Stfld, fieldBuilder);
            setterIL.Emit(OpCodes.Ret);

            propBuilder.SetGetMethod(getterBuilder);
            propBuilder.SetSetMethod(setterBuilder);
            return propBuilder;
        }
    }
}
