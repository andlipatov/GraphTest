using GraphTest;
using UnityEngine;

public class Level : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private ResourceDisplay _resourceDisplay;
    [SerializeField] private GraphManager _graphManager;
    [SerializeField] private Train[] _trains;

    private GraphTest.Resources _resources;

    private void Awake()
    {
        _resources = new GraphTest.Resources();
        _resourceDisplay.Setup(_resources);
        _resources.Setup();

        _graphManager.Setup();

        foreach (Train train in _trains)
        {
            train.Setup(_resources);
            _graphManager.SetupTrain(train);
        }
    }
}