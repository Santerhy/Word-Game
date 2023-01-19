using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LetterShuffleButton : MonoBehaviour
{
    public GameManager gameManager;
    public float cooldownTimer, cooldownTimerMax;
    public Image fillImage;
    public bool filling = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (filling)
        {
            fillImage.fillAmount = cooldownTimer / cooldownTimerMax;
            cooldownTimer += Time.deltaTime;
            if (cooldownTimer >= cooldownTimerMax)
                filling = false;
        }
    }

    public void StartFilling()
    {
        if (!filling)
        {
            gameManager.RearraingeLetters();
            filling = true;
            fillImage.fillAmount= 0;
            cooldownTimer = 0f;
        }
    }
}
