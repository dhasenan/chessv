
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
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace ChessV.GUI
{
  public partial class CreateReferenceDocsForm : Form
  {
    // *** CONSTRUCTION *** //

    #region Constructor
    public CreateReferenceDocsForm()
    {
      InitializeComponent();
      abstractClassesWritten = new List<string>();
    }
    #endregion


    // *** MESSAGE HANDLERS *** //

    #region Browse button click event
    private void btnBrowseOutputFolder_Click(object sender, EventArgs e)
    {
      if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
        txtOutputFolder.Text = folderBrowserDialog.SelectedPath;
    }
    #endregion

    #region Cancel button click event
    private void btnCancel_Click(object sender, EventArgs e)
    {
      DialogResult = DialogResult.Cancel;
      Close();
    }
    #endregion

    #region OK button click event
    private void btnOK_Click(object sender, EventArgs e)
    {
      if (!Directory.Exists(txtOutputFolder.Text))
      {
        MessageBox.Show("You must select an output directory");
        return;
      }

      gameTreeNode = new GameClassTreeNode("Game", null);

      //	iterate through all known games
      foreach (KeyValuePair<string, GameAttribute> pair in Program.Manager.GameAttributes)
      {
        if (!pair.Value.Hidden)
        {
          string gameName = pair.Key;
          Game game = Program.Manager.CreateGame(gameName, null);
          if (game != null)
          {
            StreamWriter writer = new StreamWriter(Path.Combine(txtOutputFolder.Text, gameName + ".html"));
            writeGamePage(game, pair.Value, writer);
            writer.Close();

            if (game.NDisabledPieceTypes == 0)
            {
              //	check for abstract games this may be derived from.  since 
              //	we cannot instantiate an abstract game, we need to take 
              //	advantage of the fact that we now have a derived instance
              Type type = game.GetType().BaseType;
              while (type != typeof(Game))
              {
                object[] a = type.GetCustomAttributes(typeof(GameAttribute), false);
                if (a.Length > 0)
                {
                  GameAttribute ga = (GameAttribute)a[0];
                  if (ga.Template && !ga.Hidden && !abstractClassesWritten.Contains(ga.GameName))
                  {
                    writer = new StreamWriter(Path.Combine(txtOutputFolder.Text, ga.GameName + ".html"));
                    writeAbstractGamePage(game, type, ga, writer);
                    writer.Close();
                    abstractClassesWritten.Add(ga.GameName);
                  }
                }
                type = type.BaseType;
              }
            }
          }
        }
      }

      #region Write the index page
      StreamWriter indexwriter = new StreamWriter(Path.Combine(txtOutputFolder.Text, "index.html"));
      indexwriter.WriteLine("<!doctype html>");
      indexwriter.WriteLine("<html class=\"no-js\" lang=\"en\">");
      indexwriter.WriteLine("\t<head>");
      indexwriter.WriteLine("\t\t<meta charset=\"utf-8\">");
      indexwriter.WriteLine("\t\t<meta http-equiv=\"x-ua-compatible\" content=\"ie=edge\">");
      indexwriter.WriteLine("\t\t<link rel=\"stylesheet\" href=\"css/style.css?v=1.0\">");
      indexwriter.WriteLine("\t\t<title>ChessV Games Index</title>");
      indexwriter.WriteLine("\t\t<meta name=\"description\" content=\"Reference information for all games include in ChessV\">");
      indexwriter.WriteLine("\t\t<meta name=\"viewport\" content=\"width=device-width, initial-scale=1\">");
      indexwriter.WriteLine("\t</head>");
      indexwriter.WriteLine("\t<body background=\"page_background.gif\">");

      //	create table that encompasses the page
      indexwriter.WriteLine("\t\t<center>");
      indexwriter.WriteLine("\t\t<table width=\"840\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" bgcolor=\"#FFFFFF\">");
      indexwriter.WriteLine("\t\t<tr><td width=\"20\">&nbsp;</td><td width=\"800\">&nbsp;</td><td> &nbsp;</td></tr>");
      indexwriter.WriteLine("\t\t<tr><td width=\"20\">&nbsp;</td><td>");
      GameClassTreeNode node = gameTreeNode;
      writeNodeToIndex(indexwriter, node);
      indexwriter.WriteLine("\t\t</td><td>&nbsp;</td></tr>");
      indexwriter.WriteLine("\t\t<tr><td>&nbsp;</td><td><center><h3><pre>COPYRIGHT (C) 2020 BY GREG STRONG</pre></h3></center></td><td>&nbsp;</td></tr>");
      indexwriter.WriteLine("\t\t</table></center>");
      indexwriter.WriteLine("\t</body>");
      indexwriter.WriteLine("</html>");
      indexwriter.Close();
      #endregion

      MessageBox.Show("Done");
    }
    #endregion


    // *** HELPER FUNCTIONS *** //

    #region writeGamePage
    private void writeGamePage(Game game, GameAttribute gameAttribute, StreamWriter writer)
    {
      writer.WriteLine("<!doctype html>");
      writer.WriteLine("<html class=\"no-js\" lang=\"en\">");
      writer.WriteLine("\t<head>");
      writer.WriteLine("\t\t<meta charset=\"utf-8\">");
      writer.WriteLine("\t\t<meta http-equiv=\"x-ua-compatible\" content=\"ie=edge\">");
      writer.WriteLine("\t\t<link rel=\"stylesheet\" href=\"css/style.css?v=1.0\">");
      writer.WriteLine("\t\t<title>" + game.Name + "</title>");
      writer.WriteLine("\t\t<meta name=\"description\" content=\"Reference information for game " + game.Name + "\">");
      writer.WriteLine("\t\t<meta name=\"viewport\" content=\"width=device-width, initial-scale=1\">");
      writer.WriteLine("\t</head>");
      writer.WriteLine("\t<body background=\"page_background.gif\">");

      //	create table that encompasses the page
      writer.WriteLine("\t\t<center>");
      writer.WriteLine("\t\t<table width=\"840\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" bgcolor=\"#FFFFFF\">");
      writer.WriteLine("\t\t<tr><td width=\"20\">&nbsp;</td><td width=\"800\">&nbsp;</td><td> &nbsp;</td></tr>");
      writer.WriteLine("\t\t<tr><td width=\"20\">&nbsp;</td><td>");

      //	main page header
      writer.WriteLine("\t\t<h1>" + game.Name + "</h1>");
      if (gameAttribute.Invented != null && gameAttribute.InventedBy != null)
        writer.WriteLine("\t\t<p><big><b>Invented by</b>: " + gameAttribute.InventedBy.Replace(";", " and ") + " " + gameAttribute.Invented + "</big></p>");

      #region Game description display
      //	write description (if any)
      if (gameAttribute.GameDescription1 != null)
      {
        writer.Write("\t\t<p>" + gameAttribute.GameDescription1);
        if (gameAttribute.GameDescription2 != null)
        {
          writer.Write(Char.IsUpper(gameAttribute.GameDescription2[0]) ? "<br>" : " ");
          writer.Write(gameAttribute.GameDescription2);
        }
        writer.WriteLine("</p>");
      }
      #endregion

      #region Class Hierarchy display
      //	capture hierarchy of classes
      List<Type> typelist = new List<Type>();
      Type type = game.GetType();
      while (type != typeof(Game))
      {
        typelist.Add(type);
        type = type.BaseType;
      }
      typelist.Add(typeof(Game));
      //	write out class hierarchy
      writer.WriteLine("\t\t<h2>Game hierarchy</h2>");
      writer.WriteLine("\t\t<p class=\"hierarchy\">&bull; Game<br>");
      for (int x = typelist.Count - 2; x >= 0; x--)
      {
        GameAttribute ga = gameAttribute;
        if (x > 0)
        {
          Type t = typelist[x];
          object[] a = t.GetCustomAttributes(typeof(GameAttribute), false);
          ga = (GameAttribute)a[0];
        }
        string gameName = ga.GameName;
        writer.Write("\t\t");
        for (int y = typelist.Count - x - 1; y > 0; y--)
          writer.Write("&nbsp;&nbsp;&nbsp;&nbsp;");
        writer.Write("&bull; <a href=\"" + gameName + ".html\">" + ga.GameName + "</a>");
        writer.WriteLine("<br>");
      }
      writer.WriteLine("\t\t</p>");

      //	place node for this game in the tree
      GameClassTreeNode currentNode = gameTreeNode;
      for (int x = typelist.Count - 1; x >= 0; x--)
      {
        Type t = typelist[x];
        object[] a = t.GetCustomAttributes(typeof(GameAttribute), false);
        if (a.Length > 0)
        {
          GameAttribute ga = (GameAttribute)a[0];
          string gameName = ga.GameName;
          GameClassTreeNode childNode = null;
          for (int y = 0; y < currentNode.Children.Count; y++)
            if (currentNode.Children[y].Name == gameName)
              childNode = currentNode.Children[y];
          if (childNode == null)
          {
            childNode = new GameClassTreeNode(gameName, gameName + ".html");
            currentNode.Children.Add(childNode);
          }
          currentNode = childNode;
        }
      }
      #endregion

      #region Board display
      //	create and output board rendering
      writer.WriteLine("\t\t<h2>Board</h2>");
      if (game.Board.GetType() != typeof(Board))
      {
        string boardClass = game.Board.GetType().Name;
        boardClass = boardClass.Substring(boardClass.LastIndexOf('.') + 1);
        writer.WriteLine("\t\t<p><big>This game uses custom board class: " + boardClass + "</big></p>");
      }
      if (gameAttribute.Tags != null && gameAttribute.Tags.Contains("Random Array"))
        writer.WriteLine("\t\t<p>This game has a randomized starting array so this diagram shows only one example</p>");
      BoardPresentation presentation = PresentationFactory.CreatePresentation(game);
      Bitmap bitmap = presentation.Render();
      string imageFilename = Path.Combine(txtOutputFolder.Text, gameAttribute.GameName + ".png");
      bitmap.Save(imageFilename, ImageFormat.Png);
      writer.WriteLine("\t\t<img src=\"" + game.Name + ".png\" width=\"" + bitmap.Width.ToString() +
        "\" height=\"" + bitmap.Height.ToString() + "\">");
      writer.WriteLine("\t\t<p><b>FEN</b>: " + game.FENStart + "</p>");
      #endregion

      #region Piece type table
      //	write out piece types
      writer.WriteLine("\t\t<h2>Piece Types</h2>");
      PieceType[] pieceTypes;
      int nPieceTypes = game.GetPieceTypes(out pieceTypes);
      writer.WriteLine("\t\t<table border=\"2\" cellpadding=\"6\"><thead><tr><td><b>Internal Name</b></td><td><b>Name</b></td>" +
        "<td align=\"center\"><b>Notation</b></td><td><b>Created By</b></td><td><b>Notes</b></td></tr></thead><tbody>");
      for (int x = 0; x < nPieceTypes; x++)
      {
        PieceType pt = pieceTypes[x];
        //	 we need to find the member definition of this piece type
        Type typeDeclaredIn = game.GetClassThatCreatedPieceType(pt);
        /* FieldInfo[] fields = game.GetType().GetFields( BindingFlags.Instance | BindingFlags.Public );
				foreach( FieldInfo fi in fields )
					if( fi.FieldType == typeof(PieceType) || fi.FieldType.IsSubclassOf( typeof(PieceType) ) )
						if( fi.GetValue( game ) == pt )
							typeDeclaredIn = fi.DeclaringType; */
        GameAttribute declaredInAttr = null;
        if (typeDeclaredIn != null && typeDeclaredIn != game.GetType())
        {
          object[] a = typeDeclaredIn.GetCustomAttributes(typeof(GameAttribute), false);
          if (a.Length > 0)
            declaredInAttr = (GameAttribute)a[0];
        }
        writer.Write("\t\t\t<tr><td>" + pt.InternalName + "</td><td>" + pt.Name + "</td><td align=\"center\">" + pt.Notation[0] + "</td><td>");
        if (declaredInAttr != null && typeDeclaredIn != type)
          writer.Write(declaredInAttr.GameName + "</td><td>");
        else
          writer.Write("&nbsp;</td><td>");
        List<string> pieceTypeNotes = game.GetPieceTypeNotes(pt);
        string notes = "";
        if (pieceTypeNotes.Count >= 1)
          notes = pieceTypeNotes[0];
        for (int y = 1; y < pieceTypeNotes.Count; y++)
          notes += "; " + pieceTypeNotes[y];
        if (notes.Length > 0)
          writer.Write(notes);
        else
          writer.Write("&nbsp;");
        writer.WriteLine("</td></tr>");
      }
      writer.WriteLine("\t\t</tbody></table><br>");
      #endregion

      #region Game variable display
      //	write out game variables
      writer.WriteLine("\t\t<h2>Game Variables</h2>");
      for (int x = typelist.Count - 1; x >= 0; x--)
      {
        Type t = typelist[x];
        GameAttribute ga = gameAttribute;
        if (x > 0 && x < typelist.Count - 1)
        {
          object[] a = t.GetCustomAttributes(typeof(GameAttribute), false);
          ga = (GameAttribute)a[0];
        }
        string gameName = x == typelist.Count - 1 ? "Game" : ga.GameName;
        bool headerWritten = false;
        PropertyInfo[] properties = t.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
        foreach (PropertyInfo pi in properties)
        {
          if (pi.Name == "FENStart")
          {
            writer.Write("\t\t<b>FENFormat</b> = \"" + game.FENFormat + "\"<br>");
            writer.Write("\t\t<b>FENStart</b> = \"" + game.OriginalFENStart + "\"");
            writer.WriteLine("<br>");
            continue;
          }
          object[] attributes = pi.GetCustomAttributes(typeof(GameVariableAttribute), false);
          if (attributes.Length > 0 && !((GameVariableAttribute)attributes[0]).Hidden)
          {
            if (!headerWritten)
            {
              writer.WriteLine("\t\t<h3>From " + gameName + ":</h3><p class=\"gamevars\">");
              headerWritten = true;
            }
            bool paragraph;
            writer.Write("\t\t<b>" + pi.Name + "</b> = ");
            if (x > 0)
              writer.Write(getCurrentValue(game, (GameVariableAttribute)attributes[0], pi));
            else
              writer.Write(getPotentialValues(game, (GameVariableAttribute)attributes[0], pi, out paragraph));
            writer.WriteLine("<br>");
          }
        }
        if (headerWritten)
          writer.WriteLine("\t\t</p>");
      }
      #endregion

      #region Rules list
      //	write out rules
      writer.WriteLine("\t\t<h2>Rules</h2>");
      writer.Write("\t\t<p>");
      bool first = true;
      List<Rule> rules = game.GetRules();
      foreach (Rule rule in rules)
      {
        if (first)
          first = false;
        else
          writer.Write("\t\t<br>");
        writer.WriteLine(rule.GetType().Name);
      }
      writer.WriteLine("\t\t</p>");
      #endregion

      #region Evaluations list
      //	write out evaluations
      writer.WriteLine("\t\t<h2>Evaluations</h2>");
      writer.Write("\t\t<p>");
      first = true;
      List<Evaluation> evaluations = game.GetEvaluations();
      foreach (Evaluation eval in evaluations)
      {
        if (first)
          first = false;
        else
          writer.Write("\t\t<br>");
        writer.WriteLine(eval.GetType().Name);
      }
      writer.WriteLine("\t\t</p>");
      #endregion

      //	close out main table encompassing the entire page
      writer.WriteLine("\t\t</td><td>&nbsp;</td></tr>");
      writer.WriteLine("\t\t<tr><td>&nbsp;</td><td><center><h3><pre>Archipelago Multiworld by RAIN WHITE</pre></h3></center></td><td>&nbsp;</td></tr>");
      writer.WriteLine("\t\t<tr><td>&nbsp;</td><td><center><h3><pre>COPYRIGHT (C) 2020 BY GREG STRONG</pre></h3></center></td><td>&nbsp;</td></tr>");
      writer.WriteLine("\t\t</table></center>");
      writer.WriteLine("\t</body>");
      writer.WriteLine("</html>");
    }
    #endregion

    #region writeAbstractGamePage
    private void writeAbstractGamePage(Game game, Type type, GameAttribute gameAttribute, StreamWriter writer)
    {
      writer.WriteLine("<!doctype html>");
      writer.WriteLine("<html class=\"no-js\" lang=\"en\">");
      writer.WriteLine("\t<head>");
      writer.WriteLine("\t\t<meta charset=\"utf-8\">");
      writer.WriteLine("\t\t<meta http-equiv=\"x-ua-compatible\" content=\"ie=edge\">");
      writer.WriteLine("\t\t<link rel=\"stylesheet\" href=\"css/style.css?v=1.0\">");
      writer.WriteLine("\t\t<title>" + gameAttribute.GameName + "</title>");
      writer.WriteLine("\t\t<meta name=\"description\" content=\"Reference information for game " + gameAttribute.GameName + "\">");
      writer.WriteLine("\t\t<meta name=\"viewport\" content=\"width=device-width, initial-scale=1\">");
      writer.WriteLine("\t</head>");
      writer.WriteLine("\t<body background=\"page_background.gif\">");

      //	create table that encompasses the page
      writer.WriteLine("\t\t<center>");
      writer.WriteLine("\t\t<table width=\"840\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" bgcolor=\"#FFFFFF\">");
      writer.WriteLine("\t\t<tr><td width=\"20\">&nbsp;</td><td width=\"800\">&nbsp;</td><td> &nbsp;</td></tr>");
      writer.WriteLine("\t\t<tr><td width=\"20\">&nbsp;</td><td>");

      //	main page header
      writer.WriteLine("\t\t<h1>" + gameAttribute.GameName + "</h1>");

      #region Game description
      //	write description (if any)
      if (gameAttribute.GameDescription1 != null)
      {
        writer.Write("\t\t<p>" + gameAttribute.GameDescription1);
        if (gameAttribute.GameDescription2 != null)
        {
          writer.Write(Char.IsUpper(gameAttribute.GameDescription2[0]) ? "<br>" : " ");
          writer.Write(gameAttribute.GameDescription2);
        }
        writer.WriteLine("</p>");
      }
      #endregion

      #region Class Hierarchy display
      //	capture hierarchy of classes
      List<Type> typelist = new List<Type>();
      Type basetype = type;
      while (basetype != null && basetype != typeof(Game))
      {
        typelist.Add(basetype);
        basetype = basetype.BaseType;
      }
      typelist.Add(typeof(Game));
      //	write out class hierarchy
      writer.WriteLine("\t\t<h2>Game hierarchy</h2>");
      writer.WriteLine("\t\t<p class=\"hierarchy\">&bull; Game<br>");
      for (int x = typelist.Count - 2; x >= 0; x--)
      {
        GameAttribute ga = gameAttribute;
        if (x > 0)
        {
          Type t = typelist[x];
          object[] a = t.GetCustomAttributes(typeof(GameAttribute), false);
          ga = (GameAttribute)a[0];
        }
        string gameName = ga.GameName;
        writer.Write("\t\t");
        for (int y = typelist.Count - x - 1; y > 0; y--)
          writer.Write("&nbsp;&nbsp;&nbsp;&nbsp;");
        writer.Write("&bull; <a href=\"" + gameName + ".html\">" + ga.GameName + "</a>");
        writer.WriteLine("<br>");
      }
      writer.WriteLine("\t\t</p>");
      #endregion

      #region Piece Type table
      //	write out piece types
      bool pieceTypesFound = false;
      PieceType[] pieceTypes;
      int nPieceTypes = game.GetPieceTypes(out pieceTypes);
      for (int x = 0; x < nPieceTypes; x++)
      {
        PieceType pt = pieceTypes[x];
        //	 we need to find the member definition of this piece type
        Type typeDeclaredIn = game.GetClassThatCreatedPieceType(pt);
        /* FieldInfo[] fields = game.GetType().GetFields( BindingFlags.Instance | BindingFlags.Public );
				foreach( FieldInfo fi in fields )
					if( fi.FieldType == typeof(PieceType) || fi.FieldType.IsSubclassOf( typeof(PieceType) ) )
						if( fi.GetValue( game ) == pt )
							typeDeclaredIn = fi.DeclaringType; */
        if (typeDeclaredIn != null && typeDeclaredIn != type && !type.IsSubclassOf(typeDeclaredIn))
          continue;
        GameAttribute declaredInAttr = null;
        if (typeDeclaredIn != null && typeDeclaredIn != game.GetType())
        {
          object[] a = typeDeclaredIn.GetCustomAttributes(typeof(GameAttribute), false);
          if (a.Length > 0)
            declaredInAttr = (GameAttribute)a[0];
        }
        if (!pieceTypesFound)
        {
          pieceTypesFound = true;
          writer.WriteLine("\t\t<h2>Piece Types</h2>");
          writer.WriteLine("\t\t<table border=\"2\" cellpadding=\"6\"><thead><tr><td><b>Internal Name</b></td><td><b>Name</b></td>" +
            "<td align=\"center\"><b>Notation</b></td><td>Created By</td><td><b>Notes</b></td></tr></thead><tbody>");
        }
        writer.Write("\t\t\t<tr><td>" + pt.InternalName + "</td><td>" + pt.Name + "</td><td align=\"center\">" + pt.Notation[0] + "</td><td>");
        if (declaredInAttr != null && typeDeclaredIn != type)
          writer.Write(declaredInAttr.GameName + "</td>");
        else
          writer.Write("&nbsp;</td>");
        writer.WriteLine("<td>&nbsp;</td></tr>");
      }
      if (pieceTypesFound)
        writer.WriteLine("\t\t</tbody></table><br>");
      #endregion

      #region Game variable display
      //	write out game variables
      writer.WriteLine("\t\t<h2>Game Variables</h2>");
      for (int x = typelist.Count - 1; x >= 0; x--)
      {
        Type t = typelist[x];
        GameAttribute ga = gameAttribute;
        if (x > 0 && x < typelist.Count - 1)
        {
          object[] a = t.GetCustomAttributes(typeof(GameAttribute), false);
          ga = (GameAttribute)a[0];
        }
        string gameName = x == typelist.Count - 1 ? "Game" : ga.GameName;
        bool headerWritten = false;
        bool writeBreak = false;
        PropertyInfo[] properties = t.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
        foreach (PropertyInfo pi in properties)
        {
          if (pi.Name == "FENStart")
          {
            writer.WriteLine("<br>\t\t<b>FENFormat</b> = <i>string</i>");
            writer.WriteLine("<br>\t\t<b>FENStart</b> = <i>string</i>");
            continue;
          }
          object[] attributes = pi.GetCustomAttributes(typeof(GameVariableAttribute), false);
          if (attributes.Length > 0 && !((GameVariableAttribute)attributes[0]).Hidden)
          {
            if (!headerWritten)
            {
              writer.WriteLine("\t\t<h3>From " + gameName + ":</h3><p class=\"gamevars\">");
              headerWritten = true;
            }
            bool paragraph;
            string potentialValues = getPotentialValues(game, (GameVariableAttribute)attributes[0], pi, out paragraph);
            if (writeBreak)
              writer.WriteLine("<br>");
            if (!paragraph)
            {
              writer.Write("\t\t<b>" + pi.Name + "</b> = ");
              writer.Write(potentialValues);
              writeBreak = true;
            }
            else
            {
              writer.Write("</p><p class=\"gamevars\">");
              writer.Write("\t\t<b>" + pi.Name + "</b> = ");
              writer.Write(potentialValues);
              writer.Write("</p><p class=\"gamevars\">");
              writeBreak = false;
            }
          }
        }
        if (headerWritten)
          writer.WriteLine("\t\t</p>");
      }
      #endregion


      //	close out main table encompassing the entire page
      writer.WriteLine("\t\t</td><td>&nbsp;</td></tr>");
      writer.WriteLine("\t\t<tr><td>&nbsp;</td><td><center><h3><pre>COPYRIGHT (C) 2020 BY GREG STRONG</pre></h3></center></td><td>&nbsp;</td></tr>");
      writer.WriteLine("\t\t</table></center>");
      writer.WriteLine("\t</body>");
      writer.WriteLine("</html>");
    }
    #endregion

    #region getCurrentValue
    private string getCurrentValue(Game game, GameVariableAttribute gva, PropertyInfo pi)
    {
      if (pi.PropertyType == typeof(bool))
      {
        bool val = (bool)pi.GetValue(game);
        return val ? "true" : "false";
      }
      else if (pi.PropertyType == typeof(string))
      {
        string val = (string)pi.GetValue(game);
        if (val == null)
          return "<i>dynamic</i>";
        else
          return "\"" + val + "\"";
      }
      else if (pi.PropertyType == typeof(int))
      {
        int val = (int)pi.GetValue(game);
        return val.ToString();
      }
      else if (pi.PropertyType == typeof(ChoiceVariable))
      {
        ChoiceVariable val = (ChoiceVariable)pi.GetValue(game);
        return val.Value;
      }
      else if (pi.PropertyType == typeof(IntRangeVariable))
      {
        IntRangeVariable val = (IntRangeVariable)pi.GetValue(game);
        return "<i>integer</i> between " + val.MinValue.ToString() + " and " + val.MaxValue.ToString();
      }
      else if (pi.PropertyType == typeof(PieceType))
      {
        PieceType pt = (PieceType)pi.GetValue(game);
        if (pt != null)
          return pt.Name + (pt.Name != pt.InternalName ? " (" + pt.InternalName + ")" : "");
        else
          return "null";
      }
      return "";
    }
    #endregion

    #region getPotentialValues
    private string getPotentialValues(Game game, GameVariableAttribute gva, PropertyInfo pi, out bool paragraph, bool fullDescription = false)
    {
      paragraph = false;
      if (pi.PropertyType == typeof(bool))
      {
        return "<i>boolean</i>";
      }
      else if (pi.PropertyType == typeof(string))
      {
        string val = (string)pi.GetValue(game);
        if (val == null)
          return "<i>dynamic</i>";
        else
          return "<i>string</i>"; //"\"" + val + "\"";
      }
      else if (pi.PropertyType == typeof(int))
      {
        return "<i>integer</i>";
      }
      else if (pi.PropertyType == typeof(ChoiceVariable))
      {
        ChoiceVariable val = (ChoiceVariable)pi.GetValue(game);
        if (val.HasDescriptions)
        {
          paragraph = true;
          string s = "choice of: <blockquote><ul>";
          foreach (string choice in val.Choices)
          {
            s += "<li><b>" + choice + "</b>";
            string desc = val.DescribeChoice(choice);
            if (desc != null)
              s += ": " + desc;
            s += "</li>";
          }
          s += "</ul></blockquote>";
          return s;
        }
        else
        {
          string s = "choice of { ";
          bool first = true;
          foreach (string choice in val.Choices)
          {
            if (first)
              first = false;
            else
              s += ", ";
            s += choice;
          }
          s += " }";
          if (val.DefaultValue != null)
            s += " default: " + val.DefaultValue;
          return s;
        }
      }
      else if (pi.PropertyType == typeof(PieceType))
      {
        return "<i>PieceType</i>";
      }
      else if (pi.PropertyType == typeof(IntRangeVariable))
      {
        IntRangeVariable val = (IntRangeVariable)pi.GetValue(game);
        return "<i>integer</i> between " + val.MinValue.ToString() + " and " + val.MaxValue.ToString();
      }
      return "";
    }
    #endregion

    #region writeNodeToIndex
    protected void writeNodeToIndex(StreamWriter writer, GameClassTreeNode node)
    {
      if (node.HTMLFile != null)
      {
        writer.WriteLine("<a href=\"" + node.HTMLFile + "\">" + node.Name + "</a><br>");
      }
      else
      {
        writer.WriteLine(node.Name + "<br>");
      }
      if (node.Children.Count > 0)
      {
        writer.WriteLine("<blockquote>");
        foreach (GameClassTreeNode childNode in node.Children)
          writeNodeToIndex(writer, childNode);
        writer.WriteLine("</blockquote>");
      }
    }
    #endregion


    // *** PRIVATE DATA *** //

    protected List<string> abstractClassesWritten;
    protected GameClassTreeNode gameTreeNode;


    protected class GameClassTreeNode
    {
      public string Name;
      public string HTMLFile;
      public List<GameClassTreeNode> Children;

      public GameClassTreeNode(string name, string htmlfile)
      {
        Name = name;
        HTMLFile = htmlfile;
        Children = new List<GameClassTreeNode>();
      }
    }
  }
}
