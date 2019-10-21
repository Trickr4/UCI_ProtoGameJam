using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Input : MonoBehaviour
{
    GameManager manager;
    Queue<Vector2> First_inputs = new Queue<Vector2>();
    Queue<Vector2> Second_inputs = new Queue<Vector2>();
    private float space;
    private float FloorHeight;
    private float JumpHeight;
    bool filled = false;
    public Rigidbody2D Player1_rb;
    public Rigidbody2D Player2_rb;

    int Player1_stamina = 3;

    int Player2_stamina = 2;
    [SerializeField] float speed = 10f;

    //Pass in the player object 
    public GameObject Player1;
    public GameObject Player2;

    Vector2 Direction1;
    Vector2 Direction2;

    Vector2 end1;
    Vector2 end2;

    [SerializeField] float JumpHeightMultiplier = 1.5f;

    float waitTime = 0f;
    void Start()
    {
        manager = GetComponent<GameManager>();
        Player1_rb = Player1.GetComponent<Rigidbody2D>();
        Player2_rb = Player2.GetComponent<Rigidbody2D>();
        space = ((13f / 16f));
        FloorHeight = Player1_rb.position.y;
        JumpHeight = Mathf.Abs(FloorHeight * JumpHeightMultiplier);
    }
    // Update is called once per frame
    void Update()
    {
        waitTime += Time.deltaTime;
        if (!filled)
        {
            Get_Input();
        }
        waitTime += Time.deltaTime;
        if (waitTime > .2f)
        {
            Flipper();

            // if both player queue is full, take the first input from both players
            // and create the first end point. Then set filled to true.
            if (!filled)
            {
                if (CheckIfPastEndPoint(Direction1, Player1_rb, end1) && CheckIfPastEndPoint(Direction2, Player2_rb, end2))
                {
                    Direction1 = Vector2.zero;
                    Direction2 = Vector2.zero;
                }
            }
            if (First_inputs.Count == 3 && Second_inputs.Count == 3)
            {
                //catch it dequeue on empty stack
                Debug.Log("Start");
                Direction1 = First_inputs.Dequeue();
                Direction2 = Second_inputs.Dequeue();
                end1 = NextInput(Direction1, Player1_rb);
                end2 = NextInput(Direction2, Player2_rb);
                filled = true;
            }

            //if player is queues are filled.
            else if (filled)
            {
                // Check if both player have reached their end points.
                if (CheckIfPastEndPoint(Direction1, Player1_rb, end1) && CheckIfPastEndPoint(Direction2, Player2_rb, end2))
                {

                    //Check if player queues are empty and set to filled.
                    if (filled && First_inputs.Count == 0 && Second_inputs.Count == 0)
                    {
                        Player1_rb.velocity = Vector2.zero;
                        Player2_rb.velocity = Vector2.zero;

                        if (Player1_rb.position.y > FloorHeight + .1)
                            Player1_rb.MovePosition(new Vector2(end1.x, FloorHeight));
                        if (Player2_rb.position.y > FloorHeight + .1)
                            Player2_rb.MovePosition(new Vector2(end2.x, FloorHeight));
                        filled = false;
                        end1 = NextInput(Direction1, Player1_rb);
                        end2 = NextInput(Direction2, Player2_rb);
                        Direction1 = Vector2.zero;
                        Direction2 = Vector2.zero;
                        waitTime = 0f;
                    }

                    if (First_inputs.Count != 0)
                    {
                        //Debug.Log(end1);
                        if (Direction1.y >= 0)
                            Player1_rb.position = end1;
                        Direction1 = First_inputs.Dequeue();
                        end1 = NextInput(Direction1, Player1_rb);
                    }
                    if (Second_inputs.Count != 0)
                    {
                        //Debug.Log(end2);
                        if (Direction2.y >= 0)
                            Player2_rb.position = end2;
                        Direction2 = Second_inputs.Dequeue();
                        end2 = NextInput(Direction2, Player2_rb);
                    }
                    //Log(Direction1.y);
                    //Check if both player are attacking.
                    if (Direction1.y == -1)
                    {
                        Attack(true);
                    }
                    if (Direction2.y == -1)
                    {
                        Attack(false);
                    }

                    //reset the wait timer.
                    waitTime = 0f;
                }


            }

            //Move only if both players are not moving.
            if (Player1_rb.velocity == Vector2.zero && Player2_rb.velocity == Vector2.zero)
            {
                //Time buffer for movement.

                Move(Direction1, Player1_rb);
                Move(Direction2, Player2_rb);

            }
        }
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

    void Flipper()
    {
        if (Player1.transform.position.x > Player2.transform.position.x)
            Player1.GetComponent<SpriteRenderer>().flipX = true;
        else
            Player1.GetComponent<SpriteRenderer>().flipX = false;
        if (Player2.transform.position.x > Player1.transform.position.x)
            Player2.GetComponent<SpriteRenderer>().flipX = true;
        else
            Player2.GetComponent<SpriteRenderer>().flipX = false;
    }

    //Stops the Player movement and changes the endpoint.
    Vector2 NextInput(Vector2 direction, Rigidbody2D rb)
    {
        Vector2 end = rb.position;
        rb.velocity = Vector2.zero;
        Debug.Log(Mathf.Abs((rb.position + (direction * space)).x));
        if (direction.x != 0)
            if (Mathf.Abs((rb.position + (direction * space)).x) < 8.4)
                end = rb.position + (direction * space);
            else
            {
                rb.MovePosition(new Vector2(direction.x * 8.3f, end.y));
                end = rb.position;
            }
        else if (direction.y > 0)
            end = rb.position + (direction * JumpHeight);
        return end;
    }

    //Moves the player on the x-axis
    void Move(Vector2 input, Rigidbody2D rb)
    {
        if (input.x != 0 && rb.position.x < 8.45)
            rb.velocity = input * speed;
        else if (input.y > 0)
            rb.velocity = input * speed;
        
    }

    //Checks if the Player GameObject passes or reach an endpoint on the x axis
    bool CheckIfPastEndPoint(Vector2 direction, Rigidbody2D rb, Vector2 end)
    {
        //Checks if the player is going right and its end point on the x-axis.
        if (direction.x >= 0 && rb.position.x >= end.x)
        {
            return true;
        }
        //Checks if the player is going left and its end point on the x-axis.
        else if (direction.x <= 0 && rb.position.x <= end.x)
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
        return false;
    }

    private void Attack(bool playerOneAttacked)
    {
        //Debug.Log("Player One Attacked = " + playerOneAttacked);
        //Debug.Log(AttackInRange());
        if (AttackInRange())
        {
            if (playerOneAttacked)
            {
                Debug.Log("player two takes damage");
                // send damage event to the GameManager
                manager.SetPlayerOneDamaged(10);
                if (manager.GetPlayerOneHealth() <= 0)
                {
                    First_inputs.Clear();
                    Second_inputs.Clear();
                }
                Debug.Log(manager.GetPlayerOneHealth());
            }
            else
            {
                Debug.Log("player one takes damage");
                // send damage event to the GameManager
                manager.SetPlayerTwoDamaged(10);
                if (manager.GetPlayerTwoHealth() <= 0)
                {
                    First_inputs.Clear();
                    Second_inputs.Clear();
                }
                Debug.Log(manager.GetPlayerTwoHealth());
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
        Debug.Log("Distance" + Mathf.Abs(Larger - Smaller));
        Debug.Log("Space" + (space + (space / 2)));
        return Mathf.Abs(Larger - Smaller) <= space + (space / 2);
    }
    /*
    public void OnTriggerEnter2D(Collider2D collider)
    {
        Debug.Log("here");
        if (collider)
        {
            Debug.Log("here");
            if (First_inputs.Count != 0)
            {
                //Debug.Log(end1);
                Player1_rb.position = end1;
                Direction1 = First_inputs.Dequeue();
                end1 = NextInput(Direction1, Player1_rb, end1);
            }
            else
            {
                Player1_rb.position = end1;
                Direction1 = Vector2.zero;
            }
            if (Second_inputs.Count != 0)
            {
                //Debug.Log(end2);
                Player2_rb.position = end2;
                Direction2 = Second_inputs.Dequeue();
                end2 = NextInput(Direction2, Player2_rb, end2);
            }
            else
            {
                Player2_rb.position = end2;
                Direction2 = Vector2.zero;
            }
            if (filled && First_inputs.Count == 0 && Second_inputs.Count == 0)
            {
                filled = false;
            }
        }
    }
    */
}