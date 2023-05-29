using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager<T> : MonoBehaviour where T : MonoBehaviour
{
    [SerializeField]
    private static T instance;
    private static object instLock = new object();
    public static T Inst
    {
        get
        {
            lock (instLock)
            {
                if (instance == null)
                {
                    //search existing instance
                    instance = (T)FindObjectOfType(typeof(T));

                    if (instance == null)
                    {
                        var singletonObject = new GameObject();
                        instance = singletonObject.AddComponent<T>();
                        singletonObject.name = typeof(T).ToString();
                        DontDestroyOnLoad(instance.gameObject);
                    }
                }
            }

            return instance;
        }
    }

    protected virtual void Awake()
    {

    }
    protected virtual void Start() { }
    protected virtual void OnDestroy()
    {

    }
    protected virtual void OnApplicationQuit()
    {
        instance = null;
    }


}
