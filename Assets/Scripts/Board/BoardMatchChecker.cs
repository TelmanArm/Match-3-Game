using System.Collections.Generic;
using UnityEngine;

namespace Board
{
    public class BoardMatchChecker
    {
        private Dictionary<Vector2Int, IBordItem> _boardItems;
        private int _matchCount = 3;

        public BoardMatchChecker(Dictionary<Vector2Int, IBordItem> bordItems)
        {
            _boardItems = bordItems;
        }

        public Dictionary<Vector2Int, IBordItem> GetItemsMatch(Vector2Int firstItem, Vector2Int secondItem,
            int successMatchCount = -1)
        {
            if (successMatchCount == -1) successMatchCount = _matchCount;
            Dictionary<Vector2Int, IBordItem> matchItems = new Dictionary<Vector2Int, IBordItem>();
            var matchFirst = GetItemMatch(firstItem, successMatchCount);
            var matchSecond = GetItemMatch(secondItem, successMatchCount);
            if (matchFirst.Count >= successMatchCount)
            {
                foreach (var item in matchFirst)
                {
                    matchItems.Add(item.Key, item.Value);
                }
            }

            if (matchSecond.Count >= successMatchCount)
            {
                foreach (var item in matchSecond)
                {
                    if (matchItems.ContainsKey(item.Key)) continue;
                    matchItems.Add(item.Key, item.Value);
                }
            }

            return matchItems;
        }

        public Dictionary<Vector2Int, IBordItem> GetItemMatch(Vector2Int startItem, int successMatchCount = -1)
        {
            if (successMatchCount == -1) successMatchCount = _matchCount;
            Dictionary<Vector2Int, IBordItem> matched = new Dictionary<Vector2Int, IBordItem>();
            IBordItem startBoardItem = _boardItems[startItem];
            BoardItemType itemType = startBoardItem.BoardItemType;
            Dictionary<Vector2Int, IBordItem> matchedX = new Dictionary<Vector2Int, IBordItem>();
            Dictionary<Vector2Int, IBordItem> matchedY = new Dictionary<Vector2Int, IBordItem>();
            matchedX.Add(startItem, startBoardItem);
            matchedY.Add(startItem, startBoardItem);
            CheckNext(matchedX, startItem, Vector2Int.right, itemType);
            CheckNext(matchedX, startItem, Vector2Int.left, itemType);
            CheckNext(matchedY, startItem, Vector2Int.up, itemType);
            CheckNext(matchedY, startItem, Vector2Int.down, itemType);
            if (matchedX.Count >= successMatchCount)
            {
                foreach (var item in matchedX)
                {
                    if (matched.ContainsKey(item.Key)) continue;
                    matched.Add(item.Key, item.Value);
                }
            }

            if (matchedY.Count >= successMatchCount)
            {
                foreach (var item in matchedY)
                {
                    if (matched.ContainsKey(item.Key)) continue;
                    matched.Add(item.Key, item.Value);
                }
            }

            return matched;
        }

        private void CheckNext(Dictionary<Vector2Int, IBordItem> matchedItem, Vector2Int startItem,
            Vector2Int direction, BoardItemType itemType)
        {
            Vector2Int nextItemKey = startItem + direction;
            if (_boardItems.ContainsKey(nextItemKey) && _boardItems[nextItemKey] != null)
            {
                IBordItem nexItem = _boardItems[nextItemKey];
                if (nexItem.BoardItemType == itemType)
                {
                    matchedItem.Add(nextItemKey, nexItem);
                    CheckNext(matchedItem, nextItemKey, direction, itemType);
                }
            }
        }
    }
}