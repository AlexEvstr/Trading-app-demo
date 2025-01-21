using UnityEngine;
using UnityEngine.UI;

public class SingleCandle : MonoBehaviour
{
    [Header("Settings")]
    public float candleWidth = 40f; // Ширина тела свечи
    public float shadowWidth = 15f; // Ширина тени свечи
    public float priceChangeSpeed = 5f; // Скорость изменения цены (на 5 за секунду)

    [Header("References")]
    public RectTransform grid; // Контейнер для графика
    public Text priceText; // Текст текущей цены

    private RectTransform bodyRect; // Тело свечи
    private RectTransform shadowRect; // Тень свечи
    private float openPrice = 200f; // Цена открытия (начальная цена 200)
    private float currentPrice = 200f; // Текущая цена
    private float targetPrice; // Целевая цена
    private float highPrice = 200f; // Максимальная цена
    private float lowPrice = 200f; // Минимальная цена

    private float minPrice = 180f; // Минимальная возможная цена
    private float maxPrice = 220f; // Максимальная возможная цена
    private float thresholdPrice = 200f; // Граница между зеленой и красной свечой

    void Start()
    {
        InitializeCandle();
    }

    void Update()
    {
        // Двигаемся к целевой цене
        MoveTowardsTargetPrice();

        // Обновляем отображение свечи
        UpdateCandleDisplay();
    }

    void InitializeCandle()
    {
        // Устанавливаем начальные значения
        openPrice = currentPrice; // Цена открытия равна текущей цене
        highPrice = openPrice;
        lowPrice = openPrice;

        // Назначаем начальную целевую цену
        AssignNewTargetPrice();

        // Создаем свечу, если еще не была создана
        if (bodyRect == null || shadowRect == null)
        {
            CreateCandle();
        }
    }

    void MoveTowardsTargetPrice()
    {
        // Двигаемся к целевой цене с заданной скоростью
        currentPrice = Mathf.MoveTowards(currentPrice, targetPrice, priceChangeSpeed * Time.deltaTime);

        // Обновляем максимум и минимум
        if (currentPrice > highPrice) highPrice = currentPrice;
        if (currentPrice < lowPrice) lowPrice = currentPrice;

        // Если достигли целевой цены, назначаем новую цель
        if (Mathf.Approximately(currentPrice, targetPrice))
        {
            AssignNewTargetPrice();
        }

        // Обновляем текст текущей цены
        priceText.text = currentPrice.ToString("F2");
    }

    void AssignNewTargetPrice()
    {
        // Генерируем новую целевую цену, которая отличается на 5, 10, или другое значение
        float priceChange = Random.Range(5f, 11f); // Изменение на 5-10
        targetPrice = Random.value > 0.5f ? currentPrice + priceChange : currentPrice - priceChange;

        // Ограничиваем целевую цену в пределах допустимого диапазона
        targetPrice = Mathf.Clamp(targetPrice, minPrice, maxPrice);
    }

    void CreateCandle()
    {
        // Тень свечи
        GameObject shadow = new GameObject("Shadow");
        shadow.transform.SetParent(grid, false);
        shadowRect = shadow.AddComponent<RectTransform>();
        Image shadowImage = shadow.AddComponent<Image>();
        shadowImage.color = Color.green;

        // Тело свечи
        GameObject body = new GameObject("Body");
        body.transform.SetParent(grid, false);
        bodyRect = body.AddComponent<RectTransform>();
        Image bodyImage = body.AddComponent<Image>();
        bodyImage.color = Color.green;

        // Настройка размеров
        shadowRect.sizeDelta = new Vector2(shadowWidth, 0);
        bodyRect.sizeDelta = new Vector2(candleWidth, 0);

        shadowRect.anchorMin = shadowRect.anchorMax = new Vector2(0.5f, 0f);
        shadowRect.pivot = new Vector2(0.5f, 0f);

        bodyRect.anchorMin = bodyRect.anchorMax = new Vector2(0.5f, 0f);
        bodyRect.pivot = new Vector2(0.5f, 0f);
    }

    void UpdateCandleDisplay()
    {
        float chartHeight = grid.rect.height;
        float chartRange = maxPrice - minPrice;

        // Обновляем тень
        float shadowHeight = (highPrice - lowPrice) / chartRange * chartHeight;
        float shadowPosition = (lowPrice - minPrice) / chartRange * chartHeight;

        shadowRect.sizeDelta = new Vector2(shadowWidth, shadowHeight);
        shadowRect.anchoredPosition = new Vector2(0, shadowPosition);

        // Обновляем тело
        float bodyHeight = Mathf.Abs(currentPrice - openPrice) / chartRange * chartHeight;
        float bodyPosition = (Mathf.Min(openPrice, currentPrice) - minPrice) / chartRange * chartHeight;

        bodyRect.sizeDelta = new Vector2(candleWidth, bodyHeight);
        bodyRect.anchoredPosition = new Vector2(0, bodyPosition);

        // Обновляем цвет тела и тени
        Color candleColor = currentPrice > thresholdPrice ? Color.green : Color.red;
        bodyRect.GetComponent<Image>().color = candleColor;
        shadowRect.GetComponent<Image>().color = candleColor;
    }
}
