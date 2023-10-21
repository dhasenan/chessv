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

namespace Archipelago.APChessV
{
  public class ArchipelagoClient
  {
    public ArchipelagoClient() {
      StartedEventHandler seHandler = (match) => this.match = match;
      Starter.getInstance().StartedEventHandlers.Add(((match) => seHandler((ChessV.Match) match)));
      // TODO: PlayAsWhite
      Starter.getInstance().GeriProvider.Add(() => 1);
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
      var BerolinaPawn = new BerolinaPawn("Berolina Pawn", "Ƥ / ƥ", 100, 125, preferredImageName: "Ferz");
      // Cwda
      var Archbishop = new Archbishop("Archbishop", "A", 875, 875);
      var WarElephant = new WarElephant("War Elephant", "E", 475, 475);
      var Phoenix = new Phoenix("Phoenix", "X", 315, 315);
      var Cleric = new Cleric("Cleric", "C", 450, 500);
      var Chancellor = new Chancellor("Chancellor", "CQ", 950, 950);
      var ShortRook = new ShortRook("Short Rook", "S", 400, 425);
      var Tower = new Tower("Tower", "T", 325, 325);
      var Lion = new Lion("Lion", "I", 500, 500);
      var ChargingRook = new ChargingRook("Charging Rook", "HR", 495, 530);
      var NarrowKnight = new NarrowKnight("Lancer", "L", 325, 325);
      var ChargingKnight = new ChargingKnight("Charging Knight", "HN", 365, 365);
      var Colonel = new Colonel("Colonel", "CN", 950, 950);
      // Eurasian
      var Cannon = new Cannon("Cannon", "O", 400, 275);
      var Vao = new Vao("Vao", "V", 300, 175);

      var starters = new Dictionary<KeyValuePair<int, int>, PieceType>();
      Starter.getInstance().PlayerPieceSetProvider.Add(() => starters);
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

    private ArchipelagoSession session;

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

      var result = session.TryConnectAndLogin("ChessV", slotName, itemsHandlingFlags: ItemsHandlingFlags.AllItems, new Version(3, 4, 0));

      if (!result.Successful)
      {
        LoginFailure failureResult = (LoginFailure)result;
        foreach (var err in failureResult.Errors)
        {
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

      //LocationCheckBar.ItemPickupStep = ItemLogic.ItemPickupStep;

      //session.Socket.PacketReceived += Session_PacketReceived;
      //session.Socket.SocketClosed += Session_SocketClosed;
      //ItemLogic.OnItemDropProcessed += ItemLogicHandler_ItemDropProcessed;

      //new ArchipelagoStartMessage().Send(NetworkDestination.Clients);
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
  }
}
