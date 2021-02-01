using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MockItUp.Core.PlaceHolder
{
    public class VariableTreeNode<T>
    {
        public VariableTreeNode(string variable)
        {
            Variable = variable;
        }

        public string Variable { get; }

        public T Value { get; private set; }
        public bool IsLeaf { get; private set; }

        public List<VariableTreeNode<T>> Children { get; private set; }

        public VariableTreeNode<T> AddChildIfNotExists(string variable)
        {
            if (Children == null)
                Children = new List<VariableTreeNode<T>>();

            var child = Children.SingleOrDefault(x => x.Variable == variable);
            if (child == null)
            {
                child = new VariableTreeNode<T>(variable);
                Children.Add(child);
            }

            return child;
        }

        public VariableTreeNode<T> FindNode(string variable)
        {
            if (Children == null)
                return null;

            return Children.SingleOrDefault(x => x.Variable == variable);
        }

        public void SetValue(T value)
        {
            Value = value;
            IsLeaf = true;
        }
    }
}
