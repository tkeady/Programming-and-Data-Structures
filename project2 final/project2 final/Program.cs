using System;
using System.Collections.Generic;
using System.Threading;

namespace project2b
{
    class Program
    {
        public class Game // 'Game' class
        {
            static Player[] players;
            static int turn = 0;
            static int maximumScore = 50;
            public bool isGameOver = false;
            public Game(int playerAmount, int nowMaximumScore)
            {
                if (playerAmount == 1)
                {
                    players = new Player[playerAmount + 1];
                    players[0] = new Player(false); // Check if AI is needed
                    players[1] = new Player(true);
                }
                else
                {
                    players = new Player[playerAmount]; // Add new human player
                    for (int i = 0; i < playerAmount;
                        i++)
                    {
                        players[i] = new Player(false);
                    }
                }
                maximumScore = nowMaximumScore; // Set score
            }
            public void nextTurn()
            {
                Console.WriteLine("Three or more dice game"); // User intro to game
                Console.WriteLine("Players: " + players.Length); // Player amount
                Console.WriteLine("Goal: " + maximumScore);

                players[turn].turn();
                if (players[turn].getScore() >= maximumScore)
                {
                    endGame(turn); // End game conditions reached
                }
                else
                {
                    turn++; // New turn
                    if (turn > players.Length - 1)
                    {
                        turn = 0;
                    }
                }
            }
            public void endGame(int winner) // For when the target score is attained
            {
                Console.Clear(); // Clear display
                Console.WriteLine(players[winner].getName() + " wins"); // Winner declared
                Console.ReadKey(true); // Game will loop otherwise

                Console.WriteLine("Statistics: "); // Player statistics
                int totalDiceRolls = 0;
                int[] diceRollValues = { 0, 0, 0, 0, 0, 0 };
                int totalNumber = 0;
                for (int i = 0; i < players.Length; i++)
                {
                    Console.WriteLine(i);
                    totalDiceRolls += players[i].previousRolls.Count;
                    foreach (int roll in players[i].previousRolls)
                    {
                        diceRollValues[roll - 1]++;
                        totalNumber = totalNumber + roll;
                    }
                    isGameOver = true;
                }
                int averageRoll = totalNumber / totalDiceRolls;
                // Displays all statistics on console
                Console.WriteLine("Total dice rolls: " + totalDiceRolls);
                Console.WriteLine("1 was rolled " + diceRollValues[0] + " times.");
                Console.WriteLine("2 was rolled " + diceRollValues[1] + " times.");
                Console.WriteLine("3 was rolled " + diceRollValues[2] + " times.");
                Console.WriteLine("4 was rolled " + diceRollValues[3] + " times.");
                Console.WriteLine("5 was rolled " + diceRollValues[4] + " times.");
                Console.WriteLine("6 was rolled " + diceRollValues[5] + " times.");
                Console.WriteLine("Average roll: " + averageRoll);
                Console.WriteLine("Total dice number: " + totalNumber);
                Console.ReadLine();
            }
        }
        public class Player // 'Player' class
        {
            int score = 0;
            bool PrimitiveAI;
            public List<int> previousRolls = new List<int>(); // List to store old turns
            string name = "player";
            Die[] dice = new Die[5];
            public Player(bool isAI) // For computer controlled opponent

            {
                PrimitiveAI = isAI;
                for (int i = 0; i < 5; i++)
                {
                    dice[i] = new Die();
                }
                if (PrimitiveAI == false)
                {
                    Console.WriteLine("Enter name"); // Player 2 name entry
                    name = Console.ReadLine();
                }
                else
                {
                    name = "Dice Master"; // AI name
                }
            }
            public int turn()
            {
                int turnScore = 0;
                Console.WriteLine("Your turn, " + name + ".\nScore: " + score + "\n(1) - Throw and reroll three\n(2) - Throw all dice *double points*"); // Turn options
                char option;
                if (PrimitiveAI == true)
                {
                    Thread.Sleep(250); // These indicate momentary pauses
                    option = '1';
                }
                else
                {
                    option = Console.ReadKey().KeyChar;
                }
                if (option == '1') // If user selects option 1 to throw repeatedly
                {
                    Console.WriteLine("\nPress any key to roll");
                    for (int i = 0; i < 5; i++)
                    {
                        if (PrimitiveAI == true)
                        {
                            Thread.Sleep(250);
                        }
                        else
                        {
                            Console.ReadKey(true);
                        }
                        dice[i].roll();
                        Console.Write(", "); // Displays each dice roll
                        previousRolls.Add(dice[i].getValue()); // Adds new dice score to current score
                    }

                    Console.WriteLine("\nAll dice rolled. Press any key to continue");
                    if (PrimitiveAI == true)
                    {
                        Thread.Sleep(500);
                    }
                    else
                    {
                        Console.ReadKey(true);
                    }
                    turnScore = calculateScore();
                    addScore(turnScore);
                }
                else if (option == '2') // If user selects option 2
                {
                    Console.WriteLine("\nPress any key to throw all dice at once");
                    for (int i = 0; i < 5; i++)
                    {
                        dice[i].roll();
                        Console.Write(", ");
                        previousRolls.Add(dice[i].getValue());
                    }
                    Console.WriteLine("\nAll dice thrown, press any key to continue");
                    if (PrimitiveAI == true)
                    {
                        Thread.Sleep(500);
                    }
                    else
                    {
                        Console.ReadKey(true);
                    }
                    turnScore = calculateScore(true) * 2;
                    addScore(turnScore);
                }
                Console.WriteLine("You scored " + turnScore, "Press any key to continue");
                if (PrimitiveAI == true)
                {
                    Thread.Sleep(1000);
                }
                else
                {
                    Console.ReadKey(true);
                }
                return 0;
            }
            public int calculateScore(bool threwAll = false)
            {
                Console.WriteLine("Result:");
                int[] results = { 0, 0, 0, 0, 0, 0 }; // Shows results
                for (int i = 0; i < 5; i++)
                {
                    results[dice[i].getValue() - 1]++; // Goes through dice
                }
                int[] bestResult = calculateBestResult(results); // Finds maximum possible score from turn
                Console.WriteLine(bestResult[0] + " of a kind! (" + bestResult[1] + ")");
                if (bestResult[0] == 2 && threwAll == false) // If low score gives chance to re-roll three dice
                {
                    Console.WriteLine("Reroll three dice, press any key to continue");
                    for (int i = 0; i < 5; i++)
                    {
                        if (dice[i].getValue() != bestResult[1])
                        {
                            if (PrimitiveAI == true)
                            {
                                Thread.Sleep(250);
                            }
                            else
                            {
                                Console.ReadKey(true);
                            }
                            dice[i].roll();
                            results[dice[i].getValue() - 1]++;
                            Console.Write(", ");
                            previousRolls.Add(dice[i].getValue());
                        }
                    }
                    Console.WriteLine("\nAll dice thrown, press any key to continue");
                    if (PrimitiveAI == true)
                    {
                        Thread.Sleep(500);
                    }
                    else
                    {
                        Console.ReadKey(true);
                    }
                }
                bestResult = calculateBestResult(results);
                switch (bestResult[0]) // Gives points depending on score
                {
                    case 3: // If 3 match
                        return 3;
                    case 4: // If 4 match
                        return 6;
                    case 5: // If 5 match
                        return 12;
                    default:
                        return 0;
                }
            }
            public int[] calculateBestResult(int[] results)
            {
                int[] bestResult = { 1, 0 }; // Value, dice number

                for (int i = 0; i < 6; i++)
                {
                    if (results[i] >= bestResult[0])
                    {
                        bestResult[0] = results[i];
                        bestResult[1] = i + 1;
                    }
                }

                return bestResult;
            }

            public void addScore(int toAdd)
            {
                score += toAdd; // Score addition function
            }
            public int getScore()
            {
                return score;
            }
            public string getName()
            {
                return name;
            }
        }
        public class Die // 'Die' class
        {
            int dieValue;
            static Random round = new Random(); // Picks random number
            public Die()
            { // Needs brackets to work
            }
            public void roll()
            {
                dieValue = round.Next(1, 7);
                Console.Write(dieValue);
            }

            public int getValue()
            {
                return dieValue;
            }
        }
        static void Main(string[] args)
        {
            bool isPlaying = true;
            while (isPlaying == true)
            {
                Console.Clear(); // Clear display for intro
                Console.WriteLine("Dice Game");
                Console.Write("Number of players: ");
                int playerCount = 1;
                try
                {
                    playerCount = Convert.ToInt32(Console.ReadLine());
                }
                catch (FormatException e)
                {
                    Console.WriteLine("Error: " + e); // Error catch
                }
                Console.Write("Enter what you would like the goal to be ");
                int maxScore = 50;
                try
                {
                    maxScore = Convert.ToInt32(Console.ReadLine());
                }
                catch (FormatException e)
                {
                    Console.WriteLine("Error: " + e);
                }
                Game game = new Game(playerCount, maxScore); // Resets game
                while (game.isGameOver == false)
                {
                    Console.Clear();
                    game.nextTurn(); // New turn
                }
                while (1 == 1)
                {
                    Console.WriteLine("\nWould you like to play again? y/n Play again? Please only enter 'y' or 'n':");
                    char option = Console.ReadKey().KeyChar;
                    if (option == 'n')
                    {
                        isPlaying = false;
                        break;
                    }
                    else if (option == 'y')
                    {
                        break;
                    }
                }
            }
        }

    }
    // 16608272 Thomas Keady
}