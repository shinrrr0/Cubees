using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CommandsNamespace;
using DataSpace;


public class SaveProgressCommand : MonoBehaviour, IControllerCommand
{
    public string levelName;

    public void Act(Context context){
        if (!PlayerPrefs.HasKey(levelName)) {
            new JsonFile().addNewLevel(levelName); 
        }
    }
}


namespace DataSpace{
    public class JsonFile{
        private string jsonName = "/data.json";

        public string getFileName(){
            return jsonName;
        }

        public void createFile(){
            PlayerData data = new PlayerData();
            string jsonData = JsonUtility.ToJson(data);

            System.IO.File.WriteAllText(Application.persistentDataPath + jsonName, jsonData);
        }

        public PlayerData getPlayerData(){
            string dataFromJson = System.IO.File.ReadAllText(Application.persistentDataPath + jsonName);
            return JsonUtility.FromJson<PlayerData>(dataFromJson);
        }

        public void addNewLevel(string levelName){
            if (!System.IO.File.Exists(Application.persistentDataPath + jsonName)){
                createFile();
            }

            PlayerData data = getPlayerData();

            if (data.doneLevels.Contains(levelName)) return;

            data.doneLevels.Add(levelName);

            string jsonData = JsonUtility.ToJson(data);
            System.IO.File.WriteAllText(Application.persistentDataPath + jsonName, jsonData);
        }

        public void changeVolumeValue(float volume){
            volume = volume > 1? 1: volume;
            volume = volume < 0? 0: volume;

            if (!System.IO.File.Exists(Application.persistentDataPath + jsonName)){
                createFile();
            }
            PlayerData data = getPlayerData();

            data.volume_value = volume;

            string jsonData = JsonUtility.ToJson(data);
            System.IO.File.WriteAllText(Application.persistentDataPath + jsonName, jsonData);
        }
    }

    [System.Serializable]
    public class PlayerData
    {
        public List<string> doneLevels = new List<string>();
        public float volume_value = 1;

        public PlayerData() => doneLevels = new List<string>();
        public PlayerData(List<string> _doneLevels) => doneLevels = _doneLevels;
    }
}