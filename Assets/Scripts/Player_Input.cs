using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Input : MonoBehaviour
{
    //Store Player input into a queue
    Queue<Vector2> First_inputs = new Queue<Vector2>();
    Queue<Vector2> Second_inputs = new Queue<Vector2>();

    //Max Space player is allowed to move per turn.
    private float space;

    //Store bool of the status of both Player queue being full.
    bool filled = false;

    //Players RigidBody
    Rigidbody2D Player1_rb;
    Rigidbody2D Player2_rb;

    //Stores player movement speed.
    [SerializeField] float speed = 13f/16f;

    //Pass in the player object 
    [SerializeField] GameObject Player1;
    [SerializeField] GameObject Player2;

    //Stores the Players Directions.
    Vector2 Direction1;
    Vector2 Direction2;

    //Stores the Players EndPoints.
    Vector2 end1;
    Vector2 end2;

    float waitTime = 0f;

    // Start is called before the first frame update
    void Start()
    {
        Player1_rb = Player1.GetComponent<Rigidbody2D>();
        Player2_rb = Player2.GetComponent<Rigidbody2D>();

        space = (13f / 16f);
    }

    // Update is called once per frame
    void Update()
    {

        //Progress the timer
        waitTime += Time.deltaTime;

        // if both player queue is full, take the first input from both players
        // and create the first end point. Then set filled to true.
        if (First_inputs.Count == 3 && Second_inputs.Count == 3)
        {
            //catch it dequeue on empty stack
            Debug.Log("Start");
            Direction1 = First_inputs.Dequeue();
            Direction2 = Second_inputs.Dequeue();
            end1 = Player1_rb.position + (Direction1 * space);
            end2 = Player2_rb.position + (Direction2 * space);
            filled = true;

            
            
        }
        
        //if player queue is not full.
        //Get players input until they are full.
        //Check if player is at an endpoint and stop movement.
        if (!filled)
        {
            Get_Input();
            if (CheckIfPastEndPoint(Direction1, Player1_rb, end1) && CheckIfPastEndPoint(Direction2, Player2_rb, end2))
            {
                Direction1 = Vector2.zero;
                Direction2 = Vector2.zero;

            }
        }

        //if player is queues are filled.
        else if (filled)
        {
            // Check if both player have reached their end points.
            if (CheckIfPastEndPoint(Direction1, Player1_rb, end1) && CheckIfPastEndPoint(Direction2, Player2_rb, end2))
            {
                if (filled)
                {
                    //Check if both player are attacking.
                    if (Direction1.y >= -1)
                    {
                        Attack(true);
                    }
                    if (Direction2.y >= -1)
                    {
                        Attack(false);
                    }

                    //Check if player queues are empty and set to filled.
                    if (filled && First_inputs.Count == 0 && Second_inputs.Count == 0)
                    {
                        Player1_rb.velocity = Vector2.zero;
                        Player2_rb.velocity = Vector2.zero;
                        filled = false;
                    }
                    if (First_inputs.Count != 0)
                    {
                        //Debug.Log(end1);
                        if (Direction1.y == 0)
                            Player1_rb.position = end1;
                        Direction1 = First_inputs.Dequeue();
                        end1 = NextInput(Direction1, Player1_rb, end1);
                    }
                    if (Second_inputs.Count != 0)
                    {
                        //Debug.Log(end2);
                        if (Direction2.y == 0)
                            Player2_rb.position = end2;
                        Direction2 = Second_inputs.Dequeue();
                        end2 = NextInput(Direction2, Player2_rb, end2);
                    }

                    //reset the wait timer.
                    waitTime = 0f;
                }
            }

        }

        //Move only if both players are not moving.
        if (Player1_rb.velocity == Vector2.zero && Player2_rb.velocity == Vector2.zero)
        {
            //Time buffer for movement.
            if (waitTime > 2f)
            {
                Move(Direction1, Player1_rb);
                Move(Direction2, Player2_rb);
            }
        }
            
        
    }

    //Checks if the Player GameObject passes or reach an endpoint on the x axis
    bool CheckIfPastEndPoint(Vector2 direction, Rigidbody2D rb, Vector2 end)
    {
        //Checks if the player is going right and its end point on the x-axis.
        if (direction.x > 0 && rb.position.x >= end.x)
        {
            /*
            Debug.Log("RIght " + direction.x);
            Debug.Log(rb.position.x);
            Debug.Log(end);
            */
            return true;
        }
        //Checks if the player is going left and its end point on the x-axis.
        else if (direction.x < 0 && rb.position.x <= end.x)
        {
            /*
            Debug.Log("Left " + direction.x);
            Debug.Log(rb.position.x);
            Debug.Log(end);
            */
            return true;
        }
        //Check if the direction is down and the x-axis endpoint is the same as the
        //players position.
        else if (direction.y == -1 && rb.position.x == end.x)
        {
            return true;
        }
        return false;
    }

    //Stops the Player movement and changes the endpoint.
    Vector2 NextInput(Vector2 direction, Rigidbody2D rb, Vector2 end)
    {
        rb.velocity = Vector2.zero;
        end = rb.position + (direction * space);
        
        return end;
    }

    //Moves the player on the x-axis
    void Move(Vector2 input, Rigidbody2D rb)
    {
        if (input.x != 0)
            rb.velocity = input * speed;

    }

    //Player is attacked if they are in range of the variable space,
    //And then sneds this event to the GameManager
    private void Attack(bool playerOneAttacked)
    {
        //Debug.Log("Player One Attacked = " + playerOneAttacked);

        if (AttackInRange())
        {
            if (playerOneAttacked)
            {
                Debug.Log("player two takes damage");
                // send damage event to the GameManager
            }
            else
            {
                Debug.Log("player one takes damage");
                // send damage event to the GameManager
            }
        }
    }

    //Caluclates the distance between the Players and send a bool if they
    //are in range.
    private bool AttackInRange()
    {
        float Larger = Mathf.Max(Player1.transform.position.x, Player2.transform.position.x);
        float Smaller = Mathf.Min(Player1.transform.position.x, Player2.transform.position.x);
        //Debug.Log(Larger - Smaller);
        return Larger - Smaller <= space;
    }

    //Gets the player input and store them in their respective player queue.
    void Get_Input()
    {
        //WASD for player 1. UP DOWN LEFT RIGHT for player 2.
        if (Input.GetKeyDown(KeyCode.W))
            if (First_inputs.Count < 3)
                First_inputs.Enqueue(Vector2.up);
        if (Input.GetKeyDown(KeyCode.S))
            if (First_inputs.Count < 3)
                First_inputs.Enqueue(Vector2.down);
        if (Input.GetKeyDown(KeyCode.A))
            if (First_inputs.Count < 3)
                First_inputs.Enqueue(Vector2.left);
        if (Input.GetKeyDown(KeyCode.D))
            if (First_inputs.Count < 3)
                First_inputs.Enqueue(Vector2.right);

        if (Input.GetKeyDown(KeyCode.UpArrow))
            if (Second_inputs.Count < 3)
                Second_inputs.Enqueue(Vector2.up);
        if (Input.GetKeyDown(KeyCode.DownArrow))
            if (Second_inputs.Count < 3)
                Second_inputs.Enqueue(Vector2.down);
        if (Input.GetKeyDown(KeyCode.LeftArrow))
            if (Second_inputs.Count < 3)
                Second_inputs.Enqueue(Vector2.left);
        if (Input.GetKeyDown(KeyCode.RightArrow))
            if (Second_inputs.Count < 3)
                Second_inputs.Enqueue(Vector2.right);
    }
}