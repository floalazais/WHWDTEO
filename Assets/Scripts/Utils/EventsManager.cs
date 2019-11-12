using System.Collections.Generic;
using UnityEngine;

public class Event { }

#region Inputs Events

public class OnCrossButton : Event { }
public class OnTriangleButton : Event { }
public class OnSquareButton : Event { }
public class OnRoundButton : Event { }

public class OnRightStickMove : Event
{
    public Vector3 move;
    public OnRightStickMove(Vector3 pVec)
    {
        move = pVec;
    }
}

public class OnLeftStickMove : Event
{
    public Vector3 move;
    public OnLeftStickMove(Vector3 pVec)
    {
        move = pVec;
    }
}

public class OnTouch : Event { }
public class OnReleaseTouch : Event { }

#endregion

#region Gamepad Events

public class ONR1Button : Event { }
public class ONR2Button : Event { }
public class ONL1Button : Event { }
public class ONL2Button : Event { }

public class OnMenuButton : Event { }
public class OnShareButton : Event { }
public class OnOptionsButton : Event { }

public class OnLeftStickButton : Event { }
public class OnRightStickButton : Event { }

public class OnDPadRightButton : Event { }
public class OnDPadLeftButton : Event { }
public class OnDPadBottomButton : Event { }
public class OnDPadUpButton : Event { }

public class OnVibrate : Event
{
    public bool right;
    public bool both;
    public OnVibrate(bool pRight, bool pBoth = false)
    {
        right = pRight;
        both = pBoth;
    }
}

public class OnStopVibrate : Event { }
#endregion

#region PS4 Events

public class OnLightSwitchColor : Event { }
public class OnLightFlash : Event { }
#endregion

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

    public delegate void EventDelegate<T>(T e) where T : Event;
    private delegate void EventDelegate(Event e);

    private Dictionary<System.Type, EventDelegate> delegates = new Dictionary<System.Type, EventDelegate>();
    private Dictionary<System.Delegate, EventDelegate> delegateLookup = new Dictionary<System.Delegate, EventDelegate>();

    //Events.Instance.AddListener<Event>(Event);
    public void AddListener<T>(EventDelegate<T> del) where T : Event
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

    public void RemoveListener<T>(EventDelegate<T> del) where T : Event
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

    //EventsManager.Instance.Raise(new Event(param = null));
    public void Raise(Event e)
    {
        EventDelegate del;
        if (delegates.TryGetValue(e.GetType(), out del))
        {
            del.Invoke(e);
        }
    }
}

