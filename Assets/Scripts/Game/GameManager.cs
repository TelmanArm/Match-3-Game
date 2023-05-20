using System;
using Cysharp.Threading.Tasks;
using Services.Audio;
using Services.Log;
using Services.Save;
using Services.UI;
using UI.Loading;
using UniRx;
using UnityEngine;
using Zenject;
using AudioType = Services.Audio.AudioType;

namespace Game
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private ReactiveProperty<bool> log;
        [SerializeField] private ReactiveProperty<bool> errorLog;
        [SerializeField] private float minimalWaitTime = 3;
        
        private UiService _uiService;
        private LogService _logService;
        private SaveService _saveService;
        private GameData _gameData;
        private AudioService _audioService;
      
        [Inject]
        private void Initialize(UiService uiService, LogService logService, SaveService saveService, GameData gameData, AudioService audioService)
        {
            _uiService = uiService;
            _logService = logService;
            _saveService = saveService;
            _gameData = gameData;
            _audioService = audioService;
        }

        private async void Start()
        {
            SetupLog();
            _uiService.Show<LoadingPopup>(PresenterType.Loading);
            UniTask loadDataTask = LoadGameDataAsync();
            await UniTask.WhenAll(loadDataTask, UniTask.Delay(TimeSpan.FromSeconds(minimalWaitTime)));
            _uiService.CloseLast();
            _audioService.Play(AudioType.Bg);
            
        }
        
        private async UniTask LoadGameDataAsync()
        {
            _gameData = await _saveService.LoadDataAsync();
            if (_gameData == null)
            {
                _gameData = GenerateDefaultData();
                _logService.Log("Generate Default date");
            }
        }
        private void SetupLog()
        {
            errorLog.ObserveEveryValueChanged(value => value.Value).Subscribe(value => _logService.IsErrorLog = value).AddTo(this);
            log.ObserveEveryValueChanged(value => value.Value).Subscribe(value => _logService.IsLog = value).AddTo(this);
        }

        private GameData GenerateDefaultData()
        {
            GameData gameData = new GameData();

            gameData.Coin = 0;
            gameData.Level = 1;
            return gameData;
        }

        private void OnApplicationQuit()
        {
            _saveService.Save(_gameData);
        }
    }
}