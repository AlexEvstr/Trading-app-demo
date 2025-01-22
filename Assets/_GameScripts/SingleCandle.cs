using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class SingleCandle : MonoBehaviour
{
    [Header("Settings")]
    public Dropdown timeDropdown;
    public float candleLifetime = 5f; // Время жизни одной свечи
    public float candleWidth = 25; // Ширина тела свечи
    public float shadowWidth = 5; // Ширина тени свечи
    public float candleSpacing = 35; // Расстояние между свечами по оси X
    public float priceChangeSpeed = 5f; // Скорость изменения цены

    [Header("References")]
    public RectTransform grid; // Контейнер для графика
    public Text priceText; // Текст текущей цены

    private RectTransform currentBodyRect; // Тело текущей свечи
    private RectTransform currentShadowRect; // Тень текущей свечи
    private float openPrice = 200f; // Цена открытия текущей свечи
    public float currentPrice = 200f; // Текущая цена
    private float targetPrice; // Целевая цена
    private float highPrice = 200f; // Максимальная цена
    private float lowPrice = 200f; // Минимальная цена
    private float previousClosePrice = 200f; // Цена закрытия предыдущей свечи
    private float elapsedTime; // Время с момента создания текущей свечи

    private float minPrice = 180f; // Минимальная возможная цена
    private float maxPrice = 220f; // Максимальная возможная цена

    private List<RectTransform> candleBodies = new List<RectTransform>(); // Список тел свечей
    private List<RectTransform> candleShadows = new List<RectTransform>(); // Список теней свечей

    void Awake()
    {
        InitializeCandle();
        timeDropdown.onValueChanged.AddListener(OnTimeDropdownChanged);
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;

        // Двигаемся к целевой цене
        MoveTowardsTargetPrice();

        // Обновляем отображение текущей свечи
        UpdateCandleDisplay();

        // Если время жизни текущей свечи истекло, фиксируем ее и создаем новую
        if (elapsedTime >= candleLifetime)
        {
            FixCurrentCandle();
            ShiftAllCandlesLeft(); // Сдвигаем все свечи влево
            InitializeCandle(); // Создаем новую свечу
        }
    }

    public void OnTimeDropdownChanged(int index)
    {
        Debug.Log($"Index received: {index}");
        switch (index)
        {
            case 0:
                candleLifetime = 5f;
                break;
            case 1:
                candleLifetime = 10f;
                break;
            case 2:
                candleLifetime = 15f;
                break;
        }
        Debug.Log($"candleLifetime updated to: {candleLifetime}");
    }


    void InitializeCandle()
    {
        elapsedTime = 0f;

        // Устанавливаем начальные значения
        openPrice = currentPrice; // Цена открытия равна текущей цене
        highPrice = openPrice;
        lowPrice = openPrice;

        // Назначаем начальную целевую цену
        AssignNewTargetPrice();

        // Создаем новую свечу
        CreateCandle();
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
        // Генерируем новую целевую цену, которая отличается на 5-10
        float priceChange = Random.Range(5f, 11f);
        targetPrice = Random.value > 0.5f ? currentPrice + priceChange : currentPrice - priceChange;

        // Ограничиваем целевую цену в пределах допустимого диапазона
        targetPrice = Mathf.Clamp(targetPrice, minPrice, maxPrice);
    }

    void CreateCandle()
    {
        // Тень свечи
        GameObject shadow = new GameObject("Shadow");
        shadow.transform.SetParent(grid, false);
        currentShadowRect = shadow.AddComponent<RectTransform>();
        Image shadowImage = shadow.AddComponent<Image>();
        shadowImage.color = Color.green;

        // Тело свечи
        GameObject body = new GameObject("Body");
        body.transform.SetParent(grid, false);
        currentBodyRect = body.AddComponent<RectTransform>();
        Image bodyImage = body.AddComponent<Image>();
        bodyImage.color = Color.green;

        // Настройка начальной позиции свечи (в центре области отображения)
        Vector2 candlePosition = new Vector2(0, 0);
        currentShadowRect.anchoredPosition = candlePosition;
        currentBodyRect.anchoredPosition = candlePosition;

        // Настройка размеров
        currentShadowRect.sizeDelta = new Vector2(shadowWidth, 0);
        currentBodyRect.sizeDelta = new Vector2(candleWidth, 0);

        currentShadowRect.anchorMin = currentShadowRect.anchorMax = new Vector2(0.5f, 0f);
        currentShadowRect.pivot = new Vector2(0.5f, 0f);

        currentBodyRect.anchorMin = currentBodyRect.anchorMax = new Vector2(0.5f, 0f);
        currentBodyRect.pivot = new Vector2(0.5f, 0f);

        // Добавляем свечу в списки
        candleBodies.Add(currentBodyRect);
        candleShadows.Add(currentShadowRect);
    }

    void FixCurrentCandle()
    {
        // Сохраняем цену закрытия текущей свечи как предыдущую
        previousClosePrice = currentPrice;

        // Оставляем текущую свечу как зафиксированную (не изменяемую)
        currentBodyRect = null;
        currentShadowRect = null;
    }

    void ShiftAllCandlesLeft()
    {
        // Сдвигаем все свечи влево на candleSpacing
        for (int i = 0; i < candleBodies.Count; i++)
        {
            candleBodies[i].anchoredPosition += new Vector2(-candleSpacing, 0);
            candleShadows[i].anchoredPosition += new Vector2(-candleSpacing, 0);
        }
    }

    void UpdateCandleDisplay()
    {
        float chartHeight = grid.rect.height;
        float chartRange = maxPrice - minPrice;

        // Обновляем тень
        float shadowHeight = (highPrice - lowPrice) / chartRange * chartHeight;
        float shadowPosition = (lowPrice - minPrice) / chartRange * chartHeight;

        currentShadowRect.sizeDelta = new Vector2(shadowWidth, shadowHeight);
        currentShadowRect.anchoredPosition = new Vector2(0, shadowPosition);

        // Обновляем тело
        float bodyHeight = Mathf.Abs(currentPrice - openPrice) / chartRange * chartHeight;
        float bodyPosition = (Mathf.Min(openPrice, currentPrice) - minPrice) / chartRange * chartHeight;

        currentBodyRect.sizeDelta = new Vector2(candleWidth, bodyHeight);
        currentBodyRect.anchoredPosition = new Vector2(0, bodyPosition);

        // Определяем цвет свечи:
        // Если это первая свеча, отталкиваемся от 200, иначе от предыдущей цены закрытия
        float colorThreshold = previousClosePrice;
        if (candleBodies.Count <= 1) // Если первая свеча
        {
            colorThreshold = 200f;
        }

        Color candleColor = currentPrice > colorThreshold ? Color.green : Color.red;
        currentBodyRect.GetComponent<Image>().color = candleColor;
        currentShadowRect.GetComponent<Image>().color = candleColor;
    }
}