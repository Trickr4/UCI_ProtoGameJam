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

    float waitTime = 0f;

    IEnumerator delayCoroutine;

    IEnumerator delay()
    {
        Debug.Log("here");
        yield return new WaitForSeconds(2.0f);
    }

    // Start is called before the first frame update
    void Start()
    {
        Player1_rb = Player1.GetComponent<Rigidbody2D>();
        Player2_rb = Player2.GetComponent<Rigidbody2D>();


        Screen_size = Screen.width;
        space = (13f/ 16f);

        delayCoroutine = delay();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        waitTime += Time.deltaTime;
        // Goes into statement if the list is full
        if (First_inputs.Count == 3 && Second_inputs.Count == 3)
        {
            //catch it dequeue on empty stack

            Direction1 = First_inputs.Dequeue();
            Direction2 = Second_inputs.Dequeue();
            end1 = Player1_rb.position + (Direction1 * 2);
            end2 = Player2_rb.position + (Direction2 * 2);
            filled = true;

            if (Direction1.y == -1)
            {
                Attack(true);
            }
            if (Direction2.y == -1)
            {
                Attack(false);
            }
        }

        //checks if its at the end postion of the x direction.
        if (waitTime > 2f)
        {
            if (CheckIfPastEndPoint(Direction1, Player1_rb, end1) && CheckIfPastEndPoint(Direction2, Player2_rb, end2))
            {
                if (First_inputs.Count != 0 && Second_inputs.Count != 0)
                {
                    if (Direction1.y == -1)
                    {
                        Attack(true);
                    }
                    if (Direction2.y == -1)
                    {
                        Attack(false);
                    }
                    Direction1 = First_inputs.Dequeue();
                    Direction2 = Second_inputs.Dequeue();
                    end1 = NextInput(Direction1, Player1_rb, end1);
                    end2 = NextInput(Direction2, Player2_rb, end2);

                    waitTime = 0f;
                }
                else if (First_inputs.Count == 0 && Second_inputs.Count == 0)
                {
                    if (Direction1.y == -1)
                    {
                        Attack(true);
                    }
                    if (Direction2.y == -1)
                    {
                        Attack(false);
                    }
                    Player1_rb.velocity = Vector2.zero;
                    Player2_rb.velocity = Vector2.zero;

                    filled = false;
                }
            }
        }
        if (!filled)
        {
            Get_Input();
        }
        if (filled)
        {

            
            if (Player1_rb.velocity == Vector2.zero && Player2_rb.velocity == Vector2.zero)
            {
                if (waitTime > 2f)
                {
                    Move(Direction1, Player1_rb);
                    Move(Direction2, Player2_rb);
                }
            }
        }
    }

    bool CheckIfPastEndPoint(Vector2 direction, Rigidbody2D rb, Vector2 end)
    {
        
        if (direction.x >= 0 && rb.position.x >= end.x && rb.position.x >= end.x)
        {
            return true;
        }
        else if (direction.x <= 0 && rb.position.x <= end.x && rb.position.x <= end.x)
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

    private void Attack(bool playerOneAttacked)
    {
        Debug.Log("Player One Attacked = " + playerOneAttacked);

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

    private bool AttackInRange()
    {
        return Mathf.Abs(Player1.transform.position.x) - Mathf.Abs(Player2.transform.position.x) <= space;
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
