using UnityEngine;

public class LanguageWindow : MonoBehaviour
{
    [SerializeField] private GameObject[] _selectLanguageTexts;
    [SerializeField] private GameObject[] _nextBtns;
    [SerializeField] private GameObject[] _languages;

    [SerializeField] private GameObject _languageWindow;
    [SerializeField] private GameObject[] _tutorialWindows;

    private void Start()
    {
        if (PlayerPrefs.GetString("FirstEnterGame", "") != "")
        {
            int languageIndex = PlayerPrefs.GetInt("SelectedLanguage", 0);
            SelectNewLanguage(languageIndex);
        }
    }

    public void SelectNewLanguage(int index)
    {
        foreach (var item in _selectLanguageTexts)
        {
            item.SetActive(false);
        }
        foreach (var item in _languages)
        {
            item.SetActive(false);
        }
        foreach (var item in _nextBtns)
        {
            item.SetActive(false);
        }
        _selectLanguageTexts[index].SetActive(true);
        _nextBtns[index].SetActive(true);
        _languages[index].SetActive(true);
        PlayerPrefs.SetInt("SelectedLanguage", index);
    }

    public void NextBtn()
    {
        int languageIndex = PlayerPrefs.GetInt("SelectedLanguage", 0);
        _languageWindow.SetActive(false);
        _tutorialWindows[languageIndex].SetActive(true);
    }
}