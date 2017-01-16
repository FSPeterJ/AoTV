using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;
using System;
using System.Runtime.Serialization;
using System.Reflection;

public class LoadnSave : MonoBehaviour
{

	// Use this for initialization
	void Start()
    {

	}
	
	// Update is called once per frame
	void Update()
    {
		
	}
}

//Holder class
[Serializable()]
public class SaveData : ISerializable
{
    public int pLevel = 1;
    public int pScore = 0;

    public SaveData()
    {
    }

    public SaveData(SerializationInfo info, StreamingContext ctxt)
    {
        pLevel = (int)info.GetValue("Level", typeof(int));
        pScore = (int)info.GetValue("Score", typeof(int));
    }

    public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
    {
        info.AddValue("Level", pLevel);
        info.AddValue("Score", pScore);
    }

    public void Save()
    {
        SaveData data = new SaveData();
        Stream stream = File.Open("MySavedGame.game", FileMode.Create);
        BinaryFormatter bformatter = new BinaryFormatter();
        bformatter.Binder = new VersionDeserializationBinder();
        Debug.Log("Reading data");
        data = (SaveData)bformatter.Deserialize(stream);
        stream.Close();
    }

    public void Load()
    {
        SaveData data = new SaveData();
        Stream stream = File.Open("Mysavedgame.game", FileMode.Open);
        BinaryFormatter bformatter = new BinaryFormatter();
        bformatter.Binder = new VersionDeserializationBinder();
        Debug.Log("Reading data");
        data = (SaveData)bformatter.Deserialize(stream);
        stream.Close();
    }
}

public sealed class VersionDeserializationBinder : SerializationBinder
{
    public override Type BindToType(string assemblyName, string typeName)
    {
        if (!string.IsNullOrEmpty(assemblyName) && !string.IsNullOrEmpty(typeName))
        {
            Type typeToDeserialize = null;
            assemblyName = Assembly.GetExecutingAssembly().FullName;
            //returns the type
            typeToDeserialize = Type.GetType(string.Format("{0}, {1}", typeName, assemblyName));

            return typeToDeserialize;
        }

        return null;
    }
}



