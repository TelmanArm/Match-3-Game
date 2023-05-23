using System;
using UnityEngine;

namespace Board.Item
{
    public interface IBordItem
    {
        public event Action<IBordItem, Vector2Int> OnMove; 
        public Vector2Int Key { get; set; }
        public BoardItemType BoardItemType { get; }
        void Setup(Vector2Int key, BoardItemData boardItemData);
        void SetActivity(bool isActive);
        void MoveTo(Vector2 position, float duration = 0);
        GameObject GetGameObject();
        void Shake();
        void Mark();
        void Rest();
        void RemoveEffect(float speed);

    }
}