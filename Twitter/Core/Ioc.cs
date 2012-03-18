using System;
using System.Collections.Generic;

namespace Tweety.Core
{
    /// <summary>
    /// Simple implementation of an Ioc Container.
    /// </summary>
    public static class Ioc
    {
        public delegate object Creator();

        private static readonly Dictionary<Type, Creator> TypeToCreator
                       = new Dictionary<Type, Creator>();

        public static void Register<T>(Creator creator) {
            TypeToCreator.Add(typeof(T), creator);
        }

        public static T Create<T>() {
            return (T)TypeToCreator[typeof(T)]();
        }
    }
}
