using DaBois.Settings;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[EasySettings("Example Settings/Settings", "ExampleSettings", "Example Title", new string[] { "TestTag" })]
public class ExampleEasySettings : EasySettingsProvider<ExampleEasySettings>
{
    [SerializeField]
    private string _exampleField = "Default Value";

    public string ExampleField { get => _exampleField; }

    public override void RuntimeInit()
    {
        _instance = this;
    }
}

#if UNITY_EDITOR
static class ExampleSettingsRegister
{
    [SettingsProvider]
    public static SettingsProvider CreateSettingsProvider()
    {
        return ExampleEasySettings.Instance.GenerateProvider();
    }
}
#endif