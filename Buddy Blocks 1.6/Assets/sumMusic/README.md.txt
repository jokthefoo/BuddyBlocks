# sumMusic v1.0

## OVERVIEW
sumMusic is a super fast way to add music to your Unity3d game. Drag-and-drop prefabs into your scene and 
instantly have background music, with a button for players to toggle it on and off. Includes option to save 
status to PlayerPrefs. Uses the native Unity UI so you can easily change the look and feel of everything right in 
the editor. Licensed under MIT and CC0 so there are no worries on usage rights.

For an advanced music player with tons of options you will want to look elsewhere. This is for a project that 
just needs... some music. (ba-dum-tss)

- Project Homepage: http://www.cyberlogical.com/summusic/
- Repository: https://github.com/jerrydenton/sumMusic


## USAGE
- Create a Canvas if there is not one in your scene already. [Create > UI > Canvas] in Hierarchy
- Copy one of the sumMusic prefabs into your Canvas.
- (Optional) Adjust position on screen in the RectTransform component
- (Optional) Select a different AudioClip from the 'Audio' folder and attach it to the AudioSource on the prefabs
- Your game now has music and a simple way to mute it!

**Check 'SampleScene' for example of proper setup**

## OPTIONS
The following options are available on the 'sumMusic' component on each prefab
- *Start On* : Determine default state [bool, def: true]
- *Save Settings* : Save state to PlayerPrefs [bool, def: true]
- *Music On Sprite* : Sprite to display when music is on (Additional sprites included in 'Icons' folder)
- *Music Off Sprite* : Sprite to display when music is off (Additional sprites included in 'Icons' folder)
- *Image* : Just a link. No need to change this one.

## EXAMPLE
'SampleScene' contains an example with both prefabs.

## PROJECT LICENSE
- The MIT License (MIT) - https://opensource.org/licenses/MIT
- Copyright (c) 2016 Jerry Denton

## OTHER ASSETS
- Icons and Audio assets are from the awesome CC0 collection by asset creator Kenney - https://kenney.itch.io/
- License (Creative Commons Zero, CC0) - http://creativecommons.org/publicdomain/zero/1.0/

## CREATED BY
- Jerry Denton
- http://www.cyberlogical.com

### CHANGE NOTES
----------------------------------------------------------

- v 1.0
- Initial version

----------------------------------------------------------
