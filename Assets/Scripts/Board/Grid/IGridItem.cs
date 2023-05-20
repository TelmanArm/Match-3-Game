using UnityEngine;

namespace Board.Grid
{
    public interface IGridItem
    {
        public bool IsClose { get; set; }
        public Vector2Int Key { get; set; }
        public Vector2 Position { get; set; }
    }
}