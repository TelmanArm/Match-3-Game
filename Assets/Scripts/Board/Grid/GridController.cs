using System.Collections.Generic;
using System.Linq;
using Board.Configs;
using UnityEngine;

namespace Board.Grid
{
    public class GridController : IGrid
    {
        public Dictionary<Vector2Int, IGridItem> GridItems {
            get
            {
                return _gridItems;
            }
        }
        
        private Dictionary<Vector2Int, IGridItem> _gridItems;
        public void Setup(BoardConfig boardConfig)
        {
            _gridItems = new Dictionary<Vector2Int, IGridItem>();
            for (int x = 0; x < boardConfig.GridSize.x; x++)
            {
                for (int y = 0; y < boardConfig.GridSize.y; y++)
                {
                    IGridItem gridItem = new GridItem();
                    Vector2Int key = new Vector2Int(x, y);
                    gridItem.Key = key;
                    gridItem.Position = new Vector2(boardConfig.StartPoint.x + x, boardConfig.StartPoint.y + y);
                    bool containsVector = boardConfig.CloseGridFields.Any(item => item.x == x && item.y == y);
                    gridItem.IsClose = containsVector;
                    _gridItems.Add(key, gridItem);
                }
            }
        }

        public IGridItem GetGridItem(Vector2Int key)
        {
            if (_gridItems.ContainsKey(key))
            {
                return _gridItems[key];
            }
            else
            {
                return null;
            }
        }

       
    }
}