namespace MockItUp.Core.PlaceHolder
{
    public class VariableTree<T>
    {
        public VariableTree()
        {
            Root = new VariableTreeNode<T>("-");
        }

        public VariableTreeNode<T> Root { get; }

        public void AppendNode(string variablePath, T value)
        {
            var leafNode = AppendNode(Root, variablePath);
            leafNode.SetValue(value);
        }

        public bool TryGet(string variablePath, out T value)
        {
            var node = FindNode(Root, variablePath);
            if (node == null || !node.IsLeaf)
            {
                value = default(T);
                return false;
            }

            value = node.Value;
            return true;
        }

        private VariableTreeNode<T> AppendNode(VariableTreeNode<T> currentNode, string variablePath)
        {
            var dotIndex = variablePath.IndexOf('.');
            var current = variablePath.Substring(0, dotIndex < 0 ? variablePath.Length : dotIndex);
            if (currentNode == Root)
            {
                current = GetFirstLevel(current);
            }
            currentNode = currentNode.AddChildIfNotExists(current);

            return dotIndex < 0 ? currentNode : AppendNode(currentNode, variablePath.Substring(dotIndex + 1));
        }

        private VariableTreeNode<T> FindNode(VariableTreeNode<T> currentNode, string variablePath)
        {
            var dotIndex = variablePath.IndexOf('.');
            var current = variablePath.Substring(0, dotIndex < 0 ? variablePath.Length : dotIndex);
            if (currentNode == Root)
            {
                current = GetFirstLevel(current);
            }
            currentNode = currentNode.FindNode(current);

            return dotIndex < 0 ? currentNode : FindNode(currentNode, variablePath.Substring(dotIndex + 1));
        }

        private string GetFirstLevel(string current)
        {
            switch (current)
            {
                case "b":
                case "body":
                    return "body";
                case "h":
                case "header":
                case "headers":
                    return "header";
                case "p":
                case "path":
                    return "path";
                default:
                    return current;
            }
        }
    }
}
