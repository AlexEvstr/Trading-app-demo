using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class TradingManager : MonoBehaviour
{
    [Header("References")]
    public GameObject sellWindow; // Окно Sell
    public GameObject buyWindow; // Окно Buy
    public GameObject resultWindow; // Окно результата сделки
    public Text resultText; // Текст результата сделки
    public Text cashText; // Текущий баланс пользователя

    // Поля для окна Buy
    public Text buyConfirmationCashText; // Баланс в окне подтверждения Buy
    public Text buyConfirmationPriceText; // Цена в реальном времени в окне подтверждения Buy
    public Button[] buyStakeButtons; // Кнопки со ставками для Buy
    public Button[] buyMultiplierButtons; // Кнопки с множителями для Buy

    // Поля для окна Sell
    public Text sellConfirmationCashText; // Баланс в окне подтверждения Sell
    public Text sellConfirmationPriceText; // Цена в реальном времени в окне подтверждения Sell
    public Button[] sellStakeButtons; // Кнопки со ставками для Sell
    public Button[] sellMultiplierButtons; // Кнопки с множителями для Sell

    [Header("Settings")]
    private float multiplier = 0f; // Выбранный множитель (1x, 5x и т.д.)
    private float selectedAmount = 0f; // Выбранная сумма
    private float cash; // Текущий баланс

    private bool isBuy; // Покупка (true) или продажа (false)
    private float entryPrice; // Цена при входе
    private float closePrice; // Цена при выходе

    public Button sellButton; // Кнопка подтверждения Sell
    public Button buyButton;  // Кнопка подтверждения Buy

    public Button mainSellButton; // Кнопка Sell на главном экране
    public Button mainBuyButton;  // Кнопка Buy на главном экране
    public Button closeDealButton; // Кнопка закрытия сделки (вместо главных Sell/Buy)

    public Color selectedColor = Color.yellow; // Цвет выбранной кнопки
    public Color defaultColor = Color.white;  // Цвет кнопок по умолчанию
    [SerializeField] private Text _totalCashBalance;
    [SerializeField] private SingleCandle _singleCandle;

    void Start()
    {
        cash = PlayerPrefs.GetFloat("CashTotal", 10000);
        _totalCashBalance.text = cash.ToString();
        UpdateCashDisplay();
        CloseAllWindows();
        InitializeButtons();
    }

    void Update()
    {
        // Обновляем цену в реальном времени в соответствующем окне
        if (buyWindow.activeSelf)
        {
            buyConfirmationPriceText.text = $"{GetCurrentPrice():F2}";
        }
        if (sellWindow.activeSelf)
        {
            sellConfirmationPriceText.text = $"{GetCurrentPrice():F2}";
        }
    }

    public void OpenSellWindow()
    {
        CloseAllWindows();
        sellWindow.SetActive(true);
        isBuy = false;
        UpdateConfirmationWindow();
    }

    public void OpenBuyWindow()
    {
        CloseAllWindows();
        buyWindow.SetActive(true);
        isBuy = true;
        UpdateConfirmationWindow();
    }

    public void SetAmount(float amount)
    {
        selectedAmount = amount;

        // Обновляем цвета кнопок ставок для Buy
        foreach (var button in buyStakeButtons)
        {
            Image buttonImage = button.GetComponent<Image>();
            buttonImage.color = button.name == $"Stake_{amount}" ? selectedColor : defaultColor;
        }

        // Обновляем цвета кнопок ставок для Sell
        foreach (var button in sellStakeButtons)
        {
            Image buttonImage = button.GetComponent<Image>();
            buttonImage.color = button.name == $"Stake_{amount}" ? selectedColor : defaultColor;
        }

        UpdateButtonsState(); // Проверяем доступность кнопок Sell и Buy
    }



    public void SetMultiplier(float value)
    {
        multiplier = value;

        // Обновляем цвета кнопок множителей для Buy
        foreach (var button in buyMultiplierButtons)
        {
            Image buttonImage = button.GetComponent<Image>();
            buttonImage.color = button.name == $"Multiplier_{value}x" ? selectedColor : defaultColor;
        }

        // Обновляем цвета кнопок множителей для Sell
        foreach (var button in sellMultiplierButtons)
        {
            Image buttonImage = button.GetComponent<Image>();
            buttonImage.color = button.name == $"Multiplier_{value}x" ? selectedColor : defaultColor;
        }

        UpdateButtonsState(); // Проверяем доступность кнопок Sell и Buy
    }



    public void CloseDealManually()
    {
        closePrice = GetCurrentPrice(); // Фиксируем текущую цену

        // Проверяем результат сделки
        bool isWin = (isBuy && closePrice > entryPrice) || (!isBuy && closePrice < entryPrice);
        float profit = selectedAmount * multiplier;
        cash = PlayerPrefs.GetFloat("CashTotal", 10000);
        int languageIndex = PlayerPrefs.GetInt("SelectedLanguage", 0);
        if (isWin)
        {
            cash += selectedAmount + profit;
            if (languageIndex == 0)
                ShowResultWindow("You win", profit);
            else if (languageIndex == 1)
                ShowResultWindow("Du gewinnst", profit);
            else if (languageIndex == 2)
                ShowResultWindow("Você ganha", profit);
        }
        else
        {
            cash -= selectedAmount + profit;
            if (languageIndex == 0)
                ShowResultWindow("You lose", -profit);
            else if (languageIndex == 1)
                ShowResultWindow("Du verlierst", -profit);
            else if (languageIndex == 1)
                ShowResultWindow("Você perde", -profit);
        }
        PlayerPrefs.SetFloat("CashTotal", cash);
        _totalCashBalance.text = cash.ToString();

        UpdateCashDisplay();

        // Возвращаем главные кнопки и отключаем кнопку закрытия сделки
        mainSellButton.gameObject.SetActive(true);
        mainBuyButton.gameObject.SetActive(true);
        closeDealButton.gameObject.SetActive(false);
    }


    private void UpdateButtonsState()
    {
        // Проверяем, выбраны ли ставка и множитель
        bool isValid = selectedAmount > 0 && multiplier > 0;
        // Включаем/отключаем кнопки Sell и Buy в окне подтверждения
        sellButton.interactable = isValid;
        buyButton.interactable = isValid;
    }


    public void ConfirmTrade()
    {
        entryPrice = GetCurrentPrice(); // Фиксируем текущую цену
        UpdateCashDisplay();            // Обновляем баланс

        // Закрываем окно подтверждения
        CloseAllWindows();

        // Скрываем главные кнопки и активируем кнопку закрытия сделки
        mainSellButton.gameObject.SetActive(false);
        mainBuyButton.gameObject.SetActive(false);
        closeDealButton.gameObject.SetActive(true);
    }

    public void CloseConfirmationWindow()
    {
        // Закрываем окно подтверждения без изменений
        CloseAllWindows();
    }

    private void ShowResultWindow(string result, float amountChange)
    {
        resultWindow.SetActive(true);
        resultText.text = $"{result}\n{(amountChange > 0 ? "+" : "")}{amountChange:F2}";
    }

    private void CloseAllWindows()
    {
        sellWindow.SetActive(false);
        buyWindow.SetActive(false);
        resultWindow.SetActive(false);
    }

    private void UpdateCashDisplay()
    {
        cashText.text = cash.ToString("F2");
    }

    private void UpdateConfirmationWindow()
    {
        if (isBuy)
        {
            buyConfirmationCashText.text = $"{cash:F2}";
            buyConfirmationPriceText.text = $"{GetCurrentPrice():F2}";
        }
        else
        {
            sellConfirmationCashText.text = $"{cash:F2}";
            sellConfirmationPriceText.text = $"{GetCurrentPrice():F2}";
        }

        UpdateButtonsState();
    }

    private float GetCurrentPrice()
    {
        // Получить текущую цену из CandleChart
        return _singleCandle.currentPrice;
    }

    private void InitializeButtons()
    {
        // Устанавливаем кнопки для Buy
        InitializeStakeButtons(buyStakeButtons);
        InitializeMultiplierButtons(buyMultiplierButtons);

        // Устанавливаем кнопки для Sell
        InitializeStakeButtons(sellStakeButtons);
        InitializeMultiplierButtons(sellMultiplierButtons);
    }

    private void InitializeStakeButtons(Button[] stakeButtons)
    {
        float[] stakeValues = { 10f, 20f, 50f, 100f };
        for (int i = 0; i < stakeButtons.Length; i++)
        {
            float stakeValue = stakeValues[i];
            stakeButtons[i].onClick.AddListener(() => SetAmount(stakeValue));
        }
    }

    private void InitializeMultiplierButtons(Button[] multiplierButtons)
    {
        float[] multipliers = { 1f, 5f, 10f, 50f };
        for (int i = 0; i < multiplierButtons.Length; i++)
        {
            float multiplierValue = multipliers[i];
            multiplierButtons[i].onClick.AddListener(() => SetMultiplier(multiplierValue));
        }
    }
}
