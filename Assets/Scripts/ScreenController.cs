using TMPro;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR;
using System;
using UnityEngine.Audio;

/*Note: The integer of the puzzle type enum should match the answer list 
add order in BuildAnswers(). IE expressionAnswers should be added to
allAnswers first, objectAnswers second, etc. in BuildAnswers()*/
enum PuzzleType
{
    ExpressionType = 0, //Expression recognition
    ObjectType = 1, //Object recognition
    EasyMathType = 2, //Easy arithmetic
    MediumAnimalType = 3, //Animal recognition
    MediumMathType = 4, //Algebra simplification
    MediumFlagType = 5, //Medium flags
    HardFlagType = 6, //Hard Flags
    HardMathType = 7, //Hard math
    HardAnimalType = 8, //Hard animal trivia
    
}
static class Constants
{
    //constants go here
}
public class ScreenController : MonoBehaviour
{
    public TextMeshProUGUI promptString; //The text at the top of the screen
    public TextMeshProUGUI numCorrectText;

    //Placeholders for image/text answers
    public GameObject upOptionImage; //The image answer in the up position
    public GameObject rightOptionImage; //The image answer in the right position
    public GameObject downOptionImage; //The image answer in the down position
    public GameObject leftOptionImage; //The image answer in the left position

    public GameObject upOptionText; //The Text answer in the up position
    public GameObject rightOptionText; //The text answer in the right position
    public GameObject downOptionText; //The text answer in the down position
    public GameObject leftOptionText; //The test answer in the left position

    public AudioMixerGroup MyMixer;

    //Difficulties
    public bool easyModeEnabled;
    public bool mediumModeEnabled;
    public bool hardModeEnabled = true;

    //Sounds
    public AudioSource rightSound;
    public AudioSource wrongSound;

    //A list of answers to aid the creation of each corresponding puzzle type
    private List<Answer> expressionAnswers;
    private List<Answer> objectAnswers;
    private List<Answer> easyMathAnswers;

    private List<Answer> mediumMathAnswers;
    private List<Answer> mediumAnimalAnswers;
    private List<Answer> mediumFlagAnswers;

    private List<Answer> hardFlagAnswers;
    private List<Answer> hardMathAnswers;
    private List<Answer> hardAnimalAnswers;


    private List<List<Answer>> allAnswerLists;

    private GameObject timerScript;

    //The final list of puzzles for the game
    private List<Puzzle> puzzleList;

    private int currentPuzzleIndex; //The current position in the main puzzle list

    private int numCorrect; // the number of correct user answers 

    public AudioClip right;
    public AudioClip wrong;

    System.Random random = new System.Random();
    
    //debug variables
    private int totalPuzzles = 0;
    // Start is called before the first frame update
    void Start()
    {
        timerScript = GameObject.FindGameObjectWithTag("TimerScript");
        Debug.Log("Starting phone script.");

        InitializeAnswerLists();

        //set difficulty bools
        //easyModeEnabled = false;
        //mediumModeEnabled = false;
        //hardModeEnabled = false;

        //hardModeEnabled = true;

        BuildAnswers();
        BuildMainPuzzleList();
        
        //BuildPuzzlesList(); 
        currentPuzzleIndex = -1;
        DisplayNextPuzzle(-1);
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.UpArrow))
        //{
        //    DisplayNextPuzzle(0);
        //}
        //else if (Input.GetKeyDown(KeyCode.DownArrow))
        //{
        //    DisplayNextPuzzle(1);
        //}
        //else if (Input.GetKeyDown(KeyCode.RightArrow))
        //{
        //    DisplayNextPuzzle(2);
        //}
        //else if(Input.GetKeyDown(KeyCode.LeftArrow))
        //{
        //    DisplayNextPuzzle(3);
        //}

        //If using VR
        if (SteamVR_Actions._default.UpClick.GetStateDown(SteamVR_Input_Sources.RightHand))
        {
            DisplayNextPuzzle(0);
        }
        else if (SteamVR_Actions._default.DownClick.GetStateDown(SteamVR_Input_Sources.RightHand))
        {
            DisplayNextPuzzle(1);
        }
        else if (SteamVR_Actions._default.RightClick.GetStateDown(SteamVR_Input_Sources.RightHand))

        {
            DisplayNextPuzzle(2);
        }
        else if (SteamVR_Actions._default.LeftClick.GetStateDown(SteamVR_Input_Sources.RightHand))
        {
            DisplayNextPuzzle(3);
        }
    }

    // Takes in an int that represents up, down, left or right. -1 is passed at the start and when the user runs out of time
    public void DisplayNextPuzzle(int userAnswer)
    {
        Debug.Log("Displaying next puzzle after user answer: " + userAnswer);

        if (currentPuzzleIndex < puzzleList.Count - 1)
        {
            List<Answer> answerList = null;
            if(userAnswer >= 0 && puzzleList[currentPuzzleIndex].correctAnswerPosition == userAnswer)
            {
                numCorrect++;
                rightSound.PlayOneShot(right);
            }
            else if(userAnswer >= 0 && puzzleList[currentPuzzleIndex].correctAnswerPosition != userAnswer)
            {
                if(numCorrect > 0)
                   numCorrect--;
                wrongSound.PlayOneShot(wrong);
            }
            timerScript.GetComponent<Timer>().resetTimer();
            currentPuzzleIndex++;

            ////Determine which answer list to work 
            answerList = allAnswerLists[(int)puzzleList[currentPuzzleIndex].puzzleType];

            Puzzle puzzle = puzzleList[currentPuzzleIndex];

            //Update prompt
            promptString.SetText(answerList[puzzle.answerIndex].promptString);
            numCorrectText.SetText("Number correct: \n" + numCorrect);
            Debug.Log("Answer index: " + puzzle.answerIndex);
            totalPuzzles++;
            Debug.Log("Total puzzles: " + totalPuzzles);
            Debug.Log("Correct answer position: " + puzzleList[currentPuzzleIndex].correctAnswerPosition);

            HandleOptionsUpdating(puzzleList[currentPuzzleIndex].puzzleType,
                answerList, puzzle);
            
        }
        else
        {
            //All puzzles have been visited, regenerate new puzzle list.
            InitializeAnswerLists();
            BuildAnswers();
            BuildMainPuzzleList();

            currentPuzzleIndex = -1;
            DisplayNextPuzzle(-1);
        }

    }

    void HandleOptionsUpdating(PuzzleType puzzleType, List<Answer> answerList, Puzzle puzzle)
    {
        upOptionImage.SetActive(false);
        downOptionImage.SetActive(false);
        rightOptionImage.SetActive(false);
        leftOptionImage.SetActive(false);
        upOptionText.SetActive(false);
        downOptionText.SetActive(false);
        rightOptionText.SetActive(false);
        leftOptionText.SetActive(false);

        if (puzzleType == PuzzleType.MediumAnimalType 
            || puzzleType ==PuzzleType.ExpressionType
            || puzzleType == PuzzleType.ObjectType
            || puzzleType == PuzzleType.HardFlagType
            || puzzleType == PuzzleType.HardAnimalType
            || puzzleType == PuzzleType.MediumFlagType)
        {
            //puzzle is image based
            //Update OptionImages
            upOptionImage.SetActive(true);
            downOptionImage.SetActive(true);
            rightOptionImage.SetActive(true);
            leftOptionImage.SetActive(true);
            upOptionImage.GetComponent<Image>().sprite = answerList[puzzle.options[0]].imageAnswer;
            downOptionImage.GetComponent<Image>().sprite = answerList[puzzle.options[1]].imageAnswer;
            rightOptionImage.GetComponent<Image>().sprite = answerList[puzzle.options[2]].imageAnswer;
            leftOptionImage.GetComponent<Image>().sprite = answerList[puzzle.options[3]].imageAnswer;
        }
        else
        {
            //puzzle is text based
            //update option text
            upOptionText.SetActive(true);
            downOptionText.SetActive(true);
            rightOptionText.SetActive(true);
            leftOptionText.SetActive(true);
            upOptionText.GetComponent<TextMeshProUGUI>().SetText(answerList[puzzle.options[0]].textAnswer);
            downOptionText.GetComponent<TextMeshProUGUI>().SetText(answerList[puzzle.options[1]].textAnswer); 
            rightOptionText.GetComponent<TextMeshProUGUI>().SetText(answerList[puzzle.options[2]].textAnswer);
            leftOptionText.GetComponent<TextMeshProUGUI>().SetText(answerList[puzzle.options[3]].textAnswer); 

        }
    }
    void InitializeAnswerLists()
    {
        expressionAnswers = new List<Answer>();
        objectAnswers = new List<Answer>();
        easyMathAnswers = new List<Answer>();

        mediumMathAnswers = new List<Answer>();
        mediumAnimalAnswers = new List<Answer>();
        mediumFlagAnswers = new List<Answer>();

        hardFlagAnswers = new List<Answer>();
        hardMathAnswers = new List<Answer>();
        hardAnimalAnswers = new List<Answer>();
    }
    //Creates every prompt/answer pair and add them to the appropriate answer list
    void BuildAnswers()
    {
        //Sound puzzles?
        //Sequence puzzles?

        allAnswerLists = new List<List<Answer>>();
        Debug.Log("Building answers [BuildAnswers()].");

        string promptString = "Select the image for: ";

        //Build expressions answers
        string expressionsPath = "PuzzleAssets/Expressions/";
        expressionAnswers.Add(new Answer(promptString + "Crazy", GenerateSprite(expressionsPath + "Crazy")));
        expressionAnswers.Add(new Answer(promptString + "Crying", GenerateSprite(expressionsPath + "Crying")));
        expressionAnswers.Add(new Answer(promptString + "Drooling",  GenerateSprite(expressionsPath + "Drooling")));
        expressionAnswers.Add(new Answer(promptString + "Happy",  GenerateSprite(expressionsPath + "Happy")));
        expressionAnswers.Add(new Answer(promptString + "Kissy",  GenerateSprite(expressionsPath + "Kissy")));
        expressionAnswers.Add(new Answer(promptString + "Unhappy",  GenerateSprite(expressionsPath + "Unhappy")));
        expressionAnswers.Add(new Answer(promptString + "Sick",  GenerateSprite(expressionsPath + "Sick")));
        expressionAnswers.Add(new Answer(promptString + "Sleeping",  GenerateSprite(expressionsPath + "Sleeping")));
        expressionAnswers.Add(new Answer(promptString + "Surprised",  GenerateSprite(expressionsPath + "Surprised")));
        expressionAnswers.Add(new Answer(promptString + "Winking",  GenerateSprite(expressionsPath + "Winking")));
        allAnswerLists.Add(expressionAnswers);

        //build object answers
        string objectsPath = "PuzzleAssets/Objects/";
        objectAnswers.Add(new Answer(promptString + "Bell", GenerateSprite(objectsPath + "Bell")));
        objectAnswers.Add(new Answer(promptString + "Bone", GenerateSprite(objectsPath + "Bone")));
        objectAnswers.Add(new Answer(promptString + "Bread", GenerateSprite(objectsPath + "Bread")));
        objectAnswers.Add(new Answer(promptString + "Cookie", GenerateSprite(objectsPath + "Cookie")));
        objectAnswers.Add(new Answer(promptString + "Flashlight", GenerateSprite(objectsPath + "Flashlight")));
        objectAnswers.Add(new Answer(promptString + "Present", GenerateSprite(objectsPath + "Present")));
        objectAnswers.Add(new Answer(promptString + "Scissors", GenerateSprite(objectsPath + "Scissors")));
        objectAnswers.Add(new Answer(promptString + "Shoe", GenerateSprite(objectsPath + "Shoe")));
        objectAnswers.Add(new Answer(promptString + "Skateboard", GenerateSprite(objectsPath + "Skateboard")));
        objectAnswers.Add(new Answer(promptString + "Sponge", GenerateSprite(objectsPath + "Sponge")));
        allAnswerLists.Add(objectAnswers);

        //build math answers
        easyMathAnswers.Add(new Answer("29 + 41 =", "70"));
        easyMathAnswers.Add(new Answer("20 + 33 =", "53"));
        easyMathAnswers.Add(new Answer("38 + 43 =", "81"));
        easyMathAnswers.Add(new Answer("14 + 32 =", "46"));
        easyMathAnswers.Add(new Answer("31 + 36 =", "67"));
        easyMathAnswers.Add(new Answer("43 - 39 =", "4"));
        easyMathAnswers.Add(new Answer("46 - 33 =", "13"));
        easyMathAnswers.Add(new Answer("33 - 36 =", "-3"));
        easyMathAnswers.Add(new Answer("34 - 25 =", "9"));
        easyMathAnswers.Add(new Answer("50 - 50 =", "0"));
        allAnswerLists.Add(easyMathAnswers);

        //build animal answers
        string animalsPath = "PuzzleAssets/Animals/";
        mediumAnimalAnswers.Add(new Answer(promptString + "Bat", GenerateSprite(animalsPath + "Bat")));
        mediumAnimalAnswers.Add(new Answer(promptString + "Cat", GenerateSprite(animalsPath + "Cat")));
        mediumAnimalAnswers.Add(new Answer(promptString + "Cow", GenerateSprite(animalsPath + "Cow")));
        mediumAnimalAnswers.Add(new Answer(promptString + "Elephant", GenerateSprite(animalsPath + "Elephant")));
        mediumAnimalAnswers.Add(new Answer(promptString + "Giraffe", GenerateSprite(animalsPath + "Giraffe")));
        mediumAnimalAnswers.Add(new Answer(promptString + "Kangaroo", GenerateSprite(animalsPath + "Kangaroo")));
        mediumAnimalAnswers.Add(new Answer(promptString + "Monkey", GenerateSprite(animalsPath + "Monkey")));
        mediumAnimalAnswers.Add(new Answer(promptString + "Pig", GenerateSprite(animalsPath + "Pig")));
        mediumAnimalAnswers.Add(new Answer(promptString + "Rabbit", GenerateSprite(animalsPath + "Rabbit")));
        mediumAnimalAnswers.Add(new Answer(promptString + "Zebra", GenerateSprite(animalsPath + "Zebra")));
        allAnswerLists.Add(mediumAnimalAnswers);

        //build math answers
        mediumMathAnswers.Add(new Answer("6x - 2x + 3 + 2 =", "4x + 5"));
        mediumMathAnswers.Add(new Answer("2 * (3x + 4) =", "6x + 8"));
        mediumMathAnswers.Add(new Answer("x + 4x - 2", "5x - 2"));
        mediumMathAnswers.Add(new Answer("3x - 3x + 2 =", "2"));
        mediumMathAnswers.Add(new Answer("5x - 4 - 4 + 5 =", "5x + 5"));
        mediumMathAnswers.Add(new Answer("2x + 5x - 7 + 1 =", "7x - 6"));
        mediumMathAnswers.Add(new Answer("x - 3x + 4 =", "-2x + 4"));
        mediumMathAnswers.Add(new Answer("3x - 7 + 9x =", "12x - 7"));
        mediumMathAnswers.Add(new Answer("7x - 2 + 5 =", "7x + 3"));
        mediumMathAnswers.Add(new Answer("6x - 6x + 2 - 2 =", "0"));
        allAnswerLists.Add(mediumMathAnswers);

        //build medium flag answers
        string mediumFlagsPath = "PuzzleAssets/MediumFlags/";
        string flagPromptString = "Select the image for the flag of: ";
        mediumFlagAnswers.Add(new Answer(flagPromptString + "Australia", GenerateSprite(mediumFlagsPath + "Australia")));
        mediumFlagAnswers.Add(new Answer(flagPromptString + "Canada", GenerateSprite(mediumFlagsPath + "Canada")));
        mediumFlagAnswers.Add(new Answer(flagPromptString + "China", GenerateSprite(mediumFlagsPath + "China")));
        mediumFlagAnswers.Add(new Answer(flagPromptString + "France", GenerateSprite(mediumFlagsPath + "France")));
        mediumFlagAnswers.Add(new Answer(flagPromptString + "Germany", GenerateSprite(mediumFlagsPath + "Germany")));
        mediumFlagAnswers.Add(new Answer(flagPromptString + "Italy", GenerateSprite(mediumFlagsPath + "Italy")));
        mediumFlagAnswers.Add(new Answer(flagPromptString + "Japan", GenerateSprite(mediumFlagsPath + "Japan")));
        mediumFlagAnswers.Add(new Answer(flagPromptString + "Mexico", GenerateSprite(mediumFlagsPath + "Mexico")));
        mediumFlagAnswers.Add(new Answer(flagPromptString + "Russia", GenerateSprite(mediumFlagsPath + "Russia")));
        mediumFlagAnswers.Add(new Answer(flagPromptString + "United States", GenerateSprite(mediumFlagsPath + "UnitedStates")));
        allAnswerLists.Add(mediumFlagAnswers);

        //build hard flag answers
        string hardFlagsPath = "PuzzleAssets/HardFlags/";
        hardFlagAnswers.Add(new Answer(flagPromptString + "Comoros", GenerateSprite(hardFlagsPath + "Comoros")));
        hardFlagAnswers.Add(new Answer(flagPromptString + "Hungary", GenerateSprite(hardFlagsPath + "Hungary")));
        hardFlagAnswers.Add(new Answer(flagPromptString + "Kyrgyzstan", GenerateSprite(hardFlagsPath + "Kyrgyzstan")));
        hardFlagAnswers.Add(new Answer(flagPromptString + "Martinique", GenerateSprite(hardFlagsPath + "Martinique")));
        hardFlagAnswers.Add(new Answer(flagPromptString + "Mauritius", GenerateSprite(hardFlagsPath + "Mauritius")));
        hardFlagAnswers.Add(new Answer(flagPromptString + "Mozambique", GenerateSprite(hardFlagsPath + "Mozambique")));
        hardFlagAnswers.Add(new Answer(flagPromptString + "Nepal", GenerateSprite(hardFlagsPath + "Nepal")));
        hardFlagAnswers.Add(new Answer(flagPromptString + "Panama", GenerateSprite(hardFlagsPath + "Panama")));
        hardFlagAnswers.Add(new Answer(flagPromptString + "Seychelles", GenerateSprite(hardFlagsPath + "Seychelles")));
        hardFlagAnswers.Add(new Answer(flagPromptString + "Somalia", GenerateSprite(hardFlagsPath + "Somalia")));
        allAnswerLists.Add(hardFlagAnswers);

        //Build hard math answers
        hardMathAnswers.Add(new Answer("Derivative of: \n6x<sup>3</sup> − 9x + 4  ", "18x<sup>2</sup> − 9"));
        hardMathAnswers.Add(new Answer("Derivative of: \n7x<sup>4</sup> − 2x + 1  ", "28x<sup>3</sup> − 2"));
        hardMathAnswers.Add(new Answer("Derivative of: \n10x<sup>4</sup> + 2x + 1  ", "40x<sup>3</sup> + 2"));
        hardMathAnswers.Add(new Answer("Derivative of: \n3x<sup>5</sup> + 8x + 10  ", "15x<sup>4</sup> + 8"));
        hardMathAnswers.Add(new Answer("Derivative of: \n4x<sup>4</sup> − x + 1  ", "16x<sup>3</sup> − 1"));
        hardMathAnswers.Add(new Answer("Derivative of: \n7x<sup>2</sup> + 7x + 8  ", "14x + 7"));
        hardMathAnswers.Add(new Answer("Derivative of: \n9x<sup>3</sup> - 5x + 5  ", "27x<sup>2</sup> - 5"));
        hardMathAnswers.Add(new Answer("Derivative of: \n2x<sup>9</sup> + 9x + 7  ", "18x<sup>8</sup> + 9"));
        hardMathAnswers.Add(new Answer("Derivative of: \nx<sup>10</sup> − 2x + 9  ", "10x<sup>9</sup> − 2"));
        hardMathAnswers.Add(new Answer("Derivative of: \n12x<sup>3</sup> + x + 5  ", "36x<sup>2</sup> + 1"));
        allAnswerLists.Add(hardMathAnswers);

        //build animal answers
        string hardAnimalsPath = "PuzzleAssets/HardAnimals/";
        hardAnimalAnswers.Add(new Answer("This animal sees with sound.", GenerateSprite(hardAnimalsPath + "Dolphin")));
        hardAnimalAnswers.Add(new Answer("This animal's coat deters insects.", GenerateSprite(hardAnimalsPath + "Zebra")));
        hardAnimalAnswers.Add(new Answer("This animal can freeze without dying.", GenerateSprite(hardAnimalsPath + "Frog")));
        hardAnimalAnswers.Add(new Answer("This animal sleeps up to 22 hours per day.", GenerateSprite(hardAnimalsPath + "Koala")));
        hardAnimalAnswers.Add(new Answer("This animal tastes with its arms.", GenerateSprite(hardAnimalsPath + "Octopus")));
        hardAnimalAnswers.Add(new Answer("This animal can sleep for 3 years.", GenerateSprite(hardAnimalsPath + "Snail")));
        hardAnimalAnswers.Add(new Answer("This animal's heart is located in its head.", GenerateSprite(hardAnimalsPath + "Shrimp")));
        hardAnimalAnswers.Add(new Answer("It takes 2 weeks for this animal to digest food.", GenerateSprite(hardAnimalsPath + "Sloth")));
        hardAnimalAnswers.Add(new Answer("This animal can grow for 30 years.", GenerateSprite(hardAnimalsPath + "Crocodile")));
        hardAnimalAnswers.Add(new Answer("Animals of this type hold hands while sleeping.", GenerateSprite(hardAnimalsPath + "Otter")));
        allAnswerLists.Add(hardAnimalAnswers);
    }
    //Insert answers from various answer lists depending on the puzzle difficulty
    void BuildMainPuzzleList()
    {
        Debug.Log("Building main puzzle list [BuildMainPuzzleList()].");

        InitializeMainPuzzleList();

        if (easyModeEnabled)
        {
            InsertAnswersIntoPuzzleList(expressionAnswers, PuzzleType.ExpressionType);
            InsertAnswersIntoPuzzleList(objectAnswers, PuzzleType.ObjectType);
            InsertAnswersIntoPuzzleList(easyMathAnswers, PuzzleType.EasyMathType);
        }
        else if (mediumModeEnabled)
        {
            InsertAnswersIntoPuzzleList(mediumMathAnswers, PuzzleType.MediumMathType);
            InsertAnswersIntoPuzzleList(mediumAnimalAnswers, PuzzleType.MediumAnimalType);
            InsertAnswersIntoPuzzleList(mediumFlagAnswers, PuzzleType.MediumFlagType);

        }
        else if (hardModeEnabled)
        {
            InsertAnswersIntoPuzzleList(hardFlagAnswers, PuzzleType.HardFlagType);
            InsertAnswersIntoPuzzleList(hardMathAnswers, PuzzleType.HardMathType);
            InsertAnswersIntoPuzzleList(hardAnimalAnswers, PuzzleType.HardAnimalType);

        }
        else
        {
            //zombie mode goes here if completed
        }
    }
    void InitializeMainPuzzleList()
    {
        Debug.Log("Initializing main puzzle list [InitializeMainPuzzleList()].");

        puzzleList = new List<Puzzle>();
        int totalPuzzleCount = 0;
        //Determine the size of the main puzzle list
        if (easyModeEnabled)
        {
            totalPuzzleCount = expressionAnswers.Count + objectAnswers.Count +  easyMathAnswers.Count;
        }
        else if(mediumModeEnabled)
        {
            totalPuzzleCount = mediumMathAnswers.Count + mediumAnimalAnswers.Count + mediumFlagAnswers.Count;
        }
        else if(hardModeEnabled)
        {
            totalPuzzleCount = hardFlagAnswers.Count + hardMathAnswers.Count + hardAnimalAnswers.Count;
        }
        else
        {
            //zombie mode enabled?
        }
        //Add an empty puzzle to each position in the puzzle list to avoid exceptions during access
        for(int i = 0; i < totalPuzzleCount; i++)
        {
            puzzleList.Add(null);
        }
    }

    //Given an list of answers, combine with the main list of puzzles at the first available random index.
    void InsertAnswersIntoPuzzleList(List<Answer> answerList, PuzzleType puzzleType)
    {
        Debug.Log("Inserting puzzle type " + puzzleType + " answers into main puzzle list.");

        for(int i = 0; i < answerList.Count; i++)
        {
            answerList[i].isAddedToAnswerSet = true; //Answer is being added
            int[] answerSet = { -1, -1, -1, -1 }; //an array of ints to hold answer list indices, resets each loop              

            //first randomly choose which position (up, down, right, left) the correct answer will be
            int correctAnswerPosition = random.Next(0, 3); //determine where to place the answer
            answerSet[correctAnswerPosition] = i; 

            //next fill the rest of answerSet
            for (int j = 0; j < answerSet.Length; j++)
            { 
                while(answerSet[j] < 0) 
                {
                    //option has not been set
                    //keep attempting generate a random option
                    int answerIndex = random.Next(0, answerList.Count - 1);
                    if(answerList[answerIndex].isAddedToAnswerSet == false)
                    {
                        //answer has not been added to set
                        answerSet[j] = answerIndex;
                        answerList[answerSet[j]].isAddedToAnswerSet = true;
                    }
                }
            }

            SetAllAnswersFalse(answerList); //reset for next loop

            puzzleList[GetRandomPuzzleIndex()] = new Puzzle(i, correctAnswerPosition, puzzleType, answerSet);
             
        }
    }

    //Returns a random int for an index of the puzzle list that is not set already
    int GetRandomPuzzleIndex()
    {
        int index = random.Next(0, puzzleList.Count - 1);
        while(puzzleList[index] != null)
        {
            index = random.Next(0, puzzleList.Count);
        }
        return index;
    }

    Sprite GenerateSprite(string FilePath, float PixelsPerUnit = 100.0f)
    {
        
        Texture2D SpriteTexture = Resources.Load<Texture2D>(FilePath);
        Sprite aSprite = Sprite.Create(SpriteTexture, new Rect(0, 0, SpriteTexture.width, SpriteTexture.height), new Vector2(0.5f, 0.5f), PixelsPerUnit);

        return aSprite;
    }

    void SetAllAnswersFalse(List<Answer> aList)
    {
        for(int i = 0; i < aList.Count; i++)
        {
            aList[i].isAddedToAnswerSet = false;
        }
    }

    private class Answer
    {
        public string promptString { get; set; } //The prompt associated with this answer
        public bool isAddedToAnswerSet { get; set; } //Determines if this answer has been already added to an answer set
        public Sprite imageAnswer { get; set; } //in the case of image answer
        public string textAnswer { get; set; } //In the case of text answer type

        //Constructor for image answer
        public Answer(string promptString, Sprite imageAnswer)
        {
            this.promptString = promptString;
            isAddedToAnswerSet = false;
            this.imageAnswer = imageAnswer;
        }

        //Constructor for text answer
        public Answer(string promptString, string textAnswer)
        {
            this.promptString = promptString;
            isAddedToAnswerSet = false;
            this.textAnswer = textAnswer;
        }
    }

    private class Puzzle
    {
        public int answerIndex { get; set; } //where in the answer list the answer is
        public int correctAnswerPosition { get; set; } //where in the answer set the answer is (0, 1, 2, 3)
        public int[] options { get; set; } //Array that hold four answer indices
        public PuzzleType puzzleType { get; set; }
        //determines if puzzle has been added to the Puzzles list
        public bool isAddedToGame { get; set; } 
        
        public Puzzle(int answerIndex, 
            int correctAnswerPosition,
            PuzzleType puzzleType,
            int[] options)
        {
            this.answerIndex = answerIndex;
            this.correctAnswerPosition = correctAnswerPosition;
            this.isAddedToGame = true;
            this.puzzleType = puzzleType;
            this.options = options;
        }

        public Puzzle()
        {
            isAddedToGame = false;
        }
    }

    public int NumCorrectReturn()
    {
        return numCorrect;
    }

    public void PassCorrectAnswer()
    {
        Debug.Log("Pass answer");
        DisplayNextPuzzle(puzzleList[currentPuzzleIndex].correctAnswerPosition);
    }

}
