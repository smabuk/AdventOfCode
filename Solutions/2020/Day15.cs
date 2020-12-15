using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using AdventOfCode.Solutions.Helpers;

using static AdventOfCode.Solutions.Helpers.ArgumentHelpers;

namespace AdventOfCode.Solutions.Year2020 {
	/// <summary>
	/// Day 15: Rambunctious Recitation
	/// https://adventofcode.com/2020/day/15
	/// </summary>
	public class Day15 {

		record Spoken(int No, int TurnNo, bool FirstTime);

		private static int Solution1(string[] input) {
			string[] inputs = input[0].Split(",");
			Dictionary<int, Spoken> gamePlay = new();
			//int lastNo = 0;
			Spoken lastGo = new(0,0,false);
			for (int i = 1; i <= inputs.Length; i++) {
				lastGo = new Spoken(int.Parse(inputs[i-1]), i, true);
				gamePlay.Add(lastGo.No, lastGo);
			}
			int newLastNo;
			for (int turnNo = inputs.Length + 1; turnNo <= 2020; turnNo++) {
				if (lastGo.FirstTime) {
					newLastNo = 0;
					gamePlay[lastGo.No] = lastGo with
					{
						FirstTime = false
					};
				} else {
					newLastNo = turnNo - 1 - gamePlay[lastGo.No].TurnNo;
					gamePlay[lastGo.No] = lastGo with
					{
						TurnNo = turnNo - 1
					};
				}
				lastGo = new(newLastNo, turnNo, !gamePlay.ContainsKey(newLastNo));
			}


			return lastGo.No;
		}

		private static int Solution2(string[] input) {
			string[] inputs = input[0].Split(",");
			Dictionary<int, Spoken> gamePlay = new();
			Spoken lastGo = new(0, 0, false);
			for (int i = 1; i <= inputs.Length; i++) {
				lastGo = new Spoken(int.Parse(inputs[i - 1]), i, true);
				gamePlay.Add(lastGo.No, lastGo);
			}
			int newLastNo;
			for (int turnNo = inputs.Length + 1; turnNo <= 2020; turnNo++) {
				if (lastGo.FirstTime) {
					newLastNo = 0;
					gamePlay[lastGo.No] = lastGo with
					{
						FirstTime = false
					};
				} else {
					newLastNo = turnNo - 1 - gamePlay[lastGo.No].TurnNo;
					gamePlay[lastGo.No] = lastGo with
					{
						TurnNo = turnNo - 1
					};
				}
				lastGo = new(newLastNo, turnNo, !gamePlay.ContainsKey(newLastNo));
			}


			return lastGo.No;
		}





		#region Problem initialisation
		public static string Part1(string[]? input, params object[]? args) {
			if (input is null) { return "Error: No data provided"; }
			input = input.StripTrailingBlankLineOrDefault();
			return Solution1(input).ToString();
		}
		public static string Part2(string[]? input, params object[]? args) {
			if (input is null) { return "Error: No data provided"; }
			input = input.StripTrailingBlankLineOrDefault();
			return Solution2(input).ToString();
		}
		#endregion

	}
}
