#nullable enable
using System;
using static System.Console;

namespace Bme121
{    
    class Player
    {
        public readonly string Colour;
        public readonly string Symbol;
        public readonly string Name;
        
        public Player( string Colour, string Symbol, string Name )
        {
            this.Colour = Colour;
            this.Symbol = Symbol;
            this.Name = Name;
        }
    }
    
    static partial class Program
    {
        // Display common text for the top of the screen.
        static void Welcome( )
        {
			WriteLine("Welcome to Othello: Professor Freeman's Powershell Edition!");
			WriteLine("To play, enter your move with the letter of the row then the letter of the column, like so: 'rc'");
			WriteLine("Type 'skip' to skip your turn or 'quit' to end the game.");
        }
        
        // Collect a player name or default to form the player record.
        static string PlayerInformation(int playerNum)
        {
			WriteLine("Player {0}, please enter your name: ",playerNum);
			string info = ReadLine();
			if(info==""){
				return "Player "+playerNum;
			}
			return info;
		}
        
        static Player NewPlayer( string colour, string symbol, string defaultName )
        {
            return new Player(colour, symbol, defaultName);
        }
        
        // Determine which player goes first randomly.
        static int GetFirstTurn( Player[ ] players, int defaultFirst )
        {	
            Random rnd = new Random();
            return rnd.Next(0,2);
        }
        
        // Get a board size (between 4 and 26 and even) or default, for one direction.
        
        static int GetBoardSize( string direction, int defaultSize )
        {	
			WriteLine("Please enter an even integer for the desired number of {0} on the board: ",direction);
			
			string dim = ReadLine();
			
			if(dim==""){
				return defaultSize;
			}
			
			int dimension = int.Parse(dim);
			
			while(dimension%2==1 || dimension<4 || dimension>26){
				WriteLine("Invalid number. Please enter an even integer in between 4 and 26 inclusive: ");
				dimension = int.Parse(ReadLine());
			}
			
            return dimension;
        }
        
        // Get a move from a player.
        static string GetMove( Player player )
        {
            return ReadLine();
        }
        
        // Try to make a move. Return true if it worked.
        static bool[] TryMove( string[ , ] board, Player player, string move)
        {	
			
			//Looking back, BFS would have been much more effective than DFS, but I don't know the syntax for BFS in C#
			//and by the time I thought of that it was too late lol
			
			//Get the column and row of the move and convert them to integers
            string srow = move.Substring(0,1);
            string scol = move.Substring(move.Length-1);
            int row = IndexAtLetter(srow);
            int col = IndexAtLetter(scol);
            //This array will be used to determine the validity of each direction, with valid[0] being in the
            //top left direction and valid[1] being in the top direction, moving clockwise.
            bool [] valid = new bool[8]{false,false,false,false,false,false,false,false};
																						
			//Each direction is checked as follows: first, an if statement checks if a move is possible in that direction
			//Then, if it is possible (i.e. the board still exists and the opponent's tile is there), a while loop runs through
			//that direction and checks if the move is valid. If so, it sets its part in the valid array to true, and breaks out of
			//the loop. Then, it moves on to the next direction, for a total of 8 directions.
			//Example:
			//If you can go in the top left direction, and it isn't black, and it is the opponent's symbol....
			if(row-1!=-1 && col-1!=-1 && board[row-1,col-1]!=" " && board[row-1,col-1]!=player.Symbol){
				while(row-1!=-1 && col-1!=-1 && board[row-1,col-1]!=" "){	//Checking top-left direction
					row--;
					col--;
					//If you reached one of your squares, the move is valid, because there are continuous opponent squares until that point
					if(board[row,col]==player.Symbol){
						valid[0] = true;
						WriteLine("Works in top-left direction");
						break;
					}
				}
			}
			//Resetting intitial variables
			row = IndexAtLetter(srow);
			col = IndexAtLetter(scol);
			if(row-1>-1 && board[row-1,col]!=" " && board[row-1,col]!=player.Symbol){
				while(row-1>-1 && board[row-1,col]!=" "){					//Checking top direction
					row--;
					if(board[row,col]==player.Symbol){
						valid[1] = true;
						WriteLine("Works in top direction");
						break;
					}
				}
			}
			
			row = IndexAtLetter(srow);
			col = IndexAtLetter(scol);
			if(row-1>-1 && col+1<board.GetLength(1) && board[row-1,col+1]!=" " && board[row-1,col+1]!=player.Symbol){
				while(row-1>-1 && col+1<board.GetLength(1) && board[row-1,col+1]!=" "){	//Checking top-right direction
					row--;
					col++;
					if(board[row,col]==player.Symbol){
						valid[2] = true;
						WriteLine("Works in top-right direction");
						break;
					}
				}
			}
			
			row = IndexAtLetter(srow);
			col = IndexAtLetter(scol);	
			if(col+1<board.GetLength(1) && board[row,col+1]!=" " && board[row,col+1]!=player.Symbol){				
				while(col+1<board.GetLength(1) && board[row,col+1]!=" "){					//Checking right direction
					col++;
					if(board[row,col]==player.Symbol){
						valid[3] = true;
						WriteLine("Works in right direction");
						break;
					}
				}
			}
            
            row = IndexAtLetter(srow);
            col = IndexAtLetter(scol);
            if(row+1<board.GetLength(0) && col+1<board.GetLength(1) && board[row+1,col+1]!=" " && board[row+1,col+1]!=player.Symbol){
				while(row+1<board.GetLength(0) && col+1<board.GetLength(1) && board[row+1,col+1]!=" "){//Checking bottom-right direction
					row++;
					col++;
					if(board[row,col]==player.Symbol){
						valid[4] = true;
						WriteLine("Works in bottom-right direction");
						break;
					}
				}
			}
			
			row = IndexAtLetter(srow);
			col = IndexAtLetter(scol);
			if(row+1<board.GetLength(0) && board[row+1,col]!=" " && board[row+1,col]!=player.Symbol){
				while(row+1<board.GetLength(0) && board[row+1,col]!=" "){			//Checking bottom direction
					row++;
					WriteLine("Checking "+row+","+col+" to see if same");
					if(board[row,col]==player.Symbol){
						valid[5] = true;
						WriteLine("Works in bottom direction");
						break;
					}
				}
			}
			
			row = IndexAtLetter(srow);
			col = IndexAtLetter(scol);
			if(row+1<board.GetLength(0) && col-1>-1 && board[row+1,col-1]!=" " && board[row+1,col-1]!=player.Symbol){
				while(row+1<board.GetLength(0) && col-1>-1 && board[row+1,col-1]!=" "){//Checking bottom-left direction
					row++;
					col--;
					if(board[row,col]==player.Symbol){
						valid[6] = true;
						WriteLine("Works in bottom-left direction");
						break;
					}
				}
			}
			
			row = IndexAtLetter(srow);
			col = IndexAtLetter(scol);
			if(col-1>-1 && board[row,col-1]!=" " && board[row,col-1]!=player.Symbol){
				while(col-1>-1 && board[row,col-1]!=" "){								//Checking left direction
					col--;
					if(board[row,col]==player.Symbol){
						valid[7] = true;
						WriteLine("Works in left direction");
						break;
					}
				}
			}
			//Returns the array of directions for which the move is valid.
            return valid;
            
        }
        
        // Do the flips along a direction specified by the row and column delta for one step.
        
        static void TryDirection( string[ , ] board, Player player,
            int moveRow, int deltaRow, int moveCol, int deltaCol)
        {	
			//Sets the move square to the player's symbol
			board[moveRow,moveCol] = player.Symbol;
			//Moves in a direction, and flips each piece along the way
			while(board[moveRow+deltaRow,moveCol+deltaCol]!=player.Symbol){
				board[moveRow+deltaRow,moveCol+deltaCol] = player.Symbol;
				moveRow+=deltaRow;
				moveCol+=deltaCol;
			}
            return;
        }
        
        // Count the discs to find the score for a player.
        static int GetScore( string[ , ] board, Player player )
        {	
			int count = 0;
			for(int i=0;i<board.GetLength(0);i++){
				for(int k=0;k<board.GetLength(1);k++){
					if(board[i,k]==player.Symbol){
						count++;
					}
				}
			}
            return count;
        }
        
        // Display a line of scores for all players.
        
        static void DisplayScores( string[ , ] board, Player[ ] players )
        {
			WriteLine($"{players[0].Name}, with symbol {players[0].Symbol}, has currently scored {GetScore(board,players[0])} tiles");
			WriteLine($"{players[1].Name}, with symbol {players[1].Symbol}, has currently scored {GetScore(board,players[1])} tiles");
        }
        
        // Display winner(s) and categorize their win over the defeated player(s).
        
        static void DisplayWinners( string[ , ] board, Player[ ] players )
        {	
			int score1 = GetScore(board,players[0]);
			int score2 = GetScore(board,players[1]);
			if(score1>score2){
				WriteLine($"{players[0].Name}, with symbol {players[0].Symbol}, won the game with {score1} tiles.");
				WriteLine($"{players[1].Name}, with symbol {players[1].Symbol}, lost with {score2} tiles.");
			}else if(score1<score2){
				WriteLine($"{players[1].Name}, with symbol {players[1].Symbol}, won the game with {score2} tiles.");
				WriteLine($"{players[0].Name}, with symbol {players[0].Symbol}, lost with {score1} tiles.");
			}else{
				WriteLine($"Both players tied the game with {score1} tiles.");
			}
        }
        
        static void Main( )
        {            
            Welcome( );
            //Get player information
            string p1 = PlayerInformation(1);
			string p2 = PlayerInformation(2);
            //Create the players
            Player[ ] players = new Player[ ] 
            {					
                NewPlayer("white", "O", p1 ),
                NewPlayer("black", "X", p2 ),
            };
            
            int turn = GetFirstTurn( players, defaultFirst: 0 );
           
            int rows = GetBoardSize( direction: "rows",    defaultSize: 8 );
            int cols = GetBoardSize( direction: "columns", defaultSize: 8 );
            
            string[ , ] game = NewBoard( rows, cols );
            
            // Play the game.
            
            bool gameOver = false;
            bool moveWorks = false;
            bool [] madeMove = new bool [8];
            string move;
            Welcome( );
            while(gameOver==false)
            {	
				moveWorks = false;				
				//Display game
                DisplayBoard( game ); 
                DisplayScores( game, players );
                //Get move from player
                WriteLine("It is "+players[turn].Name+"'s turn");
                move = GetMove( players[ turn ] );
                WriteLine(move);
                //If player wants to end game, game ends
                if( move == "quit" ){
					gameOver = true;
				//If they did not want to skip their turn, check their move. If it is a valid move, do the move in each possible direction
				}else if( move != "skip"){
                    madeMove = TryMove( game, players[ turn ], move);
                    for(int i=0;i<madeMove.Length;i++){
						if(madeMove[i]) moveWorks = true;
					}
                    if(madeMove[0]) TryDirection(game,players[turn],IndexAtLetter(move.Substring(0,1)),-1,IndexAtLetter(move.Substring(move.Length-1)),-1);
                    if(madeMove[1]) TryDirection(game,players[turn],IndexAtLetter(move.Substring(0,1)),-1,IndexAtLetter(move.Substring(move.Length-1)),0);
                    if(madeMove[2]) TryDirection(game,players[turn],IndexAtLetter(move.Substring(0,1)),-1,IndexAtLetter(move.Substring(move.Length-1)),1);
                    if(madeMove[3]) TryDirection(game,players[turn],IndexAtLetter(move.Substring(0,1)),0,IndexAtLetter(move.Substring(move.Length-1)),1);
                    if(madeMove[4]) TryDirection(game,players[turn],IndexAtLetter(move.Substring(0,1)),1,IndexAtLetter(move.Substring(move.Length-1)),1);
                    if(madeMove[5]) TryDirection(game,players[turn],IndexAtLetter(move.Substring(0,1)),1,IndexAtLetter(move.Substring(move.Length-1)),0);
                    if(madeMove[6]) TryDirection(game,players[turn],IndexAtLetter(move.Substring(0,1)),1,IndexAtLetter(move.Substring(move.Length-1)),-1);
                    if(madeMove[7]) TryDirection(game,players[turn],IndexAtLetter(move.Substring(0,1)),0,IndexAtLetter(move.Substring(move.Length-1)),-1);
                    //Give turn to next player
                    if( moveWorks ) turn = ( turn + 1 ) % players.Length;
                    else 
                    {
                        Write( " Your choice didn't work!" );
                        Write( " Press <Enter> to try again." );
                        ReadLine( ); 
                    }
                //If they wanted to skip their turn, skip it
                }else{
					WriteLine(" Turn skipped.");
					turn = ( turn + 1 ) % players.Length;
				}
            }
            
            // Show fhe final results.
            
            DisplayWinners( game, players );
            WriteLine( );
        }
    }
}
