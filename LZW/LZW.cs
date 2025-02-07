using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LZW
{
    class LZW
    {
        private Trie dictionary;
        private Dictionary<string, byte> codeTable;
        private Dictionary<byte, List<byte>> decodeTable;
        private byte nextCode;

        public LZW()
        {
            dictionary = new Trie();
            codeTable = new Dictionary<string, byte>();
            decodeTable = new Dictionary<byte, List<byte>>();
            nextCode = 0;

            for (int i = 0; i < 256; i++)
            {
                List<byte> singleByte = new List<byte> { (byte)i };
                dictionary.Add(singleByte);
                codeTable[string.Join("-", singleByte.Select(b => b.ToString("X2")))] = nextCode;
                decodeTable[nextCode] = singleByte;
                nextCode++;
            }
        }

        public List<byte> Encode(List<byte> input)
        {
            List<byte> output = new List<byte>();
            List<byte> current = new List<byte>();

            foreach (byte b in input)
            {
                List<byte> combined = new List<byte>(current) { b };
                List<byte> combinedArray = combined.ToList();

                if (dictionary.Contains(combinedArray))
                {
                    current = combined;
                }
                else
                {
                    output.Add(codeTable[string.Join("-", current.Select(b => b.ToString("X2")))]);
                    dictionary.Add(combinedArray);
                    codeTable[string.Join("-", combinedArray.Select(b => b.ToString("X2")))] = nextCode;
                    decodeTable[nextCode] = combinedArray;
                    nextCode++;
                    current = new List<byte> { b };
                }
            }

            if (current.Count > 0)
            {
                output.Add(codeTable[string.Join("-", current.Select(b => b.ToString("X2")))]);
            }

            return output;
        }

        public List<byte> Decode(List<byte> input)
        {
            List<byte> output = new List<byte>();
            List<byte> previous = decodeTable[input[0]];
            output.AddRange(previous);

            for (int i = 1; i < input.Count; i++)
            {
                byte code = input[i];
                List<byte> current = new List<byte>();

                if (decodeTable.ContainsKey(code))
                {
                    current = decodeTable[code];
                }
                else
                {
                    current.AddRange(previous);
                    current[previous.Count] = previous[0];
                }

                output.AddRange(current);
                List<byte> newEntry = new List<byte>(previous);

                //newEntry.AddRange(previous);
                newEntry.Add(current[0]);
                decodeTable[nextCode] = newEntry;
                nextCode++;
                previous = current;
            }

            return output;
        }
    }
}