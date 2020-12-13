using AdventOfCode.Solutions;

using Xunit;

namespace AdventOfCode.Tests.Year2015 {
	public class Tests_19_Medicine_for_Rudolph {
		[Theory]
		[InlineData(new string[] {
			"H => HO",
			"H => OH",
			"O => HH",
			"",
			"HOH"       
		}, 4)]
		[InlineData(new string[] {
			"H => HO",
			"H => OH",
			"O => HH",
			"",
			"HOHOHO"       
		}, 7)]
		public void Part1(string[] input, int expected) {
			_ = int.TryParse(SolutionRouter.SolveProblem(2015, 19, 1, input), out int actual);
			Assert.Equal(expected, actual);
		}

		[Theory]
		[InlineData(new string[] {
			"e => H",
			"e => O",
			"H => HO",
			"H => OH",
			"O => HH",
			"",
			"HOH"
		}, 3)]
		[InlineData(new string[] {
			"e => H",
			"e => O",
			"H => HO",
			"H => OH",
			"O => HH",
			"",
			"HOHOHO"
		}, 6)]
		[InlineData(new string[] {
			"Al => ThF",
			"Al => ThRnFAr",
			"B => BCa",
			"B => TiB",
			"B => TiRnFAr",
			"Ca => CaCa",
			"Ca => PB",
			"Ca => PRnFAr",
			"Ca => SiRnFYFAr",
			"Ca => SiRnMgAr",
			"Ca => SiTh",
			"F => CaF",
			"F => PMg",
			"F => SiAl",
			"H => CRnAlAr",
			"H => CRnFYFYFAr",
			"H => CRnFYMgAr",
			"H => CRnMgYFAr",
			"H => HCa",
			"H => NRnFYFAr",
			"H => NRnMgAr",
			"H => NTh",
			"H => OB",
			"H => ORnFAr",
			"Mg => BF",
			"Mg => TiMg",
			"N => CRnFAr",
			"N => HSi",
			"O => CRnFYFAr",
			"O => CRnMgAr",
			"O => HP",
			"O => NRnFAr",
			"O => OTi",
			"P => CaP",
			"P => PTi",
			"P => SiRnFAr",
			"Si => CaSi",
			"Th => ThCa",
			"Ti => BP",
			"Ti => TiTi",
			"e => HF",
			"e => NAl",
			"e => OMg",
			"",
			"ORnPBPMgArCaCaCaSiThCaCaSiThCaCaPBSiRnFArRnFArCaCaSiThCaCaSiThCaCaCaCaCaCaSiRnFYFArSiRnMgArCaSiRnPTiTiBFYPBFArSiRnCaSiRnTiRnFArSiAlArPTiBPTiRnCaSiAlArCaPTiTiBPMgYFArPTiRnFArSiRnCaCaFArRnCaFArCaSiRnSiRnMgArFYCaSiRnMgArCaCaSiThPRnFArPBCaSiRnMgArCaCaSiThCaSiRnTiMgArFArSiThSiThCaCaSiRnMgArCaCaSiRnFArTiBPTiRnCaSiAlArCaPTiRnFArPBPBCaCaSiThCaPBSiThPRnFArSiThCaSiThCaSiThCaPTiBSiRnFYFArCaCaPRnFArPBCaCaPBSiRnTiRnFArCaPRnFArSiRnCaCaCaSiThCaRnCaFArYCaSiRnFArBCaCaCaSiThFArPBFArCaSiRnFArRnCaCaCaFArSiRnFArTiRnPMgArF",
		}, 6)]
		public void Part2(string[] input, int expected) {
			_ = int.TryParse(SolutionRouter.SolveProblem(2015, 19, 2, input, true), out int actual);
			Assert.Equal(expected, actual);
		}
	}
}
