using System;
using System.Collections.Generic;

namespace Core
{
    /// <summary>
    /// Simple implementation of an Ioc Container.
    /// </summary>
    static class Ioc
    {
        public delegate object Creator();

        private static readonly Dictionary<Type, Creator> _typeToCreator
                       = new Dictionary<Type, Creator>();

        public static void Register<T>(Creator creator) {
            _typeToCreator.Add(typeof(T), creator);
        }

        public static T Create<T>() {
            return (T)_typeToCreator[typeof(T)]();
        }
    }
}
