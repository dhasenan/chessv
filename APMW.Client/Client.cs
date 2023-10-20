using Archipelago.MultiClient.Net;
using Archipelago.MultiClient.Net.Packets;
using APMW.Core;
using ChessV;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Archipelago.MultiClient.Net.Enums;

namespace Archipelago.APChessV
{
  public class ArchipelagoClient
  {
    ArchipelagoClient() {
      StartedEventHandler seHandler = (match) => this.match = match;
      Starter.getInstance().StartedEventHandlers.Add(((match) => seHandler((ChessV.Match) match)));
      // TODO: PlayAsWhite
      Starter.getInstance().GeriProvider.Add(() => 1);
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
