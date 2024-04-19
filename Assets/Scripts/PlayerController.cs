using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using static PlayerController;

public class PlayerController : MonoBehaviour
{
    public Rigidbody playerRb;
    public WheelColliders wheelColliders;
    public WheelMeshes wheelMeshes;

    //Inputs
    public float gasInput;
    public float steerInput;
    public float brakeInput;

    //Powers
    public float slipAngle;
    public float brakePower;
    public float motorPower;

    //Speed
    private float speedTxt;
    private float speed;
    public float maxSpeed;
    private float speedClamped;

    //Save the current Scene
    public String CurrentScene;

    public AnimationCurve steeringCurve;

    public Vector3 centerMass;

    [SerializeField] TextMeshProUGUI speedText;

    EndLapController end;
    private bool verified;


    public void Start()
    {
        playerRb = gameObject.GetComponent<Rigidbody>();
        playerRb.centerOfMass = centerMass;
    }

    private void Update()
    {
        //Restart
        if (Input.GetKeyDown(KeyCode.R))
        {
            Restart();
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            Menu();
        }



        speedTxt = playerRb.velocity.magnitude*2f;//saber a velocidade a que o carro está

        //Alterar a forma de saber o speed por causa do som do carro, em vez de saber a velocidade do motor sabemos a velocidade das rodas
        speed = (float)(wheelColliders.RRWheel.rpm * wheelColliders.RRWheel.radius * 2f * Math.PI / 20f);
        speedClamped = Mathf.Lerp(speedClamped, speed, Time.deltaTime);

        CheckInput();
        ApplyMotorPower();
        ApplySteering();
        ApplyWheelRotation();
        ApplyBrake();

        speedText.text = speedTxt.ToString("F0")+ "Km/h"; //Mostrar o texto com a velocidade
    }

    void ApplySteering()
    {
        float steeringAngle = steerInput * steeringCurve.Evaluate(speed);
        
        steeringAngle += Vector3.SignedAngle(transform.forward, playerRb.velocity + transform.forward, Vector3.up);
        steeringAngle = Mathf.Clamp(steeringAngle, -90f, 90f);

        wheelColliders.FRWheel.steerAngle = steeringAngle;
        wheelColliders.FLWheel.steerAngle = steeringAngle;
    }

    void ApplyMotorPower()
    {
        if (speed < maxSpeed)
        {
            wheelColliders.RRWheel.motorTorque = motorPower * gasInput;
            wheelColliders.RLWheel.motorTorque = motorPower * gasInput;
            wheelColliders.FRWheel.motorTorque = motorPower * gasInput * 0.5f;
            wheelColliders.FLWheel.motorTorque = motorPower * gasInput * 0.5f;
        }
        else
        {
            wheelColliders.RRWheel.motorTorque = 0;
            wheelColliders.RLWheel.motorTorque = 0;
            wheelColliders.FRWheel.motorTorque = 0;
            wheelColliders.FLWheel.motorTorque = 0;
        }
        
    }


    void CheckInput()
    {
        gasInput = Input.GetAxis("Vertical");// WS Seta cima e baixo
        steerInput = Input.GetAxis("Horizontal");// A,D,<- ,->

        slipAngle = Vector3.Angle(transform.forward,playerRb.velocity-transform.forward);


        if (slipAngle < 120f)
        { 
            if (gasInput < 0){ 
                //brakeInput = 0;
                brakeInput = Mathf.Abs(gasInput);
                gasInput = 0;
            }
            else
            {
                brakeInput = 0;
            
            }
        }
        else
        {
            brakeInput = 0;
        }
        
    }

    void ApplyBrake()
    {
        wheelColliders.FRWheel.brakeTorque = brakeInput * brakePower * 0.6f;
        wheelColliders.FLWheel.brakeTorque = brakeInput * brakePower * 0.6f;

        wheelColliders.RRWheel.brakeTorque = brakeInput * brakePower * 0.4f;
        wheelColliders.RLWheel.brakeTorque = brakeInput * brakePower * 0.4f;

    }

    //Rotação das rodas
    void ApplyWheelRotation()
    {
        UpdateWheel(wheelColliders.FRWheel, wheelMeshes.FRWheel);
        UpdateWheel(wheelColliders.FLWheel, wheelMeshes.FLWheel);
        UpdateWheel(wheelColliders.RRWheel, wheelMeshes.RRWheel);
        UpdateWheel(wheelColliders.RLWheel, wheelMeshes.RLWheel);
    }

    void UpdateWheel(WheelCollider coll, MeshRenderer wheelMesh)
    {
        Quaternion quat;//"rotação da roda"?
        Vector3 position;

        coll.GetWorldPose(out position, out quat);
        wheelMesh.transform.position = position;
        wheelMesh.transform.rotation = quat;
    }

    public float GetSpeedRatio()
    {
        var gas = Mathf.Clamp(gasInput, 0.5f, 1f);

        return speedClamped*gas / maxSpeed;
    }

    void Restart()
    {
        CurrentScene = SceneManager.GetActiveScene().name; //saber em que scene está
        SceneManager.LoadScene(CurrentScene);//Carregar essa scene para dar restart
    }

    void Menu()
    {
        CurrentScene = SceneManager.GetActiveScene().name; //saber em que scene está
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

