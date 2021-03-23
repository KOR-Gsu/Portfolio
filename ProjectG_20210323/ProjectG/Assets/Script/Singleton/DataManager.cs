using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using Newtonsoft.Json;

public class DataManager
{
    public LogData currentLog { get; set; }

    private string filePath = "/SaveData";
    private string fileExtension = ".json";

    public void DataToJson<T>(string filename, T data)
    {
        string saveString = JsonConvert.SerializeObject(data);
        byte[] saveData = Encoding.UTF8.GetBytes(saveString);

        FileStream fileStream = new FileStream(string.Format("{0}/{1}", Application.dataPath + filePath, filename + fileExtension), FileMode.Create);
        fileStream.Write(saveData, 0, saveData.Length);
        fileStream.Close();
    }

    public T JsonToData<T>(string filename)
    {
        FileStream fileStream = new FileStream(string.Format("{0}/{1}", Application.dataPath + filePath, filename + fileExtension), FileMode.Open);
        byte[] loadData = new byte[fileStream.Length];
        fileStream.Read(loadData, 0, loadData.Length);
        fileStream.Close();

        string loadString = Encoding.UTF8.GetString(loadData);
        return JsonConvert.DeserializeObject<T>(loadString);
    }
}