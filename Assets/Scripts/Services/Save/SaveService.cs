using System.IO;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Game;
using Services.Log;
using UnityEngine;

namespace Services.Save
{
    public class SaveService
    {
        private readonly string _path;
        private LogService _logService;

        public SaveService(LogService logService)
        {
            _path = Application.persistentDataPath + "/saveData.json";
            _logService = logService;
        }

        public async UniTask Save(GameData gameData)
        {
            _logService.Log("Starr save Game data");
            string jsonString = JsonUtility.ToJson(gameData);
            await File.WriteAllTextAsync(_path, jsonString);
            _logService.Log("Game data was saved");
        }

        public async UniTask<GameData> LoadDataAsync()
        {
            GameData gameData = null;
            if (File.Exists(_path))
            {
                _logService.Log("Data exist");
                string jsonString = await File.ReadAllTextAsync(_path);
                gameData = JsonUtility.FromJson<GameData>(jsonString);
            }
            else
            {
                _logService.Log("Data is null");
            }

            return gameData;
        }
    }
}