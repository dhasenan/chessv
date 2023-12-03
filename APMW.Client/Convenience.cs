using System.IO;
using System.Threading.Tasks;

namespace Archipelago.APChessV
{
  public class Convenience
  {
    public static Convenience _instance;

    public static Convenience getInstance()
    {
      if (_instance == null)
      {
        lock (typeof(Convenience))
        {
          if (_instance == null)
          {
            _instance = new Convenience();
          }
        }
      }
      return _instance;
    }

    static string DEFAULT_HOST = "archipelago.gg";

    string port;
    string slotName;
    string host;

    string Url
    {
      get
      {
        return (host ?? "") + ":" + (port ?? "");
      }
    }

    public void success(string port, string slotName, string hostName)
    {
      if (this.port == null || !this.port.Equals(port) ||
        this.slotName == null || !this.slotName.Equals(slotName))
      {
        new Task(() =>
        {
          using (StreamWriter writetext = new StreamWriter("apmw.txt", false))
          {
            writetext.WriteLine(port);
            writetext.WriteLine(slotName);
            if (!hostName.Equals("archipelago.gg"))
              writetext.WriteLine(hostName);
          }
        }).Start();
      }
      this.port = port;
      this.slotName = slotName;
      this.host = hostName;
    }

    public string getRecentUrl()
    {
      if (port == null)
      {
        lock (DEFAULT_HOST)
        {
          if (port == null)
          {
            try
            {
              using (StreamReader readtext = new StreamReader("apmw.txt"))
              {
                if (!readtext.EndOfStream)
                {
                  port = readtext.ReadLine();
                  slotName = readtext.ReadLine();
                  if (readtext.EndOfStream)
                    host = DEFAULT_HOST;
                  else
                    host = readtext.ReadLine();
                }
                return Url;
              }
            }
            catch (FileNotFoundException ex)
            {
              File.Create("apmw.txt");
              port = "";
              slotName = "";
              host = DEFAULT_HOST;
            }
          }
        }
      }
      return Url;
    }

    public string getRecentSlotName()
    {
      if (slotName == null)
      {
        lock (DEFAULT_HOST)
        {
          if (slotName == null)
          {
            try
            {
              using (StreamReader readtext = new StreamReader("apmw.txt"))
              {
                if (!readtext.EndOfStream)
                {
                  port = readtext.ReadLine();
                  slotName = readtext.ReadLine();
                  if (!readtext.EndOfStream)
                    host = readtext.ReadLine();
                }
                return slotName ?? "";
              }
            }
            catch (FileNotFoundException ex)
            {
              File.Create("apmw.txt");
              port = "";
              slotName = "";
              host = DEFAULT_HOST;
            }
          }
        }
      }
      return slotName ?? "";
    }
  }
}
