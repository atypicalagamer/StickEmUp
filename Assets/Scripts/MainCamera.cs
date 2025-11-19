using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    public Transform player;
    public Vector3 offset = new Vector3(0, 2, -4);
    public float smoothSpeed = 8f;

    // Start is called before the first frame update
    /*void Start()
    {
        if (player != null)
        {
            offset = transform.position - player.position;
        }
    }*/

    // Update is called once per frame
    void LateUpdate()
    {
        /*if (player != null)
        {
            transform.position = player.position + offset;
        }*/
        Vector3 desiredPos = player.position + player.rotation * offset;

        transform.position = Vector3.Lerp(transform.position, desiredPos, smoothSpeed * Time.deltaTime);

        transform.LookAt(player);
    }
}
