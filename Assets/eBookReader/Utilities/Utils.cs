using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Text;

public static class Utils
{
    public static string ReverseLines(string text)
    {
        StringBuilder sb = new StringBuilder();

        string[] textLines = text.Split(new string[] { "\n", System.Environment.NewLine }, System.StringSplitOptions.RemoveEmptyEntries);
        for (int i = textLines.Length - 1; i >= 0; i--)
        {
            sb.Append(textLines[i]);
            if (i > 0)
            {
                sb.Append(System.Environment.NewLine);
            }
        }

        return sb.ToString();
    }

    public static string Reverse(string s)
    {
        char[] charArray = s.ToCharArray();
        Array.Reverse(charArray);
        return new string(charArray);
    }
}
