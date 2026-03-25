using System;
using System.Collections.Generic;
using UnityEngine;

public class UserInput : Singleton<UserInput>
{
    private Dictionary<KeyCode, Action> keyDownEvents = new();
    private Dictionary<KeyCode, Action> keyUpEvents = new();
    private Dictionary<KeyCode, Action> keyHoldEvents = new();

    public Action OnMouseDown;
    public Action OnMouseUp;
    public Action OnMouseHold;

    void Update()
    {
        HandleKeyboard();
        HandleMouse();
    }

    void HandleKeyboard()
    {
        foreach (var key in keyDownEvents)
        {
            if (Input.GetKeyDown(key.Key))
                key.Value?.Invoke();
        }

        foreach (var key in keyUpEvents)
        {
            if (Input.GetKeyUp(key.Key))
                key.Value?.Invoke();
        }

        foreach (var key in keyHoldEvents)
        {
            if (Input.GetKey(key.Key))
                key.Value?.Invoke();
        }
    }

    void HandleMouse()
    {
        if (Input.GetMouseButtonDown(0))
            OnMouseDown?.Invoke();

        if (Input.GetMouseButtonUp(0))
            OnMouseUp?.Invoke();

        if (Input.GetMouseButton(0))
            OnMouseHold?.Invoke();
    }
    public void BindKeyDown(KeyCode key, Action action)
    {
        if (!keyDownEvents.ContainsKey(key))
            keyDownEvents[key] = action;
        else
            keyDownEvents[key] += action;
    }

    public void BindKeyUp(KeyCode key, Action action)
    {
        if (!keyUpEvents.ContainsKey(key))
            keyUpEvents[key] = action;
        else
            keyUpEvents[key] += action;
    }

    public void BindKeyHold(KeyCode key, Action action)
    {
        if (!keyHoldEvents.ContainsKey(key))
            keyHoldEvents[key] = action;
        else
            keyHoldEvents[key] += action;
    }

    public void UnbindKeyDown(KeyCode key, Action action)
    {
        if (keyDownEvents.ContainsKey(key))
            keyDownEvents[key] -= action;
    }

    public void UnbindKeyUp(KeyCode key, Action action)
    {
        if (keyUpEvents.ContainsKey(key))
            keyUpEvents[key] -= action;
    }

    public void UnbindKeyHold(KeyCode key, Action action)
    {
        if (keyHoldEvents.ContainsKey(key))
            keyHoldEvents[key] -= action;
    }
}