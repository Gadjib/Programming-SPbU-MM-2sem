namespace LZW
{
    class Program
    {
        public static int Main(string[] args)
        {
            using (FileStream readFile = new FileStream(args[1], FileMode.Open))
            {
                List<byte> inputBytes = new List<byte>();
                BinaryReader reader = new BinaryReader(readFile);
                for (int i = 0; i < readFile.Length; i++)
                {
                    inputBytes.Add(reader.ReadByte());
                }

                LZW lzw = new LZW();

                if (args[0] == "-c")
                {
                    List<byte> encodeList = lzw.Encode(inputBytes);
                    Console.WriteLine("Original file size: " + readFile.Length + " bytes");
                    Console.WriteLine("Compressed size: " + encodeList.Count + " bytes");

                    using (FileStream newFile = new FileStream(readFile.Name + ".zipped", FileMode.Create))
                    {
                        BinaryWriter writer = new BinaryWriter(newFile);

                        foreach (byte b in encodeList)
                        {
                            writer.Write(b);
                        }
                    }
                }

                else if (args[0] == "-u")
                {
                    string extention = ".zipped";
                    if (Path.GetExtension(args[1]) != extention)
                    {
                        Console.WriteLine("Wrong file type");
                        return 1;
                    }

                    List<byte> decodeList = lzw.Decode(inputBytes);
                    Console.WriteLine("Compressed size: " + readFile.Length + " bytes");
                    Console.WriteLine("Decoded size: " + decodeList.Count + " bytes");

                    using (FileStream newFile = new FileStream(Path.GetFileNameWithoutExtension(args[1]) + ".unzipped", FileMode.Create))
                    {
                        BinaryWriter writer = new BinaryWriter(newFile);

                        foreach (byte b in decodeList)
                        {
                            writer.Write(b);
                        }
                    }
                }
            }
            return 0;
        }
    }
}