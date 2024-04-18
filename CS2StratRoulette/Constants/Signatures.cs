using CounterStrikeSharp.API.Modules.Memory;
using CounterStrikeSharp.API.Modules.Memory.DynamicFunctions;

namespace CS2StratRoulette.Constants
{
	public static class Signatures
	{
		public const string GetModelSignatureWindows =
			@"\x40\x53\x48\x83\xEC\x20\x48\x8B\x41\x30\x48\x8B\xD9\x48\x8B\x48\x08\x48\x8B\x01\x2A\x2A\x2A\x48\x85";

		public const string GetModelSignatureLinux =
			@"\x55\x48\x89\xE5\x53\x48\x89\xFB\x48\x83\xEC\x08\x48\x8B\x47\x38";

		public const string CSmokeGrenadeProjectileLinux =
			@"\x55\x4c\x89\xc1\x48\x89\xe5\x41\x57\x41\x56\x49\x89\xd6\x48\x89\xf2\x48\x89\xfe\x41\x55\x45\x89\xcd\x41\x54\x4d\x89\xc4\x53\x48\x83\xec\x28\x48\x89\x7d\xb8\x48";

		public static readonly VirtualFunctionWithReturn<System.IntPtr, string> GetModel =
			new(Signatures.GetModelSignatureLinux);

		public static readonly MemoryFunctionWithReturn<nint, nint, nint, nint, nint, nint, nint, int> CreateSmoke =
			new(Signatures.CSmokeGrenadeProjectileLinux);
	}
}
