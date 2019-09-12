#nullable enable

using System;

namespace MessageRoutine
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public sealed class MessageAttribute : Attribute
    {
        public string Name { get; }
        public MessageAttribute(string name) => Name = name;
    }
}
