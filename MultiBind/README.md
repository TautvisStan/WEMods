# MultiBind

This mod lets players rebind their keyboard controls and share a single keyboard. Thanks to Nifty for an idea.

You can now toggle a controller mode by pressing numpad enter in the main menu. This mode will trick the game into thinking your keyboard is a xbox controller, allowing you to navigate the menus using the keyboard without the need of mouse.

Now supports rebinding buttons to the mouse. `Mouse0` - left mouse button, `Mouse1` - right mouse button, `Mouse2` - middle mouse button, `Mouse3` `Mouse4` `Mouse5` `Mouse6` - additional buttons on your mouse (you will have to figure out these yourself)


Controls can be changed in the mod config file. Keybinds that can be changed:
- Movement (arrow keys)
- Attack (A) (West button in the controller mode)
- Grapple (S) (North button in the controller mode)
- Run (Z) (South button in the controller mode)
- Pick up (X) (East button in the controller mode)
- Taunt (Space)
- Change focus (Control and Shift) (Right and Left shoulder buttons in the controller mode)
- Change control (Tab)(Right trigger in the controller mode)
- (NEW) Left trigger for the use in controller mode.
- Join the game (NEW) - previously was only available on the controller, now you can join a match in progress (eg. AI only match)

Changing `Esc` and `P` to different buttons is currently not supported.

Pressing `+` on numpad while in the main menu will add a keyboard player, pressing `-` will remove them instead. Supports up to 3 additional players. You can verify it by going into an exhibition mode and clicking on Play.
![Imgur](https://i.imgur.com/RsvOZBK.png)
Controls for each player can be customized in the config file. Additional players can also join the free roam or match in progress by pressing the Join button.

NOTE: In case the main keyboard controller disappears, go back in the main menu and toggle the controller mode to restore it back.

NOTE: Going into controller calibration screen will break added keyboard players. Press Refresh to remove them then add them again in the main menu.

NOTE: This mod may or may not break with an existing controller plugged in.