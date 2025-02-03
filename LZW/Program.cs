namespace LZW
{
    class Program
    {
        public static void Main(string[] args)
        {
            Trie trie = new Trie();
            trie.Add("aboba");
            trie.Add("abob");
            trie.Add("ababa");

            Console.WriteLine(trie.Size);
            Console.WriteLine(trie.HowManyStartsWithPrefix("ab"));
            Console.WriteLine(trie.HowManyStartsWithPrefix("abo"));
        }
    }
}