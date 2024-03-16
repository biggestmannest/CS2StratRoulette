using CounterStrikeSharp.API.Modules.Memory;

namespace CS2StratRoulette.Constants
{
	public static class Signatures
	{
		public const string GetModelSignatureWindows =
			@"\x40\x53\x48\x83\xEC\x20\x48\x8B\x41\x30\x48\x8B\xD9\x48\x8B\x48\x08\x48\x8B\x01\x2A\x2A\x2A\x48\x85";

		public const string GetModelSignatureLinux =
			@"\x55\x48\x89\xE5\x53\x48\x89\xFB\x48\x83\xEC\x08\x48\x8B\x47\x38";

		public static readonly VirtualFunctionWithReturn<System.IntPtr, string> GetModel =
			new(Signatures.GetModelSignatureLinux);
	}
}
