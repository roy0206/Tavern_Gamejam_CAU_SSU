using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine<T> : Module where T : MonoThing
{
    private Dictionary<string, State<T>> states;
    private string currentState;
    private string globalState;

    public StateMachine(T thing) : base(thing){}

    public override void Init(params object[] objects) // Dictionary<string,state<T>> states,  string defaultState 
    { 
        currentState = null;
        globalState = null;

        states = objects[0] as Dictionary<string, State<T>>;

        ChangeState((string)objects[1]);
        base.Init();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        if (globalState != null) states[globalState].Execute((T)Thing);
        if (currentState != null) states[currentState].Execute((T)Thing);
    }

    public void ChangeState(string state)
    {
        if (!states.ContainsKey(state))
        {
            Debug.LogError($"There is no such a state named {state}");
            return;
        }

        if (currentState != null)
        {
            states[currentState].Exit((T)Thing);
        }

        currentState = state;
        states[currentState].Enter((T)Thing);
    }

    public void SetGlobalState(string newState)
    {
        globalState = newState;
    }
    public System.Type GetCurrentState() => states[currentState].GetType();
}
