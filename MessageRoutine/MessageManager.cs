#nullable enable

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace MessageRoutine
{
    public class MessageManager
    {
        public MessageManager()
        {
            serviceTypes = new List<Type>();
            services = new Dictionary<Type, object>();
            singletons = new Dictionary<Type, object>();
        }

        private readonly List<Type> serviceTypes;
        private readonly Dictionary<Type, object> services;
        private readonly Dictionary<Type, object> singletons;

        private Routine? InvokeRoutine(Type? type, MethodInfo? method, string message, object? param)
        {
            if (method != null && type != null && services.TryGetValue(type, out object service))
            {
                int paramsCount = method.GetParameters().Length;
                object?[] paramsArray = paramsCount == 0 ? Array.Empty<object>() : new object?[paramsCount];
                int? messageParamIndex = method.GetParameters().FirstOrDefault(p => p.GetCustomAttributes<FromMessageAttribute>().Any())?.Position;
                if (messageParamIndex != null)
                {
                    paramsArray[messageParamIndex.Value] = message;
                }
                for (int i = 0; i < paramsCount; i++)
                {
                    if (paramsArray[i] == null)
                    {
                        paramsArray[i] = param;
                        break;
                    }
                }
                if (method.Invoke(service, paramsArray) is Routine next)
                {
                    return next;
                }
            }
            return null;
        }

        private (Type?, MethodInfo?) FindMethod(Routine routine)
        {
            if (routine.ServiceType != null)
            {
                return (routine.ServiceType,
                    routine.ServiceType.GetMethods()
                        .FirstOrDefault(m => m.GetCustomAttributes(typeof(MessageAttribute), true)
                            .Any(attr => ((MessageAttribute)attr).Name == routine.Message)));
            }
            else
            {
                return serviceTypes
                    .Reverse<Type>()
                    .Select(t => (t, t.GetMethods()
                        .FirstOrDefault(m => m.GetCustomAttributes(typeof(MessageAttribute), true)
                            .Any(attr => ((MessageAttribute)attr).Name == routine.Message))))
                    .FirstOrDefault(m => m.Item2 != null);
            }
        }

        public object? StartRoutine(Routine routine)
        {
            while (routine.Message != null)
            {
                var (type, method) = FindMethod(routine);
                Routine? next = InvokeRoutine(type, method, routine.Message, routine.Parameter);
                if (next != null)
                {
                    routine = next.Value;
                    continue;
                }
                routine.Message = null;
            }
            return routine.Parameter;
        }

        public T StartRoutine<T>(Routine routine) => StartRoutine(routine) is T result ? result : default;

        public object? StartRoutine(string? message, object? parameter) => StartRoutine(new Routine(message, parameter));

        public T StartRoutine<T>(string? message, object? parameter) => StartRoutine(message, parameter) is T result ? result : default;

        public object? StartRoutine(string? message) => StartRoutine(message, null);

        public T StartRoutine<T>(string? message) => StartRoutine<T>(message, null);

        public MessageManager RegisterService(Type serviceType)
        {
            if (!serviceTypes.Contains(serviceType))
            {
                serviceTypes.Add(serviceType);
                if (!services.ContainsKey(serviceType) || services[serviceType] == null)
                {
                    services.Add(serviceType, ConstructSingleton(serviceType));
                }
            }
            return this;
        }

        public MessageManager RegisterService<T>() => RegisterService(typeof(T));

        public MessageManager UnregisterService(Type serviceType)
        {
            if (serviceTypes.Remove(serviceType))
            {
                services.Remove(serviceType);
            }
            return this;
        }

        public MessageManager UnregisterService<T>() => UnregisterService(typeof(T));

        private object ConstructSingleton(Type type)
        {
            var obj = Activator.CreateInstance(type);
            foreach (var prop in type.GetRuntimeProperties().Where(p => p.GetCustomAttributes(typeof(InjectAttribute), true).Any()))
            {
                if (singletons.ContainsKey(prop.PropertyType))
                {
                    prop.SetValue(obj, singletons[prop.PropertyType]);
                }
            }
            return obj;
        }

        public MessageManager RegisterSingleton(Type interfaceType, Type implementType)
        {
            Debug.Assert(interfaceType.IsAssignableFrom(implementType));
            singletons.Add(interfaceType, ConstructSingleton(implementType));
            return this;
        }

        public MessageManager RegisterSingleton<TInterface, TImplement>() => RegisterSingleton(typeof(TInterface), typeof(TImplement));

        public MessageManager RegisterSingleton(Type type) => RegisterSingleton(type, type);

        public MessageManager RegisterSingleton<T>() => RegisterSingleton(typeof(T));
    }
}
