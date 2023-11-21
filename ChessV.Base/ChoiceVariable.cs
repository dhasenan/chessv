
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

namespace ChessV
{
  public sealed class ChoiceVariable : ICloneable
  {
    // *** PROPERTIES *** //

    #region Choices
    public List<string> Choices
    {
      get
      { return choices; }
    }
    #endregion

    #region Value
    public string Value
    {
      get
      { return value; }

      set
      {
        if (value == null)
          this.value = null;
        else
        {
          foreach (string choice in Choices)
            if (choice.ToUpper() == value.ToUpper())
            {
              this.value = choice;
              return;
            }
          throw new Exception("Specified value not valid for choice variable: " + value);
        }
      }
    }
    #endregion

    #region DefaultValue
    public string DefaultValue { get; set; }
    #endregion


    // *** CONSTRUCTION *** //

    #region Construction
    public ChoiceVariable()
    {
      choices = new List<string>();
      descriptions = new Dictionary<string, string>();
      hasDescriptions = false;
    }

    public ChoiceVariable(ChoiceVariable original)
    {
      //	Copy constructor
      choices = new List<string>();
      descriptions = new Dictionary<string, string>();
      foreach (string choice in original.Choices)
        Choices.Add(choice);
      defaultValue = original.defaultValue;
      value = original.value;
    }

    public ChoiceVariable(string[] choices, string defaultChoice = null) : this()
    {
      foreach (string choice in choices)
        Choices.Add(choice);
      this.defaultValue = defaultChoice;
    }

    public ChoiceVariable(string[] choices) : this(choices, null)
    {
    }

    public ChoiceVariable(List<string> choices, string defaultChoice)
    {
      this.choices = choices;
      this.defaultValue = defaultChoice;
    }

    public ChoiceVariable(List<string> choices) : this(choices, null)
    {
    }
    #endregion


    // *** PROPERTIES *** //

    #region Properties
    public bool HasDescriptions
    { get { return hasDescriptions; } }
    #endregion


    // *** OPERATIONS *** //

    #region Operations
    public void AddChoice(string newChoice, string description = null)
    {
      Choices.Add(newChoice);
      if (description != null)
      {
        hasDescriptions = true;
        descriptions.Add(newChoice, description);
      }
    }

    public void RemoveChoice(string choice)
    {
      Choices.Remove(choice);
      if (descriptions.ContainsKey(choice))
        descriptions.Remove(choice);
    }

    public string DescribeChoice(string choice)
    {
      if (descriptions.ContainsKey(choice))
        return descriptions[choice];
      return null;
    }
    #endregion


    // *** OVERRIDES *** //

    #region Overrides
    public override string ToString()
    {
      return value;
    }

    object ICloneable.Clone()
    {
      return new ChoiceVariable(this);
    }
    #endregion


    // *** PRIVATE DATA *** //

    #region Private Data
    private List<string> choices;
    private string value;
    private string defaultValue;
    private bool hasDescriptions;
    private Dictionary<string, string> descriptions;
    #endregion
  }
}
