﻿
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

namespace ChessV
{
  public class SymbolTable
  {
    // *** CONSTRUCTION *** //

    public SymbolTable(SymbolTable parentSymbolTable = null)
    {
      Parent = parentSymbolTable;
      table = new Dictionary<string, object>();
    }


    // *** PROPERTIES *** //

    public SymbolTable Parent { get; protected set; }


    // *** OPERATIONS *** //

    public void SetParent(SymbolTable parent)
    {
      if (parent != null)
        throw new Exception("SymbolTable.SetParent: Parent table is already set");
      Parent = parent;
    }

    public object Lookup(string symbol, bool inherit = true)
    {
      if (table.ContainsKey(symbol))
        return table[symbol];
      if (inherit)
      {
        SymbolTable parentTable = Parent;
        while (Parent != null)
        {
          if (Parent.table.ContainsKey(symbol))
            return Parent.table[symbol];
          Parent = Parent.Parent;
        }
      }
      return null;
    }

    public void Set(string symbol, object value)
    {
      table[symbol] = value;
    }


    // *** INTERNAL DATA *** //

    protected Dictionary<string, object> table;
  }
}
