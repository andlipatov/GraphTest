using System.Collections.Generic;
using System.Linq;

namespace GraphTest
{
    public class Graph
    {
        private readonly List<Vertex> _vertices;

        private readonly Dictionary<Vertex, Dictionary<Vertex, int>> _shortestMap;
        private readonly Dictionary<Vertex, Dictionary<Vertex, Vertex>> _previousMap;

        public Graph(List<Vertex> vertices)
        {
            _vertices = vertices;

            _shortestMap = new Dictionary<Vertex, Dictionary<Vertex, int>>();
            _previousMap = new Dictionary<Vertex, Dictionary<Vertex, Vertex>>();

            foreach (Vertex vertex in _vertices)
            {
                SetShortestPaths(vertex);
            }
        }

        private void SetShortestPaths(Vertex startVertex)
        {
            Dictionary<Vertex, int> shortest = _vertices.ToDictionary(v => v, v => int.MaxValue);
            Dictionary<Vertex, Vertex> previous = new();
            HashSet<Vertex> unvisited = new(_vertices);

            shortest[startVertex] = 0;

            while (unvisited.Any())
            {
                Vertex nearest = unvisited.OrderBy(v => shortest[v]).FirstOrDefault();

                foreach (KeyValuePair<Vertex, Edge> neighbor in nearest.Neighbors)
                {
                    if (unvisited.Contains(neighbor.Key))
                    {
                        int distance = shortest[nearest] + neighbor.Value.Weight;

                        if (distance < shortest[neighbor.Key])
                        {
                            shortest[neighbor.Key] = distance;
                            previous[neighbor.Key] = nearest;
                        }
                    }
                }

                unvisited.Remove(nearest);
            }

            _shortestMap[startVertex] = shortest;
            _previousMap[startVertex] = previous;
        }

        public int GetShortestDistance(Vertex startVertex, Vertex finishVertex)
        {
            return _shortestMap[startVertex][finishVertex];
        }

        public List<Vertex> GetShortestPath(Vertex startVertex, Vertex finishVertex)
        {
            List<Vertex> path = new(_vertices.Count);
            Vertex previous = finishVertex;

            while (previous != startVertex)
            {
                path.Add(previous);
                previous = _previousMap[startVertex][previous];
            }

            path.Add(startVertex);
            path.Reverse();

            return path;
        }
    }
}