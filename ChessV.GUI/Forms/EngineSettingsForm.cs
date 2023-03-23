
/***************************************************************************

                                 ChessV

                  COPYRIGHT (C) 2012-2019 BY GREG STRONG
  
  THIS FILE DERIVED FROM CUTE CHESS BY ILARI PIHLAJISTO AND ARTO JONSSON

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
	public partial class EngineSettingsForm: Form
	{
		// *** PROPERTIES *** //

		public int TTSizeInMB { get; set; }
		public int Variation { get; set; }
		public int Weakening { get; set; }
		public int XBoardMemory { get; set; }
		public int XBoardCores { get; set; }


		// *** CONSTRUCTION *** //

		#region Constructor
		public EngineSettingsForm()
		{
			InitializeComponent();

			ttsizeTrackStops = new int[] { 16, 32, 64, 128, 256, 512, 1024, 2048, 4096 };
			variationLabels = new string[] { "None", "Small", "Medium", "Large" };
			weakeningLabels = new string[] { "None", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15" };
			XBoardMemory = 32;
			XBoardCores = 1;
		}
		#endregion


		// *** EVENT HANDLERS *** //

		#region Event Handlers
		#region Form Load event
		private void EngineSettingsForm_Load( object sender, EventArgs e )
		{
			//	initialize the TT Size trackbar and label
			bool found = false;
			for( int x = 0; x < ttsizeTrackStops.Length && !found; x++ )
			{
				if( ttsizeTrackStops[x] == TTSizeInMB )
				{
					trackTTSize.Value = x;
					found = true;
				}
			}
			if( !found )
			{
				trackTTSize.Value = 2;
				TTSizeInMB = ttsizeTrackStops[2];
			}
			lblTTSize.Text = sizeInMBToString( ttsizeTrackStops[trackTTSize.Value] );

			//	initialize the Variation trackbar and label
			trackVariation.Value = Variation;
			lblVariation.Text = variationLabels[Variation];

			//	initialize the Weakening trackbar and label
			trackWeakening.Value = Weakening;
			lblWeakening.Text = weakeningLabels[Weakening];
		}
		#endregion

		#region trackTTSize Scroll event
		private void trackTTSize_Scroll( object sender, EventArgs e )
		{
			TTSizeInMB = ttsizeTrackStops[trackTTSize.Value];
			lblTTSize.Text = sizeInMBToString( TTSizeInMB );
		}
		#endregion

		#region trackVariation Scroll event
		private void trackVariation_Scroll( object sender, EventArgs e )
		{
			Variation = trackVariation.Value;
			lblVariation.Text = variationLabels[Variation];
		}
		#endregion

		#region trackWeakening Scroll event
		private void trackWeakening_Scroll( object sender, EventArgs e )
		{
			Weakening = trackWeakening.Value;
			lblWeakening.Text = weakeningLabels[Weakening];
		}
		#endregion

		#region OK Button Click event
		private void btnOK_Click( object sender, EventArgs e )
		{
			DialogResult = DialogResult.OK;
			int mem;
			if( Int32.TryParse( txtXBoardMemory.Text, out mem ) )
				XBoardMemory = mem;
			int cores;
			if( Int32.TryParse( txtXBoardCores.Text, out cores ) )
				XBoardCores = cores;
			Close();
		}
		#endregion

		#region Cancel Button Click Event
		private void btnCancel_Click( object sender, EventArgs e )
		{
			DialogResult = DialogResult.Cancel;
			Close();
		}
		#endregion
		#endregion


		// *** HELPER FUNCTIONS *** //

		#region sizeInMBToString
		protected string sizeInMBToString( int sizeInMB )
		{
			if( sizeInMB < 1024 )
				return sizeInMB.ToString() + " MB";
			else
				return (sizeInMB / 1024).ToString() + " GB";
		}
		#endregion


		// *** DATA MEMBERS *** //

		public int[] ttsizeTrackStops;
		public string[] variationLabels;
		public string[] weakeningLabels;
	}
}
