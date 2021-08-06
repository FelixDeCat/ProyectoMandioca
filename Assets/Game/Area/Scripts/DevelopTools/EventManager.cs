using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace DevelopTools
{
    public class EventManager
    {
        public delegate void EventReceiverParam(params object[] parameterContainer);

        private Dictionary<string, EventReceiverParam> eventsParam= new Dictionary<string, EventReceiverParam>();

        private Dictionary<string, Action> events= new Dictionary<string, Action>();


        public void SubscribeToEvent(string eventType, EventReceiverParam listener)
        {
            if (eventsParam == null)
                eventsParam = new Dictionary<string, EventReceiverParam>();

            if (!eventsParam.ContainsKey(eventType))
                eventsParam.Add(eventType, null);

            eventsParam[eventType] += listener;
        }

        public void SubscribeToEvent(string eventType, Action listener)
        {
            if (events == null)
                events = new Dictionary<string, Action>();

            if (!events.ContainsKey(eventType))
                events.Add(eventType, null);

            events[eventType] += listener;
        }

        public void UnsubscribeToEvent(string eventType, EventReceiverParam listener)
        {
            if (eventsParam != null)
            {
                if (eventsParam.ContainsKey(eventType))
                    eventsParam[eventType] -= listener;
            }
        }

        public void UnsubscribeToEvent(string eventType, Action listener)
        {
            if (events != null)
            {
                if (events.ContainsKey(eventType))
                    events[eventType] -= listener;
            }
        }

        public void TriggerEvent(string eventType)
        {
            TriggerEvent(eventType, null);
        }

        public void TriggerEvent(string eventType, params object[] parametersWrapper)
        {
            if (eventsParam.Count == 0 && events.Count==0)
            {
                return;
            }

            if (eventsParam.ContainsKey(eventType))
            {
                if (eventsParam[eventType] != null)
                    eventsParam[eventType]?.Invoke(parametersWrapper);
            }

            if (events.ContainsKey(eventType))
            {
                if (events[eventType] != null)
                    events[eventType]?.Invoke();
            }
        }
    }
}
