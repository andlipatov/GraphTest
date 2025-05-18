using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GraphTest
{
    public class Train : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private GameObject ResourceObject;

        [Header("Settings")]
        [SerializeField, Range(1f, 400f)] private int _moveSpeed;
        [SerializeField, Range(1f, 40f)] private int _mineTime;

        private Resources _resources;

        private Graph _graph;
        private Edge[] _edges;

        private List<BaseVertex> _baseVertices;
        private List<MineVertex> _mineVertices;

        private List<Vertex> _path;

        private int _pathIndex;

        private Vertex _vertex1;
        private Vertex _vertex2;

        private float _time;

        private float _edgeWeight;
        private float _edgeDistance;
        private float _edgeRatio;

        private Vector3 _poisition;
        private Quaternion _rotation;

        private State _state;

        private enum State
        {
            Move,
            Base,
            Mine
        }

        private void Update()
        {
            switch (_state)
            {
                case State.Move:
                {
                    _time += Time.deltaTime;

                    _edgeDistance = _moveSpeed * _time;
                    _edgeRatio = _edgeDistance / _edgeWeight;

                    if (_edgeRatio >= 1)
                    {
                        switch (_vertex2)
                        {
                            case BaseVertex:
                            {
                                _edgeRatio = 1;

                                _time = 0;
                                _state = State.Base;
                                break;
                            }
                            case MineVertex:
                            {
                                _edgeRatio = 1;

                                _time = 0;
                                _state = State.Mine;


                                break;
                            }
                            case VoidVertex:
                            {
                                SetNextVertex();

                                _time = 0;

                                break;
                            }
                        }
                    }

                    SetPositionAndRotation();

                    break;
                }
                case State.Base:
                {
                    ResourceObject.SetActive(false);
                    _resources.Add(Resources.BASE_RESOURCE * (_vertex2 as BaseVertex).ResourceMultiplier);

                    FindMinPath(_vertex2, out Vertex baseVertex, out Vertex mineVertex, out float resourcesFactor);
                    SetPath(_vertex2, mineVertex, baseVertex);

                    _time = 0;
                    _state = State.Move;

                    break;
                }
                case State.Mine:
                {
                    _time += Time.deltaTime;

                    if (_time >= _mineTime * (_vertex2 as MineVertex).TimeMultiplier)
                    {
                        ResourceObject.SetActive(true);

                        SetNextVertex();
                        SetPositionAndRotation();

                        _time = 0;
                        _state = State.Move;
                    }

                    break;
                }
            }
        }

        public void Setup(Resources resources)
        {
            _resources = resources;
        }

        public void Setup(Graph graph, Edge[] edges, List<BaseVertex> baseVertices, List<MineVertex> mineVertices)
        {
            _graph = graph;
            _edges = edges;

            _baseVertices = baseVertices;
            _mineVertices = mineVertices;

            _path = new List<Vertex>();

            int edgeIndex = UnityEngine.Random.Range(0, _edges.Length);
            _edgeRatio = UnityEngine.Random.value;

            FindMinPath(_edges[edgeIndex].From, out Vertex baseVertex1, out Vertex mineVertex1, out float resourcesFactor1);
            FindMinPath(_edges[edgeIndex].To, out Vertex baseVertex2, out Vertex mineVertex2, out float resourcesFactor2);

            if (resourcesFactor1 >= resourcesFactor2)
            {
                SetPath(_edges[edgeIndex].To, mineVertex1, baseVertex1);
            }
            else
            {
                SetPath(_edges[edgeIndex].From, mineVertex2, baseVertex2);
            }

            SetPositionAndRotation();

            ResourceObject.SetActive(false);

            _state = State.Move;
        }

        private void SetNextVertex()
        {
            _pathIndex++;

            _vertex1 = _vertex2;
            _vertex2 = _path[_pathIndex];
            
            _edgeWeight = _vertex1.Neighbors[_vertex2].Weight;

            _edgeRatio = 0;
        }

        private void SetPositionAndRotation()
        {
            _poisition = Vector3.Lerp(_vertex1.Position, _vertex2.Position, _edgeRatio);
            _rotation = Quaternion.LookRotation(_vertex2.Position - _vertex1.Position);

            transform.SetPositionAndRotation(_poisition, _rotation);
        }

        private void FindMinPath(Vertex startVertex, out Vertex baseVertex, out Vertex mineVertex, out float resourcesFactor)
        {
            float timeMoveToMine;
            float timeMine;
            float timeMoveToBase;

            float time;
            float resources;

            resourcesFactor = 0;
            float maxResourcesFactor = 0;

            baseVertex = _baseVertices[0];
            mineVertex = _mineVertices[0];

            for (int i = 0; i < _mineVertices.Count; i++)
            {
                timeMoveToMine = _graph.GetShortestDistance(startVertex, _mineVertices[i]) / _moveSpeed;
                timeMine = _mineTime * _mineVertices[i].TimeMultiplier;
                
                for (int j = 0; j < _baseVertices.Count; j++)
                {
                    timeMoveToBase = _graph.GetShortestDistance(_mineVertices[i], _baseVertices[j]) / _moveSpeed;
                    
                    time = timeMoveToMine + timeMine + timeMoveToBase;
                    resources = Resources.BASE_RESOURCE * _baseVertices[j].ResourceMultiplier;

                    resourcesFactor = resources / time;

                    if (resourcesFactor > maxResourcesFactor)
                    {
                        maxResourcesFactor = resourcesFactor;

                        baseVertex = _baseVertices[j];
                        mineVertex = _mineVertices[i];
                    }
                }
            }
        }

        private void SetPath(Vertex startVertex, Vertex mineVertex, Vertex baseVertex)
        {
            _path.Clear();
            _path.AddRange(_graph.GetShortestVerticesPath(startVertex, mineVertex));
            _path.AddRange(_graph.GetShortestVerticesPath(mineVertex, baseVertex).Skip(1));

            _vertex1 = _path[0];
            _vertex2 = _path[1];

            _edgeWeight = _vertex1.Neighbors[_vertex2].Weight;
            _time = _edgeRatio * _edgeWeight / _moveSpeed;

            _pathIndex = 1;
        }
    }
}