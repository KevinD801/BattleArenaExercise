using System;
using System.Collections.Generic;
using System.Text;

namespace BattleArena
{
    // Test commit

    /// <summary>
    /// Represents any entity that exists in game
    /// </summary>
    struct Character
    {
        public string name;
        public float health;
        public float attackPower;
        public float defensePower;
    }

    class Game
    {
        bool gameOver;
        int currentScene;
        Character player;
        Character[] enemies;
        private int currentEnemyIndex = 0;
        private Character currentEnemy;

        // Enemies
        Character slime;
        Character zomb;
        Character kris;


        /// <summary>
        /// Function that starts the main game loop
        /// </summary>
        public void Run()
        {
            Start();

            while (!gameOver)
            {
                gameOver = true;
            }
            End();

        }

        /// <summary>
        /// Function used to initialize any starting values by default
        /// </summary>
        public void Start()
        {
            //Player
            player.name = "";
            player.health = 0;
            player.attackPower = 0;
            player.defensePower = 0;
            
            // Enemies
            slime.name = "Slime";
            slime.health = 10f;
            slime.attackPower = 1f;
            slime.defensePower = 0f;

            zomb.name = "Zom-b";
            zomb.health = 15f;
            zomb.attackPower = 5f;
            zomb.defensePower = 2f;

            kris.name = "guy named Kris";
            kris.health = 25f;
            kris.attackPower = 10f;
            kris.defensePower = 5f;

            enemies = new Character[] { slime, zomb, kris };

            ResetCurrentEnemy();
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
        int GetInput(string description, string option1, string option2)
        {
            string input = "";
            int inputReceived = 0;

            while (inputReceived != 1 && inputReceived != 2)
            {//Print options
                Console.WriteLine(description);
                Console.WriteLine("1. " + option1);
                Console.WriteLine("2. " + option2);
                Console.Write("> ");

                //Get input from player
                input = Console.ReadLine();

                //If player selected the first option...
                if (input == "1" || input == option1)
                {
                    //Set input received to be the first option
                    inputReceived = 1;
                }
                //Otherwise if the player selected the second option...
                else if (input == "2" || input == option2)
                {
                    //Set input received to be the second option
                    inputReceived = 2;
                }
                //If neither are true...
                else
                {
                    //...display error message
                    Console.WriteLine("Invalid Input");
                    Console.ReadKey();
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
            switch (currentScene)
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


            //If the player decides to restart...
            if (input == 1)
            {
                //...set their current area to be the start and update the player state to be alive
                ResetCurrentEnemy();
                currentScene = 0;

            }
            //Otherwise if the player wants to quit...
            else if (input == 2)
            {
                //...set game over to be true
                gameOver = true;
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
                Console.WriteLine("Welcome! Please enter your name.");
                Console.Write("> ");
                player.name = Console.ReadLine();

                Console.Clear();

                int input = GetInput("You've entered " + player.name + " are you sure you want to keep this name?",
                    "Keep Name", " Rename");

                if (input == 1)
                {
                    userName = true;
                }
            }
            string name = Console.ReadLine();

            int input = GetInput("Do you wish change your name", "Rename", "Keep Name");
            
            if(input == 1)
            {
                GetPlayerName();
            }
            else if (input == 2)
            {
                return;
            }
        }

        /// <summary>
        /// Gets the players choice of character. Updates player stats based on
        /// the character chosen.
        /// </summary>
        public void CharacterSelection()
        {
            int input = GetInput("Nice to meet you" + player.name + ". Please select a character." ,
                "Knight", "Wizard");
            Console.Write("> ");

            if (input == 1)
            {
                player.health = 50f;
                player.attackPower = 25f;
                player.defensePower = 5f;
            }
            else if (input == 2)
            {
                player.health = 75f;
                player.attackPower = 15f;
                player.defensePower = 10f;
            }
            currentScene++;
        }

        /// <summary>
        /// Prints a characters stats to the console
        /// </summary>
        /// <param name="character">The character that will have its stats shown</param>
        void DisplayStats(Character character)
        {
            Console.WriteLine("Name: " + character.name);
            Console.WriteLine("Health: " + character.health);
            Console.WriteLine("Attack Power: " + character.attackPower);
            Console.WriteLine("Defense Power: " + character.defensePower);
        }

        /// <summary>
        /// Calculates the amount of damage that will be done to a character
        /// </summary>
        /// <param name="attackPower">The attacking character's attack power</param>
        /// <param name="defensePower">The defending character's defense power</param>
        /// <returns>The amount of damage done to the defender</returns>
        float CalculateDamage(float attackPower, float defensePower)
        {
            return attackPower - defensePower;
        }

        /// <summary>
        /// Deals damage to a character based on an attacker's attack power
        /// </summary>
        /// <param name="attacker">The character that initiated the attack</param>
        /// <param name="defender">The character that is being attacked</param>
        /// <returns>The amount of damage done to the defender</returns>
        public float Attack(ref Character attacker, ref Character defender)
        {
            float damageTaken = CalculateDamage(attacker.attackPower, defender.defensePower);
            if (damageTaken >= 0)
            {
                defender.health -= damageTaken;
            }
            else if (damageTaken < 0)
            {
                damageTaken = 0;
            }
            return damageTaken;
        }

        /// <summary>
        /// Simulates one turn in the current monster fight
        /// </summary>
        public void Battle()
        {
            // Display stats on Player
            DisplayStats(player);

            // Display stats on Enemies 
            DisplayStats(currentEnemy);

            int input = GetInput("A " + currentEnemy.name + " stands in front of you! What will you do?", 
                "Attack", "Dodge");

            if (input == 1)
            {
                // The player attacks the enemy
                float damageDealt = Attack(ref player, ref currentEnemy);
                Console.WriteLine("You dealt " + damageDealt + " damage!");

                // The enemy attacks the player
                damageDealt = Attack(ref currentEnemy, ref player);
                Console.WriteLine("The " + currentEnemy.name + " dealt " + damageDealt);
            }
            else if (input == 2)
            {
                Console.WriteLine("You dodged the enemy's attack!");
            }

        }

        /// <summary>
        /// Checks to see if either the player or the enemy has won the current battle.
        /// Updates the game based on who won the battle..
        /// </summary>
        void CheckBattleResults()
        {
            if (currentEnemy.health <= 0)
            {
                Console.ReadKey(true);
                Console.Clear();
                Console.WriteLine("You slayed the " + currentEnemy.name);

                currentEnemyIndex++;

                if (TryStopGame())
                {
                    return;
                }

                currentEnemy = enemies[currentEnemyIndex];
            }
        }

        bool TryStopGame()
        {
            bool stopGame = currentEnemyIndex >= enemies.Length;

            if (stopGame)
            {
                currentScene = 2;
            }
            return stopGame;
        }

        void ResetCurrentEnemy()
        {
            currentEnemyIndex = 0;

            currentEnemy = enemies[currentEnemyIndex];
        }

        void Scene()
        {
            Console.WriteLine("A Slime stands ");
            Console.WriteLine("You slayed the Slime");
        }

        void Scene2()
        {
            Console.WriteLine("A Zom-B stands in front of you! What will you do?");
            Console.WriteLine("You slayed the Zom-B");
        }

        void Scene3()
        {
            Console.WriteLine("A guy named Kris stands in front of you! What will you do?");
            Console.WriteLine("You slayed the guy named Kris");
        }
    }
}
