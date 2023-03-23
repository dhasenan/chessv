
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
using System.IO;
using ChessV.Manager;

namespace ChessV.GUI
{
	public partial class ToolsForm: Form
	{
		public ToolsForm()
		{
			InitializeComponent();
		}

		private void btnClose_Click( object sender, EventArgs e )
		{
			Close();
		}

		private void btnRunBatch_Click( object sender, EventArgs e )
		{
			string workingPath = null;
			MatchSet matches = null;
			AutomatedMatchesSettingsForm form = new AutomatedMatchesSettingsForm();
			if( form.ShowDialog() == DialogResult.OK )
			{
				string controlFileName = form.ControlFile;
				StreamReader reader = new StreamReader( controlFileName );
				workingPath = Path.GetFullPath( controlFileName ).Substring( 0, Path.GetFullPath( controlFileName ).LastIndexOf( Path.DirectorySeparatorChar ) );
				matches = Program.Manager.LoadMatches( reader, workingPath );
				string outputFileName = workingPath + Path.DirectorySeparatorChar +
					"results " + DateTime.Now.ToString( "yyyy-MM-dd HH_mm" ) + " " +
					controlFileName.Substring( controlFileName.LastIndexOf( Path.DirectorySeparatorChar ) >= 0 ? 
					controlFileName.LastIndexOf( Path.DirectorySeparatorChar ) + 1 : 0 );
				if( form.OutputFileToResume != null )
					outputFileName = form.OutputFileToResume;
				if( matches != null )
				{
					AutomatedMatchesProgressForm form2 = new AutomatedMatchesProgressForm( matches, workingPath, outputFileName );
					form2.Show();
				}
			}
		}

		private void btnAddMoveToGames_Click( object sender, EventArgs e )
		{
			AddMoveToGamesForm form = new AddMoveToGamesForm();
			if( form.ShowDialog() == DialogResult.OK )
			{
				AddMoveToGamesProgressForm f2 = new AddMoveToGamesProgressForm( form.GamePath, form.FixedDepth, form.FixedTimeMinutes, form.Variation );
				f2.ShowDialog();
			}
		}

		private void btnSaveGamesAsImages_Click( object sender, EventArgs e )
		{
			SaveGamesAsImagesForm form = new SaveGamesAsImagesForm();
			form.ShowDialog();
		}

		private void btnTranspositionDetector_Click( object sender, EventArgs e )
		{
			TranspositionDetectorForm form = new TranspositionDetectorForm();
			form.ShowDialog();
		}

		private void btnCreateGameReferenceDocs_Click( object sender, EventArgs e )
		{
			CreateReferenceDocsForm form = new CreateReferenceDocsForm();
			form.ShowDialog();
		}
	}
}
