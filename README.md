# Fallin

A turn-based CLI RPG with ASCII graphics. Currently Work-In-Progress.  
What started as a small learning task to build a 2D map and a player moving around it, proceeded to get a bunch of additional mechanics (including proper Character class, enemies, fights, and a leveling system) resulting in a single file of 1 300 lines of code (as the original commit) and later was fully refactored into 15+ files.

## Install and Run

1. Install [.NET](https://dotnet.microsoft.com/en-us/download/dotnet/8.0) 8.0 or higher.

2. Clone the repository:

    ```bash
	git clone https://github.com/UnLuckyNikolay/fallin
    cd fallin
	```

3. Initialize the submodule:

	```bash
	git submodule init
	git submodule update
	```

4. Build and run:

	```bash 
	dotnet build 
	dotnet run
	```

## Features

* Randomized maps filled with enemies
* Player with an inventory, leveling mechanic, Fallout-like SPECIAL stat system and an incredibly useful battlepass with generous rewards
* Fighting system
* ASCII graphics (including map, enemies and certain menus)
* Cheats

## How to Play

* Enter your name
* Move around the map to kill enemies

## Important Commands

* Menu Switching:
	* `map` - Switch to the map
	* `character` - View character information
	* `inventory` - Open inventory

* Movement (Map):
	* `move <direction>` or `go <direction>` - Move around the map
	* `exit` - Quit the game

* Character Menu:
	* `levelup` - Spend skill points on SPECIAL stats
	* `battlepass` - View battlepass progression

* Inventory:
	* `use <name>` - Use the chosen item

* Combat:
	* `attack` - Attack the enemy
	* `defense` - Take defensive position

* Debug/Cheats:
	* `cheat *command*` - Various commands used for testing (heal, levelup, etc.), works from every menu

**Note:** Commands are context-sensitive. Main commands are displayed during player's turn based on the current menu. There are more commands and some of them include multiple versions/spellings.

## Screenshots

* Map:

![Map Showcase](https://imgur.com/xd7BYJI.png)

* Fight:

![Fight On-Going Showcase](https://imgur.com/TlSwBv6.png)

![Fight Won Showcase](https://imgur.com/2nT0X3R.png)

* Battle Pass:

![Battle Pass Showcase](https://imgur.com/JWPIHCb.png)

## Technical Features

* Complete refactoring from a single 1 300-line file to 15+ modular files using OOP principles
* Command processing using nested switches that includes a cheat list available in every menu
* Builder classes for the Main character and Enemies
* Multiple helper functions extracted into a separate library to be used as a submodule

## Planned Features

* Exit to progress to next level
* Save system
* More enemies and items
* Trader NPC
* Equipment
* SPECIAL system additions

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.