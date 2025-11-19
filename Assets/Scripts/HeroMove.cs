using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroMove : MonoBehaviour
{
    public Rigidbody rb;
    public Vector3 InputKey;
    public Transform cam;
    float Myfloat;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    // Update is called once per frame
    void Update()
    {
        InputKey = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));    
    }

    void FixedUpdate()
    {
        float x = InputKey.x;
        float z = InputKey.z;

        // No input? Don't rotate
        if (InputKey.magnitude >= 0.1f)
        {
            // Camera-based direction
            Vector3 camForward = cam.forward;
            Vector3 camRight = cam.right;

            // Flatten so player doesn't look up/down
            camForward.y = 0;
            camRight.y = 0;
            camForward.Normalize();
            camRight.Normalize();

            // Calculate movement direction relative to camera
            Vector3 moveDir = (camForward * z + camRight * x).normalized;

            // Rotate player toward movement direction
            float targetAngle = Mathf.Atan2(moveDir.x, moveDir.z) * Mathf.Rad2Deg;
            float smooth = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref Myfloat, 0.1f);
            transform.rotation = Quaternion.Euler(0f, smooth, 0f);

            // Apply movement (keep Y from gravity)
            Vector3 velocity = moveDir * 10f;
            rb.velocity = new Vector3(velocity.x, rb.velocity.y, velocity.z);
        }
        else
        {
            // no movement input — keep vertical velocity only
            rb.velocity = new Vector3(0, rb.velocity.y, 0);
        }
    }
}
