using System;
using DG.Tweening;
using Services.UI;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Loading
{
    public class LoadingPopup : BasePresenter
    {
        [SerializeField] private GameObject loadingBar;
        private float _rotateSpeed = 60f;
        public override void Show(IPresenterData presenterData, Action onShow)
        {
            base.Show(presenterData, onShow);
            RotateBar();
        }

        private void RotateBar()
        {
        
            Observable.EveryUpdate().Subscribe(_ =>
            {
                float rotate = _rotateSpeed * Time.time;
                loadingBar.transform.rotation = Quaternion.Euler(0f, 0f, rotate);
            }).AddTo(this);
        }
    }
}
