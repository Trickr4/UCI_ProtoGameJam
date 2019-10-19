using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Input : MonoBehaviour
{
    Queue<KeyCode> First_inputs = new Queue<KeyCode>();
    Queue<KeyCode> Second_inputs= new Queue<KeyCode>();
    private int Screen_size;
    private float space;

    IEnumerator timer;

    Rigidbody2D Player1_rb;
    Rigidbody2D Player2_rb;
    [SerializeField] float speed = 10f;
    //Pass in the player object 
    [SerializeField] GameObject Player1;
    [SerializeField] GameObject Player2;

    // Start is called before the first frame update
    void Start()
    {
        Player1_rb = Player1.GetComponent<Rigidbody2D>();
        Player2_rb = Player2.GetComponent<Rigidbody2D>();


        Screen_size = Screen.width;
        space = Screen_size - ((13f/ 16f) * Screen_size);
    }
/*
    if (rb.position.x<rb.position.x + space)
    {
        Debug.Log("heh");
        rb.velocity = new Vector2(1, 0) * 0;
        break;
    }
*/
// Update is called once per frame
    void Update()
    {

        Get_Input();
        if (First_inputs.Count == 3 && Second_inputs.Count == 3)
            while (First_inputs.Count != 0 || Second_inputs.Count != 0)
            {
                Move(First_inputs, Player1_rb);
                timer = WaitforStop(Player1_rb);
                Move(Second_inputs, Player2_rb);
                timer = WaitforStop(Player1_rb);
                Player2_rb.velocity = new Vector2(1, 0) * 0;
            }
    }

    IEnumerator WaitforStop(Rigidbody2D rb)
    {
        Player1_rb.velocity = new Vector2(1, 0) * 0;
        yield return new WaitForSeconds(0.5f);
    }

    void Move(Queue<KeyCode> inputs, Rigidbody2D rb )
    {
        KeyCode direction = inputs.Dequeue();
        if (direction == KeyCode.W || direction == KeyCode.UpArrow) 
           rb.velocity = new Vector2(0, 1) * speed;
        if (direction == KeyCode.S || direction == KeyCode.DownArrow)
            rb.velocity = new Vector2(0, -1) * speed;
        if (direction == KeyCode.D || direction == KeyCode.RightArrow)
        {
            rb.velocity = new Vector2(1, 0) * speed;
        }
        if (direction == KeyCode.A || direction == KeyCode.LeftArrow)
            rb.velocity = new Vector2(-1, 0) * speed;
        
    }

    void Get_Input()
    {
        //WASD for player 1. UP DOWN LEFT RIGHT for player 2.
        if (Input.GetKeyDown(KeyCode.W))
            if (First_inputs.Count < 3)
                First_inputs.Enqueue(KeyCode.W);
        if (Input.GetKeyDown(KeyCode.S))
            if (First_inputs.Count < 3)
                First_inputs.Enqueue(KeyCode.S);
        if (Input.GetKeyDown(KeyCode.A)) 
            if (First_inputs.Count < 3)
                First_inputs.Enqueue(KeyCode.A);
        if (Input.GetKeyDown(KeyCode.D))
            if (First_inputs.Count < 3)
                First_inputs.Enqueue(KeyCode.D);

        if (Input.GetKeyDown(KeyCode.UpArrow))
            if (Second_inputs.Count < 3)
                Second_inputs.Enqueue(KeyCode.UpArrow);
        if (Input.GetKeyDown(KeyCode.DownArrow))
            if (Second_inputs.Count < 3)
                Second_inputs.Enqueue(KeyCode.DownArrow);
        if (Input.GetKeyDown(KeyCode.LeftArrow))
            if (Second_inputs.Count < 3)
                Second_inputs.Enqueue(KeyCode.LeftArrow);
        if (Input.GetKeyDown(KeyCode.RightArrow))
            if (Second_inputs.Count < 3)
                Second_inputs.Enqueue(KeyCode.RightArrow);
    }
}
