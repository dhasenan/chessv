
/***************************************************************************

                                 ChessV

                  COPYRIGHT (C) 2012-2019 BY GREG STRONG

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
using System.Windows.Forms;

namespace ChessV.GUI
{
  public partial class EngineStatisticsForm : Form
  {
    public EngineStatisticsForm(Statistics statistics)
    {
      Statistics = statistics;

      InitializeComponent();
    }

    private void EngineStatisticsForm_Load(object sender, EventArgs e)
    {
    }

    public void UpdateStatistics()
    {
      lblNodes.Text = Statistics.Nodes.ToString("N0");
      lblQNodes.Text = Statistics.QNodes.ToString("N0");
      lblQNodePercent.Text = (100.0 * Statistics.QNodes / Statistics.Nodes).ToString("N2") + "%";
      TimeSpan elapsedTime = DateTime.Now - Statistics.SearchStartTime;
      lblNodesPerSecond.Text = (Statistics.Nodes / elapsedTime.TotalSeconds / 1000).ToString("N") + "k";
      lblPawnHitPercent.Text = (100.0 * Statistics.PawnHashHits / Statistics.PawnHashLookups).ToString("N2") + "%";
      lblMaterialHitPercent.Text = (100.0 * Statistics.MaterialHashHits / Statistics.MaterialHashLookups).ToString("N2") + "%";
    }

    public Statistics Statistics { get; private set; }
  }
}
