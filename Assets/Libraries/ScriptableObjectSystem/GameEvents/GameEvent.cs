using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameEvent", menuName = "ScriptableObjectSystem/GameEvent")]
public class GameEvent : ScriptableObject
{
#if UNITY_EDITOR
    [TextArea(1, 10)]
    public string _eventExplanation;
    public bool _logWhenRaise;
    public List<string> _subcribers;
#endif

    public delegate void EventActionDel(params object[] eventParam);
    private event EventActionDel _eventAction;

    public void Subcribe(EventActionDel action)
    {
        _eventAction += action;
#if UNITY_EDITOR
        var st = new System.Diagnostics.StackTrace();
        var stackFrame = st.GetFrame(1);
        _subcribers.Add(stackFrame.GetMethod().DeclaringType.Name);
#endif
    }

    public void Unsubcribe(EventActionDel action)
    {
        _eventAction -= action;
#if UNITY_EDITOR
        var st = new System.Diagnostics.StackTrace();
        var stackFrame = st.GetFrame(1);
        _subcribers.Remove(stackFrame.GetMethod().DeclaringType.Name);
#endif
    }

    public void Raise(params object[] eventParam)
    {
        _eventAction?.Invoke(eventParam);

#if UNITY_EDITOR
        if (_logWhenRaise)
        {
            Debug.Log($"Event [{name}] Raised");
        }
#endif
    }

    public void Raise()
    {
        _eventAction?.Invoke();
#if UNITY_EDITOR
        if (_logWhenRaise)
        {
            Debug.Log($"Event [{name}] Raised");
        }
#endif
    }

#if UNITY_EDITOR
    private void OnDisable()
    {
        _subcribers.Clear();
    }
#endif
}
