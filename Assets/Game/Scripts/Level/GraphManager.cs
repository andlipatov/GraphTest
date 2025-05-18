using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace GraphTest
{
    public class GraphManager : MonoBehaviour
    {
        private readonly Vector3 _labelOffset = new(0.2f, 0.0f, 0.1f);

        [Header("References")]
        [SerializeField] private Transform _waysTransform;
        [SerializeField] private Way _wayPrefab;

        [Header("Settings")]
        [SerializeField] private Edge[] _edges;
        [SerializeField] private List<Vertex> _vertices;

        private Graph _graph;

        private List<BaseVertex> _baseVertices;
        private List<MineVertex> _mineVertices;

        public void Setup()
        {
            foreach (Edge edge in _edges)
            {
                edge.From.AddNeighbor(edge.To, edge);
                edge.To.AddNeighbor(edge.From, edge);

                Way way = Instantiate(_wayPrefab, _waysTransform);
                way.Setup(edge.From.Position, edge.To.Position);
            }

            _baseVertices = new List<BaseVertex>();
            _mineVertices = new List<MineVertex>();

            _graph = new Graph(_vertices);

            foreach (Vertex vertex in _vertices)
            {
                if (vertex is BaseVertex baseVertex)
                {
                    _baseVertices.Add(baseVertex);
                }
                else if (vertex is MineVertex mineVertex)
                {
                    _mineVertices.Add(mineVertex);
                }
            }
        }

        public void SetupTrain(Train train)
        {
            train.Setup(_graph, _edges, _baseVertices, _mineVertices);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.white;
            GUIStyle style = new();
            
            foreach (Edge edge in _edges)
            {
                Vector3 position = (edge.From.Position + edge.To.Position) * 0.5f;
                Handles.Label(position, edge.Weight.ToString());
                Gizmos.DrawLine(edge.From.Position, edge.To.Position);
            }

            foreach (Vertex vertex in _vertices)
            {
                if (vertex is BaseVertex baseVertex)
                {
                    style.normal.textColor = Color.blue;
                    Handles.Label(baseVertex.Position + _labelOffset, baseVertex.ResourceMultiplier.ToString(), style);
                }
                else if (vertex is MineVertex mineVertex)
                {
                    style.normal.textColor = Color.red;
                    Handles.Label(mineVertex.Position + _labelOffset, mineVertex.TimeMultiplier.ToString(), style);
                }
            }
        }
    }
}