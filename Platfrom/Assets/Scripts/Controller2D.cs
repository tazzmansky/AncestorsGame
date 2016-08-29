using UnityEngine;
using System.Collections;

[RequireComponent (typeof (BoxCollider2D))]

public class Controller2D : MonoBehaviour {

    public LayerMask collisionMask;



    const float skinWidth = .015f;                              // szerokość "skóry" obiektu
    public int horizontalRayCount = 4;                          // liczba H rayow
    public int verticalRayCount = 4;                            // liczba V rayow

    float horizontalRaySpacing;
    float verticalRaySpacing;

    BoxCollider2D collider;
    RaycastOrigins raycastOrigins;
    public CollisionInfo collisions;


	void Start ()                                               // Start
    {
        collider = GetComponent<BoxCollider2D>();
        CalculateRaySpacing();



    }

   

    public void Move(Vector3 velocity)                          // Move
    {
        UpdateRaycastOrigins();
        collisions.Reset();

        if (velocity.x != 0)
        {

            HorizontalCollisions(ref velocity);
        }

        if (velocity.y != 0)
        {
            VerticalCollisions(ref velocity);

        }

        transform.Translate(velocity);

    }

    void HorizontalCollisions(ref Vector3 velocity)             // HorizontalCollisions

    {
        float directionX = Mathf.Sign(velocity.x);              // Mathf.Sign zwraca znak wartości, 1 jeżeli wartość >=0, 0 jeżeli wartość <0
        float rayLenght = Mathf.Abs(velocity.x) + skinWidth;    // Mathf.Abs zwraca wartość bezwględną (absolute) zmiennej  



        for (int i = 0; i < horizontalRayCount; i++)
        {
            Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;
            rayOrigin += Vector2.up * (horizontalRaySpacing * i);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLenght, collisionMask);     // hit czyli obiekt koliduje z rayem

            if (hit)
            {
                velocity.x = (hit.distance - skinWidth) * directionX;
                rayLenght = hit.distance;

                collisions.left = directionX == -1;             // jeżeli idziemy w lewo i kolizja, wtedy collissions.left = true
                collisions.right = directionX == 1;
                             


            }

        }
    }

        void VerticalCollisions(ref Vector3 velocity)           // VerticalCollisions

    {
        float directionY = Mathf.Sign(velocity.y);              // Mathf.Sign zwraca znak wartości, 1 jeżeli wartość >=0, 0 jeżeli wartość <0
        float rayLenght = Mathf.Abs(velocity.y) + skinWidth;    // Mathf.Abs zwraca wartość bezwględną (absolute) zmiennej  


   
        for (int i = 0; i < verticalRayCount; i ++)
            {
            Vector2 rayOrigin = (directionY == -1) ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
            rayOrigin += Vector2.right * (verticalRaySpacing * i + velocity.x);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLenght, collisionMask);     // hit czyli obiekt koliduje z rayem

            Debug.DrawRay(rayOrigin, Vector2.up * directionY * rayLenght, Color.red);

            if (hit)
            {
                velocity.y = (hit.distance - skinWidth) * directionY;
                rayLenght = hit.distance;

                collisions.below = directionY == -1;
                collisions.above = directionY == 1;


            }

        }
    }


    void UpdateRaycastOrigins()                                 // UpdateRaycastOrigins
    {
        Bounds bounds = collider.bounds;                        // axis-alligned bounding box AABB
        bounds.Expand(skinWidth * -2);                          // zwężamy AABB o dwukrotność "skóry" 

        raycastOrigins.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
        raycastOrigins.bottomRight = new Vector2(bounds.max.x, bounds.min.y);
        raycastOrigins.topLeft = new Vector2(bounds.min.x, bounds.max.y);
        raycastOrigins.topRight = new Vector2(bounds.max.x, bounds.max.y);

    }

    void CalculateRaySpacing()                                  // CalculateRaySpacing
    {
        Bounds bounds = collider.bounds;
        bounds.Expand(skinWidth * -2);

        horizontalRayCount = Mathf.Clamp(horizontalRayCount, 2, int.MaxValue);      // zacisk horizontalRayCount między 2, a int.MaxValue czyli 2147483666MORRRDOOOORRRR!!!
        verticalRayCount = Mathf.Clamp(verticalRayCount, 2, int.MaxValue);          // tak samo dla verticalRayCount

        horizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1);
        verticalRaySpacing = bounds.size.x / (verticalRayCount - 1);




    }




    struct RaycastOrigins                               // struct określa przechowywanie wartości zmiennej, nie referencji jak w class
    {
        public Vector2 topLeft, topRight;
        public Vector2 bottomLeft, bottomRight;

    }

    public struct CollisionInfo
    {
        public bool above, below;
        public bool left, right;

        public void Reset ()
        {
            above = below = false;
            left = right = false;

        }
    }




}


