using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controls UI Button and Audio Source for music player
/// (Requires AudioSource attached to game object)
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class SumMusic : MonoBehaviour {

    // Determine default state and whether to save
    public bool startOn = true, saveSettings = true;
    // Links to components
    public Sprite musicOnSprite, musicOffSprite;
    public Image image;

    // Current music status
    bool musicOn;

	void Awake () {
        // Check that sprites are linked properly
        if (!checkReqs())
            Debug.LogError("Link references missing on <b>sumMusic</b> object. Please check assignments in editor.");
	}

    void Start () {
        // Set default state based on startOn or PlayerPrefs.
        if (saveSettings) {
            if (PlayerPrefs.HasKey("sumMusicOn"))
                musicOn = !(PlayerPrefs.GetInt("sumMusicOn") > 0);    // Convert from int to (flipped) bool
            else
                musicOn = !startOn; // Flip default before toggle
        }
        else
            musicOn = !startOn; // Flip default before toggle
        // Use toggle to set initial state
        ToggleMusic(true);
    }

    /// <summary>
    /// Toggles music status, switches sprite, and saves if needed
    /// </summary>
    public void ToggleMusic (bool isStart = false) {
        // Flip value of musicOn
        musicOn = !musicOn;
        //Debug.Log("Music status changed to " + musicOn);
        AudioSource src = GetComponent<AudioSource>();
        // Play or stop music
        if (musicOn)
            src.Play();
        else
            src.Stop();
        // Switched sprite to appropriate value
        image.sprite = musicOn ? musicOnSprite : musicOffSprite;
        // Save status to PlayerPrefs as int if needed (1=on,0=off)
        if (saveSettings && !isStart) {
            Debug.Log("Saving sound settings");
            PlayerPrefs.SetInt("sumMusicOn", musicOn ? 1 : 0);
        }
    }
	
    /// <summary>
    /// Checks to make sure necessary objects are assigned
    /// </summary>
    /// <returns>True or False</returns>
    bool checkReqs () {
        return (musicOnSprite != null && musicOffSprite != null && image != null);
    }
}
