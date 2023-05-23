using Board;
using Services.UI;
using UI.Gameplay;
using UnityEngine;
using Zenject;

namespace Game
{
    public class GameplayController : MonoBehaviour
    {
        [SerializeField] private BoardController boardController;
        private UiService _uiService;
        [Inject]
        private void Initialize(UiService uiService)
        {
            _uiService = uiService;
        }

        private void Start()
        {
            boardController.Setup();
            GameplayViewData gameplayViewData = new GameplayViewData();
            gameplayViewData.BoardController = boardController;
            _uiService.Show<GameplayView>(PresenterType.Gameplay, gameplayViewData);
        }
    }
}