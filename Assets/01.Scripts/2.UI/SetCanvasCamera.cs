using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetCanvasCamera : MonoBehaviour
{
    void Start()
    {
        Canvas canvas = GetComponent<Canvas>();

        if (canvas != null)
        {
            if (Camera.main != null) canvas.worldCamera = Camera.main;

            else Debug.Log("카메라가 없거나 못 찾았습니다.");
        }
    }
}
