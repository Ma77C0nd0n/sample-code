using System;
using System.Collections.Generic;

namespace MapperService
{
    public class OneToManyMapper : IOneToManyMapper
    {
        private const int MAX_INT = 947483647;
        private Dictionary<int, IEnumerable<int>> MappingDictionary;

        public void Add(int parent, int child)
        {
            throw new NotImplementedException();
        }

        private bool IsValidInput(int parent, int child)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<int> GetChildren(int parent)
        {
            throw new NotImplementedException();
        }

        public int GetParent(int child)
        {
            throw new NotImplementedException();
        }

        public void RemoveChild(int child)
        {
            throw new NotImplementedException();
        }

        public void RemoveParent(int parent)
        {
            throw new NotImplementedException();
        }

        public void UpdateChild(int oldChild, int newChild)
        {
            throw new NotImplementedException();
        }

        public void UpdateParent(int oldParent, int newParent)
        {
            throw new NotImplementedException();
        }
    }
}
