using UnityEngine;

public class TutorialWindow : MonoBehaviour
{
    [SerializeField] private GameObject[] _tutorialWindows;
    [SerializeField] private GameObject[] _articles;

    public void OpenArticles()
    {
        int languageIndex = PlayerPrefs.GetInt("SelectedLanguage", 0);
        _articles[languageIndex].SetActive(true);
        PlayerPrefs.SetString("FirstEnterGame", "Was");
    }

    public void CloseTutorial()
    {
        int languageIndex = PlayerPrefs.GetInt("SelectedLanguage", 0);
        _tutorialWindows[languageIndex].SetActive(false);
        PlayerPrefs.SetString("FirstEnterGame", "Was");
    }
}