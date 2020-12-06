using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Services {
    public interface IInputDataService
    {
		Task<string> GetInputData(int year, int day) => throw new NotImplementedException();
		Task<string> GetInputData(int year, int day, string? username = null) => throw new NotImplementedException();
		Task<bool> SaveInputData(string data, int year, int day, string? username = null) => throw new NotImplementedException();
		Task<bool> SaveInputData(IEnumerable<string> data, int year, int day, string? username = null) => throw new NotImplementedException();
	}
}
