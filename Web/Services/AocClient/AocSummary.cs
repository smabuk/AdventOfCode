using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Web
{
    public class AocSummary
    {
		public string UserName { get; set; } = null!;
		public int NoOfStars { get; set; }
		public Dictionary<int, DailySummary> Days { get; set; } = new();
	}

	public class DailySummary {
		public int Day { get; set; }
		public int NoOfStars { get; set; }
		public string PictureLine { get; set; } = null!;
	}
}
