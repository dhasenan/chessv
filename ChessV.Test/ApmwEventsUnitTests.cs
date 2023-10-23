
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
using ChessV.Base;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ChessV.Test
{
  [TestClass]
  public class ApmwEventsUnitTests
  {
    ApmwCore tested;

    [TestInitialize()]
    public void beforeEach()
    {
      tested = ApmwCore.getInstance();
      tested.pocketSeed = 11;
    }

    [TestMethod]
    public void generatePocketValues_makesSeveralPockets()
    {
      tested.pocketSeed = 10001;
      Assert.AreEqual(asList(0, 0, 0).ToString(), tested.generatePocketValues(0).ToString());
      Assert.AreEqual(asList(0, 0, 1).ToString(), tested.generatePocketValues(1).ToString());
      Assert.AreEqual(asList(1, 0, 1).ToString(), tested.generatePocketValues(2).ToString());
      Assert.AreEqual(asList(1, 0, 2).ToString(), tested.generatePocketValues(3).ToString());
      Assert.AreEqual(asList(2, 1, 1).ToString(), tested.generatePocketValues(4).ToString()); // demonstrates instability - the second item is called with different arguments due to the narrowed range via the first item
      Assert.AreEqual(asList(2, 1, 2).ToString(), tested.generatePocketValues(5).ToString());
      Assert.AreEqual(asList(2, 1, 3).ToString(), tested.generatePocketValues(6).ToString());
      Assert.AreEqual(asList(2, 2, 3).ToString(), tested.generatePocketValues(7).ToString());
      Assert.AreEqual(asList(2, 2, 4).ToString(), tested.generatePocketValues(8).ToString());
      Assert.AreEqual(asList(3, 2, 4).ToString(), tested.generatePocketValues(9).ToString());
      Assert.AreEqual(asList(3, 3, 4).ToString(), tested.generatePocketValues(10).ToString());
      Assert.AreEqual(asList(4, 3, 4).ToString(), tested.generatePocketValues(11).ToString());
      Assert.AreEqual(asList(4, 4, 4).ToString(), tested.generatePocketValues(12).ToString());
    }

    [TestMethod]
    public void generatePocketValues_makesEmptyPockets()
    {
      Assert.AreEqual(asList(0, 0, 0).ToString(), tested.generatePocketValues(0).ToString());
    }

    [TestMethod]
    public void generatePocketValues_makesSimplePockets()
    {
      Assert.AreEqual(asList(0, 0, 1).ToString(), tested.generatePocketValues(1).ToString());
    }

    [TestMethod]
    public void generatePocketValues_makesSomePockets()
    {
      Assert.AreEqual(asList(1, 0, 2).ToString(), tested.generatePocketValues(3).ToString());
    }

    [TestMethod]
    public void generatePocketValues_makesMorePockets()
    {
      Assert.AreEqual(asList(1, 0, 3).ToString(), tested.generatePocketValues(4).ToString());
    }

    [TestMethod]
    public void generatePocketValues_makesHalfPockets()
    {
      Assert.AreEqual(asList(2, 0, 4).ToString(), tested.generatePocketValues(6).ToString());
    }

    [TestMethod]
    public void generatePocketValues_makesBigPockets()
    {
      Assert.AreEqual(asList(2, 2, 4) .ToString(), tested.generatePocketValues(8).ToString());
    }

    [TestMethod]
    public void generatePocketValues_makesHugePockets()
    {
      Assert.AreEqual(asList(2, 3, 4).ToString(), tested.generatePocketValues(9).ToString());
    }

    [TestMethod]
    public void generatePocketValues_makesGiantPockets()
    {
      Assert.AreEqual(asList(3, 3, 4).ToString(), tested.generatePocketValues(10).ToString());
    }

    [TestMethod]
    public void generatePocketValues_makesMaxPockets()
    {
      Assert.AreEqual(asList(4, 4, 4).ToString(), tested.generatePocketValues(12).ToString());
    }

    private List<int> asList(int Item1, int Item2, int Item3)
    {
      return new List<int> { Item1, Item2, Item3 };
    }
  }
}
