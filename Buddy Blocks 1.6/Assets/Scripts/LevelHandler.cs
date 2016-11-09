using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LevelHandler : MonoBehaviour
{

    public Transform Camera;
    public GameObject MainController;
    public Transform Platform;
    public Transform Disposal;

    public Transform Background;

    //public float timeToCompleteLevel;
    public float timer { get; private set; }

    int currentLevel = 1;
    bool levelTriggered = false;

    bool cameraPan = false;
    bool platformPan = false;
    float cameraNewPos_y;

    // Scoring Output
    int Score;
    public Text ScoreText;
    public Text LevelNumText;


    // int timeBonus;
    int BlocksNum;
    int levelScore;
    public Text LevelScoreText;
    public Text NumBlocksText;
    public Text StuckBonusText;

    public Text TimerText;
    public Button ResumeButton;

    //float levelSize;

    CreateBoxes BoxMaker;
    OtherGameControls OtherControls;

    float PlatformOffset_y;
    float SpawnOffset_y;

	//declaring bool triggers for one time activation of audio when conditions are met
	bool oneuhohtrigger;
	bool onebombpocalypsetrigger;

    //Vector3 backgroundNewPos;
    //public GameObject backgroundPosition;

    // Use this for initialization
    void Start()
    {
        BoxMaker = MainController.GetComponent<CreateBoxes>();
        OtherControls = MainController.GetComponent<OtherGameControls>();
        //levelSize = MainController.GetComponent<OtherGameControls>().LevelSize;

        PlatformOffset_y = Platform.position.y - Camera.position.y;
        SpawnOffset_y = BoxMaker.spawnLocation.y - Camera.position.y;

        Platform.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionY;
        

        // Set Block Spawning right outside camera view
        MainController.GetComponent<CreateBoxes>().spawnLocation.y = Camera.transform.position.y + 7.5f;

        SetLevel(currentLevel);
        transform.localPosition = CalcTriggerPosition();
        NonScrollablePositions();

        //timer = timeToCompleteLevel;

        // Hide scoreboard
        LevelScoreText.enabled = false;
        NumBlocksText.enabled = false;
        StuckBonusText.enabled = false;
        ResumeButton.gameObject.SetActive(false);
    }

    Vector3 CalcTriggerPosition()
    {
        // Set Trigger to be levelSize *0.4f above platform
        //Vector3 TriggerPos_y = new Vector3(0, Platform.position.y + levelSize * 0.4f, 0);
        //transform.position = new Vector3(0, TriggerPos_y);

        Vector3 TriggerPos_y = new Vector3(0, PlatformOffset_y + OtherControls.LevelSize * 0.8f, 10);
        return TriggerPos_y;
    }

    void NonScrollablePositions()
    {
        //Platform.position = new Vector3(Platform.position.x, Camera.position.y + PlatformOffset_y, Platform.position.z);
        BoxMaker.spawnLocation = new Vector3(BoxMaker.spawnLocation.x, Camera.position.y + SpawnOffset_y, BoxMaker.spawnLocation.z);
    }

    // Update is called once per frame
    void Update()
    {
        if (!cameraPan)
        {
            timer -= Time.deltaTime;

		//old script
		if (levelTriggered && Time.timeScale == 1)
		{
		//resume game ();
		}
		

		//timer set to play uh oh at 3 second
		if (timer < 1) 
		{
				//oneuhohtrigger used to ensure exactly one audio trigger per event
				if (oneuhohtrigger == false) 
				{
                    GetComponent<AudioSource>().volume = 1f;
                    GetComponent<AudioSource>().pitch = 1f;
                    GetComponent<AudioSource>().PlayOneShot(MainController.GetComponent<OtherGameControls>().Sounds[4]);
                    oneuhohtrigger = true; //to play clip only once
                }
			}
		else
		{
				//oneuhohtrigger reset to trigger the next time the timer is at 3 seconds
				oneuhohtrigger = false;
		}


        if (timer > 0)
        {
				
				BoxMaker.bombpocalypse_on = false;
				//Bombpocalyse music end and reset trigger
				if (oneuhohtrigger == false) {
					
					onebombpocalypsetrigger = false;
				} 

                if(GetComponent<AudioSource>().volume > 0)
                {
                    GetComponent<AudioSource>().volume -= 0.01f;
                }
                else
                {
                    GetComponent<AudioSource>().Stop();
                }
        }
        else if (timer > -10)
        {
				BoxMaker.bombpocalypse_on = true;
				//MainController.GetComponent<CreateBoxes>().bombpocalypse_on = true;
				//Bombpocalypse music start (once due to onebombpocalysetrigger)
				if (onebombpocalypsetrigger == false) {
					onebombpocalypsetrigger = true;

					//old program
					//GetComponent<AudioSource> ().PlayOneShot (MainController.GetComponent<OtherGameControls> ().Sounds [5]);
					//end old program

					//Play and randomize start point of bomb music
					AudioClip BombpocalypseMusic = MainController.GetComponent<OtherGameControls> ().Sounds [5];
					GetComponent<AudioSource> ().clip = BombpocalypseMusic;
					GetComponent<AudioSource> ().time = Random.Range (0, 65);
                    GetComponent<AudioSource>().volume = 0.1f;
                    GetComponent<AudioSource>().pitch = 1.05f;
                    GetComponent<AudioSource>().Play();
                }
                else
                {
                    GetComponent<AudioSource>().volume += 0.01f;
                }


        }
        else
        {
            timer = CalcTimer() * 0.8f;
            //MainController.GetComponent<CreateBoxes>().bombpocalypse_on = false;
        }
        if (timer <= 0)
        {
            TimerText.text = "Bombs: 0";
        }
        else
        {
            TimerText.text = "Bombs: " + Mathf.Round(timer);
           }
       }
	}

    void FixedUpdate()
    {

        // Scroll Camera and Level Trigger Up
        if (cameraPan)
        {
            BoxMaker.spawn = false;
            Vector3 CameraNewPos = new Vector3(0, cameraNewPos_y + 4, Camera.position.z);
            Vector3 tempbackgroundpos = new Vector3(0, Background.position.y - (16f - (currentLevel* 3 / 5)), Background.position.z);

            Camera.position = Vector3.Lerp(Camera.position, CameraNewPos, 1f * Time.deltaTime);
            Background.position = Vector3.Lerp(Background.position, tempbackgroundpos, 1f * Time.deltaTime);


            // Scroll Trigger
            transform.localPosition = Vector3.Lerp(transform.localPosition, CalcTriggerPosition(), 1.5f * Time.deltaTime);

            if (Camera.position.y >= cameraNewPos_y)
            {
                cameraPan = false;
                platformPan = true;
                //DeleteBlocks();
            }
        }

        // Scroll Platform up
        if (platformPan)
        {
            Vector3 PlatformNewPos = new Vector3(Platform.position.x, Camera.position.y + PlatformOffset_y + 4, Platform.position.z);
            Platform.position = Vector3.Lerp(Platform.position, PlatformNewPos, 1.5f * Time.deltaTime);

            if(Platform.position.y >= Camera.position.y + PlatformOffset_y)
            {
                platformPan = false;
                NextLevel();
                BoxMaker.spawn = true;
            }
        }
    }

    void DeleteBlocks()
    {
        foreach (Transform child in BoxMaker.parent.GetComponentInChildren<Transform>())
        {
            Destroy(child.gameObject);
        }
    }

    // Detects when a block is in the LevelTrigger
    void OnTriggerStay2D(Collider2D aBlock)
    {
        if (!levelTriggered && aBlock.tag != "bomb")
        {
            GameObject runner = aBlock.transform.gameObject;
            while (runner.GetComponent<Rigidbody2D>() == null)
            {
                runner = runner.transform.parent.gameObject;
            }

            float blockSpeed = runner.GetComponent<Rigidbody2D>().velocity.y;

            if (Mathf.Abs(blockSpeed) <= 0.005)
            {
                levelTriggered = true;
                LevelComplete();
            }
        }
    }

    // Display the scoreboard at the end of the level
    void DisplayScoreBoard()
    {
        LevelScoreText.text = "Level Score: " + levelScore.ToString();
        NumBlocksText.text = "From Number of Blocks: " + BlocksNum.ToString() + " x10";
        StuckBonusText.text = "Bonus from Stuck Blocks: " + (levelScore - (BlocksNum * 10)).ToString();
        //TimeBonusText.text = "Time Bonus: " + timeBonus.ToString();

        LevelScoreText.enabled = true;
        NumBlocksText.enabled = true;
        StuckBonusText.enabled = true;
        //TimeBonusText.enabled = true;

        ResumeButton.gameObject.SetActive(true);
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        LevelScoreText.enabled = false;
        NumBlocksText.enabled = false;
        StuckBonusText.enabled = false;
        //TimeBonusText.enabled = false;
        ResumeButton.gameObject.SetActive(false);

        Time.timeScale = 1;

        // Scroll up for next level
        cameraNewPos_y += 10.8f;
        //backgroundNewPos = backgroundPosition.transform.position;
        cameraPan = true;
    }

    // Update the score text and the level text
    void UpdateScoreText()
    {
        ScoreText.text = "Total Score: " + Score.ToString();
        LevelNumText.text = "Level " + currentLevel.ToString();
    }

    void LevelComplete()
    {
        // Reset BLocksNum and store the current level score (BlocksNum is updated with the LevelScore() call)
        BlocksNum = 0;
        levelScore = LevelScore();

        // Increase the Score
        Score += levelScore;

        // Update the score
        UpdateScoreText();

        // Display ScoarBoard
        DisplayScoreBoard();
        //backgroundPosition.transform.position = new Vector3(0, backgroundPosition.transform.position.y - 1080f);
    }

    void NextLevel()
    {
        NonScrollablePositions();
        currentLevel++;

        // Update level
        UpdateScoreText();
        SetLevel(currentLevel);
        levelTriggered = false;
    }

    // Handles difficulty scaling
    void SetLevel(int LevelNum)
    {

        switch (LevelNum)
        {
            case 1:
                BoxMaker.numOfColors = 1;
                BoxMaker.numOfBlockTypes = 16;
                BoxMaker.bombsOn = false;
                BoxMaker.rainbowsOn = false;

                OtherControls.LevelSize--;
                goto default;

            case 2:
                BoxMaker.numOfColors += 1;
                BoxMaker.numOfBlockTypes += 2;
                BoxMaker.bombsOn = true;
                BoxMaker.bombRate = 10;
                goto default;

            case 3:
                BoxMaker.numOfBlockTypes += 2;
                BoxMaker.rainbowsOn = true;
                BoxMaker.bombRate = 8;
                BoxMaker.rainbowRate = 10;
                goto default;

            case 4:
                goto default;

            default:
                if (LevelNum > 3)
                {
                    // Add one more shape each level
                    BoxMaker.numOfBlockTypes += 2;
                    BoxMaker.numOfBlockTypes = Mathf.Clamp(BoxMaker.numOfBlockTypes, 0, 38);

                    // Add one more color every two levels
                    BoxMaker.numOfColors = 1 + LevelNum / 2;
                    BoxMaker.numOfColors = Mathf.Clamp(BoxMaker.numOfColors, 0, 8);

                    BoxMaker.bombRate = (BoxMaker.bombRate * 4) / 5;

                    BoxMaker.rainbowRate = (BoxMaker.rainbowRate * 4) / 5;

                    if (BoxMaker.spawnLocation.x <= 16f)
                    {
                        BoxMaker.spawnLocation.x = BoxMaker.spawnLocation.x + 1f;
                    }
                    if (BoxMaker.spawnDelay >= .7f)
                    {
                        BoxMaker.spawnDelay = BoxMaker.spawnDelay - .1f;
                    }
                }


                timer = CalcTimer();


                OtherControls.LevelSize ++;
                OtherControls.LevelSize = Mathf.Clamp(OtherControls.LevelSize, 0, 20);
                //MainController.GetComponent<OtherGameControls>().LevelSize = levelSize;

                //transform.position = CalcTriggerPosition();
                break;

        }
    }

    float CalcTimer()
    {
        float temp;
        temp = OtherControls.LevelSize * 3 + Mathf.Pow(currentLevel * 2, 2);
        temp = Mathf.Clamp(temp, 0, 300);
        return temp;
    }

    // Scores the level based off the number of blocks remaining at the end of the level; stuck blocks yield a higher score
    int LevelScore()
    {
        int score = 0;

        foreach (Transform child in BoxMaker.parent.GetComponentInChildren<Transform>())
        {
            if (child.name != "StuckBlock" && child.name != "RainStuck")
            {
                score += 10;
                BlocksNum++;
            }
            else
            {
                score += StuckGroupScore(child);
            }
        }
        return score;
    }

    // Scores a group of stuck blocks; each additional block in the stuck group grants 2+ bonus
    int StuckGroupScore(Transform group)
    {
        int groupScore = 4;
        int stuckBonus = 0;

        foreach (Transform child in group.GetComponentInChildren<Transform>())
        {
            if (child.name != "StuckBlock" && child.name != "RainStuck")
            {
                groupScore += 10 + stuckBonus;
                stuckBonus += 2;

                BlocksNum++;
            }
        }
        return groupScore;
    }


}
