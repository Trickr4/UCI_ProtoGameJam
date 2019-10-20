using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Input : MonoBehaviour
{
    Queue<Vector2> First_inputs = new Queue<Vector2>();
    Queue<Vector2> Second_inputs= new Queue<Vector2>();
    private int Screen_size;
    private float space;
    bool filled = false;
    Rigidbody2D Player1_rb;
    Rigidbody2D Player2_rb;

    [SerializeField] float speed = 10f;

    //Pass in the player object 
    [SerializeField] GameObject Player1;
    [SerializeField] GameObject Player2;

    Vector2 Direction1;
    Vector2 Direction2;

    Vector2 end1;
    Vector2 end2;

    float waitTime = 3;

  

    // Start is called before the first frame update
    void Start()
    {
        Player1_rb = Player1.GetComponent<Rigidbody2D>();
        Player2_rb = Player2.GetComponent<Rigidbody2D>();


        Screen_size = Screen.width;
        space = Screen_size - ((13f/ 16f) * Screen_size);
    }

    // Update is called once per frame
    void Update()
    {
        if (First_inputs.Count == 3 && Second_inputs.Count == 3)
        {
            //catch it dequeue on empty stack

            Direction1 = First_inputs.Dequeue();
            Direction2 = Second_inputs.Dequeue();
            end1 = Player1_rb.position + (Direction1 * 2);
            end2 = Player2_rb.position + (Direction2 * 2);
            filled = true;
        }
        if (!filled)
        {
            Get_Input();
        }
        if (filled)
        {
            
            if( CheckIfPastEndPoint(Direction1, Player1_rb, end1) && CheckIfPastEndPoint(Direction2, Player2_rb, end2) )
            {
                Direction1 = First_inputs.Dequeue();
                Direction2 = Second_inputs.Dequeue();
                end1 = NextInput(Direction1, Player1_rb, end1);
                end2 = NextInput(Direction2, Player2_rb, end2);
            }

            else if (First_inputs.Count == 0 && Second_inputs.Count == 0)
                filled = false;
            else
            {
                waitTime += Time.deltaTime;
                if (waitTime > 10)
                {
                    Move(Direction1, Player1_rb);
                    Move(Direction2, Player2_rb);
                }
            }
        }
    }

    bool CheckIfPastEndPoint(Vector2 direction, Rigidbody2D rb, Vector2 end)
    {
        
        if (direction.x > 0 && rb.position.x >= end.x && rb.position.x >= end.x)
        {
            return true;
        }
        else if (direction.x < 0 && rb.position.x <= end.x && rb.position.x <= end.x)
        {
            return true;
        }
        return false;
    }

    Vector2 NextInput(Vector2 direction, Rigidbody2D rb, Vector2 end)
    {
        rb.velocity = Vector2.zero;
        end = rb.position + (direction * 2);
        return end;
    }


    void Move(Vector2 input, Rigidbody2D rb)
    {
        rb.velocity = input * speed;
        
    }

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
