using System;

//Author: Tamires Boniolo

namespace ConnectFourGame
{
    class Program
    {
        static Random randomSquare = new Random();

        static void Main(string[] args)
        {
            Console.WriteLine("Author: Tamires Boniolo");
            Console.WriteLine();
            Console.WriteLine("WELCOME TO CONNECT FOUR GAME! LET'S PLAY!!");
            Console.WriteLine("==========================================================================================");
            Console.WriteLine();
            Console.WriteLine("You are Player 1 and you play with the X. Computer is Player 2 and plays with the O.");
            Console.WriteLine();
            Console.WriteLine("==========================================================================================");
            Console.WriteLine();


            //using a linear array - 8x8
            int[] boardGrid = new int[64];

            int winner = 0;
            int numTurns = 0;

            // Initialize the grid
            InitializeGrid(boardGrid);

            int currentPlayer = 1;
            int selectedColumn;
            int selectedRow;

            // As long as we don't have a winner 
            bool gameOver = false;
            while (!gameOver)
            {
                numTurns++;

                //If the player is the user, ask for input
                if (currentPlayer == 1)
                {
                    Console.WriteLine("It's your turn! Here is the current grid:");
                    // Show the grid
                    PrintGrid(boardGrid);

                    do
                    {
                        Console.WriteLine("Which column would you like to drop a checker into (1-8)?");
                        //ask for a column and make sure it is an integer on the valid range
                        while (!int.TryParse(Console.ReadLine(), out selectedColumn) || selectedColumn < 1 || selectedColumn > 8)
                        {
                            Console.WriteLine("Invalid selection! Column must be between 1 and 8, and must not be already taken.");
                            Console.WriteLine("Which column would you like to drop a checker into (1-8)?");
                        }
                        //if column is full it will be equal to -1, and it is not valid, so will ask the user to choose another column.
                        if (IsColumnAvailable(boardGrid, selectedColumn - 1) == -1)
                        {
                            Console.WriteLine("Invalid selection! Column is full already. Choose another Column please.");
                        }
                        //If not valid column, keep asking
                    } while (IsColumnAvailable(boardGrid, selectedColumn - 1) == -1);

                    //Reduce one in each column since the players chooses the 1-8 range and the board is indexed from 0-7.
                    selectedColumn--;
                    selectedRow = IsColumnAvailable(boardGrid, selectedColumn); //get the row that the checker was dropped
                    boardGrid[selectedColumn * 8 + selectedRow] = currentPlayer;
                }
                else //player is the computer, choose a random square
                {
                    Console.WriteLine("It's the computer's turn.");
                    selectedColumn = IsEmpty(boardGrid);
                    selectedRow = IsColumnAvailable(boardGrid, selectedColumn);
                    boardGrid[selectedColumn * 8 + selectedRow] = currentPlayer;
                    Console.WriteLine("The computer droped a checker in column:{0}", selectedColumn + 1);
                    Console.WriteLine();
                }
                Console.WriteLine();

                // If the player get 4 in a row
                if (FoundFourConnected(boardGrid, selectedColumn, selectedRow))
                {
                    gameOver = true;
                    winner = currentPlayer;
                }
                else if (numTurns == 64)
                {
                    // If squares are full and we do not have a winner. Display tie game.
                    gameOver = true;
                    winner = 0;
                }
                else
                {
                    // if it is not true, it is the other player turn, change the players
                    currentPlayer = (currentPlayer == 2) ? 1 : 2;
                }

            }

            // Display who is the winner or if it is a tie
            if (winner == 0)
            {
                Console.WriteLine("It is a tie! Press any key to quit.");
                Console.ReadLine();
            }
            else
            {
                PrintGrid(boardGrid);
                Console.WriteLine($"Yahoo player {winner} won! Congratulations!! :). Press any key to quit.");
                Console.ReadLine();
            }

        }

        private static void InitializeGrid(int[] boardGrid)
        {
            for (int i = 0; i < 64; i++)
            {
                boardGrid[i] = 0;
            }
        }

        //Print the board grid
        private static void PrintGrid(int[] boardGrid)
        {
            for (int x = 0; x < 8; x++)
            {
                for (int i = 0; i < 8; i++)
                {
                    char xo = " XO"[boardGrid[i * 8 + x]];
                    Console.Write(" " + xo);
                    if (i <= 6)
                        Console.Write(" |");
                }
                Console.WriteLine();
                if (x < 7)
                    Console.WriteLine("---+---+---+---+---+---+---+---");
            }
            Console.WriteLine();

        }

        //Returns the column for the empty cell found
        private static int IsEmpty(int[] boardGrid)
        {
            int i;
            do
            {
                i = randomSquare.Next(64);
            } while (boardGrid[i] != 0);

            return (i / 8);
        }

        //Returns the row number that the checker should fall, if the column is full, return -1
        private static int IsColumnAvailable(int[] boardGrid, int selectedColumn)
        {
            int rowNumber = 7;
            while (rowNumber >= 0 && boardGrid[selectedColumn * 8 + rowNumber] != 0)
            {
                rowNumber--;
            }
            return rowNumber;
        }

        //If there is a four connected, it must include the last played piece, check if the last played piece makes a four connected
        private static bool FoundFourConnected(int[] boardGrid, int selectedColumn, int selectedRow)
        {
            //At least one is always connected
            int numConnected = 1;

            //gets the player that played the last piece
            int value = boardGrid[selectedColumn * 8 + selectedRow];
            //Check Horizontal to the left of the selected piece and sum if found a piece from the same player
            int i = 1;
            while ((selectedColumn - i >= 0) && value == boardGrid[(selectedColumn - i) * 8 + selectedRow])
            {
                i++;
                numConnected++;
            }
            //Check Horizontal to the right of the selected piece and sum if found a piece from the same player
            i = 1;
            while ((selectedColumn + i < 8) && value == boardGrid[(selectedColumn + i) * 8 + selectedRow])
            {
                i++;
                numConnected++;
            }

            //Check if we have 4 connected in the horizontal
            if (numConnected >= 4) return true;

            //If we reached this point, the player has not won in the horizontal, lets check vertical.
            numConnected = 1; //Reset the number of connected pieces on the vertical to one.

            //Check Vertical to the up of the selected piece and sum if found a piece from the same player
            i = 1;
            while ((selectedRow - i >= 0) && value == boardGrid[(selectedColumn) * 8 + selectedRow - i])
            {
                i++;
                numConnected++;
            }
            //Check Vertical to the bottom of the selected piece and sum if found a piece from the same player
            i = 1;
            while ((selectedRow + i < 8) && value == boardGrid[(selectedColumn) * 8 + selectedRow + i])
            {
                i++;
                numConnected++;
            }

            //Check if we have 4 connected in the vertical
            if (numConnected >= 4) return true;

            return false;
        }
    }

}
