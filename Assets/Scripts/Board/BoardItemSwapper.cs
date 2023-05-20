using System.Collections.Generic;
using UnityEngine;

namespace Board
{
    public class BoardItemSwapper
    {
        private Dictionary<Vector2Int, IBordItem> _boardItems;

        public BoardItemSwapper(Dictionary<Vector2Int, IBordItem> bordItems)
        {
            _boardItems = bordItems;
        }

        public SwapData CheckSwapPossibility(IBordItem bordItem, Vector2Int direction)
        {
            SwapData swapData = new SwapData();
            Vector2Int moveToPosition = bordItem.Key;
            moveToPosition =  moveToPosition + direction;
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