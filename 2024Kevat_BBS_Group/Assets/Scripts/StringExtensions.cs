using System.Text.RegularExpressions;

// Copied from UnityEditor.Rendering for use outside the editor
public static class StringExtensions
{
    private static readonly Regex InvalidFilenameRegex = new (string.Format(@"([{0}]*\.+$)|([{0}]+)", Regex.Escape(new string(System.IO.Path.GetInvalidFileNameChars()))), RegexOptions.Compiled);

    public static string ReplaceInvalidFileNameCharacters(this string input, string replacement = "_") => InvalidFilenameRegex.Replace(input, replacement);
}