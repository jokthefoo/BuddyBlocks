using UnityEngine;
using System.Collections;

public class CreateBoxes : MonoBehaviour
{


    public GameObject[] blockTypes;
    public float startWait;
    public float spawnDelay;
    public bool spawn;
    public bool bombpocalypse_on = false;

    public Vector3 spawnLocation;

    [Tooltip("Higher = less bombs.")]
    public int bombRate;
    public bool bombsOn;
    int bombsInt = 1;

    [Tooltip("Higher = less rainbows.")]
    public int rainbowRate;
    public bool rainbowsOn;

    private IEnumerator blocksSpawn;

    public GameObject playerPlatform;

    public float gravity = -9.8f;

    [Tooltip("More than 9 spawns a ton of white")]
    public int numOfColors = 9;

    [Tooltip("Square, Rectangle, LBox, TBox, UBlock, Hexagon, Triangle, Circle, Stickman. (9 max)")]
    public int numOfBlockTypes = 7;

    public GameObject parent;

    public bool paused;

    // Use this for initialization
    void Start()
    {
        blocksSpawn = SpawnBlocks();
        Physics2D.gravity = new Vector2(0, gravity);
        paused = false;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void StartGame()
    {
        StartCoroutine(blocksSpawn);
    }

    // Handles Block and bomb spawning
    IEnumerator SpawnBlocks()
    {
        while (true)
        {
            if (spawn && !paused && !bombpocalypse_on)
            {
                yield return new WaitForSeconds(spawnDelay);
                Vector3 spawnPosition = new Vector3(Random.Range(-spawnLocation.x, spawnLocation.x), spawnLocation.y, spawnLocation.z);

                GameObject block;
                int colorChoice;

                // Bombs on?
                if (bombsOn)
                {
                    bombsInt = 0;
                }
                else
                {
                    bombsInt = 1;
                }

                //First check if it is a bomb, then check if it is a rainbow.
                if (Random.Range(bombsInt, bombRate + 4) == 0)
                {
                    colorChoice = 0;
                }
                else if ((Random.Range(0, rainbowRate + 4) == 0) && rainbowsOn)
                {
                    colorChoice = 500;
                }
                else
                {
                    colorChoice = Random.Range(1, numOfColors + 1);
                }

                Color block_color = new Color();
                string block_tag;

                switch (colorChoice)
                {
                    case 0:
                        block_color = Color.red;
                        block_tag = "bomb";
                        break;
                    case 500:
                        block_color = Color.red;
                        block_tag = "Rainbow";
                        break;
                    case 1:
                        block_color = Color.white;
                        block_tag = "white";
                        break;
                    case 4:
                        block_color = Color.cyan;
                        block_tag = "cyan";
                        break;
                    case 3:
                        block_color = Color.green;
                        block_tag = "green";
                        break;
                    case 7:
                        block_color = Color.blue;
                        block_tag = "blue";
                        break;
                    case 8:
                        block_color = Color.yellow;
                        block_tag = "yellow";
                        break;
                    case 2:
                        block_color = Color.magenta;
                        block_tag = "magenta";
                        break;
                    case 5:
                        block_color = Color.red;
                        block_tag = "red";
                        break;
                    case 9:
                        block_color = Color.black;
                        block_tag = "black";
                        break;
                    case 6:
                        block_color = Color.gray;
                        block_tag = "gray";
                        break;
                    default:
                        block_color = Color.white;
                        block_tag = "white";
                        break;
                }

                // Choose block shape; bombs are circles w/ /2 radius
                if (block_tag != "bomb" && block_tag != "Rainbow")
                {
                    var randomRotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
                    block = Instantiate(blockTypes[Random.Range(1, numOfBlockTypes)], spawnPosition, randomRotation) as GameObject;
                    block.AddComponent<Sticking>();
                    block.GetComponent<Sticking>().MainController = gameObject;
                    block.GetComponent<Sticking>().UnivSource = GetComponent<OtherGameControls>().UnivAudioSource;
                }
                else if (block_tag == "bomb")
                {
                    block = Instantiate(blockTypes[0], spawnPosition, Quaternion.identity) as GameObject;
                    //block.transform.localScale /= 2;
                    block.AddComponent<BombBehavior>();
                    block.GetComponent<BombBehavior>().MainController = gameObject;
                    block.GetComponent<BombBehavior>().UnivSource = GetComponent<OtherGameControls>().UnivAudioSource;
                    block.GetComponent<BombBehavior>().explosion = GetComponent<OtherGameControls>().explosion;
                    block.AddComponent<BombColor>();
                    //block.GetComponent<Sticking>().enabled = false;
                    block.name = "Bomb";
                }
                else
                {
                    GetComponent<AudioSource>().PlayOneShot(GetComponent<OtherGameControls>().Sounds[6]);

                    var randomRotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
                    block = Instantiate(blockTypes[Random.Range(1, numOfBlockTypes)], spawnPosition, randomRotation) as GameObject;
                    block.AddComponent<RainbowColor>();
                    block.AddComponent<Sticking>();
                    block.GetComponent<Sticking>().MainController = gameObject;
                    block.GetComponent<Sticking>().UnivSource = GetComponent<OtherGameControls>().UnivAudioSource;
                }

                block.transform.localScale = new Vector3(block.transform.localScale.x * 1.3f, block.transform.localScale.y * 1.3f, block.transform.localScale.z);

                SpriteRenderer spriteRend = block.GetComponent<SpriteRenderer>();

                if (spriteRend == null)
                {
                    foreach (SpriteRenderer s in block.GetComponentsInChildren<SpriteRenderer>())
                    {
                        s.color = block_color;
                        s.tag = block_tag;
                    }
                    block.tag = block_tag;
                    //block.GetComponent<SpriteRenderer>().sprite =;
                }
                else
                {
                    spriteRend.color = block_color;
                    spriteRend.tag = block_tag;
                }

                block.transform.parent = parent.transform;

                block.GetComponent<Rigidbody2D>().angularDrag = .2f;
                block.GetComponent<Rigidbody2D>().gravityScale = .8f;
                block.GetComponent<Rigidbody2D>().drag = .05f;

                block.AddComponent<MaxVelocity>();
                block.GetComponent<MaxVelocity>().OtherControls = GetComponent<OtherGameControls>();
            }

            else if (spawn && !paused)
            {
                string block_tag = "bomb";
                Color block_color = new Color();
                block_color = Color.blue;

                Vector3 spawnPos = new Vector3(Random.Range(-19.5f, 19.5f), spawnLocation.y, spawnLocation.z);
                GameObject block;

                block = Instantiate(blockTypes[0], spawnPos, Quaternion.identity) as GameObject;
                //block.transform.localScale /= 2;
                block.AddComponent<BombBehavior>();
                block.AddComponent<BombColor>();
                block.GetComponent<BombBehavior>().MainController = gameObject;
                block.GetComponent<BombBehavior>().UnivSource = GetComponent<OtherGameControls>().UnivAudioSource;
                block.GetComponent<BombBehavior>().explosion = GetComponent<OtherGameControls>().explosion;
                block.name = "Bomb";

                block.GetComponent<SpriteRenderer>().tag = block_tag;
                block.GetComponent<SpriteRenderer>().color = block_color;

                block.AddComponent<MaxVelocity>();
                block.GetComponent<MaxVelocity>().OtherControls = GetComponent<OtherGameControls>();

                yield return new WaitForSeconds(0.25f);
            }

            else
            {
                yield return new WaitForSeconds(0.5f);
            }
        }
    }
}
