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

namespace ChessV.Test
{
  [TestClass]
  public class PawnHashUnitTests
  {
    [TestMethod]
    public void PawnHashBackPawnTests()
    {
      PawnHashEntry entry = new PawnHashEntry();
      int[] a = new int[] { 0, 15, 4, 0, 9, 15, 3, 2, 11, 8, 10, 0, 15, 15, 14, 1 };
      int[] b = new int[] { 1, 0, 4, 15, 14, 15, 15, 0, 2, 9, 4, 15, 0, 15, 14, 15 };
      for (int x = 0; x < 16; x++)
      {
        entry.SetBackPawnRank(0, x, a[x]);
        entry.SetBackPawnRank(1, x, b[x]);
      }
      for (int x = 0; x < 16; x++)
      {
        Assert.AreEqual(entry.GetBackPawnRank(0, x), a[x]);
        Assert.AreEqual(entry.GetBackPawnRank(1, x), b[x]);
      }
      for (int x = 0; x < 16; x++)
      {
        entry.SetBackPawnRank(0, x, b[x]);
        entry.SetBackPawnRank(1, x, a[x]);
      }
      for (int x = 0; x < 16; x++)
      {
        Assert.AreEqual(entry.GetBackPawnRank(0, x), b[x]);
        Assert.AreEqual(entry.GetBackPawnRank(1, x), a[x]);
      }
      //	make sure copying the struct works correctly
      PawnHashEntry e2;
      e2 = entry;
      for (int x = 0; x < 16; x++)
      {
        Assert.AreEqual(e2.GetBackPawnRank(0, x), b[x]);
        Assert.AreEqual(e2.GetBackPawnRank(1, x), a[x]);
      }
    }
  }
}
