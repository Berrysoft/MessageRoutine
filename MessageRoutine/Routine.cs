using System;

namespace MessageRoutine
{
    public struct Routine
    {
        public string? Message;
        public Type? ServiceType;
        public object? Parameter;

        public Routine(string? message) : this(message, null, null) { }
        public Routine(string? message, object? parameter) : this(message, null, parameter) { }
        public Routine(string? message, Type? serviceType, object? parameter)
        {
            Message = message;
            ServiceType = serviceType;
            Parameter = parameter;
        }

        public override bool Equals(object obj) => obj is Routine routine && routine == this;
        public override int GetHashCode() => (Message?.GetHashCode() ?? 0) ^ (ServiceType?.GetHashCode() ?? 0) ^ (Parameter?.GetHashCode() ?? 0);

        public static bool operator ==(Routine left, Routine right) => left.Message == right.Message && left.ServiceType == right.ServiceType && (left.Parameter?.Equals(right.Parameter)).GetValueOrDefault();
        public static bool operator !=(Routine left, Routine right) => !(left == right);
    }
}
