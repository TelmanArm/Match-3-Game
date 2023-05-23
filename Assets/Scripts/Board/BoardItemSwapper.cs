using System;
using System.Collections.Generic;
using Board.Grid;
using Board.Item;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Board
{
    public class BoardItemSwapper
    {
        private Dictionary<Vector2Int, IBordItem> _boardItems;
        private IGrid _grid;

        public BoardItemSwapper(Dictionary<Vector2Int, IBordItem> bordItems, IGrid grid)
        {
            _boardItems = bordItems;
            _grid = grid;
        }

        public SwapData CheckSwapPossibility(IBordItem bordItem, Vector2Int direction)
        {
            SwapData swapData = new SwapData();
            Vector2Int moveToPosition = bordItem.Key;
            moveToPosition = moveToPosition + direction;
            if (_boardItems.ContainsKey(moveToPosition) && _boardItems[moveToPosition] != null)
            {
                swapData.IsPossibly = true;
                swapData.FirstItem = bordItem.Key;
                swapData.SecondItem = moveToPosition;
            }
            else
            {
                swapData.IsPossibly = false;
                swapData.FirstItem = bordItem.Key;
            }

            return swapData;
        }

        public async UniTask SwapPosAsync(float duration, SwapData swapData)
        {
            _boardItems[swapData.SecondItem].MoveTo(_grid.GridItems[swapData.SecondItem].Position, duration);
            _boardItems[swapData.FirstItem].MoveTo(_grid.GridItems[swapData.FirstItem].Position, duration);
            await UniTask.Delay(TimeSpan.FromSeconds(duration));
        }

        public void Swap(Vector2Int fires, Vector2Int second)
        {
            IBordItem firstItem = _boardItems[fires];
            IBordItem secondItem = _boardItems[second];
            firstItem.Key = second;
            secondItem.Key = fires;
            _boardItems[fires] = secondItem;
            _boardItems[second] = firstItem;
        }
    }

    public class SwapData
    {
        public bool IsPossibly;
        public Vector2Int FirstItem;
        public Vector2Int SecondItem;
    }
}