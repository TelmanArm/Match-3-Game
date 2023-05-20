using System.Collections.Generic;
using Board.Configs;
using UnityEngine;

namespace Board.Grid
{
    public interface IGrid
    {
        void Setup(BoardConfig boardConfig);
        IGridItem GetGridItem(Vector2Int key);
        Dictionary<Vector2Int, IGridItem> GridItems { get; }
        
    }
}