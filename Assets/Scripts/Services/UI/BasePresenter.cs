using System;
using UnityEngine;

namespace Services.UI
{
    public class BasePresenter : MonoBehaviour
    {
        public virtual void Show(IPresenterData presenterData, Action onShow)
        {
            onShow?.Invoke();
        }

        public virtual void Close(Action onClose)
        {
            onClose?.Invoke();
            Destroy(gameObject);
        }
    }
}