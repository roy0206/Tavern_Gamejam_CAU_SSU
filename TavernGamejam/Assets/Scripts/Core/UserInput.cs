using System;
using System.Collections.Generic;
using UnityEngine;

public class UserInput : Singleton<UserInput>
{
    private Dictionary<KeyCode, Action> keyDownEvents;
    private Dictionary<KeyCode, Action> keyUpEvents;
    private Dictionary<KeyCode, Action> keyHoldEvents;

    public Action OnMouseDown;
    public Action OnMouseUp;
    public Action OnMouseHold;

    public Vector2 MoveDirection => new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).normalized;
    public Vector2 MoveDirectionRaw => new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;

    new private void Awake()
    {
        base.Awake();
        keyDownEvents = new();
        keyHoldEvents = new();
        keyUpEvents = new();
        print("USERINPUT");
    }

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

        print(keyDownEvents.Count);
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
        print(keyDownEvents.Count);
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