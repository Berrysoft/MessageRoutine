#nullable enable

using System;

namespace MessageRoutine
{
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false)]
    public sealed class FromMessageAttribute : Attribute
    {
    }
}
