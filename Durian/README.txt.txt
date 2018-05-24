Durian
ICS 168 Project 2: Multiplayer Competitive Game

Team Members:
Crystal Agerton
Isaac Men
Richard But
Kevin Chao


Set-up Instructions:
   - Click the "Durian.exe" file
   - If that does not work
     - Open project in Unity
     - Open the "Title Screen" scene
     - For best experience, set game resolution to free aspect
   - Networked set-up
     **NOTE: The Networked version is incomplete, and will not
       provide the full experience of the game. We recommend
       playing the local version, which is complete**
     - Click the "Play Networked" button
     - One player makes the server by entering a name under
       "CREATE A GAME" and pressing "CREATE"
     - The other players will join this game by accessing
       it via the "LIST SERVERS" button.
     - Four players are required to start the game

Gameplay Instructions
   - Objective: Bring the flag back to your home base.
   - Click the "roll dice!" button to move.
   - Players will automatically move around the board.
   - If a player lands on the same tile as another player, they
     are given the option to fight or pass that player.
   - If the player dies while holding the flag, they drop it.
      - In PVP combat, the victor gets the flag.
      - In monster combat, the flag is returned to the center.
   - Tiles
      - HOME TILES: Colored to match the player, where the player
        starts and where they must return with the flag to win.
        Players can heal themselves for +2 HEALTH or teleport to 
        another home tile if they land on one during the game.
      - SWORD/SHIELD TILES: Players can stop on these tiles to
        buff either their ATK or DEF stats, or they can continue
        moving.
      - SPEED TILES: Players can stop on these tiles to boost
        their speed by adding +1 to their MOV stat or continue
        moving.
      - ARROW TILES: Players can choose one of two directions to
        move in.
      - MONSTER TILES: When players land on these tiles, they must
        fight a monster. If the player wins, they can proceed. If
        the player dies, they are knocked back.
      - STAR TILES: Players can stop on these tiles to grab the
        flag from the center of the board. If the flag has been
        captured, these tiles can only be passed.
   - Stats
      - ATK: added to a player's attack rolls.
      - DEF: added to a player's defense rolls.
      - MOV: added to a player's movement rolls.
      - HEALTH: shows the player's health. When it reaches 0 points,
           they cannot move for 2 turns, after which they regain 
           their health.