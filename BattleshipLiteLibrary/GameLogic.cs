using BattleshipLiteLibrary.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
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
        if (char.IsLetter(location[0]) && char.IsNumber(location[1]))
        {
            GridSpotModel gridSpotModel = new GridSpotModel
            {
                SpotLetter = location[0].ToString(),
                SpotNumber = location[1],
                Status = GridSpotStatus.Ship
            };

            return true;
        }
        else
        {
            return false;
        }
    }

    public static bool PlayerStillActive(PlayerInfoModel opponent)
    {
        return opponent.ShipLocations.Count > 0;
    }

    public static int GetShotCount(PlayerInfoModel winner)
    {
        int count = 0;

        foreach (GridSpotModel shot in winner.ShotGrid)
        {
            if (shot.Status == GridSpotStatus.Hit || shot.Status == GridSpotStatus.Miss)
            {
                count++;
            }
        }

        return count;
    }

    public static (string row, int column) SplitShotIntoRowAndColumn(string shot)
    {
        string row = "";
        int column = 0;

        foreach (char character in shot)
        {
            if (char.IsLetter(character))
            {
                row = character.ToString();
            }
            else if (char.IsDigit(character))
            {
                column = character;
            }
        }

        return (row, column);
    }

    public static bool ValidateShot(PlayerInfoModel activePlayer, string row, int column)
    {
        foreach (GridSpotModel spot in activePlayer.ShotGrid)
        {
            if (spot.Status == GridSpotStatus.Ship || spot.Status == GridSpotStatus.Empty)
            {
                if (spot.SpotLetter == row && spot.SpotNumber == column)
                {
                    return true;
                }
                else
                {
                    continue;
                }
            }
        }

        return false;
    }

    public static bool IdentifyShotResult(PlayerInfoModel opponent, string row, int column)
    {
        foreach (GridSpotModel spot in opponent.ShotGrid)
        {
            if (spot.Status == GridSpotStatus.Ship)
            {
                if (spot.SpotLetter == row && spot.SpotNumber == column)
                {
                    return true;
                }
                else
                {
                    continue;
                }
            }
        }

        return false;
    }

    public static void MarkShotResult(PlayerInfoModel activePlayer, string row, int column, bool isAHit)
    {
        throw new NotImplementedException();
    }
}
