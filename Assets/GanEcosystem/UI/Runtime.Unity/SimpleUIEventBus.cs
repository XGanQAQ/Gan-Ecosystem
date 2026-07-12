using System;
using System.Collections.Generic;
using GanEcosystem.UI.Core;

namespace GanEcosystem.UI.UnityRuntime
{
    public class UnitySimpleUIEventBus : IUIEventBus
    {
        private static UnitySimpleUIEventBus instance;
        private readonly Dictionary<Type, Delegate> handlersByEventType = new Dictionary<Type, Delegate>();
        public static UnitySimpleUIEventBus Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new UnitySimpleUIEventBus();
                }
                return instance;
            }
        }

        public UnitySimpleUIEventBus()
        {
            instance = this;
        }

        public void Publish<TEvent>(TEvent uiEvent) where TEvent : IEvent
        {
            if (uiEvent == null)
            {
                throw new ArgumentNullException(nameof(uiEvent));
            }

            if (handlersByEventType.TryGetValue(typeof(TEvent), out var handlers))
            {
                ((Action<TEvent>)handlers)?.Invoke(uiEvent);
            }
        }

        public void Subscribe<TEvent>(Action<TEvent> handler) where TEvent : IEvent
        {
            if (handler == null)
            {
                throw new ArgumentNullException(nameof(handler));
            }

            var eventType = typeof(TEvent);
            if (handlersByEventType.TryGetValue(eventType, out var existingHandlers))
            {
                handlersByEventType[eventType] = (Action<TEvent>)existingHandlers + handler;
                return;
            }

            handlersByEventType[eventType] = handler;
        }

        public void Unsubscribe<TEvent>(Action<TEvent> handler) where TEvent : IEvent
        {
            if (handler == null)
            {
                throw new ArgumentNullException(nameof(handler));
            }

            var eventType = typeof(TEvent);
            if (!handlersByEventType.TryGetValue(eventType, out var existingHandlers))
            {
                return;
            }

            var updatedHandlers = (Action<TEvent>)existingHandlers - handler;
            if (updatedHandlers == null)
            {
                handlersByEventType.Remove(eventType);
                return;
            }

            handlersByEventType[eventType] = updatedHandlers;
        }
    }
}
