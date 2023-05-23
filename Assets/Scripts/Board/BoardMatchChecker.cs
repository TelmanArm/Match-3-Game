using System.Collections.Generic;
using Board.Item;
using UnityEngine;

namespace Board
{
    public class BoardMatchChecker
    {
        private Dictionary<Vector2Int, IBordItem> _boardItems;
        private int _matchCount = 3;
        private List<Vector2Int> _directions;
        private BoardItemSwapper _boardItemSwapper;

        public BoardMatchChecker(Dictionary<Vector2Int, IBordItem> bordItems, BoardItemSwapper boardItemSwapper)
        {
            _boardItems = bordItems;
            _directions = new List<Vector2Int> {Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right};
            _boardItemSwapper = boardItemSwapper;
        }

        public Dictionary<Vector2Int, IBordItem> FindItemsMatches(Vector2Int firstItem, Vector2Int secondItem,
            int successMatchCount = -1)
        {
            if (successMatchCount == -1) successMatchCount = _matchCount;
            Dictionary<Vector2Int, IBordItem> matchItems = new Dictionary<Vector2Int, IBordItem>();

            var matchFirst = FindItemMatches(_boardItems[firstItem], successMatchCount);
            var matchSecond = FindItemMatches(_boardItems[secondItem], successMatchCount);
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

        public Dictionary<Vector2Int, IBordItem> FindItemMatches(IBordItem bordItem, int successMatchCount = -1)
        {
            if (successMatchCount == -1) successMatchCount = _matchCount;
            Dictionary<Vector2Int, IBordItem> matched = new Dictionary<Vector2Int, IBordItem>();
            BoardItemType itemType = bordItem.BoardItemType;
            Dictionary<Vector2Int, IBordItem> matchedX = new Dictionary<Vector2Int, IBordItem>();
            Dictionary<Vector2Int, IBordItem> matchedY = new Dictionary<Vector2Int, IBordItem>();
            matchedX.Add(bordItem.Key, bordItem);
            matchedY.Add(bordItem.Key, bordItem);
            CheckNext(matchedX, bordItem.Key, Vector2Int.right, itemType);
            CheckNext(matchedX, bordItem.Key, Vector2Int.left, itemType);
            CheckNext(matchedY, bordItem.Key, Vector2Int.up, itemType);
            CheckNext(matchedY, bordItem.Key, Vector2Int.down, itemType);
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

        public List<Vector2Int> FindPossibleMatch()
        {
            List<Vector2Int> possibleMatch = new List<Vector2Int>();
            List<Vector2Int> boardItemKey = GenerateItemsKeyLIst();
            foreach (Vector2Int itemKey in boardItemKey)
            {
                foreach (Vector2Int dVector2Int in _directions)
                {
                    SwapData swapData = _boardItemSwapper.CheckSwapPossibility(_boardItems[itemKey], dVector2Int);
                    if (swapData.IsPossibly)
                    {
                        _boardItemSwapper.Swap(swapData.FirstItem, swapData.SecondItem);
                        var matches = FindItemMatches(_boardItems[swapData.SecondItem]);
                        _boardItemSwapper.Swap(swapData.FirstItem, swapData.SecondItem);
                        if (matches.Count > 0)
                        {
                            possibleMatch.Add(itemKey);
                            break;
                        }
                    }
                }
            }

            return possibleMatch;
        }

        public List<Vector2Int> GenerateItemsKeyLIst()
        {
            List<Vector2Int> keys = new List<Vector2Int>();
            foreach (var item in _boardItems)
            {
                keys.Add(item.Key);
            }

            return keys;
        }

        public Dictionary<string, List<Vector2Int>> FindTwoMatch()
        {
            Dictionary<string, List<Vector2Int>> match = new Dictionary<string, List<Vector2Int>>();

            foreach (var boardItem in _boardItems)
            {
                var matchTwo = FindItemMatches(_boardItems[boardItem.Key], 2);
                if (matchTwo.Count > 0)
                {
                    List<Vector2Int> keys = new List<Vector2Int>();
                    string keyMatchDirOne = "";
                    string keyMatchDirTwo = "";
                    foreach (var item in matchTwo)
                    {
                        keyMatchDirOne = keyMatchDirOne + item.Key;
                        keyMatchDirTwo = item.Key + keyMatchDirTwo;
                        keys.Add(item.Key);
                    }

                    if (!match.ContainsKey(keyMatchDirOne) || !match.ContainsKey(keyMatchDirOne))
                    {
                        match.Add(keyMatchDirOne, keys);
                    }
                }
            }

            return match;
        }
    }
}