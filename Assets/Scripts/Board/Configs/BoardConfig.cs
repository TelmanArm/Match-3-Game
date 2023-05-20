using System.Collections.Generic;
using UnityEngine;

namespace Board.Configs
{
    [CreateAssetMenu(fileName = "BoardController", menuName = "Config/BoardController")]
    public class BoardConfig : ScriptableObject
    {
        [SerializeField] private Vector2Int gridSize;
        [SerializeField] private Vector2 startPoint;
        [SerializeField] private BaseBordItem boardItemPrefab;
        [SerializeField] private List<BoardItemData> boardItemData;
        [SerializeField] private List<Vector2Int> closeGridFields;
        [SerializeField] private float animationSpeed;

        public BaseBordItem BoardItemPrefab => boardItemPrefab;
        public List<BoardItemData> BoardItemData => boardItemData;
        public Vector2Int GridSize => gridSize;
        public Vector2 StartPoint => startPoint;
        public List<Vector2Int> CloseGridFields => closeGridFields;
        public float AnimationSpeed => animationSpeed;
    }
}