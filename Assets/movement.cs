using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class movement : MonoBehaviour
{
    public float speed = 5;
    public float jumpForce = 200;
    public Rigidbody rigid;
    private bool isGrounded;

    [Header("Upright Settings")]
    public float uprightStrength = 10f;
    public float uprightDamping = 5f;

    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody>(); 
        //rigid.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;   
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        //Vector3 movement = new Vector3(horizontalInput * speed, rigid.velocity.y);
        //rigid.velocity = movement;

        Vector3 moveDirection = transform.right * horizontalInput + transform.forward * verticalInput;
        moveDirection.Normalize();

        rigid.AddForce(moveDirection * speed * Time.deltaTime, ForceMode.VelocityChange);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rigid.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }
    }

    void FixedUpdate()
    {
        KeepUpRight();
    }

    void KeepUpRight()
    {
        Vector3 currentUp = transform.up;
        Vector3 desiredUp = Vector3.up;

        Vector3 tiltAxis = Vector3.Cross(currentUp, desiredUp);

        float tiltAmount = Vector3.Dot(currentUp, desiredUp);

        tiltAmount = Mathf.Clamp(tiltAmount, -1f, 1f);

        float angle = Mathf.Acos(tiltAmount);

        if (angle < .001f || tiltAxis == Vector3.zero)
        {
            return;
        }

        tiltAxis.Normalize();

        float strength = 500f;
        float damping = 50f;

        Vector3 correctiveTorque = tiltAxis * angle * strength - rigid.angularVelocity * damping;

        if (!float.IsNaN(correctiveTorque.x) && !float.IsNaN(correctiveTorque.y) && !float.IsNaN(correctiveTorque.z))
        {
            rigid.AddTorque(correctiveTorque, ForceMode.Acceleration);
        }
        /*Quaternion desiredRotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);
        Quaternion rotError = desiredRotation * Quaternion.Inverse(rigid.rotation);
        rotError.ToAngleAxis(out float angle, out Vector3 axis);

        if (angle > 180f)
        {
            angle -= 360f;
        }

        Vector3 torque = axis * (angle * Mathf.Deg2Rad * uprightStrength) - rigid.angularVelocity * uprightDamping;*/
        //rigid.AddTorque(torque, ForceMode.Acceleration);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}
