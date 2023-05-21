using System;
using System.Collections.Generic;
using Board.Configs;
using Board.Grid;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Game;
using Services.Audio;
using Services.Log;
using Services.Save;
using Services.UI;
using UnityEngine;
using Zenject;
using AudioType = Services.Audio.AudioType;

namespace Board
{
    public class BoardController : MonoBehaviour
    {
        public event Action<Vector2Int> OnItemMove;

        [SerializeField] private BoardConfig boardConfig;
        [SerializeField] private Transform itemsHolder;

        private Dictionary<Vector2Int, IBordItem> _boardItems;
        private IGrid _grid;
        private BoardMatchChecker _boardMatchChecker;
        private BoardItemSwapper _boardItemSwapper;
        private BoardShuffle _boardShuffle;

        private UiService _uiService;
        private LogService _logService;
        private SaveService _saveService;
        private GameData _gameData;
        private AudioService _audioService;

        [Inject]
        private void Initialize(UiService uiService, LogService logService, GameData gameData,
            AudioService audioService)
        {
            _uiService = uiService;
            _logService = logService;
            _gameData = gameData;
            _audioService = audioService;
        }

        public void Setup()
        {
            _grid = new GridController();
            _grid.Setup(boardConfig);
            _boardItems = new Dictionary<Vector2Int, IBordItem>();
            _boardItemSwapper = new BoardItemSwapper(_boardItems, _grid);
            _boardMatchChecker = new BoardMatchChecker(_boardItems, _boardItemSwapper);
            GenerateItems();
            _boardShuffle = new BoardShuffle(_boardItems, _grid, _boardItemSwapper);
            UniTask task = _boardShuffle.ShuffleAsync(0.1f);
        }

        public void Help()
        {
           var  possibleMatch = _boardMatchChecker.FindPossibleMatch();
           foreach (var item in possibleMatch)
           {
               _boardItems[item].Mark();
           }
        }

        private void GenerateItems()
        {
            foreach (var gridItem in _grid.GridItems)
            {
                if (gridItem.Value.IsClose) continue;
                int index = GetRandom(0, boardConfig.BoardItemData.Count);
                AddBoardItem(gridItem.Value, index);
                ChangeMatchItem(gridItem.Value, index);
            }
        }

        private void AddBoardItem(IGridItem gridItem, int index, Vector2 itemGeneratePos = default)
        {
            IBordItem boardItem = GetBoardItem();
            BoardItemData itemData = boardConfig.BoardItemData[index];
            boardItem.Setup(gridItem.Key, itemData);
            if (itemGeneratePos == default)
            {
                boardItem.MoveTo(gridItem.Position);
            }
            else
            {
                boardItem.MoveTo(itemGeneratePos);
            }

            if (_boardItems.ContainsKey(gridItem.Key))
            {
                if (_boardItems[gridItem.Key] != null)
                {
                    IBordItem bordItem = _boardItems[gridItem.Key];
                    bordItem.OnMove -= OnMove;
                    Destroy(bordItem.GetGameObject());
                    _boardItems[gridItem.Key] = null;
                }

                _boardItems[gridItem.Key] = boardItem;
            }
            else
            {
                _boardItems.Add(gridItem.Key, boardItem);
            }

            boardItem.OnMove += OnMove;
        }

        private void ChangeMatchItem(IGridItem gridItem, int indexItem)
        {
            if (_boardMatchChecker.FindItemMatches(_boardItems[gridItem.Key]).Count > 0)
            {
                indexItem++;
                if (indexItem >= boardConfig.BoardItemData.Count)
                {
                    indexItem = 0;
                }

                AddBoardItem(gridItem, indexItem);
                ChangeMatchItem(gridItem, indexItem);
            }
        }

        private IBordItem GetBoardItem()
        {
            IBordItem bordItem;
            bordItem = (IBordItem) Instantiate(boardConfig.BoardItemPrefab, itemsHolder);
            bordItem.SetActivity(true);
            return bordItem;
        }

        private int GetRandom(int min, int max)
        {
            System.Random rnd = new System.Random();
            int i = rnd.Next(min, max);
            return i;
        }

        private void OnMove(IBordItem bordItem, Vector2Int direction)
        {
            SwapData swapData = _boardItemSwapper.CheckSwapPossibility(bordItem, direction);
            if (swapData.IsPossibly)
            {
                _boardItemSwapper.Swap(swapData.FirstItem, swapData.SecondItem);
                var matchItems = _boardMatchChecker.FindItemsMatches(swapData.FirstItem, swapData.SecondItem);
                if (matchItems.Count > 0)
                {
                    UniTask task = MatchAsync(matchItems, swapData, boardConfig.AnimationSpeed);
                }
                else
                {
                    _boardItemSwapper.Swap(swapData.FirstItem, swapData.SecondItem);
                    _boardItems[swapData.FirstItem].Shake();
                }
            }
            else
            {
                _boardItems[swapData.FirstItem].Shake();
            }
        }

        private async UniTask MatchAsync(Dictionary<Vector2Int, IBordItem> matchItems, SwapData swapData,
            float duration)
        {
            await _boardItemSwapper.SwapPosAsync(duration, swapData);
            await RemoveMatchedItemAsync(matchItems, duration);
            await AddNewItemsAsync(duration);
            await UpdateBoardAsync(duration);
            await _boardShuffle.ShuffleAsync(duration);
        }

        private async UniTask RemoveMatchedItemAsync(Dictionary<Vector2Int, IBordItem> matchItems, float duration)
        {
            _audioService.Play(AudioType.Match);
            foreach (var item in matchItems)
            {
                IBordItem bordItem = _boardItems[item.Key];
                bordItem.OnMove -= OnMove;
                bordItem.GetGameObject().transform.DOScale(1.2f, boardConfig.AnimationSpeed);
            }

            await UniTask.Delay(TimeSpan.FromSeconds(duration));
            foreach (var item in matchItems)
            {
                IBordItem bordItem = _boardItems[item.Key];
                bordItem.OnMove -= OnMove;
                Destroy(bordItem.GetGameObject());
                _boardItems[item.Key] = null;
            }

            await UniTask.Delay(TimeSpan.FromSeconds(duration));
        }

        private async UniTask AddNewItemsAsync(float duration)
        {
            List<Vector2Int> newItemsKeys = new List<Vector2Int>();
            newItemsKeys = AddNewItems();
            foreach (Vector2Int vector2Int in newItemsKeys)
            {
                IGridItem gridItem = _grid.GetGridItem(vector2Int);
                IBordItem item = _boardItems[vector2Int];
                item.MoveTo(gridItem.Position, duration);
            }

            await UniTask.Delay(TimeSpan.FromSeconds(duration));
        }

        private async UniTask UpdateBoardAsync(float duration)
        {
            var allMatch = new Dictionary<Vector2Int, IBordItem>();
            foreach (var items in _boardItems)
            {
                var matchesForKey = _boardMatchChecker.FindItemMatches(items.Value);
                foreach (var matchiItemtem in matchesForKey)
                {
                    if (allMatch.ContainsKey(matchiItemtem.Key))
                    {
                        continue;
                    }

                    allMatch.Add(matchiItemtem.Key, matchiItemtem.Value);
                }
            }

            if (allMatch.Count > 0)
            {
                await RemoveMatchedItemAsync(allMatch, duration);
                await AddNewItemsAsync(duration);
                await UpdateBoardAsync(duration);
            }
        }

        private List<Vector2Int> AddNewItems()
        {
            List<Vector2Int> newItems = new List<Vector2Int>();
            for (int x = 0; x < boardConfig.GridSize.x; x++)
            {
                Vector2Int starKey = new Vector2Int(x, 0);
                Vector2Int lastItemKey = GetPosLastElementColumn(x);
                IGridItem topGriItem = _grid.GetGridItem(lastItemKey);
                Vector2 itemGeneratePos = topGriItem.Position + Vector2.up;
                AddItemsColumn(starKey, newItems, itemGeneratePos);
            }

            return newItems;
        }

        private Vector2Int GetPosLastElementColumn(int x)
        {
            int y = 0;
            bool isLast = false;
            Vector2Int leastElementKey = default;
            while (!isLast)
            {
                Vector2Int key = new Vector2Int(x, y);
                if (_grid.GridItems.ContainsKey(key))
                {
                    if (_boardItems.ContainsKey(key))
                    {
                        leastElementKey = key;
                    }
                }
                else
                {
                    isLast = true;
                }

                y++;
            }

            return leastElementKey;
        }

        private void AddItemsColumn(Vector2Int startKey, List<Vector2Int> newItems, Vector2 itemGeneratePos)
        {
            if (_grid.GridItems.ContainsKey(startKey))
            {
                if (_boardItems.ContainsKey(startKey))
                {
                    if (_boardItems[startKey] == null)
                    {
                        Vector2Int next = startKey;
                        bool isAdded = false;
                        while (!isAdded)
                        {
                            next += Vector2Int.up;
                            if (_boardItems.ContainsKey(next))
                            {
                                if (_boardItems[next] != null)
                                {
                                    IBordItem bordItem = _boardItems[next];
                                    bordItem.Key = startKey;
                                    _boardItems[next] = null;
                                    _boardItems[startKey] = bordItem;
                                    newItems.Add(startKey);
                                    isAdded = true;
                                }
                            }
                            else
                            {
                                IGridItem gridItem = _grid.GetGridItem(startKey);
                                int index = GetRandom(0, boardConfig.BoardItemData.Count);
                                AddBoardItem(gridItem, index, itemGeneratePos);
                                ChangeMatchItem(gridItem, index);
                                itemGeneratePos += Vector2.up;
                                newItems.Add(gridItem.Key);
                                isAdded = true;
                            }
                        }
                    }
                }

                startKey += Vector2Int.up;
                AddItemsColumn(startKey, newItems, itemGeneratePos);
            }
        }
    }
}