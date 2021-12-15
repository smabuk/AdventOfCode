namespace AdventOfCode.Solutions.Helpers;

public class Algorithms {

	/// <summary>
	///  Dijkstra's algorithm
	///  https://en.wikipedia.org/wiki/Dijkstra's_algorithm
	/// 
	/// PseudoCode:
	///  function Dijkstra(Graph, source):
	///      dist[source] ← 0                           // Initialization
	/// 
	///      create vertex priority queue Q
	/// 
	///      for each vertex v in Graph:          
	///          if v ≠ source
	///              dist[v] ← INFINITY                 // Unknown distance from source to v
	///              prev[v] ← UNDEFINED                // Predecessor of v
	/// 
	///          Q.add_with_priority(v, dist[v])
	/// 
	/// 
	///      while Q is not empty:                      // The main loop
	///          u ← Q.extract_min()                    // Remove and return best vertex
	///          for each neighbor v of u:              // only v that are still in Q
	///              alt ← dist[u] + length(u, v)
	///              if alt < dist[v]
	///                  dist[v] ← alt
	///                  prev[v] ← u
	///                  Q.decrease_priority(v, alt)
	/// 
	///      return dist, prev
	/// 
	/// </summary>
	/// <param name="grid"></param>
	/// <param name="start"></param>
	/// <param name="end"></param>
	/// <returns>costs</returns>
	private static Dictionary<Point, int> DijkstrasBasedOnCellValue(int[,] grid, Point start, Point end) {

		PriorityQueue<Cell<int>, int> priorityQueue = new();
		priorityQueue.Enqueue(new(start, 0), 0);
		Dictionary<Point, int> costs = new();
		costs.Add(start, 0);

		while (priorityQueue.Count > 0) {
			Cell<int> cell = priorityQueue.Dequeue();

			foreach ((int x, int y, int value) in grid.GetAdjacentCells(cell.Index)) {
				Cell<int> neighbour = new(x, y, value);
				if (!costs.ContainsKey(neighbour.Index)) {
					int cost = costs[cell.Index] + neighbour.Value;
					costs[neighbour.Index] = cost;
					if (neighbour.Index == end) {
						break;
					}
					priorityQueue.Enqueue(neighbour, cost);
				}
			}
		}

		return costs;
	}
}
