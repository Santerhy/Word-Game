using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class LetterBox : MonoBehaviour
{
    public char value;
    public bool clickable = true;
    public bool moving = false;
    public GameManager gameManager;
    public TMP_Text textField;
    public float moveSpeed;
    public Vector2 startPos;
    public Vector2 targetPos;
    // Start is called before the first frame update
    void Start()
    {
        //gameManager = GetComponent<GameManager>();
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (moving)
        {
            transform.position = Vector2.Lerp(transform.position, targetPos, moveSpeed * Time.deltaTime);
            float distance = Vector2.Distance(transform.position, targetPos);
            if (distance < 0.1f)
                moving= false;
        }
    }

    public void Clicked()
    {
        if (clickable)
        {
            targetPos = gameManager.LetterClicked(this.gameObject);
            clickable = false;
            moving = true;

        }
    }

    public void ResetPosition()
    {
        transform.position = startPos;
    }

    
}
