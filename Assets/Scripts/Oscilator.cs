using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]

public class Oscilator : MonoBehaviour {

    [SerializeField] Vector3 movementVector = new Vector3(10f, 10f, 10f);
    [SerializeField] float period = 2f;
    float movementFactor; // zero for not moved, 1 for fully moved
    private Vector3 startingPos;

    /// ////////////////////////////

    void Start () {
        startingPos = transform.position;
	}
	
	void Update () {
        // TODO protect against period 0 NaN bug
        if (period <= Mathf.Epsilon) { return; }
        float cycles = Time.time / period; // frows continually from zero

        const float tau = Mathf.PI * 2f; // about 6.28
        float rawSineWave = Mathf.Sin(cycles * tau); // don't quite understand how this goes from -1 to 1

        movementFactor = rawSineWave / 2f + 0.5f;
        Vector3 offset = movementVector * movementFactor;
        transform.position = startingPos + offset;
    }
    }
