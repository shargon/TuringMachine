using System;
using System.Collections.Generic;
using System.Reflection;

namespace TuringMachine.Core.Helpers
{
    public class ReflectionHelper
    {
        /// <summary>
        /// Devuelve todos los tipos que implementan el interface
        /// </summary>
        /// <param name="ti">Tipo</param>
        /// <param name="asms">Ensamblados donde comprobar</param>
        public static IEnumerable<Type> GetTypesAssignableFrom(Type ti, params Assembly[] asms)
        {
            foreach (Assembly asm in asms)
                foreach (Type tp in asm.GetTypes())
                {
                    if (ti != tp && ti.IsAssignableFrom(tp))
                    {
                        yield return tp;
                    }
                }
        }
        /// <summary>
        /// Devuelve si tiene algún constructor público y sin parámetros
        /// </summary>
        /// <param name="tp">Tipo</param>
        public static bool HavePublicConstructor(Type tp)
        {
            foreach (ConstructorInfo o in tp.GetConstructors())
            {
                if (!o.IsPublic) continue;
                ParameterInfo[] pi = o.GetParameters();

                if (pi != null && pi.Length > 0) continue;
                return true;
            }
            return false;
        }
    }
}