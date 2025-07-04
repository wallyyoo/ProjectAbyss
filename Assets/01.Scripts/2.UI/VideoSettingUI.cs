
using System;
using UnityEngine;
using UnityEngine.UI;

public class VideoSettingUI : MonoBehaviour
{
  [Header("전체화면 토글")]
  public Toggle fullscreenToggle;
  
  [Header("해상도별 토글")]
  public Toggle[] resolutionToggle;

  private readonly Vector2Int[] resolutions = new Vector2Int[]
  {
    new Vector2Int(2560, 1440),
    new Vector2Int(1920, 1080),
    new Vector2Int(1600, 900),
    new Vector2Int(1200, 720)

  };

  private int currentResolution = 1;
  private bool isFullscreen = true;

  private void Start()
  {
    LoadSettings();
    ApplyUI();
  }

  public void OnResolutionToggleChanged(int index)
  {
    if (resolutionToggle[index].isOn)
    {
      currentResolution= index;
    }
  }

  public void OnFullscreenToggleChanged()
  {
    isFullscreen = fullscreenToggle.isOn;
  }

  public void ApplyUI()
  {
    fullscreenToggle.isOn = isFullscreen;

    for (int i = 0; i < resolutionToggle.Length; i++)
    {
      resolutionToggle[i].isOn = (i == currentResolution);
    }
  }

  public void SaveSettings()
  {
    PlayerPrefs.SetInt("ResolutionIndex", currentResolution);
    PlayerPrefs.SetInt("Fullscreen", isFullscreen? 1 : 0);
    PlayerPrefs.Save();
    
    ApplyUI();
    ApplySettings();
  }

  public void ResetSettings()
  {
    currentResolution = 1;
    isFullscreen = true;
    ApplyUI();
    ApplySettings();
  }

  private void LoadSettings()
  {
    currentResolution = PlayerPrefs.GetInt("ResolutionIndex",1);
    isFullscreen = PlayerPrefs.GetInt("Fullscreen", 1) == 1;
  }

  private void ApplySettings()
  {
    var selectedRes = resolutions[currentResolution];
    Screen.SetResolution(selectedRes.x,selectedRes.y, isFullscreen);
    Debug.Log($"{selectedRes.x},{selectedRes.y},{isFullscreen}");
  }
  
}
