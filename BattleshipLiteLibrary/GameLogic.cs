using BattleshipLiteLibrary.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlTypes;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BattleshipLiteLibrary;

public static class GameLogic
{
    public static void InitializeGrid(PlayerInfoModel model)
    {
        List<string> letters = new List<string>
        {
            "A",
            "B",
            "C",
            "D",
            "E"
        };

        List<int> numbers = new List<int>()
        {
            1,
            2,
            3,
            4,
            5
        };

        foreach (string letter in letters)
        {
            foreach (int number in numbers)
            {
                AddGridSpot(model, letter, number);
            }
        }
    }

    private static void AddGridSpot(PlayerInfoModel model, string letter, int number)
    {
        GridSpotModel spot = new GridSpotModel
        {
            SpotLetter = letter,
            SpotNumber = number,
            Status = GridSpotStatus.Empty
        };

        model.ShotGrid.Add(spot);
    }

    public static bool PlaceShip(PlayerInfoModel model, string location)
    {
        bool output = false;
        (string row, int column) = SplitShotIntoRowAndColumn(location);

        bool isValidLocation = ValidateGridLocation(model, row, column);
        bool isSpotOpen = ValidateShipLocation(model, row, column);

        if (isValidLocation && isSpotOpen)
        {
            model.ShipLocations.Add(new GridSpotModel
            {
                SpotLetter = row.ToUpper(),
                SpotNumber = column,
                Status = GridSpotStatus.Ship
            });

            output = true;
        }

        return output;
    }

    private static bool ValidateShipLocation(PlayerInfoModel model, string row, int column)
    {
        bool isValidLocation = true;

        foreach (GridSpotModel ship in model.ShipLocations)
        {
            if (ship.SpotLetter == row.ToUpper() && ship.SpotNumber == column)
            {
                isValidLocation = false;
            }
        }

        return isValidLocation;
    }

    private static bool ValidateGridLocation(PlayerInfoModel model, string row, int column)
    {
        bool isValidLocation = false;

        foreach (GridSpotModel spot in model.ShotGrid)
        {
            if (spot.SpotLetter == row.ToUpper() && spot.SpotNumber == column)
            {
                isValidLocation = true;
            }
        }

        return isValidLocation;
    }

    public static bool PlayerStillActive(PlayerInfoModel player)
    {
        bool isActive = false;

        foreach (GridSpotModel ship in player.ShipLocations)
        {
            if (ship.Status != GridSpotStatus.Sunk)
            {
                isActive = true;
            }
        }

        return isActive;
    }

    public static int GetShotCount(PlayerInfoModel player)
    {
        int shotCount = 0;

        foreach (GridSpotModel shot in player.ShotGrid)
        {
            if (shot.Status != GridSpotStatus.Empty)
            {
                shotCount++;
            }
        }

        return shotCount;
    }

    public static (string row, int column) SplitShotIntoRowAndColumn(string shot)
    {
        string row = "";
        int column = 0;

        if (shot.Length != 2)
        {
            throw new ArgumentException("This was an invalid shot type", "shot");
        }

        char[] shotArray = shot.ToArray();

        row = shotArray[0].ToString();
        column = int.Parse(shotArray[1].ToString());

        return (row, column);
    }

    public static bool ValidateShot(PlayerInfoModel activePlayer, string row, int column)
    {
        bool isValidLocation = false;

        foreach (GridSpotModel gridSpot in activePlayer.ShotGrid)
        {
            if (gridSpot.SpotLetter == row.ToUpper() && gridSpot.SpotNumber == column)
            {
                if (gridSpot.Status == GridSpotStatus.Empty)
                {
                    isValidLocation = true;
                }
            }
        }

        return isValidLocation;
    }

    public static bool IdentifyShotResult(PlayerInfoModel opponent, string row, int column)
    {
        bool isAHit = false;

        foreach (GridSpotModel ship in opponent.ShipLocations)
        {
            if (ship.Status == GridSpotStatus.Ship)
            {
                if (ship.SpotLetter == row.ToUpper() && ship.SpotNumber == column)
                {
                    isAHit = true;
                    ship.Status = GridSpotStatus.Sunk;
                }
            }
        }

        return isAHit;
    }

    public static void MarkShotResult(PlayerInfoModel player, string row, int column, bool isAHit)
    {
        bool isValidShot = false;

        foreach (GridSpotModel ship in player.ShotGrid)
        {

            if (ship.SpotLetter == row.ToUpper() && ship.SpotNumber == column)
            {
                if (isAHit)
                {
                    ship.Status = GridSpotStatus.Hit;
                }
                else
                {
                    ship.Status = GridSpotStatus.Miss;
                }
            }
        }
    }
}
