
/***************************************************************************

                                 ChessV

                  COPYRIGHT (C) 2012-2017 BY GREG STRONG

This file is part of ChessV.  ChessV is free software; you can redistribute
it and/or modify it under the terms of the GNU General Public License as 
published by the Free Software Foundation, either version 3 of the License, 
or (at your option) any later version.

ChessV is distributed in the hope that it will be useful, but WITHOUT ANY 
WARRANTY; without even the implied warranty of MERCHANTABILITY or 
FITNESS FOR A PARTICULAR PURPOSE.  See the GNU General Public License for 
more details; the file 'COPYING' contains the License text, but if for
some reason you need a copy, please visit <http://www.gnu.org/licenses/>.

****************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using ChessV;
using Archipelago.APChessV;
using Archipelago.MultiClient.Net.Helpers;
using Archipelago.MultiClient.Net.MessageLog.Messages;
using System.Threading.Tasks;
using System.Security.Policy;
using System.Linq;

namespace ChessV.GUI
{
	public partial class ApmwForm: Form
	{
    public ApmwForm()
		{
			linesSeen = 0;

			InitializeComponent();
      archipelagoClient = new ArchipelagoClient();
    }

    private ArchipelagoClient archipelagoClient;
		private MessageLogHelper messageLog;
		private List<LogMessage> pastMessages;
    private int linesSeen = 0;
    private int nonSessionLinesSeen = 0;

    private void ApmwForm_Load( object sender, EventArgs e )
		{
			timer.Start();
    }

		private void timer_Tick( object sender, EventArgs e )
    {
      StringBuilder append = new StringBuilder(10000);
      if (pastMessages != null)
      {
        for (int x = linesSeen; x < pastMessages.Count; x++)
          append.Append(pastMessages[linesSeen++] + "\r\n");
      }
      for (int x = nonSessionLinesSeen; x < archipelagoClient.nonSessionMessages.Count; x++)
        append.Append(archipelagoClient.nonSessionMessages[nonSessionLinesSeen++] + "\r\n");
      txtApmwOutput.Text += append.ToString();

      if (archipelagoClient.session != null)
      {
        var names = archipelagoClient.session.Players.AllPlayers.Select((PlayerInfo info) => info.Name);
        if (!comboBox1.Items.Equals(names))
        {
          comboBox1.Items.Clear();
          names.Select((String name) => comboBox1.Items.Add(name));
        }
      }
    }

		private void ApmwForm_FormClosing( object sender, FormClosingEventArgs e )
		{
			Visible = false;
			e.Cancel = true;
		}

    private void timer1_Tick(object sender, EventArgs e)
    {
      timer1.Stop();
      button1_Click(sender, e);
    }

    private void timer2_Tick(object sender, EventArgs e)
    {
      timer2.Stop();
      button1.Enabled = true;
    }

    private void textBox1_TextChanged(object sender, EventArgs e)
    {
			timer1.Stop();
			timer1.Start();
    }

    private void textBox1_KeyDown(object sender, KeyEventArgs e)
    {
      timer1.Stop();
      button1_Click(sender, e);
    }

    private void button1_Click(object sender, EventArgs e)
    {
			button1.Enabled = false;
      timer2.Stop();
      timer2.Start();

      linesSeen = 0;
      nonSessionLinesSeen = 0;
      var url = new Uri("wss://" + textBox1.Text.Split('/').Last());
      var slot = comboBox1.SelectedText;
      //messageLog.OnMessageReceived -= (message) => pastMessages.Add(message);
      archipelagoClient.Connect(url, slot);
      messageLog = archipelagoClient.session.MessageLog;
      messageLog.OnMessageReceived += (message) => pastMessages.Add(message);

    }

    private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
    {
      linesSeen = 0;
      nonSessionLinesSeen = 0;
      //messageLog.OnMessageReceived -= (message) => pastMessages.Add(message);
      archipelagoClient.Connect(new Uri(textBox1.Text), comboBox1.SelectedText);
      messageLog = archipelagoClient.session.MessageLog;
      messageLog.OnMessageReceived += (message) => pastMessages.Add(message);
    }
  }
}
