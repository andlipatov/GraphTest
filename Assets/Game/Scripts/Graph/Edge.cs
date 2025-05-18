using System;
using UnityEngine;

namespace GraphTest
{
    [Serializable]
    public class Edge
    {
        [field: SerializeField] public Vertex From { get; private set; }
        [field: SerializeField] public Vertex To { get; private set; }
        [field: SerializeField, Range(10, 50)] public int Weight { get; private set; }
    }
}