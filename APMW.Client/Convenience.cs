using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Archipelago.APChessV
{
  public class Convenience
  {
    public static Convenience _instance;

    public static Convenience getInstance() {
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

    string url;
    string slotName;


    public void success(string url, string slotName)
    {
      this.url = url;
      this.slotName = slotName;
      if (!this.url.Equals(url) || !this.slotName.Equals(slotName)) {
        new Task(() =>
        {
          using (StreamWriter writetext = new StreamWriter("apmw.txt", false))
          {
            writetext.WriteLine(url);
            writetext.WriteLine(slotName);
          }
        }).Start();
      }
    }

    public string getRecentUrl()
    {
      if (url == null)
      {
        try
        {
          using (StreamReader readtext = new StreamReader("apmw.txt"))
          {
            if (!readtext.EndOfStream)
            {
              url = readtext.ReadLine();
              slotName = readtext.ReadLine();
            }
            return url ?? "";
          }
        }
        catch (FileNotFoundException ex)
        {
          File.Create("apmw.txt");
          url = "";
          slotName = "";
        }
      }
      return url ?? "";
    }

    public string getRecentSlotName()
    {
      if (slotName == null)
      {
        try
        {
          using (StreamReader readtext = new StreamReader("apmw.txt"))
          {
            if (!readtext.EndOfStream)
            {
              url = readtext.ReadLine();
              slotName = readtext.ReadLine();
            }
            return slotName ?? "";
          } 
        }
        catch (FileNotFoundException ex)
        {
          File.Create("apmw.txt");
          url = "";
          slotName = "";
        }
      }
      return slotName ?? "";
    }
  }
}
