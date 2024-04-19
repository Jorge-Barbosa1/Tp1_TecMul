using UnityEngine;

public class EngineAudioController : MonoBehaviour
{
    //Car running
    public AudioSource runningSound;
    public float runningMaxVolume;
    public float runningMaxPitch;

    //Car reversing
    public AudioSource reverseSound;
    public float reverseMaxVolume;
    public float reverseMaxPitch;

    //Car stopped
    public AudioSource idleSound;
    public float idleMaxVolume;

    //Rev limiter
    private float revLimiter;
    public float limiterSound = 1f;
    public float limiterFreq = 3f;
    public float limiterEngage = 0.8f;


    

    private float speedRatio;


    private PlayerController playerController;



    // Start is called before the first frame update
    void Start()
    {
        playerController = GetComponent<PlayerController>();
        runningSound.volume = 0f;
        reverseSound.volume = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        float speedSign = 0;
        if (playerController)
        {
            speedSign = Mathf.Sign(playerController.GetSpeedRatio());
            speedRatio = playerController.GetSpeedRatio();
        }

        if (speedRatio > limiterEngage)
        {
            revLimiter = (Mathf.Sin(Time.time *limiterFreq)+1f)*limiterSound * (speedRatio - limiterEngage);
        }
        idleSound.volume = Mathf.Lerp(0.1f, idleMaxVolume, speedRatio);
        if (speedSign > 0)
        {
            reverseSound.volume = 0;
            runningSound.volume = Mathf.Lerp(0.3f, runningMaxVolume, speedRatio);
            runningSound.pitch = Mathf.Lerp(runningSound.pitch, Mathf.Lerp(0.3f, runningMaxPitch + revLimiter, speedRatio), Time.deltaTime);
        }
        else
        {
            runningSound.volume = 0;
            reverseSound.volume = Mathf.Lerp(0.3f, reverseMaxVolume, speedRatio);
            reverseSound.pitch = Mathf.Lerp(reverseSound.pitch, Mathf.Lerp(0.3f, reverseMaxPitch + revLimiter, speedRatio), Time.deltaTime);
        }
        
    }
}
