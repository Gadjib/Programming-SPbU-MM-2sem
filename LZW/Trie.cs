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
        public Dictionary<char, TrieNode> Children {  get; set; }
        public bool IsTerminal { get; set; }
        public int EndsLower;

        public TrieNode()
        {
            Children = new Dictionary<char, TrieNode>();
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

        public bool Add(string element)
        {
            return AddRecursive(RootNode, element, 0);
        }

        private bool AddRecursive(TrieNode node, string element, int depth)
        { 
            if (depth == element.Length)
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

        public bool Contains(string element)
        {
            var node = RootNode;
            foreach (var ch in element)
            {
                if (!node.Children.ContainsKey(ch))
                {
                    return false;
                }
                node = node.Children[ch];
            }

            return node.IsTerminal;
        }

        public bool Remove(string element)
        {
            return RemoveRecursive(RootNode, element, 0);
        }

        private bool RemoveRecursive(TrieNode node, string element, int depth)
        {
            if (node == null)
            {
                return false;
            }

            if (depth == element.Length)
            {
                if (!node.IsTerminal)
                {
                    return false; 
                }

                node.IsTerminal = false; 

                return node.Children.Count == 0;
            }

            char ch = element[depth];
            if (!node.Children.ContainsKey(ch))
            {
                return false; 
            }

            bool shouldDeleteCurrentNode = RemoveRecursive(node.Children[ch], element, depth + 1);

            if (shouldDeleteCurrentNode)
            {
                node.Children.Remove(ch);

                return !node.IsTerminal && node.Children.Count == 0;
            }

            return false;
        }
        
        public int HowManyStartsWithPrefix(string  prefix)
        {
            TrieNode node = RootNode;

            foreach (char ch in prefix) 
            { 
                if (node == null || !node.Children.ContainsKey(ch))
                { 
                    return 0; 
                }
                node = node.Children[ch];
            }

            return node.EndsLower;
        }
    }
}
