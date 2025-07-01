using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using Newtonsoft.Json.Linq;

public class UpgradeJson : MonoBehaviour
{
    string path = Path.Combine(Application.streamingAssetsPath, "");

    void Start()
    {

    }

    void Show1()
    {
        string json = File.ReadAllText(path);

        JToken root = JToken.Parse(json);
        
    }
}
