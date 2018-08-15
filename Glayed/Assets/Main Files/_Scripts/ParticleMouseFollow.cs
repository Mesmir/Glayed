using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleMouseFollow : MonoBehaviour {
     
    [Range(0.0f, 30.0f)]
    public float trackSpeed;
    public float distanceFromMouse;

    public bool cameraFollow;
    private Vector3 centerWorld = new Vector3(0,50,0);
    public Camera cam;
    public float smoothTime = 0.3F;
    private Vector3 velocity = new Vector3(0, 50, 0);

    private void Start()
    {
        Cursor.visible = false;
        cam = Camera.main;
    }

    void Update () {

        Vector3 mousePos = Input.mousePosition;
        mousePos.z = distanceFromMouse;

        // Get mouse position and lerp the particle to that position over time.
        Vector3 mouseScreenPosToWorldPos = Camera.main.ScreenToWorldPoint(mousePos);
        transform.position = Vector3.Lerp(transform.position, mouseScreenPosToWorldPos, 1.0f - Mathf.Exp(-trackSpeed * Time.deltaTime));

        if (cameraFollow == true)
        {
            //cam.transform.position = Vector3.Lerp(new Vector3 (0,50,0), new Vector3 (mousePos.x, 50, 0), Time.deltaTime);
            Vector3 targetPos = new Vector3(mousePos.x, 50, mousePos.y);
            cam.transform.position = Vector3.SmoothDamp(cam.transform.position, targetPos, ref velocity, smoothTime * Time.deltaTime);
        }
	}
}
