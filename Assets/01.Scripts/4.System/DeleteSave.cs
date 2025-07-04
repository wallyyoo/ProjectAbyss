using System.Collections;
using UnityEngine;
using NaughtyAttributes;
    
public class DeleteSave:MonoBehaviour
{
    [SerializeField] private GameObject MapNodeUi;
    
    [Button("DeleteMapSave")]
    private void DeleteMapSave()
    {
        SaveLoadManager.DeleteSave();
    }

    [Button("TurnOnMap")]
    private void TurnOnMap()
    {
        MapNodeUi.SetActive(true);
    }
    [Button("TurnOffMap")]
    private void TurnOffMap()
    {
       MapNodeUi.SetActive(false);
    }

    //[Button("RunCountup")]
    
    
        
}
