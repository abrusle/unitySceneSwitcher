# Unity Scene Switcher

A small tool for quickly switching between scenes in the editor.

## Usage

Go to `Window > Quick Scenes` to open the _Quick Scenes_ window. I suggest docking it somewhere in your editor layout for ease of use.

Clicking on a scene in this window will open it in the editor.

## Configuration

There are 3 ways that you can configure which scenes are listed:
1. Right-click anywhere in the _Quick Scenes_ window and select `Change quick scenes...`.
2. In `Project Settings > Quick Scenes`.
3. Select the scene(s) you want to add in the project window and got to `Assets/Add to Quick Scenes`.

Don't forget to click "Save" to confirm your selection of scenes.

## Installation

Install via the Unity package manager using this repository's url.<br/>
More info: [Unity - Manual: Installing from a Git URL](https://docs.unity3d.com/Manual/upm-ui-giturl.html)

### Uninstall

1. Remove this package using the Unity Package Manager.
2. If you don't plan on reinstalling _Quick Scenes_ in your project or you whish to reset its configuration: delete the configuration file located at `[path-to-your-unity-project]/UserSettings/QuickScenes.asset` .

## Additional information
- _Quick Scenes_ is not meant to be used during Play Mode, thus interaction with the Quick Scenes GUI is disabled in Play Mode.
- This package only affects the Unity Editor and has no effect on your game.
- This package does not create any file in the Assets folder.

