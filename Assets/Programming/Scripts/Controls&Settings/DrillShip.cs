using TMPro;
using UnityEngine;

public class DrillShip : MonoBehaviour
{
    
    [SerializeField] private TextMeshPro positionText;
    [SerializeField] private Collider detectZone;

    [SerializeField] private DialogueData LooseDialogue;   

    [Header("Vitesse")]
    [SerializeField] private Lever speedLever;
    [SerializeField] private float maxSpeed = 10f;
    [SerializeField] private float rotationSpeed = 0.5f;

    [Header("Rotation X")]
    [SerializeField] private RotationButton rotateXPositive;
    [SerializeField] private RotationButton rotateXNegative;
    [SerializeField] private TextMeshPro rotateXText;

    [Header("Rotation Y")]
    [SerializeField] private RotationButton rotateYPositive;
    [SerializeField] private RotationButton rotateYNegative;
    [SerializeField] private TextMeshPro rotateYText;

    [Header("Température")]
    [SerializeField] private float temperatureIncreaseRate = 10f;

    [SerializeField] private float maxTemperature = 500f;
    private float temperature = 0f;
    private float rotX;
    private float rotY;

    public float Temperature => temperature;
    public float MaxTemperature => maxTemperature;


    private void Start()
    {
    }
    void Update()
    {
        Move();
        Rotate();
        UpdateTemperature();
    }

    private void Move()
    {
        float speed = speedLever.NormalizedValue * maxSpeed;
        transform.position -= transform.up * speed * Time.deltaTime;

        positionText.text = "LOCATION:" + "\nX: " + (int)transform.position.x + "\nY: " + (int)transform.position.y + "\nZ: " + (int)transform.position.z;
    }

    private void Rotate()
    {
        if (rotateXPositive.IsPressed)
        { 
            rotX += rotationSpeed * Time.deltaTime;
        }

        if (rotateXNegative.IsPressed)
        { 
            rotX -= rotationSpeed * Time.deltaTime;
        }


        if (rotateYPositive.IsPressed)
        { 
            rotY += rotationSpeed * Time.deltaTime;
        }

        if (rotateYNegative.IsPressed)
        { 
            rotY -= rotationSpeed * Time.deltaTime;
        }

        transform.eulerAngles = new Vector3((int)rotY, (int)rotX, 0f);

        rotateXText.text = "Rotation X :\n" + (int)rotX;
        rotateYText.text = "Rotation Y :\n" + (int)rotY;
    }

    private void UpdateTemperature()
    {
        if(temperature >= maxTemperature)
        {
            GameOverScreen.Instance.PlayDialogue(LooseDialogue);
        }
        temperature += temperatureIncreaseRate * Time.deltaTime;
    }


    public void ReduceTemperature(float percentage)
    {
        temperature -= temperature * (percentage / 100f);
        temperature = Mathf.Max(0f, temperature); // Empêche la température d'être négative
    }

}
