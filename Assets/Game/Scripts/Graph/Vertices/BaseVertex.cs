using UnityEngine;

namespace GraphTest
{
    public class BaseVertex : Vertex
    {
        [field: SerializeField, Range(1, 10)] public int ResourceMultiplier { get; private set; }
    }
}