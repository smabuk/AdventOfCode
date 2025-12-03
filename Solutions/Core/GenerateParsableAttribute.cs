namespace AdventOfCode.Solutions.Core;

/// <summary>
/// Marks a type for automatic generation of IParsable&lt;T&gt; boilerplate methods.
/// The type must implement Parse(string s, IFormatProvider? provider).
/// The generator will create Parse(string s) and TryParse(...) methods automatically.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, Inherited = false, AllowMultiple = false)]
public sealed class GenerateParsableAttribute : Attribute
{
}
