
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

namespace ChessV
{
  public struct BitBoard64
  {
    // *** CONSTRUCTION *** //

    #region Constructors
    public BitBoard64(UInt64 bits)
    {
      this.bits = bits;
    }
    #endregion


    // *** PROPERTIES *** //

    #region BitCount
    public int BitCount
    {
      get
      {
        ulong result = bits - ((bits >> 1) & 0x5555555555555555UL);
        result = (result & 0x3333333333333333UL) + ((result >> 2) & 0x3333333333333333UL);
        return (byte)(unchecked(((result + (result >> 4)) & 0xF0F0F0F0F0F0F0FUL) * 0x101010101010101UL) >> 56);
      }
    }
    #endregion


    // *** OPERATIONS *** //

    #region Clear
    public void Clear()
    {
      bits = 0;
    }
    #endregion

    #region SetAll
    public void SetAll()
    {
      bits = 0xFFFFFFFFFFFFFFFFUL;
    }
    #endregion

    #region GetBit
    public int GetBit(int bitnumber)
    {
      return (int)(bits >> (bitnumber % 64)) & 1;
    }
    #endregion

    #region IsBitSet
    public bool IsBitSet(int bitnumber)
    {
      return ((bits >> bitnumber) & 1) != 0UL;
    }
    #endregion

    #region SetBit
    public void SetBit(int bitnumber)
    {
      bits |= 1UL << (bitnumber % 64);
    }
    #endregion

    #region ClearBit
    public void ClearBit(int bitnumber)
    {
      bits &= 0xFFFFFFFFFFFFFFFFUL ^ (1UL << (bitnumber % 64));
    }
    #endregion

    #region GetLSB
    public int GetLSB()
    //	returns least significant bit
    {
      return index64[((bits & (UInt64)(-((Int64)bits))) * debruijn64) >> 58];
    }
    #endregion

    #region ExtractLSB
    public int ExtractLSB()
    //	returns and clears the least significant bit
    {
      int returnval = index64[((bits & (UInt64)(-((Int64)bits))) * debruijn64) >> 58];
      bits = bits & (UInt64)((Int64)bits - 1);
      return returnval;
    }
    #endregion


    // *** OVERLOADED OPERATORS *** //

    #region Overloaded Operators
    public static implicit operator bool(BitBoard64 bb)
    {
      return bb.bits != 0UL;
    }

    public static BitBoard64 operator <<(BitBoard64 bb, int shift)
    {
      return new BitBoard64(bb.bits << shift);
    }

    public static BitBoard64 operator >>(BitBoard64 bb, int shift)
    {
      return new BitBoard64(bb.bits >> shift);
    }

    public bool this[int bitnumber]
    {
      get
      { return IsBitSet(bitnumber); }
    }
    #endregion


    // *** INTERNAL DATA *** //

    private UInt64 bits;


    // *** HELPER CONSTANTS *** //

    private const UInt64 k1 = 0x5555555555555555UL; /*  -1/3   */
    private const UInt64 k2 = 0x3333333333333333UL; /*  -1/5   */
    private const UInt64 k4 = 0x0f0f0f0f0f0f0f0fUL; /*  -1/17  */
    private const UInt64 kf = 0x0101010101010101UL; /*  -1/255 */

    private static int[] index64 = {
      63,  0, 58,  1, 59, 47, 53,  2,
      60, 39, 48, 27, 54, 33, 42,  3,
      61, 51, 37, 40, 49, 18, 28, 20,
      55, 30, 34, 11, 43, 14, 22,  4,
      62, 57, 46, 52, 38, 26, 32, 41,
      50, 36, 17, 19, 29, 10, 13, 21,
      56, 45, 25, 31, 35, 16,  9, 12,
      44, 24, 15,  8, 23,  7,  6,  5
    };

    private const UInt64 debruijn64 = 0x07EDD5E59A4E28C2UL;
  }
}
