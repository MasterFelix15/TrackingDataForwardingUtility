using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardMovement : MonoBehaviour
{
    public float movementSpeed = 0.1f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //get the Input from Horizontal axis
        int horizontalInput = 0;
        //get the Input from Vertical axis
        int verticalInput = 0;

        if (Input.GetKey(KeyCode.W))
        {
            horizontalInput += 1;
        }
        if (Input.GetKey(KeyCode.S))
        {
            horizontalInput -= 1;
        }
        if (Input.GetKey(KeyCode.A))
        {
            verticalInput += 1;
        }
        if (Input.GetKey(KeyCode.D))
        {
            verticalInput -= 1;
        }

        //update the position
        transform.position = transform.position + new Vector3(horizontalInput * movementSpeed * Time.deltaTime, 0, verticalInput * movementSpeed * Time.deltaTime);
    }
}
