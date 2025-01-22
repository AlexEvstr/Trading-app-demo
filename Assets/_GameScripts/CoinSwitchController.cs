using UnityEngine;

public class CoinSwitchController : MonoBehaviour
{
    [SerializeField] private GameObject[] _coinsCanvases;
    [SerializeField] private GameObject[] _valueButtons;

    private void Start()
    {
        OpenNewCoin(0);
    }

    public void OpenNewCoin(int index)
    {
        foreach (var item in _coinsCanvases)
        {
            item.SetActive(false);
        }
        _coinsCanvases[index].SetActive(true);
        foreach (var item in _valueButtons)
        {
            item.transform.GetChild(0).gameObject.SetActive(false);
        }
        _valueButtons[index].transform.GetChild(0).gameObject.SetActive(true);
    }
}