namespace AdventOfCode.Tests.Year2016;

public class Tests_22_Grid_Computing
{
	const int DAY = 22;

	[Theory]
	[InlineData("""
		root@ebhq-gridcenter# df -h
		Filesystem              Size  Used  Avail  Use%
		/dev/grid/node-x0-y0     94T   21T    22T   76%
		/dev/grid/node-x0-y1     88T   73T    15T   82%
		/dev/grid/node-x0-y2     88T   65T    23T   73%
		/dev/grid/node-x1-y0     91T   67T    24T   73%
		/dev/grid/node-x1-y1     92T   68T    24T   73%
		/dev/grid/node-x1-y2     92T   69T    23T   75%
		/dev/grid/node-x2-y0     86T   65T    21T   75%
		/dev/grid/node-x2-y1     85T   73T    12T   85%
		/dev/grid/node-x2-y2     94T   18T    30T   68%
		/dev/grid/node-x3-y0     85T   68T    17T   80%
		/dev/grid/node-x3-y1     88T   64T    24T   72%
		/dev/grid/node-x3-y2     86T   65T    21T   75%
		/dev/grid/node-x4-y0     86T   66T    20T   76%
		/dev/grid/node-x4-y1     94T   67T    27T   71%
		/dev/grid/node-x4-y2     89T   65T    24T   73%
		/dev/grid/node-x5-y0     92T   72T    20T   78%
		/dev/grid/node-x5-y1     92T   69T    23T   75%
		/dev/grid/node-x5-y2     94T   73T    21T   77%
		/dev/grid/node-x6-y0     87T   70T    17T   80%
		/dev/grid/node-x6-y1     86T   65T    21T   75%
		/dev/grid/node-x6-y2     86T   67T    19T   77%
		/dev/grid/node-x7-y0     91T   69T    22T   75%
		/dev/grid/node-x7-y1     87T   66T    21T   75%
		/dev/grid/node-x7-y2     89T   69T    20T   77%
		""", 34)]
	public void Part1(string input, int expected)
	{
		_ = int.TryParse(SolutionRouter.SolveProblem(YEAR, DAY, PART1, input), out int actual);
		actual.ShouldBe(expected);
	}
}
