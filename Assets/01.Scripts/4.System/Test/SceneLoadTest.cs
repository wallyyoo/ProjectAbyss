using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using UnityEngine.SceneManagement;

public class SceneLoadTest : MonoBehaviour
{
  [Button("EventScene 이동")]
  private void LoadEventScene()
  { 
    SceneManager.LoadScene("EventScene");
  }
  [Button("BattleScene 이동")]
  private void LoadBattleScene()
  { 
    SceneManager.LoadScene("BattleScene");
  }
  
  
}
