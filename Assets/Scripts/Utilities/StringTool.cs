namespace Utilities
{
    using System.Text.RegularExpressions;

    public static class StringTool
    {
        /// <summary>
        /// return the lowercase name with underline.
        /// <para>such as : GrassBlock ->  grass_block</para>
        /// </summary>
        /// <returns></returns>
        public static string LowercaseWithUnderline(string s)
        {
            return Regex.Replace(s, @"\B[A-Z]", "_$0").ToLower();
        }
    }
}