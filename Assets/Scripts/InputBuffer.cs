using UnityEngine;

public class InputBuffer : MonoBehaviour
{
    public float inputBufferTime = 2.0f;
    public int comboIndex;

    PlayerAnimatorDriver animDriver;
    Attack attack;

    float timer;

    void Start()
    {
        animDriver = GetComponent<PlayerAnimatorDriver>();
        attack = GetComponent<Attack>();
        inputBufferTime = 0.0f;
    }

    void Update()
    {

        /*if (inputBufferTime > 0f)
        {
            inputBufferTime -= Time.deltaTime;

            if (inputBufferTime < 0f)
                inputBufferTime = 0f;
        }*/
    }

    /*public void Press()
    {
        if (inputBufferTime > 0) 
        {
            animDriver.TriggerAttack2();
        }
        inputBufferTime = 2.0f;
    }*/
}