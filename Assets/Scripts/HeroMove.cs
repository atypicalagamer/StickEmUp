using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroMove : MonoBehaviour
{
    public Rigidbody rb;
    public Vector3 InputKey;
    float Myfloat;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        //rb.freezeRotation = false;
        //rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
    }
    // Update is called once per frame
    void Update()
    {
        InputKey = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));    
    }

    void FixedUpdate()
    {
        /*rb.MovePosition((Vector3) transform.position + InputKey * 10 * Time.deltaTime);

        if (InputKey.magnitude >= 0.1f)
        {
            float Angle = Mathf.Atan2(InputKey.x, InputKey.z) * Mathf.Rad2Deg;
            float Smooth = Mathf.SmoothDampAngle(transform.eulerAngles.y, Angle, ref Myfloat, 0.1f);

            transform.rotation = Quaternion.Euler(0, Smooth, 0);
        }*/

        Vector3 targetVelocity = InputKey.normalized * 10f;
        Vector3 velocity = new Vector3(targetVelocity.x, rb.velocity.y, targetVelocity.z);
        rb.velocity = velocity;

        if (InputKey.magnitude >= 0.1f)
        {
            float Angle = Mathf.Atan2(InputKey.x, InputKey.z) * Mathf.Rad2Deg;
            float Smooth = Mathf.SmoothDampAngle(transform.eulerAngles.y, Angle, ref Myfloat, 0.1f);

            transform.rotation = Quaternion.Euler(0, Smooth, 0);
        }
    }
}
