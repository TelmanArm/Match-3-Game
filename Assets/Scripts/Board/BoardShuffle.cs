using System;
using System.Collections.Generic;
using System.Linq;
using Board.Grid;
using Cysharp.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using Random = System.Random;

namespace Board
{
    public class BoardShuffle
    {
        private BoardItemSwapper _boardItemSwapper;
        private BoardMatchChecker _boardMatchChecker;
        private Dictionary<Vector2Int, IBordItem> _boardItems;
        private bool _isMatch;
        private List<Vector2Int> directions;

        public BoardShuffle(Dictionary<Vector2Int, IBordItem> boardItems, IGrid grid, BoardItemSwapper boardItemSwapper)
        {
            _boardItems = boardItems;
            _boardItemSwapper = new BoardItemSwapper(_boardItems, grid);
            _boardMatchChecker = new BoardMatchChecker(_boardItems, boardItemSwapper);
            directions = new List<Vector2Int> {Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right};
        }

        public async UniTask ShuffleAsync(float duration)
        {
            List<Vector2Int> possibleMarch;
            possibleMarch = _boardMatchChecker.FindPossibleMatch();
            if (possibleMarch.Count <= 0)
            {
                Shuffle();
            }

            await UniTask.Delay(TimeSpan.FromSeconds(duration));
        }

        private void Shuffle()
        {
            var twoMatch = _boardMatchChecker.FindTwoMatch();
            List<KeyValuePair<string, List<Vector2Int>>> listMatch =
                new List<KeyValuePair<string, List<Vector2Int>>>(twoMatch);
            List<Vector2Int> twokeys = null;
            if (twoMatch.Count > 0)
            {
                int index = GetRandom(0, twoMatch.Count);
                twokeys = listMatch[index].Value;
            }

            if (twokeys == null) return;
            Vector2Int first = twokeys[0];
            Vector2Int second = twokeys[1];
            Vector2Int nextKey = FindNextKey(first, second);
            BoardItemType boardItemType = _boardItems[first].BoardItemType;
            List<Vector2Int> boardItemsKey = _boardMatchChecker.GenerateItemsKeyLIst();
            bool isShuffle = false;
            foreach (Vector2Int direction in directions)
            {
                Vector2Int itemKeyForChange = nextKey + direction;
                if (!_boardItems.ContainsKey(itemKeyForChange))
                {
                    continue;
                }

                foreach (var key in boardItemsKey)
                {
                    IBordItem itemCheck = _boardItems[key];
                    Vector2Int checkItemKey = itemCheck.Key;
                    if (twokeys.Contains(itemCheck.Key) || itemCheck.BoardItemType != boardItemType)
                    {
                        continue;
                    }

                    _boardItemSwapper.Swap(itemKeyForChange, checkItemKey);
                    var matches = _boardMatchChecker.FindItemsMatches(itemKeyForChange, itemCheck.Key);
                    if (matches.Count > 0)
                    {
                        _boardItemSwapper.Swap(itemKeyForChange, checkItemKey);
                    }
                    else
                    {
                        isShuffle = true;
                        SwapData swapData = new SwapData();
                        swapData.IsPossibly = true;
                        swapData.FirstItem = checkItemKey;
                        swapData.SecondItem = itemKeyForChange;
                        UniTask task = _boardItemSwapper.SwapPosAsync(1, swapData);
                        break;
                    }
                }

                if (isShuffle) break;
            }
        }

        private Vector2Int FindNextKey(Vector2Int first, Vector2Int second)
        {
            Vector2Int nextKey;
            Vector2Int bigValue;
            Vector2Int smallValue;
            if (first.x == second.x)
            {
                if (first.y > second.y)
                {
                    bigValue = first;
                    smallValue = second;
                }
                else
                {
                    bigValue = second;
                    smallValue = first;
                }

                if (_boardItems.ContainsKey(bigValue + Vector2Int.up))
                {
                    nextKey = bigValue + Vector2Int.up;
                }
                else
                {
                    nextKey = smallValue + Vector2Int.down;
                }
            }
            else
            {
                if (first.x > second.x)
                {
                    bigValue = first;
                    smallValue = second;
                }
                else
                {
                    bigValue = second;
                    smallValue = first;
                }

                if (_boardItems.ContainsKey(bigValue + Vector2Int.right))
                {
                    nextKey = bigValue + Vector2Int.right;
                }
                else
                {
                    nextKey = smallValue + Vector2Int.left;
                }
            }

            return nextKey;
        }

        private int GetRandom(int min, int max)
        {
            Random rnd = new Random();
            return rnd.Next(min, max);
        }
    }
}