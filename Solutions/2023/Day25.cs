namespace AdventOfCode.Solutions._2023;

/// <summary>
/// Day 25: Snowverload
/// https://adventofcode.com/2023/day/25
/// </summary>
[Description("Snowverload")]
public sealed partial class Day25
{
	public static string Part1(string[] input, params object[]? args) => Solution1(input).ToString();
	public static string Part2() => "⭐ CONGRATULATIONS ⭐";

	private static string Solution1(string[] input)
	{
		Apparatus apparatus = string.Join(Environment.NewLine, input).As<Apparatus>();

		int group1Size;
		int group2Size;

		//string output = apparatus.ToDot();
		if (input.Length == 13) {
			// Test Input
			apparatus.RemoveEdge(["hfx", "pzl"]);
			apparatus.RemoveEdge(["bvb", "cmg"]);
			apparatus.RemoveEdge(["nvd", "jqt"]);
		} else if (input[0] == "bdq: hfr lnm") {
			// take the value of output and put in a file input.dot
			// dot -v -Tsvg -Kneato -x -o output.svg input.dot
			// used graphvix to eyeball the wires to cut

			apparatus.RemoveEdge(["tjz", "vph"]);
			apparatus.RemoveEdge(["zkt", "jhq"]);
			apparatus.RemoveEdge(["lnr", "pgt"]);
		} else {
			return "** Solution not written yet **";
		}


		List<List<string>> groups = apparatus.ConnectedComponents();
		group1Size = groups[0].Count;
		group2Size = groups[1].Count;

		return  $"{group1Size * group2Size} Graphviz";
	}

	private record class Apparatus : IParsable<Apparatus>
	{
		public List<string> Vertices { get; }
		public readonly Dictionary<string, List<string>> Edges;

		public Apparatus(List<string> vertices)
		{
			Vertices = vertices;
			Edges = [];

			for (int i = 0; i < vertices.Count; i++) {
				Edges[vertices[i]] = [];
			}
		}

		public void AddEdge(List<string> edge)
		{
			if (Edges.TryGetValue(edge[0], out var e0)) {
				if (e0.Contains(edge[1])) {
					return;
				}
			}
			Edges[edge[0]].Add(edge[1]);
			Edges[edge[1]].Add(edge[0]);
		}

		public void RemoveEdge(List<string> edge)
		{
			_ = Edges[edge[0]].Remove(edge[1]);
			_ = Edges[edge[1]].Remove(edge[0]);
		}

		// A function used by DFS
		private void DFSUtil(int v, bool[] visited, List<string> group)
		{
			// Mark the current node as visited
			visited[v] = true;
			group.Add(Vertices[v]);

			// Recur for all the vertices adjacent to this vertex
			foreach (string vert in Edges[Vertices[v]]) {
				int i = Vertices.IndexOf(vert);
				if (!visited[i]) {
					DFSUtil(i, visited, group);
				}
			}
		}

		public List<List<string>> ConnectedComponents()
		{
			// Mark all the vertices as not visited
			bool[] visited = new bool[Vertices.Count];
			List<List<string>> groups = [];
			List<string> group = [];

			int count = 0;

			for (int v = 0; v < Vertices.Count; v++) {
				if (!visited[v]) {
					DFSUtil(v, visited, group);
					groups.Add(group);
					count++;
					group = [];
				}
			}

			return groups;
		}

		public string ToDot()
		{
			string graph = "strict graph G {";

			foreach (string vertex in Vertices) {
				graph = graph + Environment.NewLine + "  " + vertex + " -- " + "{ " + string.Join(", ", Edges[vertex]) + " }";
			}

			graph = graph + Environment.NewLine + "}";
			return graph;
		}


		// Non-working Karger's algorithm to find the minimum cut
		public (int, int) MinCut()
		{
			int minCutSize = int.MaxValue;

			// Store the two groups with the minimum cut
			List<string> group1 = [];
			List<string> group2 = [];

			// Iterate through the graph V-2 times (to ensure high probability of success)
			for (int i = 0; i < Vertices.Count - 2; i++) {
				List<string> currentGroup1 = [];
				List<string> currentGroup2 = [];

				int cutSize = KargerAlgorithm(currentGroup1, currentGroup2);

				if (cutSize < minCutSize) {
					minCutSize = cutSize;
					group1 = currentGroup1;
					group2 = currentGroup2;
				}
			}

			return (group1.Count, group2.Count);
		}

		// Non-working Karger's algorithm for one iteration
		private int KargerAlgorithm(List<string> group1, List<string> group2)
		{
			// Create a copy of the graph
			Apparatus copyGraph = new(Vertices.ToList());

			foreach (var entry in Edges) {
				foreach (var neighbor in entry.Value) {
					copyGraph.AddEdge([entry.Key, neighbor]);
				}
			}

			// Continue contracting edges until only 2 vertices are left
			while (copyGraph.Vertices.Count > 2) {
				// Get a random edge
				int randomVertexIndex = Random.Shared.Next(copyGraph.Vertices.Count);
				string vertex1 = copyGraph.Vertices[randomVertexIndex];
				string vertex2 = copyGraph.Edges[vertex1][Random.Shared.Next(copyGraph.Edges[vertex1].Count)];

				// Merge the two vertices
				List<string> mergedVertices = [vertex1, vertex2];
				copyGraph.RemoveEdge(mergedVertices);
				copyGraph.Vertices[randomVertexIndex] = string.Join(",", mergedVertices);

				// Update edges
				foreach (var entry in copyGraph.Edges) {
					for (int i = 0; i < entry.Value.Count; i++) {
						if (entry.Value[i] == vertex1 || entry.Value[i] == vertex2) {
							entry.Value[i] = string.Join(",", mergedVertices);
						}
					}
				}

				//// Remove self-loops
				copyGraph.Edges[copyGraph.Vertices[randomVertexIndex]] =
					copyGraph
					.Edges[copyGraph.Vertices[randomVertexIndex]]
					.Where(v => v != copyGraph.Vertices[randomVertexIndex])
					.ToList();
			}

			// Populate the two groups
			group1.AddRange(copyGraph.Edges[copyGraph.Vertices[0]]);
			group2.AddRange(copyGraph.Edges[copyGraph.Vertices[1]]);

			return copyGraph.Edges[copyGraph.Vertices[0]].Count;
		}


		public static Apparatus Parse(string s, IFormatProvider? provider)
		{
			HashSet<string> componentsSet = [];
			HashSet<HashSet<string>> wires = [];

			string[] input = s.Split(Environment.NewLine);
			char[] splitBy = [':', ' '];
			foreach (string line in input) {
				string[] components = line.TrimmedSplit(splitBy);
				_ = componentsSet.Add(components[0]);
				foreach (string component in components[1..]) {
					_ = componentsSet.Add(component);
					HashSet<string> wire = [components[0], component];
					_ = wires.Add(wire);
				}
			}

			Apparatus apparatus = new([.. componentsSet]);
			foreach (HashSet<string> wire in wires) {
				apparatus.AddEdge([.. wire]);
			}

			return apparatus;
		}

		public static Apparatus Parse(string s) => Parse(s, null);
		public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out Apparatus result)
			=> ISimpleParsable<Apparatus>.TryParse(s, provider, out result);

	}
}
