using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace GraphTest
{
    public class Vertex : MonoBehaviour
    {
        private readonly Vector3 _labelOffset = new(-0.05f, 0.0f, 0.1f);

        [field: SerializeField] public int Index { get; private set; }

        public Dictionary<Vertex, Edge> Neighbors { get; private set; } = new Dictionary<Vertex, Edge>();
        
        public Vector3 Position => transform.position;

        public void AddNeighbor(Vertex vertex, Edge edge)
        {
            Neighbors.Add(vertex, edge);
        }

        private void OnDrawGizmos()
        {
            GUIStyle style = new();
            style.normal.textColor = Color.black;
            Handles.Label(transform.position + _labelOffset, Index.ToString(), style);
        }
    }
}