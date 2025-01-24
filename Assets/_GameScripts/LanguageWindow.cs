using UnityEngine;
using UnityEngine.UI;

public class LanguageWindow : MonoBehaviour
{
    [SerializeField] private GameObject[] _selectLanguageTexts;
    [SerializeField] private GameObject[] _nextBtns;
    [SerializeField] private GameObject[] _languages;

    [SerializeField] private GameObject _languageWindow;
    [SerializeField] private GameObject[] _tutorialWindows;

    private SettingsWindow _settingsWindow;

    [SerializeField] private Image[] _sellBtns;
    [SerializeField] private Image[] _buyBtns;
    [SerializeField] private Image[] _closeBtns;
    [SerializeField] private Sprite[] _sellLanguege;
    [SerializeField] private Sprite[] _buyLanguege;
    [SerializeField] private Sprite[] _closeLanguege;

    [SerializeField] private Text[] _yourCashText;
    [SerializeField] private string[] _yourCashLanguages;


    private void Start()
    {
        _settingsWindow = GetComponent<SettingsWindow>();
        int languageIndex = PlayerPrefs.GetInt("SelectedLanguage", 0);

        if (PlayerPrefs.GetString("FirstEnterGame", "") != "")
        {
            SelectNewLanguage(languageIndex);
        }

        foreach (var item in _closeBtns)
        {
            item.sprite = _closeLanguege[languageIndex];
        }
        foreach (var item in _sellBtns)
        {
            item.sprite = _sellLanguege[languageIndex];
        }
        foreach (var item in _buyBtns)
        {
            item.sprite = _buyLanguege[languageIndex];
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

        foreach (var item in _closeBtns)
        {
            item.sprite = _closeLanguege[index];
        }
        foreach (var item in _sellBtns)
        {
            item.sprite = _sellLanguege[index];
        }
        foreach (var item in _buyBtns)
        {
            item.sprite = _buyLanguege[index];
        }
        foreach (var item in _yourCashText)
        {
            item.text = _yourCashLanguages[index];
        }
    }

    public void NextBtn()
    {
        int languageIndex = PlayerPrefs.GetInt("SelectedLanguage", 0);
        _languageWindow.SetActive(false);
        if (PlayerPrefs.GetString("FirstEnterGame", "") == "")
            _tutorialWindows[languageIndex].SetActive(true);
        else
        {
            _settingsWindow.OpenSettingsWindow();
        }
    }
}