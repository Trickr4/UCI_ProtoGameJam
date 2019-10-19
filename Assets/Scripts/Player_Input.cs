using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Input : MonoBehaviour
{
    Queue<KeyCode> First_inputs;
    Queue<KeyCode> Second_inputs;
    private int Screen_size;
    private int space;
    Vector2 force;
    GameObject Player1;
    GameObject Player2;

    // Start is called before the first frame update
    void Start()
    {
        Screen_size = Screen.width;
        space = (13 / 16) * Screen_size;
    }

    // Update is called once per frame
    void Update()
    {
        Get_Input();
        if (First_inputs.Count == 3 && Second_inputs.Count == 3)
            while (First_inputs.Count != 0 || Second_inputs.Count != 0)
            {
                Move(First_inputs, Player1);
                Move(Second_inputs, Player2);
            }
    }

    void Move(Queue<KeyCode> inputs, GameObject player )
    {
        KeyCode direction = inputs.Dequeue();
        if (direction == KeyCode.W || direction == KeyCode.UpArrow)
            player.transform.Translate(new Vector2(0, 1) * space );
        if (direction == KeyCode.S || direction == KeyCode.DownArrow)
            player.transform.Translate(new Vector2(0, -1) * space);
        if (direction == KeyCode.D || direction == KeyCode.RightArrow)
            player.transform.Translate(new Vector2(1, 0) * space);
        if (direction == KeyCode.A || direction == KeyCode.LeftArrow)
            player.transform.Translate(new Vector2(-1, 0) * space);
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
