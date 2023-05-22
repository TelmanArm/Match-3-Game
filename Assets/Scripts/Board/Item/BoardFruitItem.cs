using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Board
{
    public class BoardFruitItem : BaseBordItem, IBordItem, IMoveBoardItem, IDragHandler, IEndDragHandler,
        IBeginDragHandler
    {
        [SerializeField] private SpriteRenderer image;
        private bool _isDrag;

        public event Action<IBordItem, Vector2Int> OnMove;
        public Vector2Int Key { get; set; }

        public BoardItemType BoardItemType
        {
            get { return _boardItemData.BoardItemType; }
        }

        private BoardItemData _boardItemData;

        public void Setup(Vector2Int key, BoardItemData boardItemData)
        {
            image.sprite = boardItemData.Sprite;
            Key = key;
            _boardItemData = boardItemData;
        }

        public void SetActivity(bool isActive)
        {
            gameObject.SetActive(isActive);
        }

        public void MoveTo(Vector2 position, float duration = 0)
        {
            transform.DOMove(position, duration);
        }

        public GameObject GetGameObject()
        {
            return gameObject;
        }

        public void Shake()
        {
            transform.DOShakePosition(1f, 0.1f);
        }

        public void Mark()
        {
            transform.DOShakeScale(0.5f, 0.5f);
        }

        public void Rest()
        {
          transform.localScale = Vector3.one;
        }

        public void RemoveEffect(float speed)
        {
            transform.DOScale(1.2f, speed);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            _isDrag = false;
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (_isDrag) return;
            Vector2Int direction;
            if (Mathf.Abs(eventData.delta.x) < Mathf.Abs(eventData.delta.y))
            {
                if (eventData.delta.y > 0)
                {
                    direction = Vector2Int.up;
                }
                else
                {
                    direction = Vector2Int.down;
                }
            }
            else
            {
                if (eventData.delta.x > 0)
                {
                    direction = Vector2Int.right;
                }
                else
                {
                    direction = Vector2Int.left;
                }
            }

            OnMove?.Invoke(this, direction);
            _isDrag = true;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            _isDrag = false;
        }
    }
}