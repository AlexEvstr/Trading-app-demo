using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArticlesWindow : MonoBehaviour
{
    [SerializeField] private GameObject[] _articlesTexts;

    public void OpenArticleText(int articleIndex)
    {
        int languageIndex = PlayerPrefs.GetInt("SelectedLanguage", 0);
        _articlesTexts[articleIndex].transform.GetChild(languageIndex).gameObject.SetActive(true);
    }

    public void CloseArticleText(int articleIndex)
    {
        int languageIndex = PlayerPrefs.GetInt("SelectedLanguage", 0);
        _articlesTexts[articleIndex].transform.GetChild(languageIndex).gameObject.SetActive(false);
    }
}