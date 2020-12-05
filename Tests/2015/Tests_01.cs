using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AdventOfCode.Shared;

using Xunit;

namespace AdventOfCode.Tests.Year2015
{
    public class Tests_01
    {
		[Theory]
		[InlineData( new string[] { "(())" }, 0)]
		[InlineData( new string[] { "()()" }, 0)]
		[InlineData( new string[] { "(((" }, 3)]
		[InlineData( new string[] { "(()(()(" }, 3)]
		[InlineData( new string[] { "())" }, -1)]
		[InlineData( new string[] { "))(" }, -1)]
		[InlineData( new string[] { ")))" }, -3)]
		[InlineData( new string[] { ")())())" }, -3)]
		public void Part1_Not_Quite_Lisp(string[] input, int expected) {
			_ = int.TryParse(Solution_2015_01.Part1(input), out int actual);
			Assert.Equal(expected, actual);
		}

		[Theory]
		[InlineData( new string[] { ")" }, 1)]
		[InlineData( new string[] { "()())" }, 5)]
		public void Part2_Not_Quite_Lisp(string[] input, int expected) {
			_ = int.TryParse(Solution_2015_01.Part2(input), out int actual);
			Assert.Equal(expected, actual);
		}
	}
}
