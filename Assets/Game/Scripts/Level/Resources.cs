using System;

namespace GraphTest
{
    public class Resources
    {
        public const int BASE_RESOURCE = 1;

        private int _resources;

        public Action<int> Changed;

        public void Setup()
        {
            _resources = 0;
            Changed?.Invoke(_resources);
        }

        public void Add(int value)
        {
            _resources += value;
            Changed?.Invoke(_resources);
        }
    }
}