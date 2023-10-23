
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
      CollectionAssert.AreEqual(asList(0, 0, 0), tested.generatePocketValues(0));
      CollectionAssert.AreEqual(asList(0, 0, 1), tested.generatePocketValues(1));
      CollectionAssert.AreEqual(asList(1, 0, 1), tested.generatePocketValues(2));
      CollectionAssert.AreEqual(asList(1, 0, 2), tested.generatePocketValues(3));
      CollectionAssert.AreEqual(asList(2, 1, 1), tested.generatePocketValues(4)); // demonstrates instability - the second item is called with different arguments due to the narrowed range via the first item
      CollectionAssert.AreEqual(asList(2, 1, 2), tested.generatePocketValues(5));
      CollectionAssert.AreEqual(asList(2, 1, 3), tested.generatePocketValues(6));
      CollectionAssert.AreEqual(asList(2, 2, 3), tested.generatePocketValues(7));
      CollectionAssert.AreEqual(asList(2, 2, 4), tested.generatePocketValues(8));
      CollectionAssert.AreEqual(asList(3, 2, 4), tested.generatePocketValues(9));
      CollectionAssert.AreEqual(asList(3, 3, 4), tested.generatePocketValues(10));
      CollectionAssert.AreEqual(asList(4, 3, 4), tested.generatePocketValues(11));
      CollectionAssert.AreEqual(asList(4, 4, 4), tested.generatePocketValues(12));
    }

    [TestMethod]
    public void generatePocketValues_makesEmptyPockets()
    {
      CollectionAssert.AreEqual(asList(0, 0, 0), tested.generatePocketValues(0));
    }

    [TestMethod]
    public void generatePocketValues_makesSimplePockets()
    {
      CollectionAssert.AreEqual(asList(0, 0, 1), tested.generatePocketValues(1));
    }

    [TestMethod]
    public void generatePocketValues_makesSomePockets()
    {
      CollectionAssert.AreEqual(asList(1, 0, 2), tested.generatePocketValues(3));
    }

    [TestMethod]
    public void generatePocketValues_makesMorePockets()
    {
      CollectionAssert.AreEqual(asList(1, 0, 3), tested.generatePocketValues(4));
    }

    [TestMethod]
    public void generatePocketValues_makesHalfPockets()
    {
      CollectionAssert.AreEqual(asList(2, 0, 4), tested.generatePocketValues(6));
    }

    [TestMethod]
    public void generatePocketValues_makesBigPockets()
    {
      CollectionAssert.AreEqual(asList(2, 2, 4), tested.generatePocketValues(8));
    }

    [TestMethod]
    public void generatePocketValues_makesHugePockets()
    {
      CollectionAssert.AreEqual(asList(2, 3, 4), tested.generatePocketValues(9));
    }

    [TestMethod]
    public void generatePocketValues_makesGiantPockets()
    {
      CollectionAssert.AreEqual(asList(3, 3, 4), tested.generatePocketValues(10));
    }

    [TestMethod]
    public void generatePocketValues_makesMaxPockets()
    {
      CollectionAssert.AreEqual(asList(4, 4, 4), tested.generatePocketValues(12));
    }

    private List<int> asList(int Item1, int Item2, int Item3)
    {
      return new List<int> { Item1, Item2, Item3 };
    }
  }
}
