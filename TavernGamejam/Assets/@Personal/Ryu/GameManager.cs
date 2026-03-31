using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;


public class GameManager : MonoThing
{
    new public void OnSceneLoadStart(string sceneName)
    {
        base.OnSceneLoadStart(sceneName);

    }

    new public void OnSceneLoadComplete(string sceneName)
    {
        base.OnSceneLoadComplete(sceneName);
        DataManager.Instance.LoadGame();
    }
}

