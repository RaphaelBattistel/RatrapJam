using UnityEngine;
using TMPro;

public class TemperatureGauge : MonoBehaviour
{
    [SerializeField] private DrillShip ship;
    [SerializeField] private RectTransform cursorRectTransform; // le petit curseur

    private float gaugeHeight;

    private void Start()
    {
        // Récupère la hauteur de la jauge (le parent du curseur)
        gaugeHeight = cursorRectTransform.parent.GetComponent<RectTransform>().rect.height;
    }

    private void Update()
    {
        float temp = ship.Temperature;

        // Normalise la température (0 = bas, 1 = haut)
        float normalizedTemp = Mathf.Clamp01(temp / ship.MaxTemperature);

        // Déplace le curseur verticalement (bas = 0, haut = gaugeHeight)
        cursorRectTransform.anchoredPosition = new Vector2(
            cursorRectTransform.anchoredPosition.x,
            normalizedTemp * gaugeHeight
        );
    }
}