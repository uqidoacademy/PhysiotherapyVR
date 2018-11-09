using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationStarter : MonoBehaviour
{

    private Animator animator;
    //private Sampler sampler;
    public KeyCode startKey = KeyCode.S;
    public string AnimatorStartBoolParamName = "animate";

    // public Sampler realMovementSimulation = null;

    // Use this for initialization
    void Start()
    {
        animator = GetComponent<Animator>();
        //sampler = GetComponent<Sampler>();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetKeyDown(startKey))
        {
            animator.SetBool(AnimatorStartBoolParamName, true);
            
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            animator.SetBool(AnimatorStartBoolParamName, false);
        }

    }

    private void startSampling()
    {
        //sampler.StartSampling();
        //if (realMovementSimulation != null)
        //    realMovementSimulation.StartSampling();
    }

    private void stopSampling()
    {
        //sampler.StopSampling();
        //if (realMovementSimulation != null)
        //    realMovementSimulation.StopSampling();
    }

    public void animationEnded()
    {
        Debug.Log("Animation Ended");
        stopSampling();
    }
}
