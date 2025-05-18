using UnityEngine;

namespace GraphTest
{
    public class MineVertex : Vertex
    {
        [field: SerializeField, Range(0.1f, 1.0f)] public float TimeMultiplier { get; private set; }
    }
}