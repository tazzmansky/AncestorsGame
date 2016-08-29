using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Controller2D))]

public class Player : MonoBehaviour
{

    public float jumpHeight = 4;
    public float timeToJumpApex = 0.4f;

    public float moveSpeed = 6;
    public float gravity = -20;
    public float jumpVelocity = 8;

    Vector3 velocity;

    Controller2D controller;


    void Start()
    {
        controller = GetComponent<Controller2D>();

        gravity = -(2 * jumpHeight) / Mathf.Pow(timeToJumpApex, 2);                                      // Mathf.Pow x podniesione do potęgi y
        jumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
        print ("Gravity: " + gravity + " Jump Velocity: " + jumpVelocity);



    }

    void Update()
    {

        if (controller.collisions.above || controller.collisions.below)
        {
            velocity.y = 0;
        }


        Vector2 input = new Vector2 (Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));      // GetAxisRaw zwraca -1, 0 lub 1. Nie ma "wygładzania" ruchu postaci - trzeba zrobić inaczej.

        if (Input.GetKeyDown(KeyCode.Space) && controller.collisions.below)
        {
            velocity.y = jumpVelocity;

        }

        velocity.x = input.x * moveSpeed;
        velocity.y += gravity * Time.deltaTime;
        controller.Move (velocity * Time.deltaTime);


    }
}