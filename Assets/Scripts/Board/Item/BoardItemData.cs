using System;
using UnityEngine;

namespace Board
{
    [Serializable]
    public class BoardItemData
    {
        [SerializeField] private BoardItemType boardItemType;
        [SerializeField] private Sprite sprite;

        public BoardItemType BoardItemType => boardItemType;
        public Sprite Sprite => sprite;
    }
}