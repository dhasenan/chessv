
/***************************************************************************

                                 ChessV

                  COPYRIGHT (C) 2012-2018 BY GREG STRONG
  
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
using System.Collections;
using System.Collections.Generic;

namespace ChessV.Manager
{
  public class MatchSet : IEnumerable<MatchRecord>
  {
    // *** CONSTRUCTION *** //

    public MatchSet()
    {
      matches = new Dictionary<string, MatchRecord>();
    }


    // *** IENUMERABLE IMPLEMENETATION *** //

    #region IEnumerable Implementation
    public IEnumerator<MatchRecord> GetEnumerator()
    {
      return new Enumerator(matches.GetEnumerator());
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      throw new NotImplementedException();
    }
    #endregion


    // *** OPERATIONS *** //

    public MatchRecord GetRecordByID(string id)
    {
      if (matches.ContainsKey(id))
        return matches[id];
      return null;
    }

    public void Add(MatchRecord match)
    {
      matches.Add(match.ID, match);
    }


    // *** ENUMERATOR *** //

    #region Enumerator
    struct Enumerator : IEnumerator<MatchRecord>
    {
      public Enumerator(Dictionary<string, MatchRecord>.Enumerator setEnumerator)
      {
        internalEnumerator = setEnumerator;
      }

      public MatchRecord Current
      {
        get
        { return internalEnumerator.Current.Value; }
      }

      object IEnumerator.Current
      {
        get
        { return internalEnumerator.Current.Value; }
      }

      MatchRecord IEnumerator<MatchRecord>.Current
      {
        get
        { return internalEnumerator.Current.Value; }
      }

      public void Dispose()
      {
        internalEnumerator.Dispose();
      }

      public bool MoveNext()
      {
        return internalEnumerator.MoveNext();
      }

      void IDisposable.Dispose()
      {
        internalEnumerator.Dispose();
      }

      bool IEnumerator.MoveNext()
      {
        return internalEnumerator.MoveNext();
      }

      void IEnumerator.Reset()
      {
        throw new NotImplementedException();
      }

      private Dictionary<string, MatchRecord>.Enumerator internalEnumerator;
    }
    #endregion


    // *** DATA MEMEBERS *** //

    protected Dictionary<string, MatchRecord> matches;
  }
}
