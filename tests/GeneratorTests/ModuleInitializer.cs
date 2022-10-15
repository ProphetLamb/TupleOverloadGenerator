using System.Runtime.CompilerServices;

namespace TupleOverloadGenerator.Tests;

public static class ModuleInitializer {
    [ModuleInitializer]
    public static void Init() {
        VerifySourceGenerators.Enable();
    }
}
