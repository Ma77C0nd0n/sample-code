using System;
using System.Collections.Generic;
using System.Linq;

namespace MapperService
{
    public class OneToManyMapper : IOneToManyMapper
    {
        private const int MIN_INT = 1;
        private const int MAX_INT = 947483647;

        /// A parent may have multiple children. 
        /// A child may have only one parent.
        private Dictionary<int, int> ChildToParentDictionary;

        public OneToManyMapper()
        {
            ChildToParentDictionary = new Dictionary<int, int>();
        }

        public void Add(int parent, int child)
        {
            ValidateInput(parent, child);
            if (ChildToParentDictionary.ContainsKey(child))
            {
                throw new ApplicationException($"Child {child} already has a parent.");
            }
            ChildToParentDictionary.Add(child, parent);
            Console.WriteLine($"Mapping created successfully. Parent: {parent}, Child: {child}");
        }

        public IEnumerable<int> GetChildren(int parent)
        {
            ValidateInput(parent);
            return ChildToParentDictionary.Where(x => x.Value == parent).Select(y => y.Key);
        }

        public int GetParent(int child)
        {
            ValidateInput(child);
            if (ChildToParentDictionary.TryGetValue(child, out int defaultParent))
            {
                return defaultParent;
            }
            return 0;
        }

        public void RemoveChild(int child)
        {
            ValidateInput(child);
            if (!ChildToParentDictionary.Remove(child)){
                throw new ApplicationException($"Child {child} does not exist.");
            }
            Console.WriteLine($"Child {child} removed successfully.");
        }

        public void RemoveParent(int parent)
        {
            var parentChildren = GetChildren(parent);
            if (!parentChildren.Any())
            {
                throw new ApplicationException($"Parent {parent} does not exist.");
            }
            foreach (int child in parentChildren)
            {
                ChildToParentDictionary.Remove(child);
            }
        }

        public void UpdateChild(int oldChild, int newChild)
        {
            ValidateInput(oldChild, newChild);
            var parent = GetParent(oldChild);
            if (parent == 0)
            {
                throw new ApplicationException($"Child {oldChild} does not exist.");
            }
            RemoveChild(oldChild);
            Add(parent, newChild);
        }

        public void UpdateParent(int oldParent, int newParent)
        {
            ValidateInput(oldParent, newParent);
            var parentChildren = GetChildren(oldParent).ToList();
            if (!parentChildren.Any())
            {
                throw new ApplicationException($"Parent {oldParent} does not exist.");
            }
            foreach (int child in parentChildren)
            {
                RemoveChild(child);
                Add(newParent, child);
            }
        }

        private void ValidateInput(int input)
        {
            if (InputOutOfRange(input))
            {
                throw new ArgumentException($"Parameter {input} invalid.");
            }
        }
        
        private void ValidateInput(int input1, int input2)
        {
            if (InputOutOfRange(input1) || InputOutOfRange(input2) || input2 == input1)
            {
                throw new ArgumentException($"One or more parameters invalid. {input1}, {input2}");
            }
        }

        private bool InputOutOfRange(int input) => input > MAX_INT || input < MIN_INT;
    }
}
