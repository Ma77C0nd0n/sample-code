﻿using System;
using System.Collections.Generic;

namespace MapperService
{
    public class OneToManyMapper : IOneToManyMapper
    {
        private const int MIN_INT = 1;
        private const int MAX_INT = 947483647;

        /// A parent may have multiple children. 
        /// A child may have only one parent.
        private readonly Dictionary<int, int> ChildToParentDictionary;

        public OneToManyMapper()
        {
            ChildToParentDictionary = new Dictionary<int, int>();
        }

        public void Add(int parent, int child)
        {
            if (InputOutOfRange(parent) || InputOutOfRange(child) || child == parent)
            {
                throw new ArgumentException($"One or more parameters invalid. Parent: {parent}, Child: {child}");
            }
            if (ChildToParentDictionary.ContainsKey(child))
            {
                throw new ApplicationException($"Child {child} already has a parent.");
            }
            ChildToParentDictionary.Add(child, parent);
            Console.WriteLine($"Mapping created succesfully. Parent: {parent}, Child: {child}");
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

        private bool IsValidInput(int parent, int child)
        {
            return true;
        }

        private bool InputOutOfRange(int input) => input > MAX_INT || input < MIN_INT;
    }
}
