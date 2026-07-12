namespace GanEcosystem.UI.Core
{
    public interface IEvent
    {
        
    }
    public interface IUIEventBus
    {
        void Publish<TEvent>(TEvent uiEvent) where TEvent : IEvent;
        void Subscribe<TEvent>(System.Action<TEvent> handler) where TEvent : IEvent;
        void Unsubscribe<TEvent>(System.Action<TEvent> handler) where TEvent : IEvent;
    }
}