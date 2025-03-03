using System.Reflection;

namespace GreenSpace.WebAPI;

public static class AssemblyReference
{
    public static readonly Assembly Assembly = typeof(AssemblyReference).Assembly;
}
