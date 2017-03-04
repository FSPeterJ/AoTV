using System.IO;
using UnityEngine;
using UnityEngine.UI;

internal struct Playerinfo
{
    public string name;
    public int score;
}

public class High_Score : MonoBehaviour
{
    private string FileName = "MyFile.txt";
    private InputField GiveYourName;

    private void Start()
    {
        Playerinfo[] info = new Playerinfo[10];

        if (File.Exists(FileName))
        {
            Debug.Log(FileName + "already exists");
            return;
        }

        //writing to the file
        StreamWriter sw = File.CreateText(FileName);
        for (int i = 0; i <= 10; ++i)
        {
            sw.WriteLine(PlayerPrefs.GetString(i + "Name") + PlayerPrefs.GetInt(i + "Score"));
        }
        sw.Close();

        //reading from the file
        if (File.Exists(FileName))
        {
            StreamReader sr = File.OpenText(FileName);
            string line = sr.ReadLine();

            while (line != null)
            {
                Debug.Log(line);
                for (int i = 0; i < 10; ++i)
                {
                    info[i].name = PlayerPrefs.GetString(i + "Name");
                    info[i].score = PlayerPrefs.GetInt(i + "Score");

                    print(info[i].name + info[i].score);
                }
            }
        }
        else
        {
            Debug.Log("Could not open the file " + FileName + " for reading.");
            return;
        }
    }

    //InputField GiveYourName;
    //string NameofFile = "MyFile.txt";
    //public Text rank;
    //void Start()
    //{
    //    rank = GetComponent<Text>();
    //    Playerinfo[] info = new Playerinfo[10];
    //    DirectoryInfo direct = new DirectoryInfo(@"D:\DungeonCrawl\Codebase\Dungeon Crawl THE GAME");
    //    if (direct.Exists)
    //    {
    //        Debug.Log(NameofFile + "already exists");
    //        return;
    //    }
    //    else
    //    {
    //        direct.Create();
    //    }

    //    StreamWriter sw = new StreamWriter(NameofFile);

    //    sw = File.CreateText(NameofFile);

    //    for (int i = 0; i <= info.Length; ++i)
    //    {
    //        sw.WriteLine(PlayerPrefs.GetString(i + "Name") + PlayerPrefs.GetInt(i + "Score"));
    //    }
    //    sw.Close();

    //    List(info);
    //}

    //void Update()
    //{
    //}

    //void ReadFile(string file)
    //{
    //    StreamReader sRead;

    //    if (File.Exists(file))
    //    {
    //        sRead = File.OpenText(file);
    //        string line = sRead.ReadLine();

    //        while(line != null)
    //        {
    //            for (int i = 0; i < file.Length; ++i)
    //            {
    //                line = sRead.ReadLine();
    //            }

    //        }
    //    }
    //}

    //void List(Playerinfo[] info)
    //{
    //    for (int i = 0; i < info.Length; ++i)
    //    {
    //      info[i].name = PlayerPrefs.GetString(i + "Name");
    //            //PlayerPrefs.GetString(i + "Name");
    //      info[i].score = PlayerPrefs.GetInt(i + "Score");
    //            //PlayerPrefs.GetInt(i + "Score");

    //        rank.text = "Name: " + info[i].name + "Score: " + info[i].score;
    //    }
    //}
}