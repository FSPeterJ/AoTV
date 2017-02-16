using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

struct Playerinfo
{
    public string name;
    public int score;
}


public class High_Score : MonoBehaviour
{
    InputField GiveYourName;
    string NameofFile = "Myfile.txt";
    public Text rank;
    void Start()
    {
        rank = GetComponent<Text>();
        Playerinfo[] info = new Playerinfo[10];
        DirectoryInfo direct = new DirectoryInfo(@"D:\DungeonCrawl\Codebase\Dungeon Crawl THE GAME\MyFile.txt");
        //if (direct.Exists)
        //{
        //    Debug.Log(NameofFile + "already exists");
        //    return;
        //}
        direct.Create();

        //StreamWriter sw = new StreamWriter(direct);
        //sw = File.CreateText(NameofFile);

        for (uint i = 0; i <= info.Length; ++i)
        {
        //    sw.WriteLine(PlayerPrefs.GetString(i + "Name") + PlayerPrefs.GetInt(i + "Score"));
        }
      //  sw.Close();

        List(info);
    }

    void ReadFile(string file)
    {
        StreamReader sRead;

        if (File.Exists(file))
        {
            sRead = File.OpenText(file);
            string line = sRead.ReadLine();

            while(line != null)
            {
                for (uint i = 0; i < file.Length; ++i)
                {
                    line = sRead.ReadLine();
                }
                
            }
        }
    }

    void List(Playerinfo[] info)
    {
        
        for (int i = 0; i < info.Length; ++i)
        {
          info[i].name = PlayerPrefs.GetString(i + "Name");
                //PlayerPrefs.GetString(i + "Name");
          info[i].score = PlayerPrefs.GetInt(i + "Score");
                //PlayerPrefs.GetInt(i + "Score");
            

            rank.text = "Name: " + info[i].name + "Score: " + info[i].score;
        }
    }
}
