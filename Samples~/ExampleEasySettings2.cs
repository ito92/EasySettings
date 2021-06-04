using DaBois.Settings;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

[EasySettings("Example Settings/Settings 2", "ExampleSettings 2", "Example Title 2")]
public class ExampleEasySettings2 : EasySettingsProvider<ExampleEasySettings2>
{
    [SerializeField]
    private string _exampleField2 = "Default Value 2";

    public string ExampleField2 { get => _exampleField2; }

    public override void RuntimeInit()
    {
        _instance = this;
    }
}

#if UNITY_EDITOR
static class ExampleSettings2Register
{
    [SettingsProvider]
    public static SettingsProvider CreateSettingsProvider()
    {
        return ExampleEasySettings2.Instance.GenerateProvider();
    }
}
#endif