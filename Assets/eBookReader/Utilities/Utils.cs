using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Text;
using System.IO;

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

    public static Stream GenerateStreamFromString(string s)
    {
        MemoryStream stream = new MemoryStream();
        StreamWriter writer = new StreamWriter(stream);
        writer.Write(s);
        writer.Flush();
        stream.Position = 0;
        return stream;
    }

    public static Color ToColorFromRGBA(this string colorString)
    {
        colorString = colorString.Replace("RGBA(", "").Replace(")", "").Replace(" ", "");
        string[] values = colorString.Split(new string[] { "," }, System.StringSplitOptions.RemoveEmptyEntries);

        return new Color(float.Parse(values[0]) / 255f, float.Parse(values[1]) / 255f, float.Parse(values[2]) / 255f, float.Parse(values[3]) / 255f);
    }

    public static Color ToColorFromHEX(this string hex)
    {
        byte r = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
        byte g = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
        byte b = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
        byte a = byte.Parse(hex.Substring(6, 2), System.Globalization.NumberStyles.HexNumber);
        return new Color32(r, g, b, a);
    }

    public static Color ToColor(this string colorString)
    {
        Color col = Color.white;
        if (colorString.ToLower().Contains("rgba"))
        {
            try
            {
                col = colorString.ToColorFromRGBA();
            }
            catch (Exception exc)
            {
                Debug.LogException(exc);
            }
        }
        else
        {
            string color = colorString.Replace("#", "").Replace("0x", "");
            if (color.Length == 6)
                color += "ff";
            if (color.Length == 8)
            {
                try
                {
                    col = color.ToColorFromHEX();
                }
                catch (Exception exc)
                {
                    Debug.LogException(exc);
                }
            }
        }
        return col;
    }
}
