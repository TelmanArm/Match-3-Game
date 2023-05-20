using System;
using System.Collections.Generic;
using System.Linq;
using Services.Log;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace Services.UI
{
    public class UiService : MonoBehaviour
    {
        [SerializeField] private RectTransform presenterHolder;
        [SerializeField] private List<PresenterItem> presenterItems;
        private Dictionary<PresenterType, BasePresenter> _activePresenters;
        private BasePresenter _lastOpenPresenter;
        private LogService _logService;

        [Inject]
        public void Initialize(LogService logService)
        {
            _logService = logService;
        }

        private void Awake()
        {
            _activePresenters = new Dictionary<PresenterType, BasePresenter>();
        }

        public T Show<T>(PresenterType type, IPresenterData presenterData = null, Action onShow = null)
            where T : BasePresenter
        {
            PresenterItem presenterItem = presenterItems.FirstOrDefault(item => item.presenterType == type);
            if (presenterItem == null)
            {
                _logService.ErrorLog($"The presenter whit type : {type.ToString()} does not exist ");
                return null;
            }
            else
            {
                BasePresenter presenter = Instantiate(presenterItem.basePresenter, presenterHolder);
                _lastOpenPresenter = presenter;
                _activePresenters.Add(type, presenter);
                presenter.Show(presenterData, onShow);
                return (T)presenter;
            }
            
        }

        public void Close(PresenterType type)
        {
            if (_activePresenters.ContainsKey(type))
            {
                BasePresenter presenter = _activePresenters[type];
                presenter.Close(null);
                _activePresenters.Remove(type);
            }
            else
            {
                _logService.ErrorLog($"{type} is not open");
            }
        }
        public void CloseLast()
        {
            if (_lastOpenPresenter != null)
            {
                _lastOpenPresenter.Close(null);
            }
            else
            { 
                _logService.ErrorLog($"Last presenter was close");
            }
        }

        [Serializable]
        private class PresenterItem
        {
            public PresenterType presenterType;
            public BasePresenter basePresenter;
        }
    }
}