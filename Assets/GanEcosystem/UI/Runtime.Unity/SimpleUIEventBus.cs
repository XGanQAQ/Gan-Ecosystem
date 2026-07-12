using System;
using GanEcosystem.UI.Core;

namespace GanEcosystem.UI.UnityRuntime
{
    public class UnitySimpleUIEventBus : IUIEventBus
    {
        private static UnitySimpleUIEventBus instance;
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
            throw new NotImplementedException();
        }

        public void Subscribe<TEvent>(Action<TEvent> handler) where TEvent : IEvent
        {
            throw new NotImplementedException();
        }

        public void Unsubscribe<TEvent>(Action<TEvent> handler) where TEvent : IEvent
        {
            throw new NotImplementedException();
        }
    }
}
