using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class falling_moon : MonoBehaviour
{
    // Public references to the UI elements
    public TMP_InputField heightInputField;
    public TMP_InputField speedInputField;
    public TMP_InputField massInputField;
    public Button startButton;
    public TextMeshProUGUI resultText;
    public TextMeshProUGUI GText;
    public TextMeshProUGUI TimeText;
    public TextMeshProUGUI heightText;

    // The object's parameters
    private float height;
    private float initialSpeed;
    private float mass;

    private bool isFalling = false;
    private float fallTime = 0f;
    private float maxHeight = 0f;

    void Start()
    {
        GText.text = "1.625 m/s²";
        startButton.onClick.AddListener(StartFall);
    }

    void Update()
    {
        if (isFalling)
        {
            fallTime += Time.deltaTime;
            // Calculate the distance fallen based on the equation of motion.
            float distanceFallen = initialSpeed * fallTime + 0.5f * ((Physics.gravity.y)/6) * Mathf.Pow(fallTime, 2);
            transform.position = new Vector3(transform.position.x, height + distanceFallen, transform.position.z);
            maxHeight = (Mathf.Pow(initialSpeed, 2)) / (2 * Mathf.Abs((Physics.gravity.y)/4));
            heightText.text = "Distance: " + maxHeight.ToString("F2") + " m";
            TimeText.text = "Time: " + fallTime.ToString("F2") + "s";
            // Stop 
            if (transform.position.y <= 0)
            {
                transform.position = new Vector3(transform.position.x, 0, transform.position.z);
                isFalling = false;
                CalculateFinalVelocity();
            }
        }
    }

    // Method to start the fall when the button is clicked
    public void StartFall()
    {
        // Parse the values from the input fields
        bool validInput = true;

        if (!float.TryParse(heightInputField.text, out height))
        {
            height = 0f;
            validInput = false;
        }

        if (!float.TryParse(speedInputField.text, out initialSpeed))
        {
            initialSpeed = 0f;
            validInput = false;
        }

        if (!float.TryParse(massInputField.text, out mass))
        {
            mass = 0f;
            validInput = false;
        }



        isFalling = true;
        fallTime = 0f;
        maxHeight = height;
    }

    // Calculate the final velocity when the object reaches the ground
    private void CalculateFinalVelocity()
    {
        // Using the kinematic equation to calculate the final velocity (vf):
        // vf^2 = v0^2 + 2 * g * h
        // vf = sqrt(v0^2 + 2 * g * h)
        float g = Mathf.Abs((Physics.gravity.y)/4); // Earth's gravity
        float vf = Mathf.Sqrt(Mathf.Pow(initialSpeed, 2) + 2 * g * height);

        // Display the final velocity (vf)
        resultText.text = " (vf): " + vf.ToString("F2") + " m/s";
    }
}
