using System;
using System.Collections.Generic;
using Board.Grid;
using Cysharp.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

namespace Board
{
    public class BoardShuffle
    {
        private BoardItemSwapper _boardItemSwapper;
        private BoardMatchChecker _boardMatchChecker;
        private Dictionary<Vector2Int, IBordItem> _boardItems;
        private Dictionary<string, List<Vector2Int>> _twoMatch;
        private bool _isMatch;

        public BoardShuffle(Dictionary<Vector2Int, IBordItem> boardItems)
        {
            _boardItems = boardItems;
            _boardItemSwapper = new BoardItemSwapper(_boardItems);
            _boardMatchChecker = new BoardMatchChecker(_boardItems);
        }

        public async UniTask ShuffleAsync()
        {
            List<Vector2Int> direction = new List<Vector2Int>
                {Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right};
            _twoMatch = new Dictionary<string, List<Vector2Int>>();
            foreach (var bordItem in _boardItems)
            {
                CheckTwoMatch(bordItem.Key);
                foreach (Vector2Int dVector2Int in direction)
                {
                    SwapData swapData = _boardItemSwapper.CheckSwapPossibility(bordItem.Value, dVector2Int);
                    if (swapData.IsPossibly)
                    {
                       // _boardItemSwapper.Swap(swapData.FirstItem, swapData.SecondItem);
                        var matches = _boardMatchChecker.GetItemsMatch(swapData.FirstItem, swapData.SecondItem);
                       // _boardItemSwapper.Swap(swapData.FirstItem, swapData.SecondItem);
                        if (matches.Count > 0)
                        {
                            _isMatch = true;
                            break;
                        }
                    } 
                    if (_isMatch) 
                        break;
                }
            }

            await UniTask.Delay(TimeSpan.FromSeconds(1));
        }

        private void CheckTwoMatch(Vector2Int key)
        {
           var match = _boardMatchChecker.GetItemMatch(key, 2);
           if (match.Count > 0)
           {
               List<Vector2Int> keys = new List<Vector2Int>();
               string keyMatchDirOne = "";
               string keyMatchDirTwo = "";
               foreach (var item in match)
               {
                   keyMatchDirOne = keyMatchDirOne + item.Key;
                   keyMatchDirTwo = item.Key + keyMatchDirTwo;
                   keys.Add(item.Key);
               }

               if (!_twoMatch.ContainsKey(keyMatchDirOne) || !_twoMatch.ContainsKey(keyMatchDirOne))
               {
                   _twoMatch.Add(keyMatchDirOne, keys);
               }
           }
        }
    }
}