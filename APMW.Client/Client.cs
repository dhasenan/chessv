using Archipelago.MultiClient.Net;
using Archipelago.MultiClient.Net.Packets;
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
    private ArchipelagoSession session;

    private DataPackagePacket dataPackagePacket;
    private ConnectedPacket connectedPacket;
    private LocationInfoPacket locationInfoPacket;
    private ConnectPacket connectPacket;

    private Dictionary<int, string> itemLookupById;
    private Dictionary<int, string> locationLookupById;
    private Dictionary<int, string> playerNameById;
    private int pickedUpItemCount = 0;

    private Queue<string> itemReceivedQueue = new Queue<string>();
    private ArchipelagoConfig itemPickupStep;
    private int totalLocations;
    private bool finishedAllChecks = false;
    private ulong seed;
    private string lastServerUrl;

    private bool IsInGame
    {
      get
      {
        return ChessV.Game.Match != null;
      }
    }
  }
}
