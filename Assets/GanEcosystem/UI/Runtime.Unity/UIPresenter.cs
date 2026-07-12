using GanEcosystem.UI.Core;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace GanEcosystem.UI.UnityRuntime
{
    public class UIPresenter : MonoBehaviour, IPresenter
    {
        protected UIViewer autoViewer;

        private IUIEventBus _uiEventBus;

        private List<Action<IEvent>> _eventHandlers = new List<Action<IEvent>>();

        protected virtual void Awake()
        {
            autoViewer = GetComponent<UIViewer>();
        }

        public virtual void Init()
        {
        }

        protected virtual void OnDestroy()
        {
            foreach (var handler in _eventHandlers)
            {
                _uiEventBus.Unsubscribe(handler);
            }
            _eventHandlers.Clear();
        }

        public void SubscribeToEvent<TEvent>(Action<TEvent> callback) where TEvent : IEvent
        {
            _uiEventBus.Subscribe(callback);
        }

        public void UnsubscribeFromEvent<TEvent>(Action<TEvent> callback) where TEvent : IEvent
        {
            _uiEventBus.Unsubscribe(callback);
        }

        public virtual void OnGetEvent(IEvent passedEvent)
        {
            Debug.Log($"[UI] OnGetEvent: {passedEvent}");
        }
    }
}

