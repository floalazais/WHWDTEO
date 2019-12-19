using System.Collections.Generic;
using UnityEngine;

public class W_Event { }

#region Gamepad Events

public class OnVibrate : W_Event
{
    public bool right;
    public bool both;
    public OnVibrate(bool pRight, bool pBoth = false)
    {
        right = pRight;
        both = pBoth;
    }
}

public class OnStopVibrate : W_Event { }
#endregion

#region PS4 Events

public class OnLightSwitchColor : W_Event { }
public class OnLightFlash : W_Event { }
#endregion

public class OnAnimatorEvent : W_Event
{
    public Enums.E_CHARACTER character;
    public string animatorParameter;

    public OnAnimatorEvent(Enums.E_CHARACTER pCharacter, string pAnimatorParameter)
    {
        character = pCharacter;
        animatorParameter = pAnimatorParameter;
    }
}

public class OnFacialEvent : W_Event
{
    public Enums.E_CHARACTER character;
    public string facialParameter;

    public OnFacialEvent(Enums.E_CHARACTER pCharacter, string pFacialParameter)
    {
        character = pCharacter;
        facialParameter = pFacialParameter;
    }
}

public class EventsManager
{
    static EventsManager instance = null;
    public static EventsManager Instance {
        get {
            if (instance == null)
            {
                instance = new EventsManager();
            }

            return instance;
        }
    }

    public delegate void EventDelegate<T>(T e) where T : W_Event;
    private delegate void EventDelegate(W_Event e);

    private Dictionary<System.Type, EventDelegate> delegates = new Dictionary<System.Type, EventDelegate>();
    private Dictionary<System.Delegate, EventDelegate> delegateLookup = new Dictionary<System.Delegate, EventDelegate>();

    //Events.Instance.AddListener<W_Event>(W_Event);
    public void AddListener<T>(EventDelegate<T> del) where T : W_Event
    {
        if (delegateLookup.ContainsKey(del))
            return;

        // Create a new non-generic delegate which calls our generic one.
        // This is the delegate we actually invoke.
        // search "Expressions lambda (Guide de programmation C#)" for details
        EventDelegate internalDelegate = (e) => del((T)e);
        delegateLookup[del] = internalDelegate;

        EventDelegate tempDel;
        if (delegates.TryGetValue(typeof(T), out tempDel))
        {
            delegates[typeof(T)] = tempDel += internalDelegate;
        }
        else
        {
            delegates[typeof(T)] = internalDelegate;
        }
    }

    public void RemoveListener<T>(EventDelegate<T> del) where T : W_Event
    {
        EventDelegate internalDelegate;
        if (delegateLookup.TryGetValue(del, out internalDelegate))
        {
            EventDelegate tempDel;
            if (delegates.TryGetValue(typeof(T), out tempDel))
            {
                tempDel -= internalDelegate;
                if (tempDel == null)
                {
                    delegates.Remove(typeof(T));
                }
                else
                {
                    delegates[typeof(T)] = tempDel;
                }
            }

            delegateLookup.Remove(del);
        }
    }

    //EventsManager.Instance.Raise(new W_Event(param = null));
    public void Raise(W_Event e)
    {
        EventDelegate del;
        if (delegates.TryGetValue(e.GetType(), out del))
        {
            del.Invoke(e);
        }
    }
}

