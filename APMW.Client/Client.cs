using Archipelago.MultiClient.Net;
using Archipelago.MultiClient.Net.Packets;
using APMW.Core;
using ChessV;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Archipelago.APChessV
{
  public class ArchipelagoClient
  {
    ArchipelagoClient() {
      StartedEventHandler seHandler = (match) => this.match = match;
      Starter.getInstance().StartedEventHandlers.Add(seHandler);
    }

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
  }
}
