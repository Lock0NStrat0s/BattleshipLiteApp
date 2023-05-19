using BattleshipLiteLibrary;
using BattleshipLiteLibrary.Models;

WelcomeMessage();

PlayerInfoModel activePlayer = CreatePlayer("Player 1");
PlayerInfoModel opponent = CreatePlayer("Player 1");
PlayerInfoModel winner = null;

do
{
    // Display grid from activePlayer on where they fired
    DisplayShotGrid(activePlayer);

    // Ask activePlayer for a shot
    // Determine if it is a valid shot
    // Determine shot results
    RecordPlayerShot(activePlayer, opponent);

    // Determine if game should continue
    bool doesGameContinue = GameLogic.PlayerStillActive(opponent);

    // If over, set activePlayer as winner,
    // else, swap positions (activePlayer to opponent)
    if (doesGameContinue)
    {
        // Swap using a temp variable - OLD METHOD
        //PlayerInfoModel tempHolder = opponent;
        //opponent = activePlayer;
        //activePlayer = tempHolder;

        // Tuple method - simply swap positions without a temp variable
        (activePlayer, opponent) = (opponent, activePlayer);
    }
    else
    {
        winner = activePlayer;
    }

    IdentifyWinner(winner);

    // Clear display
    Console.Clear();

} while (winner == null);

static void IdentifyWinner(PlayerInfoModel winner)
{
    Console.WriteLine($"Congratulations to {winner.UsersName} for winning.");
    Console.WriteLine($"{winner.UsersName} took {GameLogic.GetShotCount(winner)}");
}

static void RecordPlayerShot(PlayerInfoModel activePlayer, PlayerInfoModel opponent)
{
    bool isValidShot = false;
    string row = "";
    int column = 0;

    do
    {
        // Asks for a shot (we ask for "B2" not "B" and then "2")
        string shot = AskForShot();
        // Determine what row and column that is (split it apart)
        (row, column) = GameLogic.SplitShotIntoRowAndColumn(shot);
        // Determine if that was a valid shot
        isValidShot = GameLogic.ValidateShot(activePlayer, row, column);

        // Go back to the beginning if not a valid shot
        if (!isValidShot)
        {
            Console.WriteLine("Invalid shot location. Please try again!");
        }
    } while (!isValidShot);

    // Determine shot results
    bool isAHit = GameLogic.IdentifyShotResult(opponent, row, column);

    // Record results
    GameLogic.MarkShotResult(activePlayer, row, column, isAHit);
}

static string AskForShot()
{
    Console.Write("Please enter your shot selection: ");
    string output = Console.ReadLine();

    return output;
}

static void DisplayShotGrid(PlayerInfoModel activePlayer)
{
    string currentRow = activePlayer.ShotGrid[0].SpotLetter;
    foreach (var gridSpot in activePlayer.ShotGrid)
    {
        if (gridSpot.SpotLetter != currentRow)
        {
            Console.WriteLine();
            currentRow = gridSpot.SpotLetter;
        }
        if (gridSpot.Status == GridSpotStatus.Empty)
        {
            Console.Write($" {gridSpot.SpotLetter}{gridSpot.SpotNumber} ");
        }
        else if (gridSpot.Status == GridSpotStatus.Hit)
        {
            Console.Write(" X ");
        }
        else if (gridSpot.Status == GridSpotStatus.Miss)
        {
            Console.Write(" O ");
        }
        else
        {
            Console.Write(" ? ");
        }
    }
}

static void WelcomeMessage()
{
    Console.WriteLine("Welcome to Battleship Lite\nCreated by Mohammad Jabir\n");
}

static PlayerInfoModel CreatePlayer(string playerTitle)
{
    PlayerInfoModel output = new PlayerInfoModel();

    Console.WriteLine($"Player information for {playerTitle}");

    // Ask user for their name
    output.UsersName = AskForUsersName();

    // Load up shot grid
    GameLogic.InitializeGrid(output);

    // Ask user for their 5 ship placements
    PlaceShips(output);

    // Clear
    Console.Clear();

    return output;
}

static string AskForUsersName()
{
    Console.Write("What is your name: ");
    string output = Console.ReadLine();

    return output;
}

static void PlaceShips(PlayerInfoModel model)
{
    do
    {
        Console.Write($"Where do you want to place ship #{model.ShipLocations.Count + 1}: ");
        string location = Console.ReadLine();

        bool IsValidLocation = GameLogic.PlaceShip(model, location);

        if (!IsValidLocation)
        {
            Console.WriteLine("That was not a valid location. Please try again.");
        }
    } while (model.ShipLocations.Count < 5);
}


