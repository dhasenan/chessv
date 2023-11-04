using ChessV;
using ChessV.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Archipelago.APChessV
{
  internal enum Goal
  {
    Single = 0, Progressive = 1, OrderedProgressive = 2,
  }
  internal enum PieceLocations
  {
    Chaos = 0, Stable = 1, Ordered = 2,
  }
  /** Applies to Player and Enemy */
  internal enum PieceTypes
  {
    Chaos = 0, Stable = 1, Book = 2,
  }
  internal enum FairyTypes
  {
    Vanilla = 0, Full = 1, CwDA = 2, Cannon = 3, Eurasian = 4,
  }
  internal enum FairyArmy
  {
    Chaos = 0, Limited = 1, Fair = 2,
  }
  internal enum FairyPawns
  {
    Vanilla = 0, Mixed = 1, Berolina = 2,
  }

  internal class ApmwConfig
  {
    public static ApmwConfig _instance;
    public static ApmwConfig getInstance()
    {
      if (_instance == null)
      {
        lock (typeof(ApmwConfig))
        {
          if (_instance == null)
          {
            _instance = new ApmwConfig();
          }
        }
      }
      return _instance;
    }

    public Dictionary<string, object> SlotData { get; private set; }

    public int pocketSeed = -1;
    public List<int> pocketChoiceSeed { get; private set; }
    public int pawnSeed = -1;
    public int minorSeed = -1;
    public int majorSeed = -1;
    public int queenSeed = -1;

    private Goal goal;
    public Goal Goal { get { return goal; } }
    public int GoalInt
    {
      set
      {
        goal = (Goal)value;
      }
    }
    private PieceLocations locs;
    public PieceLocations Locs { get { return locs; } }
    public int LocsInt
    {
      set
      {
        locs = (PieceLocations)value;
      }
    }
    private PieceTypes types;
    public PieceTypes Types { get { return types; } }
    public int TypesInt
    {
      set
      {
        types = (PieceTypes)value;
      }
    }
    private PieceTypes enemyTypes;
    public PieceTypes EnemyTypes { get { return enemyTypes; } }
    public int EnemyTypesInt
    {
      set
      {
        enemyTypes = (PieceTypes)value;
      }
    }

    public void Instantiate(Dictionary<string, object> slotData)
    {
      SlotData = slotData;

      // Implemented
      //SlotData["max_material"]
      //SlotData["min_material"]
      //SlotData["early_material"]

      // Progressive Goal
      //SlotData["goal"]
      //SlotData["enemy_piece_types"]

      // Chaotic Material Randomization
      // Non-Progressive Material
      //SlotData["piece_locations"]
      //SlotData["piece_types"]

      // Army-Constrained Material
      //SlotData["fairy_chess_army"]

      // Non-Fairy Chess
      //SlotData["fairy_chess_pieces"]
      //SlotData["fairy_chess_pawns"]

      // Piece Limits
      //SlotData["minor_piece_limit_by_type"]
      //SlotData["major_piece_limit_by_type"]
      //SlotData["queen_piece_limit_by_type"]
      //SlotData["queen_piece_limit"]

      // TODO(chesslogic): Check if mode is chaos, if so, set random seeds based on current time
      this.seed();
    }

    private void seed()
    {
      var seeds = new int[] {
          Convert.ToInt32(SlotData["pocketSeed"]),
          Convert.ToInt32(SlotData["pawnSeed"]),
          Convert.ToInt32(SlotData["minorSeed"]),
          Convert.ToInt32(SlotData["majorSeed"]),
          Convert.ToInt32(SlotData["queenSeed"]), };

      pocketSeed = seeds[0];
      pawnSeed = seeds[1];
      minorSeed = seeds[2];
      majorSeed = seeds[3];
      queenSeed = seeds[4];
    }

    /** Possibly not stable - will generate a different pocket distribution as the player progresses through different foundPockets - but it is uniform */
    public List<int> generatePocketValues(int foundPockets)
    {
      if (foundPockets == 0) { return new List<int>() { 0, 0, 0 }; }
      if (pocketSeed == -1) { throw new InvalidOperationException("Please set Starter.pocketSeed"); }

      // preserve choices separate from values
      Random pocketRandom = new Random(pocketSeed);
      pocketChoiceSeed = new List<int>() { pocketRandom.Next(), pocketRandom.Next(), pocketRandom.Next() };

      // probably not uniform... but it's within range so it works for now. will break FEN later
      Random random = new Random(pocketSeed);
      var x = random.Next(Math.Max(0, foundPockets - 8), Math.Min(foundPockets, 5));
      if (x == foundPockets)
      {
        return new List<int>() { x, 0, 0 };
      }
      var y = random.Next(Math.Max(0, foundPockets - 4 - x), Math.Min(foundPockets, 5));
      var z = foundPockets - (y + x);
      if (z < 0)
      {
        (x, y, z) = (4 - x, 4 - y, -z);
      }

      return new List<int>() { x, y, z };
    }

    /**
     * arg spaces should be TOTAL spaces not EMPTY spaces
     * 
     * vaguely inspired by this cacophanous suggestion:
     * https://stackoverflow.com/questions/28544808/random-distribution-of-items-in-list-with-exact-number-of-occurences
     */
    public static Dictionary<int, Item> distribute<Item>(List<Item> items, int spaces)
    {
      // Create list of items * z
      Dictionary<int, Item> allItems = new Dictionary<int, Item>();
      for (int i = 0; i < items.Count; i++)
        allItems.Add(i, items[i]);
      for (int i = items.Count; i < spaces; i++)
        allItems.Add(i, default(Item));

      Random random = new Random();
      int n = allItems.Count;
      while (n > 1)
      {
        n--;
        int k = random.Next(n + 1);
        (allItems[k], allItems[n]) = (allItems[n], allItems[k]);
      }

      return allItems;
    }
  }
}
