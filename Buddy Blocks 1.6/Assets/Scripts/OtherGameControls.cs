using UnityEngine;
using System.Collections;

public class OtherGameControls : MonoBehaviour {

    public float LevelSize = 8;
    public float MaxVel_Blocks = 8;

    // StuckID generator
    private int ID = 0;
    public int StuckID
    {
        get
        {
            return ID++;
        }
    }

    // Level Handler
    public LevelHandler level_handler;


    // Movement Controls
    public float PlatformThrust;
    public float PlatformMaxSpeed;

    // Sound Objects
    public AudioSource UnivAudioSource;
    public AudioClip[] Sounds;

    // Particle Effects
    public GameObject explosion;
    public GameObject stickEffect;
}
