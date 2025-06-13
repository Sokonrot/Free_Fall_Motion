using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FreeFallinair : MonoBehaviour
{
    public TMP_InputField heightInputField;
    public TMP_InputField speedInputField;
    public TMP_InputField massInputField;
    public TMP_InputField areaInputField;
    public Button startButton;
    public TextMeshProUGUI resultText;
    public TextMeshProUGUI GText;
    public TextMeshProUGUI TimeText;
    public TextMeshProUGUI heightText;

    public bool enabledAirresistance = false;

    private float height = 0f;
    private float initialSpeed = 0f;
    private float mass = 0f;
    private float velocity = 0f;
    private bool isOnAir = false;
    private float fallTime = 0f;
    private float maxHeight = 0f;
    private float vfrt = 0f;
    private Vector3 lastPosition;

    private float airDensity = 1.225f;
    private float dragCoefficient = 0.47f;
    private float area = 0.01f;

    void Start()
    {
        GText.text = "9.8 m/s²";
        startButton.onClick.AddListener(StartFall);
    }

    void Update()
    {
        if (isOnAir)
        {
            float gravity = Mathf.Abs(Physics.gravity.y);

            if (enabledAirresistance)
            {
                // Use direction-independent drag, always opposing movement
                float dragForce = 0.5f * airDensity * dragCoefficient * area * velocity * velocity;
                dragForce *= Mathf.Sign(velocity); // drag opposes velocity direction

                float netForce = mass * (- gravity) - dragForce;
                float acceleration = netForce / Mathf.Max(mass, 0.1f);

                velocity += acceleration * Time.deltaTime;
                transform.position += new Vector3(0, velocity * Time.deltaTime, 0);
                fallTime += Time.deltaTime;
            }
            else
            {
                fallTime += Time.deltaTime;
                // Use kinematic equation for free fall h = v_0 * t = 1/2 * gt^2
                float currentY = height + initialSpeed * fallTime - 0.5f * gravity * Mathf.Pow(fallTime, 2);
                transform.position = new Vector3(transform.position.x, currentY, transform.position.z);
                velocity = initialSpeed - gravity * fallTime;
            }

            // Track maximum height
            if (transform.position.y > maxHeight)
                maxHeight = transform.position.y;

            // UI updates
            heightText.text = "Max Height: " + maxHeight.ToString("F2") + " m";
            TimeText.text = "Time: " + fallTime.ToString("F2") + "s";

            // If hit the ground
            if (transform.position.y <= 0)
            {
                transform.position = new Vector3(transform.position.x, 0, transform.position.z);
                isOnAir = false;
                CalculateFinalVelocity();
            }
        }
    }

    public void StartFall()
    {
        height = string.IsNullOrEmpty(heightInputField.text) ? 0f : float.Parse(heightInputField.text);
        initialSpeed = string.IsNullOrEmpty(speedInputField.text) ? 0f : float.Parse(speedInputField.text);
        mass = string.IsNullOrEmpty(massInputField.text) ? 1f : float.Parse(massInputField.text);
        area = string.IsNullOrEmpty(areaInputField.text) ? 0.01f : float.Parse(areaInputField.text);

        transform.position = new Vector3(transform.position.x, height, transform.position.z);
        velocity = initialSpeed;
        maxHeight = height;
        fallTime = 0f;
        isOnAir = true;
        lastPosition = transform.position;
        resultText.text = "";
    }

    private void CalculateFinalVelocity()
    {
        resultText.text = "(vf): " + velocity.ToString("F2") + " m/s";
    }
}
