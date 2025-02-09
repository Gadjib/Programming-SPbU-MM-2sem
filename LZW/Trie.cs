using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LZW
{
    class TrieNode
    {
        public Dictionary<byte, TrieNode> Children {  get; set; }
        public bool IsTerminal { get; set; }
        public int EndsLower;

        public TrieNode()
        {
            Children = new Dictionary<byte, TrieNode>();
            IsTerminal = false;
            EndsLower = 0;
        }
    }

    class Trie
    {
        private TrieNode RootNode;
        public int Size = 0;

        public Trie()
        {
            RootNode = new TrieNode();
        }

        public bool Add(List<byte> element)
        {
            return AddRecursive(RootNode, element, 0);
        }

        private bool AddRecursive(TrieNode node, List<byte> element, int depth)
        { 
            if (depth == element.Count)
            {
                if (!node.IsTerminal)
                {
                    node.IsTerminal = true;
                    return true;
                }
                return false;
            }

            if (!node.Children.ContainsKey(element[depth]))
            {
                Size++;
                node.Children[element[depth]] = new TrieNode();
            }

            bool answer = AddRecursive(node.Children[element[depth]], element, depth + 1);

            if (answer)
            {
                node.EndsLower++;
            }

            return answer;
        }

        public bool Contains(List<byte> element)
        {
            var node = RootNode;
            foreach (var b in element)
            {
                if (!node.Children.ContainsKey(b))
                {
                    return false;
                }
                node = node.Children[b];
            }

            return node.IsTerminal;
        }

        public bool Remove(List<byte> element)
        {
            return RemoveRecursive(RootNode, element, 0);
        }

        private bool RemoveRecursive(TrieNode node, List<byte> element, int depth)
        {
            if (node == null)
            {
                return false;
            }

            if (depth == element.Count)
            {
                if (!node.IsTerminal)
                {
                    return false; 
                }

                node.IsTerminal = false; 

                return node.Children.Count == 0;
            }

            byte b = element[depth];
            if (!node.Children.ContainsKey(b))
            {
                return false; 
            }

            bool shouldDeleteCurrentNode = RemoveRecursive(node.Children[b], element, depth + 1);

            if (shouldDeleteCurrentNode)
            {
                node.Children.Remove(b);

                return !node.IsTerminal && node.Children.Count == 0;
            }

            return false;
        }
        
        public int HowManyStartsWithPrefix(List<byte>  prefix)
        {
            TrieNode node = RootNode;

            foreach (byte b in prefix) 
            { 
                if (node == null || !node.Children.ContainsKey(b))
                { 
                    return 0; 
                }
                node = node.Children[b];
            }

            return node.EndsLower;
        }
    }
}
