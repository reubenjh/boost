using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]

public class Oscilator : MonoBehaviour {

    [SerializeField] Vector3 movementVector = new Vector3(10f, 10f, 10f);
    [SerializeField] float period = 2f;

    // remove later:
    [Range(0,1)][SerializeField] float movementFactor; // zero for not moved, 1 for fully moved

    private Vector3 startingPos;
    private bool adding = true;

    // Use this for initialization
    void Start () {
        startingPos = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        //setmovementfactor
        float cycles = Time.time / period;

        const float tau = Mathf.PI * 2f;
        float rawSineWave = Mathf.Sin(cycles * tau);

        movementFactor = rawSineWave / 2f + 0.5f;
        Vector3 offset = movementVector * movementFactor;
        transform.position = startingPos + offset;

        /*
        changeMovementFactor();
        checkMovementRange();
        */

    }

    /*
    void changeMovementFactor()
    {
        if (adding == true)
        {
            movementFactor = movementFactor + 0.01f;
        } else
        {
            movementFactor = movementFactor - 0.01f;
        }
    }

    
    void checkMovementRange()
    {
        if (movementFactor > 0.9f)
        {
            adding = false;
        }
        if (movementFactor < 0.1f)
        {
            adding = true;
        }
    }
    */
    }
