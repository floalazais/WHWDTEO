using System.Collections.Generic;
using UnityEngine;

public class W_Event { }

#region Inputs Events

public class OnCrossButton : W_Event { }
public class OnTriangleButton : W_Event { }
public class OnSquareButton : W_Event { }
public class OnRoundButton : W_Event { }

public class OnRightStickMove : W_Event
{
    public Vector3 move;
    public OnRightStickMove(Vector3 pVec)
    {
        move = pVec;
    }
}

public class OnLeftStickMove : W_Event
{
    public Vector3 move;
    public OnLeftStickMove(Vector3 pVec)
    {
        move = pVec;
    }
}

public class OnTouch : W_Event { }
public class OnReleaseTouch : W_Event { }

#endregion

#region Gamepad Events

public class ONR1Button : W_Event { }
public class ONR2ButtonDown : W_Event { }
public class ONR2ButtonUp : W_Event { }
public class ONL1Button : W_Event { }
public class ONL2Button : W_Event { }

public class OnMenuButton : W_Event { }
public class OnShareButton : W_Event { }
public class OnOptionsButton : W_Event { }

public class OnLeftStickButton : W_Event { }
public class OnRightStickButton : W_Event { }

public class OnDPadRightButton : W_Event { }
public class OnDPadLeftButton : W_Event { }
public class OnDPadBottomButton : W_Event { }
public class OnDPadUpButton : W_Event { }

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

