using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
    
public class DeleteSave:MonoBehaviour
{
    [SerializeField] private Canvas MapUi;
    
    [Button("DeleteMapSave")]
    private void DeleteMapSave()
    {
        SaveLoadManager.DeleteSave();
    }

    [Button("TurnOnMap")]
    private void TurnOnMap()
    {
        MapUi.enabled = true;
    }
    [Button("TurnOffMap")]
    private void TurnOffMap()
    {
        MapUi.enabled = false;
    }
    
        
    
        
}
