using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SettingsWindow : MonoBehaviour
{
    [SerializeField] private GameObject _languageWindow;
    [SerializeField] private GameObject _closeLanguageBtn;
    private string email = "alexdrugev@me.com";
    [SerializeField] private GameObject[] _settingsWindows;

    public void OpenLanuageWindow()
    {
        _closeLanguageBtn.SetActive(true);
        _languageWindow.SetActive(true);
    }

    public void CloseLanguageWindow()
    {
        _languageWindow.SetActive(false);
    }

    public void OpenPrivacyPolicy()
    {
        Application.OpenURL("http://google.com/");
    }

    public void WriteUsButton()
    {
        string subject = Uri.EscapeDataString("Help Request");
        string body = Uri.EscapeDataString("Hello,\n\nI need help with...");
        string mailto = $"mailto:{email}?subject={subject}&body={body}";
        Application.OpenURL(mailto);
    }

    public void OpenSettingsWindow()
    {
        int languageIndex = PlayerPrefs.GetInt("SelectedLanguage", 0);
        _settingsWindows[languageIndex].SetActive(true);
    }

    public void CloseSettingsWindow()
    {
        int languageIndex = PlayerPrefs.GetInt("SelectedLanguage", 0);
        _settingsWindows[languageIndex].SetActive(false);
    }
}
