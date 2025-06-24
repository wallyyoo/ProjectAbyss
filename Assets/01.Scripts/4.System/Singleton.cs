using System;
using UnityEngine;
using TMPro;

public abstract class Singleton<T> : MonoBehaviour where T : Component
{
    private static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance == null)
                _instance = FindObjectOfType<T>();

            if (_instance == null)
                _instance = new GameObject() { name = typeof(T).Name }.AddComponent<T>();

            return _instance;
        }
    }

    protected virtual void Awake()
    {
        if (_instance != null)
            Destroy(gameObject);
    }
}