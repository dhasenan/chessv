
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
      Assert.AreEqual((0, 0, 0), tested.generatePocketValues(0));
      Assert.AreEqual((0, 0, 1), tested.generatePocketValues(1));
      Assert.AreEqual((1, 0, 1), tested.generatePocketValues(2));
      Assert.AreEqual((1, 0, 2), tested.generatePocketValues(3));
      Assert.AreEqual((2, 1, 1), tested.generatePocketValues(4)); // demonstrates instability - the second item is called with different arguments due to the narrowed range via the first item
      Assert.AreEqual((2, 1, 2), tested.generatePocketValues(5));
      Assert.AreEqual((2, 1, 3), tested.generatePocketValues(6));
      Assert.AreEqual((2, 2, 3), tested.generatePocketValues(7));
      Assert.AreEqual((2, 2, 4), tested.generatePocketValues(8));
      Assert.AreEqual((3, 2, 4), tested.generatePocketValues(9));
      Assert.AreEqual((3, 3, 4), tested.generatePocketValues(10));
      Assert.AreEqual((4, 3, 4), tested.generatePocketValues(11));
      Assert.AreEqual((4, 4, 4), tested.generatePocketValues(12));
    }

    [TestMethod]
    public void generatePocketValues_makesEmptyPockets()
    {
      Assert.AreEqual((0, 0, 0), tested.generatePocketValues(0));
    }

    [TestMethod]
    public void generatePocketValues_makesSimplePockets()
    {
      Assert.AreEqual((0, 0, 1), tested.generatePocketValues(1));
    }

    [TestMethod]
    public void generatePocketValues_makesSomePockets()
    {
      Assert.AreEqual((1, 0, 2), tested.generatePocketValues(3));
    }

    [TestMethod]
    public void generatePocketValues_makesMorePockets()
    {
      Assert.AreEqual((1, 0, 3), tested.generatePocketValues(4));
    }

    [TestMethod]
    public void generatePocketValues_makesHalfPockets()
    {
      Assert.AreEqual((2, 0, 4), tested.generatePocketValues(6));
    }

    [TestMethod]
    public void generatePocketValues_makesBigPockets()
    {
      Assert.AreEqual((2, 2, 4), tested.generatePocketValues(8));
    }

    [TestMethod]
    public void generatePocketValues_makesHugePockets()
    {
      Assert.AreEqual((2, 3, 4), tested.generatePocketValues(9));
    }

    [TestMethod]
    public void generatePocketValues_makesGiantPockets()
    {
      Assert.AreEqual((3, 3, 4), tested.generatePocketValues(10));
    }

    [TestMethod]
    public void generatePocketValues_makesMaxPockets()
    {
      Assert.AreEqual((4, 4, 4), tested.generatePocketValues(12));
    }
  }
}
