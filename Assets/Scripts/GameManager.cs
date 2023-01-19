using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    //public TextAsset jsonFile;
    public List<char> rightWord;            //Used for storing the correct answer
    public List<char> playerTry = new List<char>();     //Used for storing the player current guess
    public int currentWordCounter;
    public GameObject letterBoxPrefab;
    public GameObject fillAreaPrefab;
    public TMP_Text hintText;
    public Transform fillAreaStart;
    public List<Transform> fillArea;
    public List<GameObject> letterBoxes = new List<GameObject>();
    public Canvas canvas;
    public int letterCounter = 0;

    [System.Serializable]
    public class Word
    {
        public string letters;
        public string hint;
    }
    
    [System.Serializable]
    public class WordList
    {
        public Word[] words;
    }

    public WordList wordList = new WordList();

    // Start is called before the first frame update
    void Start()
    {
  
        TextAsset file = Resources.Load("WordList") as TextAsset;
        string content = file.ToString();
        Debug.Log(content);
        //wordList = JsonUtility.FromJson<WordList>(content);
        wordList = JsonConvert.DeserializeObject<WordList>(content);

        ApplyRightAnswer();
        GenerateWordObjects();
        GenerateFillArea();
    }

    public void GenerateWordObjects()
    {
        Debug.Log("size " + wordList.words.Length);
        //For each letter in the word, instantiate new letterBox with letter

        for (int i = 0; i < wordList.words[currentWordCounter].letters.Length; i++)
        {
            GameObject g = Instantiate(letterBoxPrefab);
            g.GetComponent<LetterBox>().textField.text = wordList.words[currentWordCounter].letters[i].ToString();
            g.GetComponent<LetterBox>().value = wordList.words[currentWordCounter].letters[i];
            g.GetComponent<LetterBox>().gameManager = this;
            g.transform.SetParent(canvas.transform, false);
            letterBoxes.Add(g);
        }
        //Shuffle letters order
        ShuffleList(letterBoxes);

        //Place letters
        for(int i = 0; i < letterBoxes.Count; i++)
        {
            Vector2 pos = letterBoxes[i].transform.position;
            pos.x += i * 100f;
            letterBoxes[i].transform.position = pos;
        }
    }

    public void GenerateFillArea()
    {
        //Replace with randomly chosen word
        int wordCount = wordList.words[currentWordCounter].letters.Length;
        Vector2 startPos = fillAreaStart.transform.position;

        for (int i = 1; i < wordCount; i++)
        {
            GameObject g = Instantiate(fillAreaPrefab);
            startPos.x += 105f;
            g.transform.position = startPos;
            fillArea.Add(g.transform);
            g.transform.SetParent(canvas.transform);
        }
    }

    //Set every letter from the right answer to rightWord list
    public void ApplyRightAnswer()
    {
        string word = wordList.words[currentWordCounter].letters;
        for (int i = 0; i < word.Length; i++)
        {
            rightWord.Add(word[i]);
        }
    }

    void ShuffleList(List<GameObject> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            GameObject temp = list[i];
            int randomIndex = Random.Range(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }

    public void NewWord()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Vector2 LetterClicked(GameObject go)
    {
            Vector2 retVal = fillArea[letterCounter].position;
            letterCounter++;
            playerTry.Add(go.GetComponent<LetterBox>().value);

        if (letterCounter >= wordList.words[currentWordCounter].letters.Length)
            CheckLetterOrder();

        return retVal;
    }

    public void ResetLetterPositions()
    {
        letterCounter = 0;
        playerTry.Clear();
        foreach (GameObject g in letterBoxes)
        {
            g.GetComponent<LetterBox>().ResetPosition();
            g.GetComponent<LetterBox>().clickable = true;
            g.GetComponent<LetterBox>().moving = false;
        }
    }

    public void CheckLetterOrder()
    {
        bool correctOrder = true;
        for (int i = 0; i < rightWord.Count; i++)
        {
            if (!rightWord[i].Equals(playerTry[i]))
                correctOrder = false;
        }
        string result = correctOrder ? "Right order" : "Incorrect order";
        Debug.Log(result);
    }

    public void ShowHint()
    {
        hintText.gameObject.SetActive(true);
        hintText.text = wordList.words[currentWordCounter].hint;
    }
}
