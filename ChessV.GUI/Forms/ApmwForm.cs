
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
    private Convenience convenience;
    private MessageLogHelper messageLog;
		private List<LogMessage> pastMessages = new List<LogMessage>();
    private int linesSeen = 0;
    private int nonSessionLinesSeen = 0;

    private void ApmwForm_Load( object sender, EventArgs e )
		{
			timer.Start();
      convenience = new Convenience();
      textBox1.Text += convenience.getRecentUrl();
      textBox2.Text += convenience.getRecentSlotName();
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
      button1.Enabled = true;
      timer2.Stop();
    }

    private void textBox1_TextChanged(object sender, EventArgs e)
    {
			timer1.Stop();
			timer1.Start();
    }

    private void textBox1_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode == Keys.Enter)
      {
        timer1.Stop();
        button1_Click(sender, e);
      }
    }

    private void button1_Click(object sender, EventArgs e)
    {
      // TODO(chesslogic): change to disconnect mode (for now, just close the program, lol)
			button1.Enabled = false;
      timer2.Stop();
      timer2.Start();

      linesSeen = 0;
      nonSessionLinesSeen = 0;
      var url = new Uri("wss://" + textBox1.Text.Split('/').Last());
      var slot = textBox2.Text;
      //messageLog.OnMessageReceived -= (message) => pastMessages.Add(message);
      archipelagoClient.Connect(url, slot);
      if (messageLog != archipelagoClient.session.MessageLog)
      {
        messageLog = archipelagoClient.session.MessageLog;
        messageLog.OnMessageReceived += (message) => pastMessages.Add(message);
      }
    }

    private void textBox2_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode == Keys.Enter)
      {
        timer1.Stop();
        button1_Click(sender, e);
      }
    }

    private void textBox2_TextChanged(object sender, EventArgs e)
    {

    }
  }
}
