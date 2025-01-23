using System.Collections;
using UnityEngine;

public class GameEnter : MonoBehaviour
{
    [SerializeField] private GameObject _loadingWindow;
    [SerializeField] private GameObject _languageWindow;
    [SerializeField] private GameObject _closeBtn;
    private void Start()
    {
        StartCoroutine(GoToNextWindow());
    }

    private IEnumerator GoToNextWindow()
    {
        string FirstEnter = PlayerPrefs.GetString("FirstEnterGame", "");
        yield return new WaitForSeconds(2.5f);
        if (FirstEnter == "")
        {
            _closeBtn.SetActive(false);
            _loadingWindow.SetActive(false);
            _languageWindow.SetActive(true);
            
        }
        else
        {
            _loadingWindow.SetActive(false);
        }
    }
}