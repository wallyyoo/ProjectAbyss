using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SoundSettingUI : MonoBehaviour
{
    [Header("슬라이더")]
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Slider sfxSlider;

    [Header("슬라이더 값 텍스트")]
    [SerializeField] private TMP_Text masterText;
    [SerializeField] private TMP_Text bgmText;
    [SerializeField] private TMP_Text sfxText;
    
   private void Start()
    {
        LoadSetting();
        
        masterSlider.onValueChanged.AddListener((v) => { masterText.text = Mathf.RoundToInt(v * 100f) + "%";});
        bgmSlider.onValueChanged.AddListener((v)=> {bgmText.text = Mathf.RoundToInt(v * 100f) + "%";});
        sfxSlider.onValueChanged.AddListener((v)=> {sfxText.text = Mathf.RoundToInt(v * 100f) + "%";});
        
    }

    public void SaveSetting()
    {
        PlayerPrefs.SetFloat("Volume_Master",masterSlider.value);
        PlayerPrefs.SetFloat("Volume_Bgm",bgmSlider.value);
        PlayerPrefs.SetFloat("Volume_Sfx",sfxSlider.value);
        PlayerPrefs.Save();
        
    }

    public void ResetSetting()
    {
        masterSlider.value = 0.5f;
        bgmSlider.value = 0.5f;
        sfxSlider.value = 0.5f;
    }

    private void LoadSetting()
    {
        float master = PlayerPrefs.GetFloat("Volume_Master",0.5f);
        float bgm = PlayerPrefs.GetFloat("Volume_Bgm",0.5f);
        float sfx = PlayerPrefs.GetFloat("Volume_Sfx",0.5f);
        
        masterSlider.value = master;
        bgmSlider.value = bgm;
        sfxSlider.value = sfx;
        
        masterText.text = Mathf.RoundToInt(master * 100f) + "%";
        bgmText.text = Mathf.RoundToInt(bgm * 100f) + "%";
        sfxText.text = Mathf.RoundToInt(sfx * 100f) + "%";
    }
    
}
