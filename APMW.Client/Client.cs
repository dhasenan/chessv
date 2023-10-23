using Archipelago.MultiClient.Net;
using Archipelago.MultiClient.Net.Enums;
using Archipelago.MultiClient.Net.Packets;
using ChessV;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChessV.Games;
using ChessV.Games.Pieces.Berolina;
using ChessV.Base;
using Archipelago.MultiClient.Net.MessageLog.Messages;
using Archipelago.MultiClient.Net.Helpers;
using System.ComponentModel;

namespace Archipelago.APChessV
{
  public class ArchipelagoClient
  {
    public ArchipelagoClient()
    {
      nonSessionMessages = new List<string>();

      StartedEventHandler seHandler = (match) => this.match = match;
      ApmwCore.getInstance().StartedEventHandlers.Add(seHandler);
      // TODO(chesslogic): PlayAsWhite
      ApmwCore.getInstance().GeriProvider.Add(() => 1);

      loadPieces();
    }

    public void loadPieces()
    {
      var King = new King("King", "K", 0, 0);
      var Pawn = new Pawn("Pawn", "P", 100, 125);
      var Queen = new Queen("Queen", "Q", 950, 1000);
      var Rook = new Rook("Rook", "R", 500, 550);
      var Bishop = new Bishop("Bishop", "B", 325, 350);
      var Knight = new Knight("Knight", "N", 325, 325);

      // Berolina
      var BerolinaPawn = new BerolinaPawn("Berolina Pawn", "Ƥ/ƥ", 100, 125, preferredImageName: "Ferz");
      // Cwda
      var Archbishop = new Archbishop("Archbishop", "A", 875, 875);
      var WarElephant = new WarElephant("War Elephant", "E", 475, 475);
      var Phoenix = new Phoenix("Phoenix", "X", 315, 315);
      var Cleric = new Cleric("Cleric", "Ċ/ċ", 450, 500);
      var Chancellor = new Chancellor("Chancellor", "C", 950, 950);
      var ShortRook = new ShortRook("Short Rook", "S", 400, 425);
      var Tower = new Tower("Tower", "T", 325, 325);
      var Lion = new Lion("Lion", "I", 500, 500);
      var ChargingRook = new ChargingRook("Charging Rook", "Ṙ/ṙ", 495, 530);
      var NarrowKnight = new NarrowKnight("Lancer", "L", 325, 325);
      var ChargingKnight = new ChargingKnight("Charging Knight", "Ṅ/ṅ", 365, 365);
      var Colonel = new Colonel("Colonel", "K̇/k̇", 950, 950);
      // Eurasian
      var Cannon = new Cannon("Cannon", "O", 400, 275);
      var Vao = new Vao("Vao", "V", 300, 175);

      var starters = new Dictionary<KeyValuePair<int, int>, PieceType>();
      ApmwCore.getInstance().PlayerPieceSetProvider.Add(() => starters);
      for (int i = 0; i < 8; i++)
      {
        starters.Add(new KeyValuePair<int, int>(1, i), Pawn);
      }
      starters.Add(new KeyValuePair<int, int>(2, 0), Rook);
      starters.Add(new KeyValuePair<int, int>(2, 1), Knight);
      starters.Add(new KeyValuePair<int, int>(2, 2), Bishop);
      starters.Add(new KeyValuePair<int, int>(2, 3), Queen);
      starters.Add(new KeyValuePair<int, int>(2, 4), King);
      starters.Add(new KeyValuePair<int, int>(2, 5), Bishop);
      starters.Add(new KeyValuePair<int, int>(2, 6), Knight);
      starters.Add(new KeyValuePair<int, int>(2, 7), Rook);
    }



    public delegate void ClientDisconnected(ushort code, string reason, bool wasClean);
    public event ClientDisconnected OnClientDisconnect;

    internal LocationHandler LocationHandler { get; private set; }

    public ArchipelagoSession session { get; private set; }
    public List<String> nonSessionMessages { get; private set; }

    public Dictionary<string, object> SlotData { get; private set; }

    private DataPackagePacket dataPackagePacket;
    private ConnectedPacket connectedPacket;
    private LocationInfoPacket locationInfoPacket;
    private ConnectPacket connectPacket;

    private Dictionary<int, string> itemLookupById;
    private Dictionary<int, string> locationLookupById;
    private Dictionary<int, string> playerNameById;
    private ArchipelagoProgress apmwProgress;

    private ChessV.Match match;

    private Queue<string> itemReceivedQueue = new Queue<string>();
    private ArchipelagoConfig apmwConfig;
    private int totalLocations;
    private bool finishedAllChecks = false;
    private ulong seed;
    private string lastServerUrl;
    private Task connectionTask;

    private bool IsInGame
    {
      get
      {
        return match != null;
      }
    }

    public void Connect(Uri url, string slotName, string password = null)
    {
      //ChatMessage.SendColored($"Attempting to connect to Archipelago at ${url}.", Color.green);
      Dispose();

      //LastServerUrl = url;

      session = ArchipelagoSessionFactory.CreateSession(url);
      //ItemLogic = new ArchipelagoItemLogicController(session);
      //LocationCheckBar = new ArchipelagoLocationCheckProgressBarUI();

      connectionTask = new Task(() =>
      {
        var result = session.TryConnectAndLogin(
          "ChecksMate",
          slotName,
          itemsHandlingFlags: ItemsHandlingFlags.AllItems,
          new Version(4, 3, 0),
          tags: new string[] { "DeathLink" },
          password: password);

        if (!result.Successful)
        {
          LoginFailure failureResult = (LoginFailure)result;
          foreach (var errCode in failureResult.ErrorCodes)
          {
            nonSessionMessages.Add("Error code: " + errCode.ToString());
          }
          foreach (var err in failureResult.Errors)
          {
            nonSessionMessages.Add(err);
            //ChatMessage.SendColored(err, Color.red);
            //Log.LogError(err);
          }
          return;
        }

        LoginSuccessful successResult = (LoginSuccessful)result;
        if (successResult.SlotData.TryGetValue("FinalStageDeath", out var stageDeathObject))
        {
          //finalStageDeath = Convert.ToBoolean(stageDeathObject);
        }
        Convenience.getInstance().success(url.Port.ToString(), slotName);

        LocationHandler = new LocationHandler(session.Locations);
        SlotData = session.DataStorage.GetSlotData();

        var seeds = new int[] {
          Convert.ToInt32(SlotData["pocketSeed"]),
          Convert.ToInt32(SlotData["pawnSeed"]),
          Convert.ToInt32(SlotData["minorSeed"]),
          Convert.ToInt32(SlotData["majorSeed"]),
          Convert.ToInt32(SlotData["queenSeed"]), };
        // TODO(chesslogic): Check if mode is chaos, if so, set random seeds based on current time
        ApmwCore.getInstance().seed(seeds);

        ItemHandler handler = new ItemHandler(session);

        //LocationCheckBar.ItemPickupStep = ItemLogic.ItemPickupStep;

        //session.Socket.PacketReceived += Session_PacketReceived;
        session.Socket.SocketClosed += Session_SocketClosed;
      });
      connectionTask.Start();
    }

    public void Dispose()
    {
      if (session != null && session.Socket.Connected)
      {
        session.Socket.DisconnectAsync();
      }

      //if (ItemLogic != null)
      //{
      //  ItemLogic.OnItemDropProcessed -= ItemLogicHandler_ItemDropProcessed;
      //  ItemLogic.Dispose();
      //}

      //if (LocationCheckBar != null)
      //{
      //  LocationCheckBar.Dispose();
      //}

      session = null;
    }

    // TODO(chesslogic): warn user to reconnect
    private void Session_SocketClosed(string reason)
    {
      Dispose();
      // new ArchipelagoEndMessage().Send(NetworkDestination.Clients);

      if (OnClientDisconnect != null)
      {
        //OnClientDisconnect(reason);
      }
    }
  }
}
