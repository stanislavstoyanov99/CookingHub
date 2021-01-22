namespace CookingHub.Common.Attributes
{
    using System;

    [AttributeUsageAttribute(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class SkipPasswordExpirationCheckAttribute : Attribute
    {
    }
}
