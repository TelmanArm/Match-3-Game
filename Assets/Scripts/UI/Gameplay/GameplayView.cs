using System;
using Game;
using Services.Audio;
using Services.Log;
using Services.UI;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using AudioType = Services.Audio.AudioType;

namespace UI.Gameplay
{
    public class GameplayView : BasePresenter
    {
        [SerializeField] private Button helpButton;
        private GameplayViewData _gameplayViewData;
        private AudioService _audioService;

        [Inject]
        private void Initialize(AudioService audioService)
        {
            _audioService = audioService;
        }

        private void Awake()
        {
            helpButton.onClick.AddListener(OnHelp);
        }

        public override void Show(IPresenterData presenterData, Action onShow)
        {
            base.Show(presenterData, onShow);
            if (presenterData != null)
            {
                _gameplayViewData = (GameplayViewData) presenterData;
            }
        }

        private void OnHelp()
        {
            _audioService.Play(AudioType.Help);
            _gameplayViewData.BoardController.Help();
        }

        private void OnDestroy()
        {
            helpButton.onClick.RemoveAllListeners();
        }
    }
}