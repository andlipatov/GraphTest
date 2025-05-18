using UnityEngine;

namespace GraphTest
{
    public class Way : MonoBehaviour
    {
        private const int _START_INDEX = 0;
        private const int _END_INDEX = 1;

        [SerializeField] private LineRenderer _lineRenderer;

        public void Setup(Vector3 startPosition, Vector3 endPosition)
        {
            _lineRenderer.SetPosition(_START_INDEX, startPosition);
            _lineRenderer.SetPosition(_END_INDEX, endPosition);
        }
    }
}