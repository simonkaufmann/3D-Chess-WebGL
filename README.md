# Chess

3D Multiplayer Chess Game for WebGL created with Unity Game Engine.

<img src="images/chess_01.png" widt="50%">
<img src="images/chess_02.png" widt="50%">
<img src="images/chess_03.png" widt="50%">
<img src="images/chess_04.png" widt="50%">

## Useful links

https://www.turbosquid.com/FullPreview/Index.cfm/ID/1408325  
https://answers.unity.com/questions/247810/how-to-get-gameobject-that-is-clicked-by-a-mouse.html  
https://docs.unity3d.com/ScriptReference/Input.GetMouseButtonDown.html  
https://answers.unity.com/questions/1065971/how-do-you-detect-a-mouse-button-click-on-a-game-o.html  
https://answers.unity.com/questions/609267/how-to-change-the-origin-of-a-gameobject.html  
https://answers.unity.com/questions/1065971/how-do-you-detect-a-mouse-button-click-on-a-game-o.html  
https://docs.unity3d.com/ScriptReference/Input.GetMouseButtonDown.html  
https://answers.unity.com/questions/247810/how-to-get-gameobject-that-is-clicked-by-a-mouse.html  
https://www.raywenderlich.com/1142814-introduction-to-multiplayer-games-with-unity-and-photon  
https://doc.photonengine.com/en-us/pun/v2/demos-and-tutorials/pun-basics-tutorial/lobby-ui  
https://forum.unity.com/threads/getting-error-rebuilding-the-library-because-the-assetdatabase-could-not-be-found.487801/  
http://forum.orkframework.com/discussion/1214/solved-my-whole-project-is-corrupted  
https://medium.com/@johntucker_48673/unity-exploring-multiplayer-solutions-20000ccab3b6  
https://thoughtbot.com/blog/how-to-git-with-unity  
https://www.turbosquid.com/FullPreview/Index.cfm/ID/1408325  
https://github.com/dantasulisses/WebMobileInputFix  
https://forum.unity.com/threads/keyboard-input-webgl-mobile.505193/  

## Attributions

3D model of chess board created by user **usman039**  
https://www.turbosquid.com/FullPreview/Index.cfm/ID/1408325  

## Notes

If cloned from git, the first start with Unity takes longer since the Library folder has
to be created by Unity. If the chessboard is not shown, click `File -> Open Scene` and
open the ChessGame.unity file.

Current version connects to Photon EU region automatically instead of selecting the fastest
available region. This is to allow all players using the game to play together independent
from where they are located (read up on https://doc.photonengine.com/en-us/realtime/current/connection-and-authentication/regions).
To change this setting in the Unity Editor, click on **Window -> Photon Unity Networking** and then
set in the **Inspector** the **Fixed Region**.
