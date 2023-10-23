using System;
using System.Collections.Generic;

namespace ChessV
{
  public interface IPieceType
  {
    double AverageDirectionsAttacked { get; set; }
    double AverageMobility { get; set; }
    double AverageSafeChecks { get; set; }
    Board Board { get; }
    CustomMoveGenerationHandler CustomMoveGenerator { get; set; }
    bool Enabled { get; set; }
    int EndgameValue { get; set; }
    string FallbackImage { get; set; }
    Game Game { get; }
    bool HasMovesWithConditionalLocation { get; }
    bool HasMovesWithPaths { get; }
    List<string> ImagePreferenceList { get; }
    string InternalName { get; }
    bool IsPawn { get; }
    bool IsSliced { get; }
    int MidgameValue { get; set; }
    string Name { get; set; }
    string[] Notation { get; }
    string[] NotationClean { get; }
    int NumMoveCapabilities { get; }
    int NumSlices { get; }
    string PreferredImage { get; set; }
    int PSTEndgameForwardness { get; set; }
    int PSTEndgameGlobalOffset { get; set; }
    int PSTEndgameInLargeCenter { get; set; }
    int PSTEndgameInSmallCenter { get; set; }
    int PSTEndgameLargeCenterAttacks { get; set; }
    int PSTEndgameSmallCenterAttacks { get; set; }
    int PSTMidgameForwardness { get; set; }
    int PSTMidgameGlobalOffset { get; set; }
    int PSTMidgameInLargeCenter { get; set; }
    int PSTMidgameInSmallCenter { get; set; }
    int PSTMidgameLargeCenterAttacks { get; set; }
    int PSTMidgameSmallCenterAttacks { get; set; }
    bool SimpleMoveGeneration { get; }
    int[] SliceLookup { get; }

    MoveCapability AddMoveCapability(MoveCapability moveCapability);
    void AddMovesOf(PieceType other);
    void AddMovesOf(Type otherType);
    double CalculateMobilityStatistics(Game game, double density);
    MoveCapability CannonMove(Direction direction);
    MoveCapability FindMove(Direction dir);
    void GetEmptyBoardMobility(Game game, int player, int square, bool[] boardSquares);
    int[] GetEndgamePST();
    int GetEndgamePST(int square);
    ulong GetHashKey(int player, int square);
    ulong GetMaterialHashKey(int player, int nSlice, int nPieces);
    int[] GetMidgamePST();
    int GetMidgamePST(int square);
    int GetMoveCapabilities(out MoveCapability[] moves);
    ulong GetPawnHashKey(int player, int square);
    void Initialize(Game game);
    void InitializePST(int variation);
    void RemoveMoveCapability(Direction direction);
    void ResetMoveCapabilities();
    MoveCapability RifleCapture(Direction direction, int maxSpaces);
    void SetNotation(string notation);
    MoveCapability Slide(Direction direction);
    MoveCapability Slide(Direction direction, int maxSteps);
    MoveCapability SlideCaptureOnly(Direction direction);
    MoveCapability SlideMoveOnly(Direction direction);
    MoveCapability Step(Direction direction);
    MoveCapability StepCaptureOnly(Direction direction);
    MoveCapability StepMoveOnly(Direction direction);
  }
}