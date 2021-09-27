﻿using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace BattleArena
{
    public enum ItemType
    {
        DEFENSE,
        ATTACK,
        NONE
    }

    public enum Scene
    {
        STARTMENU,
        NAMECREATION,
        CHARACTERSELECTION,
        BATTLE,
        RESTARTMENU
    }

    public struct Item
    {
        public string Name;
        public float StatBoost;
        public ItemType Type;
    }

    class Game
    {
        private bool _gameOver;
        private Scene _currentScene;
        private Player _player;
        private Entity[] _enemies;
        private int _currentEnemyIndex;
        private Entity _currentEnemy;
        private string _playerName;

        private Item[] _wizardItems;
        private Item[] _knightItems;


        /// <summary>
        /// Function that starts the main game loop
        /// </summary>
        public void Run()
        {
            Start();

            while (!_gameOver)
            {
                Update();
            }

            End();
        }

        /// <summary>
        /// Function used to initialize any starting values by default
        /// </summary>
        public void Start()
        {
            _gameOver = false;
            _currentScene = 0;
            InitializeEnemies();
            InitializeItems();
        }

        public void InitializeItems()
        {
            // Wizard Items
            Item bigWand = new Item { Name = "Big Wand", StatBoost = 5, Type = ItemType.ATTACK };
            Item bigShield = new Item { Name = "Big Shield", StatBoost = 15, Type = ItemType.DEFENSE};

            // Knight Item
            Item wand = new Item { Name = "Wand", StatBoost = 1025, Type = ItemType.ATTACK };
            Item shoes = new Item { Name = "Shoes", StatBoost = 900, Type = ItemType.DEFENSE };

            // Initialize Arrays
            _wizardItems = new Item[] { bigWand, bigShield };
            _knightItems = new Item[] { wand, shoes };
        }

        /// <summary>
        /// Initialize Enemies from start. 
        /// </summary>
        public void InitializeEnemies()
        {
            _currentEnemyIndex = 0;

            // Enemies

            // Name = "Slime";
            // Health = 10f;
            // AttackPower = 1f;
            // DefensePower = 0f;

            Entity slime = new Entity("Slime", 10f, 1f, 0f);

            // Name = "Zom-b";
            // Health = 15f;
            // AttackPower = 5f;
            // DefensePower = 2f;

            Entity zomb = new Entity("Zomb-B", 15f, 5f, 2f);

            // Name = "guy named Kris";
            // Health = 25f;
            // AttackPower = 10f;
            // DefensePower = 5f;

            Entity kris = new Entity("guy named Kris", 25f, 10f, 5f);

            _enemies = new Entity[] { slime, zomb, kris };

            _currentEnemy = _enemies[_currentEnemyIndex];
        }

        /// <summary>
        /// This function is called every time the game loops.
        /// </summary>
        public void Update()
        {
            DisplayCurrentScene();
            Console.Clear();
        }

        /// <summary>
        /// This function is called before the applications closes
        /// </summary>
        public void End()
        {
            Console.WriteLine("Fairwell Adventurer.");
        }

        public void Save()
        {
            // Create a new stream writer
            StreamWriter writer = new StreamWriter("SaveData.txt");

            // Save current enemy index
            writer.WriteLine(_currentEnemyIndex);

            // Save player and enemy stats
            _player.Save(writer);
            _currentEnemy.Save(writer);

            // Close writer when done saving
            writer.Close();
        }

        public bool Load()
        {
            bool loadSuccessful = true;

            // If the file doesn't exist...
            if (!File.Exists("SaveData.txt"))
            {
                // ... return false
                loadSuccessful = false;
            }

            // Create a new reader to read from the text file (.txt) 
            StreamReader reader = new StreamReader("SaveData.txt");

            // If the first line can't be converted into an integer...
            if (!int.TryParse(reader.ReadLine(), out _currentEnemyIndex))
            {
                // ... return false
                loadSuccessful =false;
            }

            // Load Player Job
            string job = reader.ReadLine();

            // Assign items based on player job
            if (job == "Wizard")
            {
                _player = new Player(_wizardItems);
            }
            else if (job == "Knight")
            {
                _player = new Player(_knightItems);
            }
            else
            {
                loadSuccessful = false;
            }

            _player.Job = job;

            if (!_player.Load(reader))
            {
                // ... return false
                loadSuccessful = false;
            }

            // Create a new instance and try to load the enemy
            _currentEnemy = new Entity();
            if (!_currentEnemy.Load(reader))
            {
                loadSuccessful = false;
            }

            // Update the array to match the current enemy stats
            _enemies[_currentEnemyIndex] = _currentEnemy;

            // Close the reader once loading is finished
            reader.Close();

            return loadSuccessful;
        }

        /// <summary>
        /// Gets an input from the player based on some given decision
        /// </summary>
        /// <param name="description">The context for the input</param>
        /// <param name="option1">The first option the player can choose</param>
        /// <param name="option2">The second option the player can choose</param>
        /// <returns></returns>
        int GetInput(string description, params string[] options)
        {
            string input = "";
            int inputReceived = -1;

            while (inputReceived == -1)
            {
                // Print options
                Console.WriteLine(description);
                for (int i = 0; i < options.Length; i++)
                {
                    Console.WriteLine((i + 1) + ". " + options[i]);
                }

                Console.Write("> ");

                // Get input from player
                input = Console.ReadLine();

                // If the player typed an int...
                if (int.TryParse(input, out inputReceived))
                {
                    // ...decrement the input and check if it's within the bounds of the array
                    inputReceived--;
                    if(inputReceived < 0 || inputReceived >= options.Length)
                    {
                        // Set input received to be default value
                        inputReceived = -1;

                        // Display error message
                        Console.WriteLine("Invalid Input");
                        Console.ReadKey(true);
                    }
                }

                // If the player didn't type an int
                else
                {
                    // Set input received to be the default value
                    inputReceived = -1;
                    Console.WriteLine("Invalid Input");
                    Console.ReadKey(true);
                }
                Console.Clear();
            }
            return inputReceived;
        }

        /// <summary>
        /// Calls the appropriate function(s) based on the current scene index
        /// </summary>
        void DisplayCurrentScene()
        {
            switch (_currentScene)
            {
                case Scene.STARTMENU:
                    DisplayStartMenu();
                    break;
                case Scene.NAMECREATION:
                    GetPlayerName();
                    break;
                case Scene.CHARACTERSELECTION:
                    CharacterSelection();
                    break;
                case Scene.BATTLE:
                    Battle();
                    CheckBattleResults();
                    break;
                case Scene.RESTARTMENU:
                    DisplayRestartMenu();
                    break;

                default:
                    Console.WriteLine("Invalid scene index");
                    break;
            }
        }

        /// <summary>
        /// Displays the menu that allows the player to start or quit the game
        /// </summary>
        void DisplayRestartMenu()
        {
            //Display question and store input
            int input = GetInput("Play Again?", "Yes", "No");

            // If the player decides to restart...
            if (input == 0)
            {
                //...set their current scene to be the start and update the player state to be alive
                _currentScene = 0;
                InitializeEnemies();
            }
            //Otherwise if the player wants to quit...
            else if (input == 1)
            {
                //...set game over to be true
                _gameOver = true;
            }
            else
            {
                //...display error message
                Console.WriteLine("Invalid Input");
                Console.ReadKey();
            }
        }

        public void DisplayStartMenu()
        {
            int input = GetInput("Welcome to Battle Arena!", "Start New Game", "Load Game");

            if (input == 0)
            {
                _currentScene = Scene.NAMECREATION;
            }

            else if (input  == 1)
            {
                if (Load())
                {
                    Console.WriteLine("Load Successful");
                    Console.ReadKey(true);
                    Console.Clear();
                    _currentScene = Scene.BATTLE;
                }
            }
            else
            {
                Console.WriteLine("Load Failed");
                Console.ReadKey(true);
                Console.Clear();
            }
        }

        /// <summary>
        /// Displays text asking for the players name. Doesn't transition to the next section
        /// until the player decides to keep the name.
        /// </summary>
        void GetPlayerName()
        {
            // Introduction and get player name
            Console.WriteLine("Welcome! Please enter your name.");
            Console.Write("> ");
            _playerName = Console.ReadLine();

            Console.Clear();

            int input = GetInput("You've entered " + _playerName + " are you sure you want to keep this name?",
                "Keep Name", "Rename");

            if (input == 0)
            {
                _currentScene++;
            }
        }

        /// <summary>
        /// Gets the players choice of character. Updates player stats based on
        /// the character chosen.
        /// </summary>
        public void CharacterSelection()
        {
            int input = GetInput("Nice to meet you " + _playerName + ". Please select a character." ,
                "Knight", "Wizard");
            Console.Write("> ");

            // Wizard
            if (input == 0)
            {
                // Display for Wizard Stats

                // Player Health = 50f;
                // Player AttackPower = 25f;
                // Player DefensePower = 5f;
                // Items: Big Wand and Big Shield

                _player = new Player(_playerName, 50, 25, 5, _wizardItems, "Wizard");
                _currentScene++;

            }

            // Knight
            else if (input == 1)
            {
                // Display for Knight Stats

                // Player Health = 75f;
                // Player AttackPower = 15f;
                // Player DefensePower = 10f;
                // Items: Wand and Shoes

                _player = new Player(_playerName, 75, 15, 10, _knightItems, "Knight");
                _currentScene++;
            }
        }

        /// <summary>
        /// Prints a characters stats to the console
        /// </summary>
        /// <param name="character">The character that will have its stats shown</param>
        void DisplayStats(Entity character)
        {
            Console.WriteLine("Name: " + character.Name);
            Console.WriteLine("Health: " + character.Health);
            Console.WriteLine("Attack Power: " + character.AttackPower);
            Console.WriteLine("Defense Power: " + character.DefensePower + "\n" );
        }

        public void DisplayEquipItemMenu()
        {
            // Get item index
            int choice = GetInput("Select an Item to equip.", _player.GetItemNames());

            // Equip item at given index
            if (!_player.TryEquipItem(choice))
            {
                Console.WriteLine("You couldn't find that item in your bag.");
            }

            // Print feedback
            Console.WriteLine("You equipped " + _player.CurrentItem.Name + "!");
        }

        /// <summary>
        /// Simulates one turn in the current monster fight
        /// </summary>
        public void Battle()
        {
            float damageDealt = 0;

            // Display stats on Player
            DisplayStats(_player);

            // Display stats on Enemies 
            DisplayStats(_currentEnemy);

            int input = GetInput("A " + _currentEnemy.Name + " stands in front of you! What will you do?", 
                "Attack", "Equip Item", "Remove Current Item", "Save");

            if (input == 0)
            {
                // The player attacks the enemy
                damageDealt = _player.Attack(_currentEnemy);
                Console.WriteLine("You dealt " + damageDealt + " damage!");

               
            }
            else if (input == 1)
            {
                DisplayEquipItemMenu();
            }
            else if (input == 2)
            {
                if (!_player.TryRemoveCurrentItem())
                {
                    Console.WriteLine("You're don't have anything equipped.");
                }
                else
                {
                    Console.WriteLine("You placed the item in your bag.");
                }
                Console.ReadKey(true);
                Console.Clear();
                return;
            }
            else if (input == 3)
            {
                Save();
                Console.WriteLine("Saved Game");
                Console.ReadKey(true);
                Console.Clear();
                return;
            }
            // The enemy attacks the player
            damageDealt = _currentEnemy.Attack(_player);
            Console.WriteLine("The " + _currentEnemy.Name + " dealt " + damageDealt);

            Console.ReadKey(true);
            Console.Clear();
        }

        /// <summary>
        /// Checks to see if either the player or the enemy has won the current battle.
        /// Updates the game based on who won the battle..
        /// </summary>
        void CheckBattleResults()
        {
            if (_player.Health <= 0)
            {
                Console.WriteLine("You were slain ");
                Console.ReadKey(true);
                Console.Clear();
                _currentScene = Scene.RESTARTMENU; 
            }

            else if (_currentEnemy.Health <= 0)
            {
                Console.WriteLine("You slayed the " + _currentEnemy.Name);
                Console.ReadKey();
                Console.Clear();

                _currentEnemyIndex++;

                if (TryStopGame())
                {
                    return;
                }

                _currentEnemy = _enemies[_currentEnemyIndex];
            }
        }

        bool TryStopGame()
        {
            bool stopGame = _currentEnemyIndex >= _enemies.Length;

            if (stopGame)
            {
                _currentScene = Scene.RESTARTMENU;
            }
            return stopGame;
        }
    }
}
