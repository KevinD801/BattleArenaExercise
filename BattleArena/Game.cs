using System;
using System.Collections.Generic;
using System.Text;

namespace BattleArena
{
    public struct Item
    {
        public string Name;
        public float StatBoost;
        
    }

    // Test commit

    /// <summary>
    /// Represents any entity that exists in game
    /// </summary>


    class Game
    {
        private bool _gameOver;
        private int _currentScene;
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
            Item bigWand = new Item { Name = "Big Wand", StatBoost = 5 };
            Item bigShield = new Item { Name = "Big Shield", StatBoost = 15 };

            // Knight Item
            Item wand = new Item { Name = "Wand", StatBoost = 1025 };
            Item shoes = new Item { Name = "Shoes", StatBoost = 900 };

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

                Console.WriteLine("> ");

                // Get input from player
                input = Console.ReadLine();

                // If the player typed an int...
                if (int.TryParse(input, out inputReceived))
                {
                    inputReceived--;
                    if(inputReceived<0 || inputReceived >= options.Length)
                    {
                        // Set input received to be default value
                        inputReceived = -1;

                        // Display error message
                        Console.WriteLine("Invalid Input");
                        Console.ReadKey(true);
                    }
                }
            }
        }

        /// <summary>
        /// Calls the appropriate function(s) based on the current scene index
        /// </summary>
        void DisplayCurrentScene()
        {
            switch (_currentScene)
            {
                case 0:
                    GetPlayerName();
                    CharacterSelection();
                    break;

                case 1:
                    Battle();
                    CheckBattleResults();
                    Console.ReadKey(true);
                    break;

                case 2:
                    DisplayMainMenu();
                    break;

                default:
                    Console.WriteLine("Invalid scene index");
                    break;
            }
        }

        /// <summary>
        /// Displays the menu that allows the player to start or quit the game
        /// </summary>
        void DisplayMainMenu()
        {
            //Display question and store input
            int input = GetInput("Play Again?", "Yes", "No");

            // If the player decides to restart...
            if (input == 1)
            {
                //...set their current area to be the start and update the player state to be alive
                _currentScene = 0;
                InitializeEnemies();
            }
            //Otherwise if the player wants to quit...
            else if (input == 2)
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

        /// <summary>
        /// Displays text asking for the players name. Doesn't transition to the next section
        /// until the player decides to keep the name.
        /// </summary>
        void GetPlayerName()
        {
            bool userName = false;
            while(!userName)
            {
                // Introduction and get player name
                Console.WriteLine("Welcome! Please enter your name.");
                Console.Write("> ");
                _playerName = Console.ReadLine();

                Console.Clear();

                int input = GetInput("You've entered " + _playerName + " are you sure you want to keep this name?",
                    "Keep Name", "Rename");

                if (input == 1)
                {
                    userName = true;
                }
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
            if (input == 1)
            {
                // Display for Wizard Stats

                // Player Health = 50f;
                // Player AttackPower = 25f;
                // Player DefensePower = 5f;
                // Items Big Wand Big Shield

                _player = new Player(_playerName, 50, 25, 5, _wizardItems);
                _currentScene++;

            }

            // Knight
            else if (input == 2)
            {
                // Display for Knight Stats

                // Player Health = 75f;
                // Player AttackPower = 15f;
                // Player DefensePower = 10f;

                _player = new Player(_playerName, 75, 15, 10, _knightItems);
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

        /// <summary>
        /// Simulates one turn in the current monster fight
        /// </summary>
        public void Battle()
        {
            // Display stats on Player
            DisplayStats(_player);

            // Display stats on Enemies 
            DisplayStats(_currentEnemy);

            int input = GetInput("A " + _currentEnemy.Name + " stands in front of you! What will you do?", 
                "Attack", "Equip Item");

            if (input == 1)
            {
                // The player attacks the enemy
                float damageDealt = _player.Attack(_currentEnemy);
                Console.WriteLine("You dealt " + damageDealt + " damage!");

                // The enemy attacks the player
                damageDealt = _currentEnemy.Attack(_player);
                Console.WriteLine("The " + _currentEnemy.Name + " dealt " + damageDealt);
            }
            else if (input == 2)
            {
                // Dodged the enemy's attack!
                Console.WriteLine("You dodged the enemy's attack!");
            }
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
                Console.ReadKey();
                Console.Clear();
            }

            else if (_currentEnemy.Health <= 0)
            {
                
                Console.ReadKey();
                Console.Clear();
                Console.WriteLine("You slayed the " + _currentEnemy.Name);

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
                _currentScene = 2;
            }
            return stopGame;
        }

        void ResetCurrentEnemy()
        {
            _currentEnemyIndex = 0;

            _currentEnemy = _enemies[_currentEnemyIndex];
        }
    }
}
