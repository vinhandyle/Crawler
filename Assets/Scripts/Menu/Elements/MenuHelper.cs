using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuHelper
{
    public string StringWindow(string str, int length)
    {
        string windowedStr = "";
        string s = "";

        foreach (string word in str.Split(' '))
        {
            if (s.Length + word.Length < length)
            {
                s += word + " ";
            }
            else
            {
                if (s != "")
                    windowedStr += s + "\n";

                if (word.Length < length)
                {
                    s = word + " ";
                }
                else
                {
                    windowedStr += word.Substring(0, length - 1) + "-\n";
                    s = word.Substring(9) + " ";
                }
            }
        }

        return windowedStr + s;
    }
}
