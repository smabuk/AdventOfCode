using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Solutions.Helpers
{
    public static class ArgumentHelpers
    {
		public static T GetArgument<T>(object[]? args, int argumentNumber, T defaultResult) {
			if (args is null || args.Length == 0) {
				return defaultResult;
			} else if (args.Length >= argumentNumber && args[argumentNumber - 1] is T x) {
				return x;
			} else {
				throw new ArgumentOutOfRangeException($"{nameof(GetArgument)}: {nameof(argumentNumber)}={argumentNumber}");
			}
		}
		public static T GetArgument<T>(object[]? args, int argumentNumber) {
			if (args is null || args.Length == 0) {
				throw new ArgumentOutOfRangeException($"{nameof(GetArgument)}: No args object");
			} else if (args.Length >= argumentNumber && args[argumentNumber - 1] is T x) {
				return x;
			} else {
				throw new ArgumentOutOfRangeException($"{nameof(GetArgument)}: {nameof(argumentNumber)}={argumentNumber}");
			}
		}

	}
}
