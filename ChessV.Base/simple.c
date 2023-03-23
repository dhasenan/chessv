/*********************************************************/
/* 'Simple': a very basic WB Chess engine by H.G. Muller */
/*********************************************************/

#define VERSION "0.0"

#include <stdio.h>
#include <stdlib.h>
#include <string.h>

void Report();

// Some eval parameters (centi-Pawn). Piece-square tables pack opening and end-game value in single int

#define PST(OPENING, ENDGAME) (((OPENING-ENDGAME)/3 << 16) + ENDGAME)

#define SHIELD      15             /* 1/2 Bonus per Pawn in Pawn shield   */
#define HSHIELD      8             /* half of SHIELD                      */
#define AIRHOLE      5             /* Bonus for immunity last-rank mate   */
#define BPAIR       PST( 40,  40)  /* Bishop-pair bonus                   */
#define PBONUS      PST(  5,   5)  /* extra P value when ahead in pieces  */
#define DOUBLED     PST( 40,  40)  /* penalty for isolated doubled Pawn   */
#define BACKWARD    PST( 20,  10)  /* backward- or isolated-pawn penalty  */
#define ROOK7       PST( 30,   0)  /* bonus for Rook on 7th rank          */
#define PAWN6       PST( 40,  20)  /* bonus for Pawn on 6th               */
#define PASSER6     PST(-70,  40)  /* extra bonus when it is passer       */
#define PAWN7       PST( 40, 100)  /* bonus for Pawn (passer!) on 7th     */
#define PROTPASS    PST( 40,  20)  /* extra bonus when it is protected    */
#define PASSVAL     PST( 20,  20)  /* bonus for having passer             */
#define KNIGHT_TRAP PST(200,   0)  /* penalty for N on a8 with p on a7    */
#define BISHOP_TRAP PST(150, 100)  /* penalty for B on a7 with p on b6    */
#define ROOK_TRAP   PST(250, 400)  /* penalty for R on h1 with K on f1/g1 */

#define INF 8000

#define WHITE  0
#define BLACK  8
#define NONE   4
#define PAWN   1
#define KING   2
#define KNIGHT 3
#define BISHOP 4
#define ROOK   5
#define QUEEN  6
#define PHASE  7
#define PASSER 7

int maxDepth, timeControl, mps, inc, timePerMove, timeLeft; // TC parameters

#define LOWER 1
#define UPPER 2

int ReadClock (int start);
char *MoveToText (int move);
int TimeIsUp (int mode);

typedef long long int Key;

typedef struct { // transposition table entry pair (32 bytes)
  Key signature[2];
  int move[2];
  short int score[2];
  char flags[2];
  char depth[2];
} HashBucket;

HashBucket *hashTable;
Key hashKey, pawnKey;
int hashMask;

#define PAWNMASK (1<<15)

typedef struct {     // entry for Pawn hash table (32 bytes)
  int signature;
  int score;         // packed opening & end-game scores
  char castles[4];   // Pawn shield quality at four castling locations (centi-Pawn)
  char passer[2];    // store info on one passer for each side
  char promoSqr[2];
  char promoDist[2];
  char openFiles[2]; // bitmap of half-open files
  char pawnShade[2]; // light-dark squaredPawns for determining Bishop quality
  char weakRook[2];  // penalty points for susceptability to orthogonal or diagonal attacks
  char weakBishop[2];
  char passerFlags;  // indicates whether rook-Pawn, protected passer, multiple passers
  char dummy[5];     // filler
} PawnInfo;

PawnInfo pawnHash[PAWNMASK+1]; // 1MB Pawn hash table (32K entries)

// move-generation tables
int steps[] = { // step vectors on 0x88 board as zero-terminated list for each piece type
  16, 15, 17, 0,                         // wP (0)
  -16, -15, -17, 0,                      // bP (4)
  14, 31, 33, 18, -14, -31, -33, -18, 0, // N  (8)
  1, -1, 16, -16, 0,                     // R (17)
  1, -1, 16, -16, 15, 17, -15, -17, 0    // K, Q (22) and B (26) 
};
int firstDir[] = { 3, 0, 22, 8, 26, 17, 22, 3,                 // first element in steps[] for various piece types
                   3, 4, 22, 8, 26, 17, 22, 3 };

// game state
int stm, rights;   // side to move, castling rights
char board[129];   // 0x88 board, interleaved with table of flags for square properties. Square 128 (=0x80) is dummy.
char counts[16];   // number of pieces of each (colored) type; 7th/15th element contains weighted totals white and black.
char location[16]; // location of a piece of each (colored) type. (Ill-defined if multiple pieces of the type exist.)
char dump[128];    // KLUDGE: dummy board to dump unneeded per-piece mobility counts

#define spoiler (board+8) /* 0x88 board with castling spoilers and other flags, interleaved with normal board */

// some info on the (colored) piece types
// (reminder:     .  P  K  N  B  R  Q  - )
char weight[] = { 0, 0, 0, 1, 1, 2, 4, 0,                         // contribution to game phase
                  0, 0, 0, 1, 1, 2, 4, 0 };
char code[]   = { 0, 16, 4+8+16, 32, 2+8+16, 1+4, 1+2+4+8+16, 0,  // flags that indicate where various piece types capture
                  0,  8, 4+8+16, 32, 2+8+16, 1+4, 1+2+4+8+16, 0 };
char key[]    = { 0, 0x20, 0, 0x30, 0x30, 0x40, 0x70, 0,          // for MVV sorting
                  0, 0x20, 0, 0x30, 0x30, 0x40, 0x70, 0 };
char mobFac[] = { 0, 0, 0, 4, 4, 2, 1, 0,                         // mobility bonus per square for various pieces
                  0, 0, 0, 4, 4, 2, 1, 0 };
int bitID[]   = { 0, 0, 0, 0x100, 0x400, 0x1000, 0x4000, 0,       // bits to identify possible King-neighborhood attackers
                  0, 0, 0, 0x100, 0x400, 0x1000, 0x4000, 0 };     //   note pieces coming in pairs have a 0-bit left of them
int value[]   = { 0, 80, INF, 325, 325, 500, 950, 0,              // piece base values
                  0, 80, INF, 325, 325, 500, 950, 0 };
int *pst[16];                                                     // pointers to piece-square tables, per piece
Key *zob[16], *zpk[16];                                           // pointers to tables of Zobrist keys for TT and Pawn hash

signed char rawTable[4*16*16]; // various data for 0x88 capture test (15x16 boards indexed by (signed) step vector)
Key zobTable[7*128];           // collected Zobrist data, zob[] points to the various 0x88 boards in it
int pstData[8*128];            // collected PST data, pst[] points to the various 0x88 boards in it
unsigned char kingSeige[256];  // lookup table to convert King-neighborhood attackers set to score

// offsets in rawTable to allow indexing by negative numbers
#define aligned  (rawTable + 7*16 + 8)
#define vector   (rawTable + 7*16 + 8 + 15*16)
#define nearKing (rawTable + 7*16 + 8 + 2*15*16)
#define dist     (rawTable + 9*16 + 8 + 3*15*16)

int ply, nodeCount, pstEval, forceMove, choice, rootMove, lastGameMove, rootScore, abortFlag, postThinking=1; // some frequently used data

// defaults of enables of various eval features, which can be overruled by engine options
int pawnStruct  = 0; // detect and score passers and other pawn types
int shelters    = 0; // judge the quality of the Pawn shelter around the King
int kingSafety  = 0; // evaluate threat to King based on number and kind of attackers to square next to it
int patterns    = 0; // recognize and devaluate trapped pieces, like white N on h8, B on a7, or R boxed in by own K
int mobEval     = 0; // give bonus for having many pseudo-legal moves
int drawishness = 0; // recognize material combinations with poor winning prospects, and reduce the score for those
int recognizers = 0; // recognize some position-dependent draws in end-game (like KPK) that do not quickly resolve

// Move stack: for each ply a generous 'frame' is reserved on this stack. It contains the move list and some other stuff:
// first 60 4-byte slots for captures (growing downward), then 200 for non-captures (growing upward), then a (0x88) board
// of 128 char for number of defenders per piece, interleaved with a board for storing mobility per opponent piece.
// Behind that it contains two int slots for passing one-sided mobility and King-seige from a null-move daughter to parent.

#define FRAME 300          /* size of frame on software ('move') stack */

int frame;                 // pointer to current 'frame' in move stack
unsigned int moves[30000]; // move stack
int killers[100][2];       // killer table
int pv[10000], *pvSP = pv; // PV 'tri-angular' array, and its stack pointer

#define protMob ((char*)(moves + frame + 200)) /* board hidden in move stack for counting protectors or mobility of each piece */

/********************************************************************************************/
/* Some notes on move format. This is basically 3 bytes: flags, from and to. The 4th byte   */
/* (MSB) in the int used to store it in the move stack is used for a sort key. In the hash  */
/* table the in-check information is packed here instead.                                   */
/*   When flags < 128 the move is a promotion, and upgrades the moved piece by adding flags */
/* to its type. (This includes code 0, which 'promotes' the piece to itself!) When the      */
/* upper bit of flags is set, the remaining 7 bits specify a square. Child nodes (to which  */
/* the move is passed) consider this square an 'e.p. flag': (apparent) Pawn non-captures to */
/* that square must be generated as e.p. captures, (as double-pushes set it to the skipped  */
/* square), while any pseudo-legal capture to such a square indicates the preceding castling*/
/* (which sets it on the Rook) passed through check. As squares are numbered in the 0x88    */
/* system, the '8' bit can be set to prevent matching any to-square, and this is used to    */
/* encode e.p. captures (which don't need special action in the reply). During move making  */
/* the '3rd square' (after clearing the 0x88 bits) will be e.p. captured, except for        */
/* castlings (recognized from having King as mover), which 'grow' a Rook on that square,    */
/* and clear the corner. (On double pushes the square would already be empty, and on        */
/* promotions it is set to a dummy empty square, so the e.p. capture has no effect there.)  */
/*   The code is written to allow an (extra) e.p. victim on any non-King move, and a        */
/* promotion on any move (but not both on the same move).                                   */
/********************************************************************************************/

void
Ray (int step, int code)
{ // initialize ray in 0x88 capture tables
  int i;
  for(i=1; i<8; i++) {
    aligned[i*step] = code;
    vector[i*step] = step;
    dist[i*step] = i;
  }
  aligned[step] *= 4; // contact attack has different code
}

void
SetPST (int *table, int baseValue, int opCenter, int egCenter, int push, int rank6, int rank7, int rotate)
{
  int f, r, i, opValue = 0.9*baseValue, egValue = 1.1*baseValue;
  for(i=0; i<8; i++) {
    r = (rotate ? 7-i : i)*16;
    for(f=0; f<8; f++) {
      int op, eg, score, d = (f-4)*(f-4) + (i-3.5)*(i-3.5) - 6;
      op = opValue - opCenter * d;   // base piece value and centralization in opening
      eg = egValue - egCenter * d;   // and end-game
      op = (op - eg)/3;              // pre-correct for game phase running upto 24 instead of 16
      score = (op << 16) + eg;       // pack
      if(i == 6) score += rank7;     // 7th-rank bonus
      if(i == 5) score += rank6;     // 6th-rank bonus
      score += (i - 1)*push;         // bonus for pushing forward
      table[r + f] = score; 
    }
  }
}

void
Init ()
{
  char *p;
  int i, f, r, dir = firstDir[KNIGHT], step;

  // distance table (nr of King steps needed to travel given vector)
  for(r=0; r<10; r++) { // KLUDGE: rank runs up to 9 to allow vertically off-board squares
    // KLUDGE: 
    for(f=0; f<8; f++) dist[16*r+f] = dist[-16*r+f] = dist[16*r-f] = dist[-16*r-f] = r;
    for(f=r; f<8; f++) dist[16*r+f] = dist[-16*r+f] = dist[16*r-f] = dist[-16*r-f] = f;
  }

  // initialize capture codes and vectors
  Ray(1, 1);  Ray(-1, 1);  Ray(16, 1); Ray(-16, 1); // Rook
  Ray(15, 2); Ray(-15, 2); Ray(17, 2); Ray(-17, 2); // Bishop & Pawn
  aligned[15] = aligned[17] = 16;                   // white Pawn
  while((step = steps[dir++])) aligned[step] = 32;  // Knight

  // initialize Zobrist keys
  for(i=0; i<7; i++) {                      // loop over piece types
    zob[i]   = zobTable + i*128;
    zob[i+8] = zobTable + i*128 + 8;        // interleave white and black tables
    zpk[i]   = zobTable + i*128;            // pawn keys
    zpk[i+8] = zobTable + i*128 + 8;        // interleave white and black tables
    if(i > 1) zpk[i] = zpk[i+8] = zobTable; // non-Pawns use zero table
  }
  for(p=(char*)&zobTable[128]; p<(char*)&zobTable[7*128]; p++)  *p = rand() >> 6;

  // initialize piece-square tables
  for(i=0; i<8; i++) {
    pst[i]   = pstData + i*128;
    pst[i+8] = pstData + i*128 + 8; // interleave white and black tables
  }
  for(i=0; i<9; i+=8) { // for both colors
    SetPST(pst[i+KING],  100,          -5, 3, 0, 0, 0, i); // dummy value to avoid negative PST
    SetPST(pst[i+QUEEN], value[QUEEN],  0, 0, 0, 0, 0, i);
    SetPST(pst[i+ROOK],  value[ROOK],   0, 0, 0, 0, ROOK7, i);
    SetPST(pst[i+BISHOP],value[BISHOP], 2, 2, 2, 0, 0, i);
    SetPST(pst[i+KNIGHT],value[KNIGHT], 2, 2, 2, 0, 0, i);
    SetPST(pst[i+PAWN],  value[PAWN],   1, 2, 1, PAWN6, PAWN7, i);
    SetPST(pst[i+PASSER], PASSVAL,      0, 0, 5, PASSER6, 0, i);
  }

  // special-squares table (interleaved with spoiler)
  spoiler[0]   = 1; spoiler[4]   = 1|4; spoiler[7] = 4;   // white castling spoilers
  spoiler[112] = 2; spoiler[116] = 2|8; spoiler[119] = 8; // black castling spoilers
  for(r=0; r<8; r++) for(f=0; f<8; f++) {                 // promotion and double-push zones
    spoiler[16*r+f] |= (r==7)*16 + (r==0)*32 + (r==2)*64 + (r==5)*128;
  }

  // King-seige decoding
  dir = firstDir[KING];
  while((step = steps[dir++])) nearKing[step] = 1; // flags King neighborhood
  for(i=0; i<256; i++) { // table used to multiply weighted attacks (Q=4, R=2, minor=1 per square)
    int nMinor = (i&1) + (i>>1&1) + (i>>2&1) + (i>>3&1);
    int nRook  = (i>>4&1) + (i>>5&1);
    int nQueen = (i>>6&1) + (i>>7&1);
    kingSeige[i] = nMinor + nRook + 2*nQueen;  // e.g. Q, 2sq = 16, B(2sq)+Q(2sq) = 30, 2R(4sq) = 16, Q(2sq)+R(3sq) = 42 
  }
}

/**********************************************************************************************/
/* Evaluation routines                                                                        */
/* - Patterns for trapped pieces, that are likely lost                                        */
/* - Detection of some end-games wheredrawishness depend on piece location (KPK, KBPK, KQKP)  */
/* - Detection of drawishness based on material (lack of Pawns or other mating potential)     */
/* - Pawn evaluation (passers, isolated/backwards, doubled)                                   */
/* game-phase-dependent piece-square scoring is done incrementally in Search()                */
/**********************************************************************************************/

int
Patterns ()
{
  int score = 0;

  // Knight in enemy corner
  if(board[0]   == BLACK+KNIGHT && board[16] == WHITE+PAWN) score += KNIGHT_TRAP;
  if(board[7]   == BLACK+KNIGHT && board[23] == WHITE+PAWN) score += KNIGHT_TRAP;
  if(board[112] == WHITE+KNIGHT && board[96] == BLACK+PAWN) score -= KNIGHT_TRAP;
  if(board[119] == WHITE+KNIGHT && board[103]== BLACK+PAWN) score -= KNIGHT_TRAP;

  // Bishop at enemy edge Pawn
  if(board[16] == BLACK+BISHOP && board[33] == WHITE+PAWN) score += BISHOP_TRAP;
  if(board[23] == BLACK+BISHOP && board[38] == WHITE+PAWN) score += BISHOP_TRAP;
  if(board[96] == WHITE+BISHOP && board[81] == BLACK+PAWN) score -= BISHOP_TRAP;
  if(board[103]== WHITE+BISHOP && board[86] == BLACK+PAWN) score -= BISHOP_TRAP;

  // Rook boxed in by own King
  if((board[1]  == WHITE+KING || board[2]  == WHITE+KING) &&
     (board[0]  == WHITE+ROOK || board[16] == WHITE+ROOK) &&
     (board[16] == WHITE+PAWN || board[32] == WHITE+PAWN)   ) score -= ROOK_TRAP;
  if((board[6]  == WHITE+KING || board[5]  == WHITE+KING) &&
     (board[7]  == WHITE+ROOK || board[23] == WHITE+ROOK) &&
     (board[23] == WHITE+PAWN || board[39] == WHITE+PAWN)   ) score -= ROOK_TRAP;
  if((board[113]== BLACK+KING || board[114]== BLACK+KING) &&
     (board[112]== BLACK+ROOK || board[96] == BLACK+ROOK) &&
     (board[96] == BLACK+PAWN || board[80] == BLACK+PAWN)   ) score += ROOK_TRAP;
  if((board[118]== BLACK+KING || board[117]== BLACK+KING) &&
     (board[119]== BLACK+ROOK || board[103]== BLACK+ROOK) &&
     (board[103]== BLACK+PAWN || board[87] == BLACK+PAWN)   ) score += ROOK_TRAP;

  return score;
}

int
KQKPdraw (int side)
{
  int pawn, promo, king, hisKing, d;

  if(stm == side, 1) return 0;                    // on our move we might be able to capture his king
  pawn = location[side+PAWN];
  if(board[pawn] != side+PAWN) return 0;       // sanity check, as location[] is not 100% reliable for Pawns
  promo = (pawn & 7) + 7*16*!side;             // promotion square
  if(5*promo & 4) return 0;                    // never draw for b,d,e or g Pawn ('1' and '4' bit of file differ!)
  if(pawn != (promo^16)) return 0;             // do not assume draw before P on 7th-rank
  hisKing = location[KING+8-side];             // weak-side King
  king = location[side+KING];                  // strong-side King
  if((d = dist[king - promo]) < 4) return 0;   // ... is dangerously close
  if(3*promo & 2) {                            // c- or f-Pawn (tests if '1' and '2' bit differ)
    if(pawn == (promo^16));
  } else {                                     // a- or h-Pawn
    if(dist[hisKing - promo] < 2) return 1;    // weak King protects or is on promotion square
  }

  return 0; // TODO
}

int
KPKdraw (int side)
{ // identify some draw positions that will not quickly resolve.
  int king = location[side+KING], pawn = location[side+PAWN], hisKing = location[KING+8-side];
  int myTurn = (side == stm), forward = 16-4*side; // from strong-side POV
  int promo = (pawn & 7) + 7*16*!side;             // promotion square
  int lastRank = !((hisKing^promo) & 0x70);        // bare King is on last rank
  int next = (king == pawn+1 || king == pawn-1);   // strong-side King standing beside Pawn
  int edge = (pawn & 7) == 0 || (pawn & 7) == 7;   // Pawn is on edge file
  int d = dist[pawn-promo], stopSqr;

  if(board[pawn] != side+PAWN) return 0;       // sanity check, as location[] is not 100% reliable for Pawns
  if(dist[hisKing-king] < 2) return 0;         // reject illegal position. (KPKdraw is called before legality test...)
  if(myTurn && aligned[hisKing-pawn] & code[side+PAWN]) return 0; // Pawn captures King

  // rule of squares
  d -= (d ==6);                                      // account for initial double-push
  if(d - myTurn < dist[hisKing-promo] - 1) return 0; // outside 'square' is always won

  if(edge) { // rook-pawns: detect some cases that don't apply to other Pawns
    int dPath = (hisKing^pawn) & 7;                                       // distance of bare King to Pawn path
    if(dPath < 2 && dist[hisKing - promo] < d - dPath) return 1;          // bare King in or next to path of Pawn
    if(((king^pawn) & 7) == 0 && dist[hisKing - (king^2)] < 2 - myTurn)   // own King is trapped in path of Pawn
      return 1;
  }

  if(hisKing == pawn + forward)   return (!lastRank | myTurn);            // I must stalemate him or abandon Pawn
  if(pawn == (promo^16)) return 0;                                        // play out other 7th-rank Pawns

  stopSqr = pawn + 2*forward;
  if(stopSqr == promo) { // last-rank defense; quite tricky!
    return (dist[hisKing - promo] < 2 && (!next || dist[king + 2*forward - hisKing] + myTurn == 1));
  } else return (dist[stopSqr - king] - myTurn > dist[stopSqr - hisKing]);
  if(hisKing == stopSqr) return (edge | !lastRank | !myTurn | !next);     // 2 squares in front of Pawn

  if((pawn&0x70) == (king&0x70)) { // attacking K is on same rank as P
    if(dist[stopSqr - hisKing] <= 1 && !myTurn && !lastRank) return 1;    // he can step in path
    if(hisKing == king + 2*forward) return (!lastRank || next && myTurn); // oppose K next to P (cut off on last rank!)
    if(dist[king + 4*forward - hisKing] + myTurn == 1) return 1;                   // he has far opposition (or can take it)
  }
  if((pawn&0x70) == (king&0x70) - forward) { // attacking K is on rank before P
    return (!lastRank & dist[king + 2*forward - hisKing] + myTurn == 1);           // he has opposition, or can take it
  }

  return 0;
}

int
KBPKdraw (int side)
{
  int pawn = location[side+PAWN];
  int file = pawn & 7;
  int d, dk, promo, bishop, king, hisKing;

  if(file != 0 && file != 7) return 0;   // only drawish with rook Pawns
  if(stm == side) return 0;              // do not use after weak side's move, as he might have stepped in check
  if(board[pawn] != side+PAWN) return 0; // sanity check, as location[] is not 100% reliable for Pawns

  hisKing = location[KING+8-side];
  promo = file + 7*16*!side;                // promotion square
  d = dist[pawn-promo]; d -= (d == 6);      // number of turns needed to promote Pawn
  dk = dist[hisKing-promo] - (side != stm); // weak-side King to promotion on strong side's turn
  if(dk > d) return 0;                      // Pawn reaches promotion square 3 ply before defending King

  king = location[side+KING];
  if(dist[hisKing-king] < 2) return 0;      // reject illegal position. (KBPKdraw is called before legality test...)
  if(dk >= dist[king-promo] - 1) return 0;  // weak-side K cannot reach promo-square 3 ply before strong K

  bishop = location[side+BISHOP];
  if(board[bishop] != side+BISHOP) return 0;      // another sanity check
  if(((promo ^ bishop)*0x11 & 16) == 0) return 0; // Bishop has color of promotion square

  return 1; // draw
}

int
Fortress0 (int side)
{ // called when 'side' (who is stronger) has no Pawns
  int n;
  if(!recognizers) return 0;
  if(counts[PHASE+8-side]) return 0;      // Opponent has pieces
  if(counts[PAWN+8-side] != 1) return 0;  // Opponent has zero or multiple Pawns
  n = counts[side+PHASE];
  if(n == 4 && counts[side+QUEEN] == 1) return KQKPdraw(side);
  // TODO: KRKP
  return 0;
}

int
Fortress1 (int side)
{ // called when 'side' (who is stronger) has single Pawn
  int n;
  if(!recognizers) return 0;
  if((n = counts[side+PHASE]) == 0) { // the Pawn is the only thing we have
    if(counts[PAWN+8-side]) return 0; // note we cannot be the leading side if he also has pieces
    return KPKdraw(side);
  }
  if(n == 1) { // minor
    if(counts[side+BISHOP]) return KBPKdraw(side);
  }
  return 0;
}

int
KBNKhelp (int side)
{
  int bishop = location[side+BISHOP], hisKing = location[KING+8-side];
  int good = (0x11*bishop & 16 ? 7 : 0); // corner of the same shade than the Bishop
  int d1 = dist[good - hisKing], d2 = dist[(good ^ 0x77) - hisKing];  
  if(d1 > d2) d1 = d2;                   // d1 is distance to closest deadly corner
  return (d1 > 3 ? 2 : d1>1);            // discount (x 1/2 or 1/4) when bare King far from deadly corner
}

int
Scaling (int side)
{ // calculate by which factor the score of the given (stronger) side should be reduced due to lack of winning prospects
  int oppo = side ^ 8; // weaker side
  int me = counts[side+PHASE], him = counts[oppo+PHASE];
  if(me + him == 2 && counts[WHITE+BISHOP] && counts[BLACK+BISHOP] && // only material is two Bishops, one each
       (location[WHITE+BISHOP] ^ location[BLACK+BISHOP])*0x11 & 16)   // they are not on squares of the same shade
      return 1;                                                       // so 'unlike Bishops' (x 1/2) 
  if(counts[side+PAWN] > 1) return 0;                              // otherwise no discount with multiple Pawns
  if(counts[side+PAWN] == 0) {                                     // no Pawns at all usually makes it far more difficult
    if(me == 1) return 5;                                          // only single minor: no hope at all (x 1/32)
    if(me == 2 && counts[side+KNIGHT] == 2)                        // NOTE: PHASE contains minors + 2*Rooks + 4*Queens
      return 5 - 2*(him == 0 && counts[oppo+PAWN]);                // only 2 Knights, x 1/32 (for perhaps KNNKP[P...] x 1/8)
    if(me <= him + 1) return 3;                                    // minor (or less) ahead usually is dead draw (x 1/8)
    if(him + counts[oppo+PAWN] == 0) {                             // he has bare King
      if(me == 2 && counts[side+KNIGHT] && counts[side+BISHOP])    // KBNK (or in general, mates where corner color matters)
        return KBNKhelp(side);                                     // KLUDGE: abuse scaling to discourage wrong corner
      return 0;                                                    // no discount in other known mates against bare King
    }
    if(Fortress0(side)) return 4;                                  // Detects known draw positions in KQKP etc.(x 1/16)
    return (him + counts[oppo+PAWN] > 0);                          // if not against bare King, end-games without Pawns x 1/2
  } else {                                                         // one Pawn, trouble if opponent can afford to sac piece for it
    if(me <= him) {                                                // he is now not behind in pieces
      if(counts[oppo+KNIGHT] + counts[oppo+BISHOP]) return 2;      // and has a minor to sac (x 1/4, turns into x 1/8 after sac)
      if(counts[oppo+ROOK] && me < him ) return 2;                 // he can sac Rook if he is at least one minor ahead (x 1/4)
    } else
    if(me == 2 && him == 1 && counts[side+KNIGHT] == 2) return 1;  // he has 1 minor vs our 2, but against 2N he can sac it (x 1/2)
    if(Fortress1(side)) return 8;                                  // Detects know draw positions in KPK, KBPK etc.
  }
  return 0;                                                        // 1 Pawn, but if he makes a sac for it we have winning advantage
}

int
PasserBlock (int side, PawnInfo *pi)
{ // Kings-dependent bonus for most advanced passer of 'side'
  int dk, s = side>>3, k = location[side+KING];          // supporting King
  int stopSqr = pi->passer[s] + 32 - 64*s;               // two squares before Pawn
  int score, d = pi->promoDist[s];                       // racing distance to promotion
  score = 15 - 2*dist[k - stopSqr];                      // bonus for being close before Pawn to support its advance
  if((k-stopSqr & 7) == 0) score -= 10, d++;             // but penalty for being in same file, delays promotion
  k = location[KING+8-side];                             // defending King
  dk = dist[k - pi->promoSqr[s]];                        // distance to its promotion square
  score -= 25 - 3*(dist[k - stopSqr] + dk);              // penalty for defending King hindering Pawn
  if(dk - (side != stm) > pi->promoDist[s]) {            // defending King is outside 'square'!
    score += 20;                                         // extra bonus for that
    if(counts[WHITE+PHASE] + counts[BLACK+PHASE] == 0) { // if this happens in Pawn ending
      if(pi->passer[1-s] < 0 &&                          // and he has no passer
         pi->promoDist[1-s] - (side != stm) > d)         // or his passer promotes at least 3 ply after ours
      score += 250;                                      // we count ourselves rich (score will still be doubled!)
    }
  }
  return score;
}

int
Shield (int side, int sqr, int myOpen, int hisOpen)
{ // calculate quality of the Pawn shield at the given location
  int score = 0, wall = 0, file = sqr & 7, pawn = side + PAWN, forward = (4 - side)*4;
  sqr += forward;                    // first check three Pawns directly in front of King
  if(sqr + forward & 0x88) return 0; // King must not be on 7th or 8th rank

  if(board[sqr] == pawn) score += 3*HSHIELD, wall++;
  if(sqr+1 & 0x88 || board[sqr+1] == pawn) score += SHIELD, wall++;
  if(sqr-1 & 0x88 || board[sqr-1] == pawn) score += SHIELD, wall++;
  if(wall == 3) score -= AIRHOLE;    // penalty for 'mate behind the Pawns' possibility

  sqr += forward; // Pawns advanced by one square are somewhat less useful 
  if(board[sqr] == pawn) score += HSHIELD;
  if(!(sqr+1 & 0x88) && board[sqr+1] == pawn) score += HSHIELD;
  if(!(sqr-1 & 0x88) && board[sqr-1] == pawn) score += HSHIELD;

  if(myOpen & 1 << file) score -= HSHIELD;     // penalty for half-open own files
  if(myOpen & 2 << file) score -=  SHIELD;     // central one is worst
  if(myOpen & 4 << file) score -= HSHIELD;

  if(hisOpen & 1 << file) score -= 3*HSHIELD;  // penalty for half-open opponent files...
  if(hisOpen & 2 << file) score -= 3*HSHIELD;  //    directed against our shield
  if(hisOpen & 4 << file) score -= 3*HSHIELD;

  return score;
}

void
PawnEval(PawnInfo *info)
{
  int i, sq, pawnFiles, w=0, b=0, score = 0;
  char white[8], black[8], rearWhite[10], rearBlack[10];

  for(i=0; i<10; i++) rearWhite[i] = 0x70, rearBlack[i] = 0; // initialize most backward Pawn for each file
  info->passer[0] = info->passer[1] = -1;                    // invalid square number means no passer
  info->pawnShade[0] = info->pawnShade[1] = 0;               // light - dark Pawn difference

  // create pawn list
  for(sq=1*16; sq<7*16; sq=sq+9&~8) { // scan Pawn area of 0x88 board rank by rank
    int piece = board[sq];
    if(piece == WHITE+PAWN) white[w++] = sq; else // and create pawn lists
    if(piece == BLACK+PAWN) black[b++] = sq;      // for white and black separately
  }

  // determine rear guards
  for(i=w-1; i >= 0; i--) {
    sq = white[i];
    rearWhite[(sq & 7) + 1] = sq & 0x70; // because of scan order last is always most backward
  }
  for(i=0; i<b; i++) {
    sq = black[i];
    rearBlack[(sq & 7) + 1] = sq & 0x70;
  }

  // determine pawn types
  pawnFiles = 0;
  for(i=w-1; i>=0; i--) {                                                   // loop through white Pawns, high to low rank
    int d, r = (sq = white[i]) & 0x70, f = sq & 7;                          // get file and rank
    if(0x11*sq & 16) info->pawnShade[0]++; else info->pawnShade[0]--;       // update shading count
    if(!(pawnFiles & 1<<f) &&                                               // only first (= most forward Pawn) in file
       r >= rearBlack[f] && r >= rearBlack[f+1] && r >= rearBlack[f+2]) {   // Pawn is passer
      score += pst[PASSER+WHITE][sq];                                       // give position-dependent bonus for that
      if(f == 0 || f == 7) info->passerFlags |= 0x10;                       // remember passer is on edge file
      if(f != 0 && board[sq-17] == WHITE+PAWN ||                            // test for Pawns diagonally behind it
         f != 7 && board[sq-15] == WHITE+PAWN   ) {                         // we have protected passer!
        info->passerFlags |= 1; score += PROTPASS;                          // flag that fact, and give bonus
      }
      if(info->passer[0] < 0) {                                             // first passer (and thus most forward)
        info->passer[0]= sq;                                                // remember its location
        info->promoSqr[0] = sq | 0x70;                                      // promotion square
        d = dist[sq - (sq|0x70)]; info->promoDist[0] = d - (d == 6);        // and promotion distance (in moves)
      } else info->passerFlags |= 4;                                        // multiple passers, just flag
    }
    if(r < rearWhite[f] && r < rearWhite[f+2]) {                            // backward (or isolated)
      if(r < 0x30) score -= BACKWARD;                                       // penalty for holes on own board half
      if(r != rearWhite[f+1]) score -= DOUBLED;                             // isolated doubled Pawn is pretty bad
    }
    pawnFiles |= 1<<f;                                                      // mark file as Pawn-containing
  }
  info->openFiles[0] = ~pawnFiles; pawnFiles = 0;
  for(i=0; i<b; i++) {
    int d, r = (sq = black[i]) & 0x70, f = sq & 7;                          // get file and rank
    if(0x11*sq & 16) info->pawnShade[1]++; else info->pawnShade[1]--;       // update shading count
    if(!(pawnFiles & 1<<f) &&                                               // only first (= most forward Pawn) in file
       r <= rearWhite[f] && r <= rearWhite[f+1] && r <= rearWhite[f+2]) {   // Pawn is passer
      score -= pst[PASSER+BLACK][sq];                                       // give position-dependent bonus for that
      if(f == 0 || f == 7) info->passerFlags |= 0x20;                       // remember passer is on edge file
      if(f != 0 && board[sq+15] == BLACK+PAWN ||                            // test for Pawns diagonally behind it
         f != 7 && board[sq+17] == BLACK+PAWN   ) {                         // we have protected passer!
        info->passerFlags |= 2; score -= PROTPASS;                          // flag that fact, and give bonus
      }
      if(info->passer[1] < 0) {                                             // first passer (and thus most forward)
        info->passer[1]= sq;                                                // remember its location
        info->promoSqr[1] = f;                                              // promotion square
        d = dist[sq - f]; info->promoDist[1] = d - (d == 6);                // and promotion distance (in moves)
      } else info->passerFlags |= 8;                                        // multiple passers, just flag
    }
    if(r > rearBlack[f] && r > rearBlack[f+2]) {                            // backward (or isolated)
      if(r > 0x40) score += BACKWARD;                                       // penalty for holes on own board half
      if(r != rearBlack[f+1]) score += DOUBLED;                             // isolated doubled Pawn is pretty bad
    }
    pawnFiles |= 1<<f;                                                      // mark file as Pawn-containing
  }
  info->openFiles[1] = ~pawnFiles;

  // determine quality of Pawn shields for Kings on b1, g1 (white) and b8, g8 (black)
  info->castles[0] = Shield(WHITE, 1,   info->openFiles[0]*2,  info->openFiles[1]*2); // b1
  info->castles[2] = Shield(WHITE, 6,   info->openFiles[0]*2,  info->openFiles[1]*2); // g1
  info->castles[1] = Shield(BLACK, 113, info->openFiles[1]*2,  info->openFiles[0]*2); // b8
  info->castles[3] = Shield(BLACK, 118, info->openFiles[1]*2,  info->openFiles[0]*2); // g8

  info->score = score; // white POV score
}

int
KingSafety (int side, PawnInfo *pawns)
{ // calculate lazy part of King safety (Pawn structure, no actual attacks)
  static int delayFactor[] = { 13, 10, 8, 6, 4 };             // in units of 1/16
  int score, bad, phase, s = side >> 3;                       // s=0 (white) or 1 (black)
  int kSide = pawns->castles[s+2], qSide = pawns->castles[s]; // value of shields at castling locations are kept in pawn hash
  if(~rights & 5 << s) { // we still can castle; evaluate safety based on that we will, and how long that will take
    int k, q; // devaluate castles based on how many pieces are still blocking the path to reach them:
    if(rights & 1 << s) q = 0; else q = qSide * delayFactor[4 - !board[1] - !board[2] - !board[3]] >> 4;
    if(rights & 4 << s) k = 0; else k = kSide * delayFactor[2 - !board[5] - !board[6]] >> 4;
    score = (k > q ? k + (q >> 3) : q + (k >> 3));  // pick best, with small bonus (1/8 x normal) for good alternative
    bad = (kSide <= SHIELD && qSide <= SHIELD);     // no good possibility to castle exists (minimal is e.g. Pg2,h3)
  } else {    // we can no longer castle. if we are close to ideal position (b1, g1), evaluate based on that
    int d, king = location[side+KING];
    bad = 0;
    if((d = dist[6 + 0x70*s - king]) < 2) score = kSide - 35*(d == 1 && kSide >= 35); else // next to g1/g8
    if((d = dist[1 + 0x70*s - king]) < 2) score = qSide - 35*(d == 1 && qSide >= 35); else // next to b1/b8
      score = 0, bad = 1; // not close to either ideal position, so forget about those
  }

  if(bad) { // no or unreachable standard fortress; improvise
    bad = Shield(side, location[side+KING], pawns->openFiles[s]*2, pawns->openFiles[1-s]*2) - 50; // try in current location
    if(bad > score) score = bad;                           // and assume we set up camp there if standard locations are worse
  }

  phase = counts[PHASE+8-side];                            // opponent material (0-12, but >= 4 if balanced)
  return (phase >= 8 ? 2*score : score*(phase - 4)  >> 1); // taper towards zero if material gets below something like QRBN, QRR
}

int Evaluate ()
{ // evaluation (lazy part), done in all nodes, to gate more accurate futility pruning
  int eval, discount = 0, gamePhase, diff, nP, xstm = stm ^ 8;
  PawnInfo *pawns = pawnHash + (pawnKey & 0x7FFF);
  if(pawns->signature != (pawnKey >> 32)) { // pawn-hash miss:
    pawns->signature = (pawnKey >> 32);     // create required entry (always-replace scheme)
    PawnEval(pawns);                        // probes Pawn hash table, and fills 'pawns' with info
  }
  gamePhase = counts[WHITE+PHASE] + counts[BLACK+PHASE];     // 0-24
  diff  = counts[WHITE+PHASE] - counts[BLACK+PHASE];         // difference in piece material
  nP    = counts[WHITE+PAWN] + counts[BLACK+PAWN];           // number of Pawns
  eval = BPAIR * ((counts[WHITE + BISHOP] > 1) - (counts[BLACK + BISHOP] > 1)); // Bishop-pair bonus
  if(patterns)   eval += Patterns();                         // add penalty for some bad patterns
//      eval += diff * nP * PBONUS;                                // incentive to avoid trading Pawns when ahead
  if(pawnStruct) {
    eval += pawns->score;                                    // add pawn-structure eval
    if(pawns->passer[0] >= 0) eval += PasserBlock(WHITE, pawns);          
    if(pawns->passer[1] >= 0) eval -= PasserBlock(BLACK, pawns);
  }
  eval = eval * (4 - stm) >> 2;                              // set correct POV
  eval += pstEval;                                           // incrementally updated PST eval (already stm POV)
  eval = eval*(gamePhase | 1<<19) >> 19;                     // interpolate opening/end-game as per game phase
  if(shelters && gamePhase > 8)                              // more than Q vs Q, RR vs RR etc.
    eval += KingSafety(stm,  pawns),                         // own King fortress (takes account of game phase!)
    eval -= KingSafety(xstm, pawns);                         // opponent King fortress
  if(drawishness) discount = Scaling(eval > 0 ? stm : xstm); // reduction factor (as power of two) for drawishness
  if(discount == 8 && ply) return eval>>2;                   // no further search on guaranteed draw 
  if(discount) eval >>= discount;                            // scale drawish endgames towards zero
  else if(gamePhase == 0) eval *= 2;                         // double advantage in (non-drawish) Pawn endings
  return eval;                                               // this is a lazy eval, without mobility and King seige
}

/**************************************************************************************/
/* Search routine that does about everything in-line. Its structure is:               */
/* 1) Probe hash table                                                                */
/* 2) Determine if the preceding (non-null) move checked us (if hash did not tell us) */
/* 3) Apply (check-)extension and reductions (passed to us from from parent)          */
/* 4) Take hash cutoff if draft allows it                                             */
/* 5) (Lazy) evaluation of position (mobility and King safety guessed from parent)    */
/* 6) QS: Stand pat on lazy-eval score if good enough (with margin)                   */
/* 7) Generate moves; In QS only keep captures. Return +INF instantly on King capture */
/* 8) Search null move, for accepting its fail high or as dummy (see below)           */
/* 9) Calculate mobility and King safety (side effect of move gen and null move)      */
/* 10) QS: Try to stand pat again on precise evaluation                               */
/* 11) Validate hash move, and put it in front of move list                           */
/* 12) Store draw in hash entry for current position, to get rep-draws along branch   */
/* 13) Search moves, using IID starting at depth from hash probe (if valid) or d=1    */
/*     A) Loop over moves:                                                            */
/*         a) Move sort: extract best capture, or validate killers (if not yet done)  */
/*         b) Decode and make move                                                    */
/*         c) Recursion with self-deepening (fail-highs always will have full depth)  */ 
/*         d) In root: we can exit here for a chosen move, leaving it performed       */
/*         e) Unmake the move                                                         */
/*         f) Process the returned score (possibly cutting off move- and IID loop)    */
/*     B) Decide on continuation of self-deepening                                    */
/*     C) Stalemate correction on ultimate fail low (i.e. when all moves illegal)     */
/* 14) Hash store                                                                     */
/* 15) Delayed-loss bonus                                                             */
/*                                                                                    */
/* KLUDGE: Search can also be used for calculating one-sided mobility, by calling it  */
/* with trheshold argument set to INF+10. This suppresses the hash probe, and passing */
/* previousPly = 0 (null move) suppresses the check test, while depth > 0 suppresses  */
/* evaluation and stand-pat, so that we directly go into move generation. It then     */
/* returns with the mobility that is calculated during it.                            */
/**************************************************************************************/

int
Search (int startAlpha, int beta, int threshold, int mobilityGuess, int previousPly, int reduction, int depth, int extraDepth)
{
  int killer1 = killers[ply][0], killer2 = killers[ply][1]; // remember original killers
  int captures, noncapts, lastMove, unsorted, bestMove;
  Key sig, savedHashKey = hashKey, savedPawnKey = pawnKey;
  int savedPstEval = pstEval, eval, alpha, startScore, bestScore, hashScore, refScore;
  int from, checker, xKing, king = location[stm + KING];
  int epFlag= previousPly >> 16 & 0xFF ^ 0x80; // square to wich we can e.p. capture (or off-board)
  int hashDepth, iterDepth = -2; // KLUDGE: use this invalid value for iterDepth to signal hash miss.
  int hashMove;
  int i, slot = -1, checkDir, checkDist, mobility, nMoves, seige, *startPV = pvSP, savedRights = rights;
  char *pieceMob, hashFlags;
  HashBucket *bucket;

  refScore = 0; // (pstEval << 16) >> 16;          // sign extend endgame eval
  startAlpha -= (refScore >= startAlpha);          // pre-adjust window for delayed-loss bonus
  beta       -= (refScore >  beta);
  if(beta <= -INF) { bestScore = -INF; goto Out; } // mate-distance pruning
  *pvSP++ = 0;                                     // set up empty PV

  if(threshold != INF+10) {                          // KLUDGE: if just mobility call we skip everything before move generation
    // probe Transposition Table
    i = (1 + stm)*(32 + epFlag);                     // stm and e.p. key. Causes undeserved misses if no actual e.p. victim...
    bucket = hashTable + (hashKey & hashMask ^ i);   //   (or after promotion/castling/e.p.capt. But who cares? These are rare)
    sig = hashKey ^ pstEval;                         // make sure bits used for index are different in signature
    if(bucket->signature[0] == sig) slot = 0; else
    if(bucket->signature[1] == sig) slot = 1; else slot = -1;
    if(slot >= 0) {                                  // we have a hash hit
      hashMove = bucket->move[slot] & 0xFFFFFF;      // in any case use the move
      if(ply > 0 &&                                  // force starting at d=0 in root. TODO: in all PV nodes? (optionally?)
         (bucket->flags[slot] & LOWER ||
          bucket->score[slot] <= startAlpha) &&
         (bucket->flags[slot] & UPPER ||
          bucket->score[slot] >= beta)  ) {          // bound is useful
        iterDepth = bucket->depth[slot];             // so IID can continue from stored depth
        bestScore = bucket->score[slot];             // and use the score
      } else iterDepth = -1;                         // signals unusable hit
      checker  = bucket->move[slot] >> 24;           // restore in-check info from hashed checker
      if(checker >= 0) checkDir = vector[king-checker], checkDist = dist[king - checker];
    } else {                                             // hash miss. Reserve slot for new entry by 'under-cut replacement':
      static int f;                                      // 'coin-flip' variable, to 'randomize' which slot we try to undercut
      if(bucket->depth[f] == bucket->depth[!f] + 1)      // if stored depths differ by 1 we replace the deepest in 50% of cases...
        slot = f, f = !f;                                //    to prevent deep entries live forever and eventually poison the table
      else slot = (bucket->depth[0] > bucket->depth[1]); // otherwise 'slot' is set (to 0 or 1) to indicate lowest-depth entry
      hashMove = -1;                                     // no hash move on hash miss. KLUDGE: -1 signals miss
      // on a hash miss we have to determine ourselves if we are in check (signalled by setting checker >= 0).
      if(!previousPly) checker = -1; else {                          // after null move never in check
        int ep = epFlag ^ 8;                                         // square where e.p. capture might just have taken place
        // first test for direct checks (by the latest mover)
        checker = previousPly & 0xFF, checkDir = 0;                  // to-square of previous move, assume contact check
        if(aligned[king - checker] & code[board[checker]]) {         // is positioned for checking us
          if(aligned[king - checker] & 1+2) {                        // but from a distance
            int step = vector[king - checker], to = king;
            while(board[to -= step] == 0);                           // scan ray up to first obstacle, from King towards checker
            if(to != checker) checker = -1;                          // attack is blocked
            else checkDir = step, checkDist = dist[king - checker];  // remember check ray & distance (to detect interposition)
          }
        } else checker = -1;
        // then test for discovered check over from-square and (possibly) square cleared by e.p. capture
        from = previousPly >> 8 & 0xFF;                                 // step from evacuated square to King
        if(ep & 0x88) ep = from;                                        // KLUDGE: invalid e.p. set to from-square
        do {                                                            // we might have to test twice, if e.p. cleared square
          if(aligned[from - king] & 1+2+4+8+16) {                       // from-square lies in principal direction
            int piece, step = vector[king - from], to = king;
            while(!((to -= step) & 0x88)) {                             // scan ray up to board edge
              if((piece = board[to])) {                                 // we find a piece there
                if(to != checker &&                                     // do not double-count an already-found checker
                   piece+stm & 8 && code[piece] & aligned[king - to]) { // which is an enemy that slides along the ray
                  if(checker < 0) checker = to, checkDir = step;        // remember the discovered check
                  else checker = king, checkDir = 0;                    // KLUDGE: fake that King checks itself on double check...
                  checkDist = dist[checker-king];                       //   so no capture of checker and interposition possible
                }
                break;                                                  // don't scan beyond obstacle
          } } }
        } while(ep != from && (from = ep));                             // after true e.p. repeat disco-check test with e.p. square
      }
    }

    // TRICKY! Extension control. Apply (and remember we applied) LMR requested by parent only if not in check.
    // When the move in the parent fails high we undo that reduction by increasing 'depth' again (stepwise).
    // Parent fail-highs are deepened this way until they can satisfy the ultimate, unreduced depth of the parent node.
    if(checker >= 0) depth++, reduction = 0; // extend evasion when in check
    else depth -= reduction;                 // or apply requested reduction when move in parent was 'late'
    reduction += extraDepth;                 // self-deepening on fail low (= parent fail high)

    // now we are in a position to determine if hash depth was sufficient
    if(iterDepth >= 0) {                           // we had a hash hit with usable score bound
      if(checker == -3) iterDepth = 256;           // mates valid to any depth  TODO: upgrade longer mates too?
      if(iterDepth >= depth + reduction ||         // if sufficient depth for unreduced search...
         iterDepth >= depth && bestScore >= beta)  //     or for reduced search when it will fail high
          goto Out;                                //     return bestScore immediately
    } else iterDepth = 0;                          // without usable hit, start IID from scratch

    if((nodeCount++ & 0x3FF) == 0 && !forceMove) abortFlag |= TimeIsUp(3); // count nodes. Every 1024 nodes we check for timeout
    killers[ply+1][0] = killers[ply+1][1] = 0;                // clear next-level's killers

    startScore = eval = Evaluate() + mobilityGuess - 40;      // this is the (pessimistic) lazy eval

    if(depth == 0 && startScore >= beta) { bestScore = startScore; goto Out; } // stand-pat cutoff (on lazy eval)
  }

  captures = noncapts = lastMove = frame += FRAME;                   // set up move stack
  for(i=0; i<4*8; i+=4) moves[frame+i+200] = moves[frame+i+201] = 0; // setup board (in move stack) for marking protected pieces

  // generate moves, and add them to list that grows in 2 directions: captures go in front, non-captures are added to tail
  // (this would be so straightforward if it were not for all the 'if(type == PAWN)' special cases...)
  // as side effect the mobility and number of protectors of all stm pieces is calculated, and stored in a board-size table
  pieceMob = (previousPly ? dump : protMob - FRAME);  // after null move write per-piece mobility in parent's protection board
  nMoves = mobility = pieceMob[0x18] = 0;             // prepare to count mobility; KLUDGE: make sure dummy e.p. square has none
  seige = 0; xKing = location[KING+8-stm];            // seige will flag attacks on King neighborhood, each bit representing a piece
  for(from=0; from<128; from = from+9&~8) {           // scan 0x88 board
    int piece = board[from];
    if(piece && (piece & 8) == stm) {                                    // own piece
      int n, step, dir = firstDir[piece], type = piece & 7;              // reduce to piece type now we know it is ours
      int hits = 0, id = bitID[piece];                                   // for recording attacks on King neighborhood
      id += (seige & id);                                                // KLUDGE: second piece of same type gets next bit
      while((step = steps[dir++])) {                                     // loop over directions
        int to = from, victim;
        do {
          to += step;                                                    // scan along ray
          if(to & 0x88) break;                                           // strays off board
          hits += nearKing[to-xKing];                                    // count moves to King neighborhood
          if((victim = board[to])) {                                     // target square occupied
            if((victim & 8) == stm) {                                    // captures own piece
              if(type != PAWN || step & 7)                               // filter out straight Pawn move
                protMob[to]++;                                           // count as protected
              break;
            }
            if(checker >= 0 && type != KING && to != checker && to != xKing) break;     // non-King x non-Checker cannot be evasion: suppress
            if(type == PAWN) {                                           // Pawns might promote in addition to capture
              if(!(step & 7) ) break;                                    // straight Pawn capture
              if(spoiler[to] & 0x30) {                                   // moves to last rank
                moves[--captures] = to | from<<8 | QUEEN-PAWN+0x8000<<16;// add promotion to captures, with highest sort key
                moves[lastMove++] = to | from<<8 | KNIGHT-PAWN<<16;      // and under-promotions to non-captures
                moves[lastMove++] = to | from<<8 |   ROOK-PAWN<<16;      // (Knight first asmost likely alternative)
                moves[lastMove++] = to | from<<8 | BISHOP-PAWN<<16;
                goto SkipNonPromotion;
            } }
            moves[--captures] = to | from<<8 | key[victim]-type << 24;   // add capture in front of list, with MVV/LVA sort key
           SkipNonPromotion:
            if(to == epFlag) victim = KING;                              // KLUDGE: Rook victim after castling is upgraded to King
            bestScore = value[victim];                                   // figure out if we capture something worth an abort
            if(bestScore > threshold) { // captures royal or other unacceptably valuable piece
              if(bestScore < INF) {                                      // capture of non-royal (quirky novelty)
                int protected = protMob[to-FRAME];                       // holds approximate number of protectors of this victim
                bestScore = INF + 1;                                     // flags abort to caller
                if(to == (previousPly & 0xFF)) {                         // this is a recapture following high x low
                  bestScore = INF + value[piece];                        // informs caller of value recapturer
                } else {                                                 // counter-strike against a more valuable piece
                  if(protected && bestScore - value[piece] <= threshold) // it just initiates a trade that doesn't gain enough
                    bestScore = 0;                                       // so ignore it 
              } }                                                        // on capturing King bestScore stays INF
              if(bestScore >= INF) goto Cleanup;                           // force an abort of this node
            }
          } else {                                                       // target square empty
            if(type == PAWN) { int rank;                                 // Pawns need some extra attention for all their quirky moves
              if(to == epFlag) {                                         // move to empty e.p. square (can only be reached diagonally!)
                moves[--captures] = to | from<<8 | (0x1098^to)<<16;      // add e.p. capture, flags field indicates e.p. victim
                break;                                                   // and break off ray scan
              }
              if(step & 7) break;                                        // diagonal Pawn non-capture is illegal
              rank = (to ^ stm-8) & 0x70;                                // to-rank (flipped vertically if white; stm-8 then is 0xF8)
              if(rank == 0) {                                            // moves to last rank
                moves[--captures] = to | from<<8 | QUEEN-PAWN+0x6000<<16;// add promotion to captures (with pretty high sort key),
                moves[lastMove++] = to | from<<8 | KNIGHT-PAWN<<16;      //     and under-promotions to the non-captures (Knight first)
                moves[lastMove++] = to | from<<8 |   ROOK-PAWN<<16;
                moves[lastMove++] = to | from<<8 | BISHOP-PAWN<<16;
                break;                                                   //     and break off ray scan
              } else if(rank == 0x50 && !board[to+step]) {               // move came from initial rank and next square also empty
                moves[lastMove++] = to+step | from<<8 | 0x80+to<<16;     // add double push, flags field indicates skipped square
            } }
            if(checker >= 0 && type != KING &&                           // non-King moving cannot block check (and is suppressed)...
               (vector[king-to] != checkDir ||                           //    if it does not step on check ray
                dist[king-to] > dist[checker-king])) continue;           //    or if it is more distant than checker
            moves[lastMove++] = to | from<<8;                            // add non-capture to tail of list
          }
        } while(!victim && type > KNIGHT);                               // if slider and non-capture, do next square on ray
      }                                                                  // next direction
      n = nMoves; nMoves = lastMove - captures;                          // determine new length of move list
      mobility += pieceMob[from] = (nMoves - n) * mobFac[piece];         // update mobility, total and for this piece
      seige += hits*weight[piece]; seige |= -hits & id;                  // record which pieces attack in bits 8-15. KLUDGE: these bits
  } }                                                                    //    ...in -hits form a mask of all 1s if hits != 0
  moves[frame + 232] = mobility; moves[frame+233] = seige;               // KLUDGE: hide mobility in move stack, so caller can get it
  if(threshold == INF+10) goto Cleanup;                                  //         and abort if called for calculating mobility only
  if(checker < 0 && depth > 0)                                           // if not in check, generate castlings (don't bother in QS)
  for(i=0; i<2; i++) {                                                   // try for i = 0 (Q-side) and 1 (K-side)
    static char cTab[] = { 1, 4,    0,    7, -1, 1, 0, 0,                // table with flag-test bit, corner and direction (all 2x)
                           2, 8, 0x70, 0x77, -1, 1, 0, 0, };             // same for black
    if(!(cTab[stm+i] & rights)) {                                        // the castling right exists
      int corner = cTab[stm+i+2], dir = cTab[stm+i+4];                   // get applicable corner square and direction
      if(board[corner^1] || board[corner^2]) continue;                   // two squares next to corner not both empty
      if(board[king+dir]) continue;                                      // square next to King not empty
      moves[lastMove++] = king+2*dir | king<<8 | king+dir+0x80<<16;      // add castling, flagged by 0x80 + square skipped by King
  } }
  unsorted = captures;     // start of unsorted part of the list (currently whole list)

  stm ^= 8;   // search null move if not QS, not in-check and no double null move, and if we have at least one slider
  if(depth > 0 && previousPly && checker < 0 && (counts[stm+PHASE] > 1 || counts[stm+BISHOP] == 1)) {
    int nullDepth = (depth > 3 ? depth-3 : 0);                                    // reduce 2 ply
    ply++; pstEval = -pstEval;
    bestScore = -Search(-beta, 1-beta, INF-1, -mobilityGuess, 0, 0, nullDepth, 0);
    ply--; pstEval = savedPstEval;
    if(bestScore >= beta) { stm ^= 8; bestScore = beta; goto Cleanup; }           // null-move pruning
  } else { // calculate exact mobility, also in non-QS as QS daughters might need them for a good mobility guess
    if(mobEval) Search(INF-1, INF, INF+10, 0, 0, 0, 256, 0); // KLUDGE: threshold INF+10 requests calculating mobility only
  }
  stm ^= 8;

  mobility = (mobEval ? mobility - moves[frame + FRAME + 232] : 0); // null-move search (or dummy) has stored opponent mobility here
  if(kingSafety) {
    mobility += counts[stm+PHASE]  *  (seige & 255) * kingSeige[seige>>8] >> 3; // attack threat to opponent King
    seige = moves[frame + FRAME + 233];     // and they left King-safety info too
    mobility -= counts[PHASE+8-stm] * (seige & 255) * kingSeige[seige>>8] >> 3; // threat to our King. Both included in mobility
  }

  if(depth == 0) { int ep;
    startScore += mobility - mobilityGuess + 40;                     // replace guessed 'mobility' by real one, to get precise eval
    if(startScore >= beta) { bestScore = startScore; goto Cleanup; } // try stand-pat again, based on it
    if(reduction) depth++, reduction--, startScore = -INF; else {    // self-deepen immediately, as d=1 starts with captures anyway
      if(hashMove > 0) { // in pure QS we must check if hash move is acceptable (i.e. capture, e.p. capture or promotion)
        ep = hashMove >> 16 & 0xFF;                                  // if 0 (i.e. not special) or castling / double push no extras
        if(board[hashMove & 0xFF] == 0 && (ep == 0 || (ep & 0x88) == 0x80)) hashMove = 0; // not suitable for QS
      }
      if(startScore > startAlpha) startAlpha = startScore;           // eval increases alpha like any better move
      lastMove = noncapts;          // throw away all generated noncaptures too
    }
  } else startScore = -INF;
  if(hashMove > 0) moves[--captures] = hashMove | 0xFF << 24; // place hash move in front of move list (in already-sorted section!)

  // remember all hashed info, to put it back when the node gets aborted by a timeout (hashMove we already have!)
  hashDepth = bucket->depth[slot]; hashScore = bucket->score[slot]; hashFlags = bucket->flags[slot];

  bucket->signature[slot] = sig;         // already reserve the hash slot
  bucket->depth[slot] = 127;             // KLUDGE:  and initialize it as 'absolute' draw
  bucket->score[slot] = 0;               //     (i.e. maximal depth, and exact score)
  bucket->flags[slot] = LOWER + UPPER;   //     to force a rep-draw when we re-encounter it

  // search the generated moves, iteratively deepening from hashed depth
  do { // IID Loop; done at least once, as hash cutoffs were already taken before
    int current, bestNr = captures;     // assume first move is best (in case all fail low)

    iterDepth++; // Even without hash hit start at 1 (in QS the move list only contains captures)

    alpha = startAlpha; bestScore = startScore; bestMove = 0; // initialize for the iteration
    extraDepth = (depth > iterDepth ? depth - iterDepth : 0); // self-deepening: make fail-highs use depth rather than iterDepth

    for(current = captures; current < lastMove; current++) { // loop through all moves (for searching them)
      int from, to, epSqr, rookSqr;
      int piece, promoted, victim, epVictim;
      int score;
      int lmr, deltaPhase = 0, ownPhaseCorr = 0;
      unsigned int move = moves[current]; // next move from list

      if(!move) continue;   // skip zapped move

      // move-sorting section
      if(current >= unsorted) {                              // in unsorted part we further the sorting...
        int i, nr = current;                                 //    by extracting move with highest priority from it
        if(current >= noncapts) {                            // we first entered the non-captures
          for(i = current; i < lastMove; i++) {              // loop through remaining moves to find killers
            if(moves[i] == killer1 || moves[i] == killer2) {
              moves[nr] = moves[i]; moves[i] = move;         // swap killers to front of non-captures
              if(nr++ > current) {                           // when second killer found
                break;                                       // and stop looking for more killers
              }
              move = moves[nr];                              // next move to swap for killer
          } }
          move = moves[current];                             // should be first killer now (if any killers found)
          unsorted = lastMove + 1;                           // suppress any further sorting;
        } else {                                             // we are doing captures
          for(i = current + 1; i < noncapts; i++) {          // run through captures
            if(move < moves[i]) {
              nr = i; move = moves[i];                       // to remember (sort-key-wise) best remaining capture
          } }
          moves[nr] = moves[current]; moves[current] = move; // swap it to current
          unsorted = current + 1;                            // the current move now is sorted, what comes after it not
        }
      }

      // at this point we have the next move we want to search
      to = move & 0xFF; from = move >> 8 & 0xFF;              // first decode it
      epSqr = move >> 16 & 0xFF; rookSqr = 0x80;              // default 'add-rook square' is dummy (i.e. when not castling)
      promoted = piece = board[from], victim = board[to];     // by default 'promoted' is set to same as 'piece'
      if(epSqr < 0x80) {                                      // move is promotion, flags holds upgrade.
        promoted += epSqr, epSqr = 0x18;                      // KLUDGE: set epSqr to (empty) dummy
        ownPhaseCorr = weight[promoted] - weight[piece];      // new piece affects game phase
      } else
      if(!(epSqr & 8) && from == king) { int rook;            // move is castling; needs some extra effort to perform
        rookSqr = epSqr & ~0x88;                              // square skipped by King contained in move's flag field
        epSqr = (to > from ? from+3 : from-4);                // KLUDGE: set epSqr to board corner, so Rook will be removed
        board[rookSqr] = rook = board[epSqr];                 // grow Rook on square skipped by King
        hashKey ^= zob[rook][rookSqr];                        // correct hash key for that
        pstEval += pst[rook][rookSqr] - 2*pst[rook][epSqr];   // and eval, pre-compensating for wrong sign of e.p. removal
        deltaPhase = -weight[rook]; counts[rook]++;           // Rook will not really disappear from board
      } else epSqr &= 0x77;                                   // move is e.p. capture; make epSqr valid by stripping flag bit
      epVictim = board[epSqr];                                // remember e.p. victim (empty on double push, Rook on castling)

      if(iterDepth <= 1 && eval + value[victim] + value[epVictim] < alpha - 200,0) { // futile move (not enough material gain to approach alpha)
        if(move > 0x1000000) continue;                                             // we might still have postponed moves that are not futile
        if(bestScore == -INF && checker < 0 && startAlpha <= 0) continue;          // no legal move yet, not in check, stalemate would beat alpha
        break;                                                                     // remaining moves must be even more futile; exit move loop
      }

      board[to] = promoted; board[from] = 0; board[epSqr] = 0;    // make move, update eval and hash key
      location[piece] = to;                                       // primarily for remembering King positions
      rights |= spoiler[from] | spoiler[to];                      // update castling rights from spoiler board
      counts[victim]--; counts[epVictim]--; counts[piece]--; counts[promoted]++; // update piece counts
      counts[PHASE+8-stm] -= deltaPhase += weight[victim] + weight[epVictim];    // opponent's game phase suffers from captures
      counts[stm + PHASE] += ownPhaseCorr;                                       // but our own will benefit from promotion
      hashKey ^= zob[promoted][to] ^ zob[piece][from] ^ zob[victim][to] ^ zob[epVictim][epSqr];
      pawnKey ^= zpk[promoted][to] ^ zpk[piece][from] ^ zpk[victim][to] ^ zpk[epVictim][epSqr];
      pstEval += pst[promoted][to] - pst[piece][from] + pst[victim][to] + pst[epVictim][epSqr];
      ply++; stm ^= 8; pstEval = -pstEval;

      threshold = INF-1; // (unsorted > lastMove ? INF-1 : value[victim] + value[epVictim]); TODO: enable this
      lmr = (iterDepth > 4 && counts[WHITE+PHASE] + counts[BLACK+PHASE] && current >= noncapts); // LMR on all non-capts
      mobilityGuess = mobility + protMob[to] + protMob[epSqr]; // correct for captured pieces, but not for moved/(un)blocked

      score = -Search(-beta, -alpha, threshold, -mobilityGuess, move, lmr, iterDepth-1, extraDepth);
      
      if(--ply == 0) { // back in root
        if(score != -INF && !(move - forceMove & 0xFFFF)) { // KLUDGE: 'exit from the first floor' at the requested (legal) move
          if(choice == 0 || choice == (move >> 16 & 255)) { //    leaving it done (and supplying its flags)
            lastGameMove = move; bestScore = INF+1;         //    For promotions also the flags should match 'choice'
            goto Cleanup;
      } } }

      stm ^= 8; rights = savedRights;
      pstEval = savedPstEval; hashKey = savedHashKey; pawnKey = savedPawnKey;     // restore incremental stuff
      counts[stm + PHASE] -= ownPhaseCorr; counts[PHASE+8-stm] += deltaPhase;     // undo game-phase changes
      board[from] = piece; board[to] = victim; board[epSqr] = epVictim;           // take back move, including e.p. victim
      board[rookSqr] = 0; counts[stm+ROOK] -= (rookSqr != 0x80);                  // also remove Rook put there by castling
      location[piece] = from;
      counts[victim]++; counts[epVictim]++; counts[piece]++; counts[promoted]--;  // restore piece counts

      if(abortFlag) goto Abort; // unwind search

      if(score <= -INF) { // move not legal or premature
        if(score == -INF) {
          moves[current] = 0;          // zap illegal move
          continue;                    // and go on with next
        } // TODO; cannot happen yet
        moves[current] &= 0xFFFFFF;           // lower sort key to postpone the aborted H x protected L capture...
        moves[current] |= (victim & 7) << 24; //   to value < 8, lower than worst MVV/LVA key
        unsorted = current--;                 // redo current move after scheduling for resorting
        continue;                             // KLUDGE: the loop logic will undo the current--, to stay at this move
      }

      if(score > bestScore) {                               // minimax the score of this move
        bestScore = score;                                  // remember the best score
        if(score > alpha) {                                 // improvement
          int *p;
          alpha = score; bestMove = move; bestNr = current; // then also remember which move it was
          if(score >= beta) {                               // cutoff
            if(move < 1<<24 && move != killers[ply][1]) {   // non-capture that is not latest killer
              killers[ply][0] = killers[ply][1];            // update killers
              killers[ply][1] = move & 0xFFFFFF;            // strip off sort key
            }
            iterDepth = depth;                              // self-deepening caused fail high to be full depth
            goto CutOff;                                    // exit move loop
          }                                                 //    as fail-highs are already 'self-deepened'
          p = startPV; *p++ = move;                         // score is in-window; move starts new PV
          while((*p++ = *pvSP++)); pvSP = p;                // append daughter PV to it to construct ours
          if(!ply) {                                        // root
            rootMove = move;                                // remember best move in a global variable so caller can know it
            if(postThinking /*	&& !forceMove*/) {                // print thinking output, if requisted (but not in MakeMove)
              printf("%d %d %d %d", iterDepth, score, ReadClock(0)/10, nodeCount);
              for(p=startPV; *p; p++) printf(" %s", MoveToText(*p));
              printf("\n"); fflush(stdout);
            }
            if(score > hashScore - 30 &&                    // we found a satisfactory move
               TimeIsUp(2) && iterDepth > 2) break;         // in 'over-time' just play it if 'oldScore' was from this search
      } } }

    } // we exit move loop here, after searching all moves without cutoff (meaning the parent node scores > alpha)

    if(iterDepth == depth && reduction && (bestScore <= startAlpha || reduction > extraDepth)) // if move in parent would fail high...
      depth++, reduction--; // we add iteration(s) to satisfy its IID, if it is PV we only undo the LMR part of reduction

    if(bestScore == -INF) {
     if(checker == -1) bestScore = 0; // stalemate correction
     iterDepth = depth = 1;           // depth to use in hash entry, making it easy to replace (and kludge to terminate deepening)
     checker = -3;                    // KLUDGE: used to flag (stale)mates in hash table
    }

    // sort best move to front
    if(bestNr > captures+1) moves[--captures] = bestMove; else     // either put duplicate at head of list
    if(bestNr > captures) {                                        // note that we do nothing if bestNr == captures
      moves[bestNr] = moves[captures]; moves[captures] = bestMove; // or swap to front if not already there
    }

 CutOff:
    // pseudo hash store (the real hash slot is still busy indicating a rep-draw), saves result of the now completed iteration
    hashMove  = bestMove; hashScore = bestScore; hashDepth = (depth ? iterDepth : 0); // in QS iterDepth (1) > depth (0)!
    hashFlags = (bestScore > startAlpha)*LOWER + (bestScore < beta)*UPPER | (checker == -3)*(UPPER+LOWER);  // mates always are exact

//    if(bestScore > INF - 100 && iterDepth >= 2*(INF - bestScore)) break;      // stop deepening if sure mate

  } while(iterDepth < depth ||                                         // start next depth iteration if not yet there
          !ply && !forceMove && iterDepth < maxDepth && !TimeIsUp(1)); // in root (for thinking) continue until time is up

 Abort:
  // commit back-logged search result in the already-reserved hash slot (can and must also be done on abort, to clear the rep-draw there)
  if(hashMove != -1) { // we finished at least one iteration, or still have the original hashed info for the node
    bucket->move[slot]  = hashMove & 0xFFFFFF | checker << 24; // pack checker & best move
    bucket->depth[slot] = hashDepth; bucket->score[slot] = hashScore; bucket->flags[slot] = hashFlags;
  } else bucket->signature[slot] = 0, bucket->depth[slot] = 0; // invalidate if we have nothing to store (and encourage replacement)


 Cleanup:
  frame -= FRAME;                              // cleanup move stack
  pvSP = startPV;                            // and PV stack (leaving pvSP at our own PV (if any) for caller to fetch)
 Out:
  return bestScore + (bestScore < refScore); // delayed-loss bonus
}

/********************************************************/
/* Example of a WinBoard-protocol driver, by H.G.Muller */
/********************************************************/

#define ANALYZE  3

// some value that cannot occur as a valid move
#define INVALID 0

// some parameter of your engine
#define MAXMOVES 500  /* maximum game length  */
#define MAXPLY   28   /* maximum search depth */

#define OFF 0
#define ON  1

#define DEFAULT_FEN "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1"

int moveNr;              // part of game state; incremented by MakeMove
int gameMove[MAXMOVES];  // holds the game history

// Some routines your engine should have to do the various essential things
void UnMake(int move);                  // unmakes the move;
void PonderUntilInput(int stm);         // Search current position for stm, deepening forever until there is input.

int
TimeIsUp (int mode)
{ // determine if we should stop, depending on time already used, TC mode, time left on clock and from where it is called ('mode')
  int t = ReadClock(0), targetTime, panicTime;
  if(timePerMove >= 0) {                               // fixed time per move
    targetTime = panicTime = 10*timeLeft - 30;
  } else if(mps) {                                     // classical TC
    int movesLeft = -(moveNr >> 1);
    while(movesLeft <= 0) movesLeft += mps;
    targetTime = 10*(timeLeft - 30) / (movesLeft + 2);
    panicTime  = 50*(timeLeft - 30) / (movesLeft + 4);
  } else {                                             // incremental TC
    targetTime = 10*timeLeft / 40 + inc;
    panicTime = 5*targetTime;
  }
  switch(mode) {
    case 1: return (t > 0.6*targetTime); // for starting new root iteration
    case 2: return (t > targetTime);     // for searching new move in root
    case 3: return (t > panicTime);      // during search
  }
  return 0; // unreachable; added to silence warning
}

int
Setup (char *fen)
{ // very flaky FEN parser
  static char pieces[] = "PKNBRQ  pknbrq", castle[] = "QqKk-", startFEN[200];
  int stm = 0, i, sqr = 112; // h1
  if(!fen) fen = startFEN; else strcpy(startFEN, fen); // remember start position, or use remembered one if not given
  for(i=0; i<128; i=i+9&~8) board[i] = 0; // clear 0x88 board
  for(i=0; i<16; i++) counts[i] = 0;      // clear piece counts
  rights = 15; pstEval = 0;               // no castling rights, balance score
  hashKey = pawnKey = 0;                  // clear hash keys
  while(*fen) {                                       // parse board-field of FEN
    if(*fen == ' ') break;
    if(*fen == '/') sqr = (sqr & ~15) - 16; else      // skip to (start of) next rank
    if(*fen <= '9') sqr += *fen - '0'; else {         // skip given number of squares
      i = 0; while(pieces[i] && pieces[i++] != *fen); // identify piece
      board[sqr] = i; location[i] = sqr; counts[i]++; // place piece
      counts[(i&8)+PHASE] += weight[i];               // and update game phase accordingly
      hashKey ^= zob[i][sqr]; pawnKey ^= zpk[i][sqr]; // update hash keys
      pstEval += (4 - (i&8) >> 2)*pst[i][sqr];        // update PST eval (white POV)
      sqr++;
    }
    fen++;
  }
  if(*++fen == 'b') stm = 8, pstEval *= -1; // black to move; flip eval
  while(*++fen == ' ');
  while(*fen) {
    i=0; while(castle[i] && castle[i] != *fen) i++;
    if(i > 3) break;
    rights &= ~(1<<i);
    fen++;
  }

  lastGameMove = 0;  // TODO: use FEN e.p. rights to fake double-push here
  return stm;
}

int
MakeMove (int move)
{
  int res;
  forceMove = move;                  // KLUDGE: requests verifying and making of move
  abortFlag = 0; ReadClock(1);       // make sure we won't time out during the 'make search'
  res = (Search(-INF, INF, INF, 0, lastGameMove, 0, 1, 0) > INF); // score <= INF and nothing done if illegal
  gameMove[moveNr] = lastGameMove;   // lastGameMove was set by Search() to forceMove, with flags field completed
  moveNr += res;                     // only add to game history if legal
  return res;
}

char *
MoveToText (int move)
{
  static char buf[20], pieceID[] = "pknbrq?!";
  char promo = move >> 16 & 0xFF; // flags field
  if(promo && !(promo & 0x80)) promo = pieceID[promo&7]; else promo = '\0';       // write null-char if not promotion
  sprintf(buf, "%c%d%c%d%c", 'a'+(move>>8&7), 1+(move>>12&7), 'a'+(move&7), 1+(move>>4&7), promo);
  return buf;
}

int
ParseMove(char *s)
{
  static char promo[] = { 0, 'k', 'n', 'b', 'r', 'q' };
  int m = (s[0] - 'a') << 8 | (s[1] - '1') << 12 | (s[2] - 'a') | (s[3] - '1') << 4; // pack from & to-square into lowest 2 bytes
  for(choice=4; choice>0; choice--) if(promo[choice] == s[4]) break; // flag promotion y setting 'choice'
  return m;
}

// Some global variables that control your engine's behavior
int ponder;
int randomize;
int resign;         // engine-defined option
int contemptFactor; // likewise

#ifdef WIN32 
#    include <windows.h>
#    define CPUtime 1000.*clock
#else
#    include <sys/time.h>
#    include <sys/times.h>
#    include <unistd.h>
     int GetTickCount() // with thanks to Tord
     {	struct timeval t;
	gettimeofday(&t, NULL);
	return t.tv_sec*1000 + t.tv_usec/1000;
     }
#endif

int
ReadClock (int start)
{
  static int startTime;
  int t = GetTickCount();
  if(start) startTime = t;
  return t - startTime; // msec
}

int
SetMemorySize (int n)
{
  static int oldSize;
  if(n == oldSize) return 0;       // nothing to do
  oldSize = n;                     // remember current size
  if(hashTable) free(hashTable);   // throw away old table
  for(hashMask = (1<<24)-1; hashMask*sizeof(HashBucket) > n*1024*1024; hashMask >>= 1);    // round down nr of buckets to power of 2
  hashTable = (HashBucket*) calloc(hashMask+1, sizeof(HashBucket));
  return !hashTable;               // return TRUE if alocation failed
}

void
TakeBack (int n)
{ // reset the game and then replay it to the desired point
  int last;
  stm = Setup(NULL); // uses FEN saved during previous Setup
  last = moveNr - n; if(last < 0) last = 0;
  for(moveNr=0; moveNr<last; moveNr++) MakeMove(gameMove[moveNr]);
}

void PrintResult(int stm, int score)
{
  if(score == 0) printf("1/2-1/2\n");
  if(score > 0 && stm == WHITE || score < 0 && stm == BLACK) printf("1-0\n");
  else printf("0-1\n");
}

int engineSide=NONE;         // side played by engine
int ponderMove;
char inBuf[80];

void
ReadLine ()
{
  int i, c;
  if(inBuf[0]) return; // buffer already holds a backlogged command;
  for(i = 0; (c = getchar()) != EOF && (inBuf[i++] = c) != '\n'; );
  inBuf[i] = 0;
}

int
DoCommand (int searching)
{
  char command[80];

  while(1) { // usually we break out of this loop after treating one command

    ReadLine();                   // read one line into inBuf (or retrieve backlogged)
    if(!*inBuf) exit(0);          // EOF, terminate
    sscanf(inBuf, "%s", command); // extract the first word
    *inBuf = 0;                   // and already mark the buffer as empty

    // recognize and execute 'easy' commands, that can be done during search
    if(!strcmp(command, "quit"))    { exit(0); }  // exit immediately
    if(!strcmp(command, "otim"))    { continue; } // move will follow immediately, wait for it
    if(!strcmp(command, "time"))    { sscanf(inBuf+4, "%d", &timeLeft); continue; }
    if(!strcmp(command, "easy"))    { ponder = OFF; return 0; }
    if(!strcmp(command, "hard"))    { ponder = ON;  return 0; }
    if(!strcmp(command, "post"))    { postThinking = ON; return 0; }
    if(!strcmp(command, "nopost"))  { postThinking = OFF;return 0; }
    if(!strcmp(command, "random"))  { randomize = ON;    return 0; }
    if(!strcmp(command, "."))       { return 0; } // periodic update request; ignore for now
    if(!strcmp(command, "option")) { // setting of engine-define option; find out which
      if(sscanf(inBuf+7, "Evaluate piece mobility=%d",   &mobEval)     == 1) return 0;
      if(sscanf(inBuf+7, "Evaluate trapped pieces=%d",   &patterns)    == 1) return 0;
      if(sscanf(inBuf+7, "Evaluate drawish material=%d", &drawishness) == 1) return 0;
      if(sscanf(inBuf+7, "Evaluate specific endings=%d", &recognizers) == 1) return 0;
      if(sscanf(inBuf+7, "Evaluate pawn structure=%d",   &pawnStruct)  == 1) return 0;
      if(sscanf(inBuf+7, "Evaluate king shelter=%d",     &shelters)    == 1) return 0;
      if(sscanf(inBuf+7, "Evaluate king seige=%d",       &kingSafety)  == 1) return 0;
      if(sscanf(inBuf+7, "Resign=%d",   &resign)         == 1) return 0;
      if(sscanf(inBuf+7, "Contempt=%d", &contemptFactor) == 1) return 0;
      return 1;
    }

    if(searching) {
      if(!strcmp(command, "usermove")) { return 1; } // TODO during search we just test for ponder hit
      *inBuf = *command;                             // backlog command (by repairing inBuf)
      return 1;                                      // and request search abort
    }

    // the following commands can (or need) only be done when not searching
    if(!strcmp(command, "force"))   { engineSide = NONE;    return 1; }
    if(!strcmp(command, "analyze")) { engineSide = ANALYZE; return 1; }
    if(!strcmp(command, "exit"))    { engineSide = NONE;    return 1; }
    if(!strcmp(command, "level"))   {
      int min, sec=0;
      sscanf(inBuf, "level %d %d %d", &mps, &min, &inc) == 3 ||  // if this does not work, it must be min:sec format
      sscanf(inBuf, "level %d %d:%d %d", &mps, &min, &sec, &inc);
      timeControl = 60*min + sec; timePerMove = -1;
      return 1;
    }
    if(!strcmp(command, "protover")){
      printf("feature ping=1 setboard=1 colors=0 usermove=1 memory=1 debug=1 reuse=0 sigint=0 sigterm=0 myname=\"Simple " VERSION "\"\n");
      printf("feature option=\"Resign -check 0\"\n");           // example of an engine-defined option
      printf("feature option=\"Contempt -spin 0 -200 200\"\n"); // and another one
      printf("feature option=\"Evaluate piece mobility -check %d\"\n", mobEval); // options to enable eval features
      printf("feature option=\"Evaluate trapped pieces -check %d\"\n", patterns);
      printf("feature option=\"Evaluate drawish material -check %d\"\n", drawishness);
      printf("feature option=\"Evaluate specific endings -check %d\"\n", recognizers); 
      printf("feature option=\"Evaluate pawn structure -check %d\"\n", pawnStruct);
      printf("feature option=\"Evaluate king shelter -check %d\"\n", shelters);
      printf("feature option=\"Evaluate king seige -check %d\"\n", kingSafety);
      printf("feature done=1\n");
      return 1;
    }
    if(!strcmp(command, "sd"))      { sscanf(inBuf+2, "%d", &maxDepth);    return 1; }
    if(!strcmp(command, "st"))      { sscanf(inBuf+2, "%d", &timePerMove); return 1; }
    if(!strcmp(command, "memory"))  { if(SetMemorySize(atoi(inBuf+7))) printf("tellusererror Not enough memory\n"), exit(-1); return 1; }
    if(!strcmp(command, "ping"))    { printf("pong%s", inBuf+4); return 1; }
//  if(!strcmp(command, ""))        { sscanf(inBuf, " %d", &); return 1; }
    if(!strcmp(command, "new"))     { engineSide = BLACK; stm = Setup(DEFAULT_FEN); maxDepth = MAXPLY; randomize = OFF; return 1; }
    if(!strcmp(command, "setboard")){ engineSide = NONE;  stm = Setup(inBuf+9); return 1; }
    if(!strcmp(command, "undo"))    { TakeBack(1); return 1; }
    if(!strcmp(command, "remove"))  { TakeBack(2); return 1; }
    if(!strcmp(command, "go"))      { engineSide = stm;  return 1; }
    if(!strcmp(command, "hint"))    { if(ponderMove != INVALID) printf("Hint: %s\n", MoveToText(ponderMove)); return 1; }
    if(!strcmp(command, "book"))    {  return 1; }
    // completely ignored commands:
    if(!strcmp(command, "xboard"))  { return 1; }
    if(!strcmp(command, "computer")){ return 1; }
    if(!strcmp(command, "name"))    { return 1; }
    if(!strcmp(command, "ics"))     { return 1; }
    if(!strcmp(command, "accepted")){ return 1; }
    if(!strcmp(command, "rejected")){ return 1; }
    if(!strcmp(command, "variant")) { return 1; }
    if(!strcmp(command, "?"))       { return 1; } // 'move now'
    if(!strcmp(command, ""))  {  return 1; }
    if(!strcmp(command, "usermove")){
      int move = ParseMove(inBuf+9);
      if(move == INVALID) printf("Illegal move\n");
      else if(!MakeMove(move)) printf("Illegal move\n");
      else {
        ponderMove = INVALID;
      }
      return 1;
    }
    printf("Error: unknown command\n");
  }
  return 0;
}

int
main ()
{
  int score;

  Init(); SetMemorySize(1);  // reserve minimal hash to prevent crash if GUI sends no 'memory' command

  while(1) { // infinite loop

    fflush(stdout);                 // make sure everything is printed before we do something that might take time

    if(stm == engineSide) {         // if it is the engine's turn to move, set it thinking, and let it move

      nodeCount = forceMove = rootMove = abortFlag = 0; ReadClock(1);
      score = Search(-INF, INF, INF, 0, lastGameMove, 0, 1, 0);

      if(!rootMove) {                  // no move, game apparently ended
        engineSide = NONE;             // so stop playing
        PrintResult(stm, score);
      } else {
        MakeMove(rootMove);            // perform chosen move (stores it in lastGameMove and changes stm)
        printf("move %s\n", MoveToText(lastGameMove)); // and output it
      }
    }

    fflush(stdout); // make sure everything is printed before we do something that might take time
#if 0
    // now it is not our turn (anymore)
    if(engineSide == ANALYZE) {       // in analysis, we always ponder the position
        PonderUntilInput(stm);
    } else
    if(engineSide != NONE && ponder == ON && moveNr != 0) { // ponder while waiting for input
      if(ponderMove == INVALID) {     // if we have no move to ponder on, ponder the position
        PonderUntilInput(stm);
      } else {
        int newStm = MakeMove(stm, ponderMove);
        PonderUntilInput(newStm);
        UnMake(ponderMove);
      }
    }
#endif
    DoCommand(0);
  }
  return 0;
}
