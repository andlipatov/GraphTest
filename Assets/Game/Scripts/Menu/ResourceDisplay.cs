using TMPro;
using UnityEngine;

namespace GraphTest
{
    public class ResourceDisplay : MonoBehaviour
    {
        private const string _TOTAL_TEXT = "Total: ";

        [SerializeField] private TMP_Text _text;

        public void Setup(Resources resources)
        {
            resources.Changed += OnChanged;
        }

        private void OnChanged(int value)
        {
            _text.text = _TOTAL_TEXT + value.ToString();
        }
    }
}