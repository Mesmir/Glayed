using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleMouseFollow : MonoBehaviour {
     
    [Range(0.0f, 30.0f)]
    public float trackSpeed;
    public float distanceFromMouse;

    private void Start()
    {
        Cursor.visible = false;
    }

    void Update () {

        Vector3 mousePos = Input.mousePosition;
        mousePos.z = distanceFromMouse;

        // Get mouse position and lerp the particle to that position over time.
        Vector3 mouseScreenPosToWorldPos = Camera.main.ScreenToWorldPoint(mousePos);
        transform.position = Vector3.Lerp(transform.position, mouseScreenPosToWorldPos, 1.0f - Mathf.Exp(-trackSpeed * Time.deltaTime));
	}
}
