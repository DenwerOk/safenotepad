using System.IO;

namespace SafeNotePad.Tests
{
    class TestLibrary
    {
        public static  string ReadAllText(string path)
        {
            return File.ReadAllText(Path.Combine("Data", path));
        }

        public static string [] ReadAllLines(string path)
        {
            return File.ReadAllLines(Path.Combine("Data", path));
        }
    }
}
