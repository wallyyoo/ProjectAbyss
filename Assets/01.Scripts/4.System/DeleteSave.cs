using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
    
public class DeleteSave:MonoBehaviour
{
    [Button("DeleteMapSave")]
    private void DeleteMapSave()
    {
        SaveLoadManager.DeleteSave();
    }
        
    
        
}
