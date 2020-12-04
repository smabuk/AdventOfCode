using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Web
{
    public class SessionState
    {
		public string UserName { get; set; } = "";
		public Dictionary<int, int> NoOfStars { get; set; } = new();
		public bool IsSummaryLoaded(int year) => NoOfStars.ContainsKey(year);
		public Dictionary<Tuple<int, int, int>, string> ProblemDescriptions { get; set; } = new();
		public Dictionary<Tuple<int, int>, string> ProblemInputs { get; set; } = new();


		public bool DoesNOfStarsExist(int year) => NoOfStars.ContainsKey(year);
		public int GetNoOfStars(int year) => NoOfStars.ContainsKey(year) ? NoOfStars[year] : 0;
		public void SetNoOfStars(int year, int value) {
			if (NoOfStars.ContainsKey(year)) {
				NoOfStars[year] = value;
			} else {
				NoOfStars.Add(year, value);
			}
		}

		public bool DoesProblemDescriptionExist(int year, int day, int problemNo) => ProblemDescriptions.ContainsKey(new(year, day, problemNo));
		public string GetProblemDescription(int year, int day, int problemNo) => ProblemDescriptions.ContainsKey(new (year, day, problemNo)) ? ProblemDescriptions[new(year, day, problemNo)] : "";
		public void SetProblemDescription(int year, int day, int problemNo, string value) {
			Tuple<int, int, int> key = new(year, day, problemNo);
			if (ProblemDescriptions.ContainsKey(key)) {
				ProblemDescriptions[key] = value;
			} else {
				ProblemDescriptions.Add(key, value);
			}
		}

		public bool DoesProblemInputExist(int year, int day) => ProblemInputs.ContainsKey(new(year, day));
		public string GetProblemInput(int year, int day) => ProblemInputs.ContainsKey(new (year, day)) ? ProblemInputs[new(year, day)] : "";
		public void SetProblemInput(int year, int day, string value) {
			Tuple<int, int> key = new(year, day);
			if (ProblemInputs.ContainsKey(key)) {
				ProblemInputs[key] = value;
			} else {
				ProblemInputs.Add(key, value);
			}
		}

	}
}
