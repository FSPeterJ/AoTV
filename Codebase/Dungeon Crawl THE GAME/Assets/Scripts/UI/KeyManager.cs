using System.Collections.Generic;
using UnityEngine;

public static class KeyManager
{
    private static Dictionary<string, KeyCode> Keys = new Dictionary<string, KeyCode>();

    public static string GetKeyName(string _key)
    {
        if (Keys.ContainsKey(_key))
            return Keys[_key].ToString();
        else
            return "";
    }

    public static KeyCode GetKeyCode(string _key)
    {
        if (Keys.ContainsKey(_key))
            return Keys[_key];
        else
            return KeyCode.None;
    }

    public static bool GetKeys(string _key)
    {
        string key = GetKeyName(_key);
        string key2 = GetKeyName(_key + "2");
        Debug.Log(key);
        Debug.Log(key2);
        if (key.Length > 0 && Input.GetKey(key) || key2.Length > 0 && Input.GetKey(key2))
        {
            return true;
        }
        else
            return false;
    }

    public static void SetKey(string _key, KeyCode value, bool Primary = true)
    {
        //if(Primary)
        Keys[_key] = value;
        // else
        //     keys[_key + "2"] = value;
    }
}