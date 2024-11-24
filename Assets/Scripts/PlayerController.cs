using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public Rigidbody playerRb;
    public WheelColliders wheelColliders;
    public WheelMeshes wheelMeshes;

    // Inputs
    public float gasInput;
    public float steerInput;
    public float brakeInput;

    // Powers
    public float slipAngle;
    public float brakePower;
    public float motorPower;

    // Speed
    private float speedTxt;
    private float speed;
    public float maxSpeed;
    private float speedClamped;

    // Save the current Scene
    public String CurrentScene;

    public AnimationCurve steeringCurve;

    public Vector3 centerMass;

    [SerializeField] TextMeshProUGUI speedText;

    public int playerID = 1; // Default player ID

    public void Start()
    {
        playerRb = gameObject.GetComponent<Rigidbody>();
        playerRb.centerOfMass = centerMass;

        if(GameMode.IsSinglePlayer && playerID != 1){
            gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        // Restart
        if (Input.GetKeyDown(KeyCode.R))
        {
            Restart();
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            Menu();
        }

        // Calculate speed
        speedTxt = playerRb.velocity.magnitude * 3.6f; // Speed in km/h
        speed = (float)(wheelColliders.RRWheel.rpm * wheelColliders.RRWheel.radius * 2f * Math.PI / 60f); // Speed in m/s
        speedClamped = Mathf.Lerp(speedClamped, speed, Time.deltaTime);

        CheckInput();
        ApplyMotorPower();
        ApplySteering();
        ApplyWheelRotation();
        ApplyBrake();

        speedText.text = $"{speedTxt:F0} Km/h"; // Display speed text
    }

    void ApplyMotorPower()
    {
        // Calcular o torque baseado na entrada de aceleração
        float torque = motorPower * gasInput;

        // Aplicar torque nas rodas traseiras (para frente ou marcha-atrás)
        if (Mathf.Abs(speed) < maxSpeed || gasInput < 0)
        {
            wheelColliders.RRWheel.motorTorque = torque;
            wheelColliders.RLWheel.motorTorque = torque;

            // Aplicar torque reduzido nas rodas dianteiras
            wheelColliders.FRWheel.motorTorque = torque * 0.5f;
            wheelColliders.FLWheel.motorTorque = torque * 0.5f;
        }
        else
        {
            // Parar de aplicar torque quando atingir a velocidade máxima
            wheelColliders.RRWheel.motorTorque = 0;
            wheelColliders.RLWheel.motorTorque = 0;
            wheelColliders.FRWheel.motorTorque = 0;
            wheelColliders.FLWheel.motorTorque = 0;
        }
    }

    void ApplySteering()
    {
        // Determinar o ângulo de direção
        float steeringAngle = steerInput * steeringCurve.Evaluate(speed);

        // Inverter o ângulo de direção durante a marcha-atrás
        if (gasInput < 0)
        {
            steeringAngle = -steeringAngle;
        }

        // Ajustar os ângulos para não ultrapassar os limites aceitáveis
        steeringAngle = Mathf.Clamp(steeringAngle, -30f, 30f);

        // Aplicar o ângulo de direção às rodas dianteiras
        wheelColliders.FRWheel.steerAngle = steeringAngle;
        wheelColliders.FLWheel.steerAngle = steeringAngle;
    }


    void CheckInput()
    {
        if (GameMode.IsSinglePlayer)
        {
            gasInput = Input.GetAxis("Vertical"); // W/S or Up/Down arrow
            steerInput = Input.GetAxis("Horizontal"); // A/D or Left/Right arrow
            slipAngle = Vector3.Angle(transform.forward, playerRb.velocity - transform.forward);
            
            // Brake only when Space key is pressed
            brakeInput = Input.GetKey(KeyCode.Space) ? 1.0f : 0.0f;
        }
        else
        {
            if (playerID == 1)
            {
                gasInput = Input.GetKey(KeyCode.W) ? 1.0f : (Input.GetKey(KeyCode.S) ? -1.0f : 0.0f);
                steerInput = Input.GetKey(KeyCode.A) ? -1.0f : (Input.GetKey(KeyCode.D) ? 1.0f : 0.0f);
            }
            else if (playerID == 2)
            {
                gasInput = Input.GetKey(KeyCode.UpArrow) ? 1.0f : (Input.GetKey(KeyCode.DownArrow) ? -1.0f : 0.0f);
                steerInput = Input.GetKey(KeyCode.LeftArrow) ? -1.0f : (Input.GetKey(KeyCode.RightArrow) ? 1.0f : 0.0f);
            }
            slipAngle = Vector3.Angle(transform.forward, playerRb.velocity - transform.forward);

            // Brake only when Space key is pressed for player 1, RightControl for player 2
            brakeInput = (playerID == 1 && Input.GetKey(KeyCode.Space)) || (playerID == 2 && Input.GetKey(KeyCode.RightControl)) ? 1.0f : 0.0f;
        }
    }

    void ApplyBrake()
    {
        float brakeTorque = brakeInput * brakePower;

        wheelColliders.FRWheel.brakeTorque = brakeTorque;
        wheelColliders.FLWheel.brakeTorque = brakeTorque;
        wheelColliders.RRWheel.brakeTorque = brakeTorque;
        wheelColliders.RLWheel.brakeTorque = brakeTorque;
    }

    void ApplyWheelRotation()
    {
        UpdateWheel(wheelColliders.FRWheel, wheelMeshes.FRWheel);
        UpdateWheel(wheelColliders.FLWheel, wheelMeshes.FLWheel);
        UpdateWheel(wheelColliders.RRWheel, wheelMeshes.RRWheel);
        UpdateWheel(wheelColliders.RLWheel, wheelMeshes.RLWheel);
    }

    void UpdateWheel(WheelCollider coll, MeshRenderer wheelMesh)
    {
        coll.GetWorldPose(out Vector3 position, out Quaternion rotation);
        wheelMesh.transform.position = position;
        wheelMesh.transform.rotation = rotation;
    }

    public float GetSpeedRatio()
    {
        var gas = Mathf.Clamp(gasInput, 0.5f, 1f);
        return speedClamped * gas / maxSpeed;
    }

    void Restart()
    {
        CurrentScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(CurrentScene);
    }

    void Menu()
    {
        CurrentScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene("Interface");
    }
}

[System.Serializable]
public class WheelColliders
{
    public WheelCollider FRWheel;
    public WheelCollider FLWheel;
    public WheelCollider RRWheel;
    public WheelCollider RLWheel;
}

[System.Serializable]
public class WheelMeshes
{
    public MeshRenderer FRWheel;
    public MeshRenderer FLWheel;
    public MeshRenderer RRWheel;
    public MeshRenderer RLWheel;
}
