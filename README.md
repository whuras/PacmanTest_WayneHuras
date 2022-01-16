# PacmanTest_WayneHuras
This is a recreation of the classic arcade game Pac-Man, developed in Unity 2020.3.7f1.

### Controls
- BEST VIEWED IN A 800X600 RESOLUTION
- After the game has been launched, press any key to start the round.
- Control Pac-Man with keyboard buttons WASD or Arrow keys.
- If you get eaten by a ghost 3 times then its game over.
- Eat large power-pellets to gain a few seconds where Pac-Man can eat ghosts

### Highlights
- A* Pathfinding for ghost movement
- Four movement states for ghost movement:
  - Wait - The ghost waits to leave the gated area
  - Chase - The ghost moves towards its target tile
  - Scatter - The ghost moves towards its pre-designated corner
  - Run - The ghost runs away from Pac-Man when it can be eaten
- Four Ghost movement "personalities" implemented during Chase-phase:
  - Red Ghost targets Pac-Man's current position
  - Pink Ghost targets two tiles ahead of Pac-Man
  - Blue Ghost targets twice times the distance from the Red Ghost to two tiles ahead of Pac-Man
  - Orange Ghost targets Pac-Man's current position if further than 8 tiles of Pac-Man, or its pre-designated corner if within 8 tiles of Pac-Man
 - HighScore SaveSystem using PlayerPrefs
 - Classic features including:
   - Three lives; lose them by getting eaten by a ghost and its Game Over
   - Points system; eating pellets, power-pellets, and ghosts increase the score
   - Ghost timers based on number of pellets eaten
   - Power-pellets enable Pac-Man to eat ghosts for bonus points
   - Automated movement in the direction indicated by the player (WASD or Arrow Buttons) via Unity InputSystem
   - Queued movement direction if the player indicates a direction change when one is not immidately available
   - Classic sound effects for Introduction, Ghosts eaten, and Game Over

### Excluded
A few classic features have been excluded due to time constraints: random item spawn for bonus points, animated sprites, gradual difficulty increase, and map teleportation.
