﻿using System.Collections.Generic;

namespace Sudoku.Windows
{
	partial class Resources
	{
		/// <summary>
		/// The language source for the globalization string "<c>en-us</c>".
		/// </summary>
		/// <remarks>
		/// This field is not <see langword="readonly"/> because it can be initialized by the
		/// module initializer.
		/// </remarks>
		internal static IDictionary<string, string> LangSourceEnUs = new Dictionary<string, string>
		{
#line 1000
			// Punctuation marks
			["Ellipsis"] = "......",
			["Colon"] = ":",

#line 2000
			// GridProgressResult
			["UnsolvedCells"] = "Unsolved cells: ",
			["UnsolvedCandidates"] = ", candidates: ",

#line 3000
			// Solver
			["Manual"] = "Manual",
			["ManualLight"] = "Manual (Light)",
			["Backtracking"] = "Backtracking",
			["Bitwise"] = "Bitwise",
			["OneLineLinq"] = "One line LINQ",

#line 4000
			// StepFinder
			["ProgressAlsWWing"] = "Almost Locked Sets W-Wing",
			["ProgressAlsXyWing"] = "Almost Locked Sets XY-Wing",
			["ProgressSinglyLinkedAlsXz"] = "Almost Locked Sets XZ Rule",
			["ProgressDeathBlossom"] = "Death Blossom",
			["ProgressNishioFc"] = "Nishio Forcing Chains",
			["ProgressRegionFc"] = "Region/Cell Forcing Chains",
			["ProgressDynamicFc"] = "Dynamic Forcing Chains",
			["ProgressErip"] = "Empty Rectangle Intersection Pair",
			["ProgressMsls"] = "Multi-sector Locked Sets",
			["ProgressSdc3d"] = "3-Dimension Sue de Coq",
			["ProgressSdc"] = "Sue de Coq",
			["ProgressSkLoop"] = "SK-Loop",
			["ProgressAic"] = "(Grouped) Alternating Inference Chain",
			["ProgressJe"] = "Junior Exocet",
			["ProgressSe"] = "Senior Exocet",
			["ProgressFrankenSwordfish"] = "Hobiwan's Fish",
			["ProgressXWing"] = "(Finned, Sashimi) Fish",
			["ProgressAlmostLockedPair"] = "Almost Locked Candidates",
			["ProgressPointing"] = "Locked Candidates",
			["ProgressBowmanBingo"] = "Bowman's Bingo",
			["ProgressBruteForce"] = "Brute Force",
			["ProgressPom"] = "Pattern Overlay",
			["ProgressTemplateSet"] = "Template",
			["ProgressEmptyRectangle"] = "Empty Rectangle",
			["ProgressGuardian"] = "Guardian",
			["ProgressTurbotFish"] = "Two Strong Links",
			["ProgressNakedSingle"] = "Singles",
			["ProgressNakedPair"] = "Subsets",
			["ProgressGsp"] = "Gurth's Symmetrical Placement",
			["ProgressBugType1"] = "Bivalue Universal Grave",
			["ProgressXrType1"] = "Extended Rectangle",
			["ProgressUlType1"] = "Unique Loop",
			["ProgressBdpType1"] = "Borescoper's Deadly Pattern",
			["ProgressQdpType1"] = "Qiu's Deadly Pattern",
			["ProgressUrType1"] = "Unique / Avoidable Rectangle",
			["ProgressUsType1"] = "Unique Square",
			["ProgressWWing"] = "Irregular Wing",
			["ProgressXyWing"] = "Regular Wing",
			["ProgressBugMultipleFc"] = "Bivalue Universal Grave with Forcing Chains",
			["Summary"] = "Summary...",
			["GeneratingProgressSingular"] = "1 time tried",
			["GeneratingProgressPlural"] = "times tried",

#line 5000
			// Separate words
			["Petal"] = "Petals",
			["Grouped"] = "Grouped ",
			["Bug"] = "Bivalue Universal Grave",
			["Rectangle"] = "Rectangle ",
			["Avoidable"] = "Avoidable ",
			["Unique"] = "Unique",
			["Hidden"] = "Hidden ",
			["Category"] = "Category: ",
			["AnalysisResultPuzzle"] = "Puzzle: ",
			["AnalysisResultSolvingTool"] = "Solving tool: ",
			["AnalysisResultSolvingSteps"] = "Solving steps:",
			["AnalysisResultBottleneckStep"] = "Bottleneck step:",
			["AnalysisResultInStep"] = " In step ",
			["AnalysisResultTechniqueUsed"] = "Technique used:",
			["AnalysisResultMin"] = "min",
			["AnalysisResultTotal"] = "total",
			["AnalysisResultTechniqueUsing"] = "  technique using",
			["AnalysisResultStepSingular"] = "step",
			["AnalysisResultStepPlural"] = "steps",
			["AnalysisResultPuzzleRating"] = "Puzzle rating: ",
			["AnalysisResultPuzzleSolution"] = "Puzzle solution: ",
			["AnalysisResultPuzzleHas"] = "Puzzle has ",
			["AnalysisResultNot"] = "not ",
			["AnalysisResultBeenSolved"] = "been solved.",
			["AnalysisResultTimeElapsed"] = "Time elapsed: ",
			["AnalysisResultAttributes"] = "Attributes:",
			["AnalysisResultBackdoors"] = "Backdoors:",

#line 6000
			// Techniques
			["FullHouse"] = "Full House",
			["LastDigit"] = "Last Digit",
			["HiddenSingleRow"] = "Hidden Single in Row",
			["HiddenSingleColumn"] = "Hidden Single in Column",
			["HiddenSingleBlock"] = "Hidden Single in Block",
			["NakedSingle"] = "Naked Single",
			["Pointing"] = "Pointing",
			["Claiming"] = "Claiming",
			["AlmostLockedPair"] = "Almost Locked Pair",
			["AlmostLockedTriple"] = "Almost Locked Triple",
			["AlmostLockedQuadruple"] = "Almost Locked Quadruple",
			["NakedPair"] = "Naked Pair",
			["NakedPairPlus"] = "Naked Pair (+)",
			["LockedPair"] = "Locked Pair",
			["HiddenPair"] = "Hidden Pair",
			["NakedTriple"] = "Naked Triple",
			["NakedTriplePlus"] = "Naked Triple (+)",
			["LockedTriple"] = "Locked Triple",
			["HiddenTriple"] = "Hidden Triple",
			["NakedQuadruple"] = "Naked Quadruple",
			["NakedQuadruplePlus"] = "Naked Quadruple (+)",
			["HiddenQuadruple"] = "Hidden Quadruple",
			["XWing"] = "X-Wing",
			["FinnedXWing"] = "Finned X-Wing",
			["SashimiXWing"] = "Sashimi X-Wing",
			["SiameseFinnedXWing"] = "Siamese Finned X-Wing",
			["SiameseSashimiXWing"] = "Siamese Sashimi X-Wing",
			["FrankenXWing"] = "Franken X-Wing",
			["FinnedFrankenXWing"] = "Finned Franken X-Wing",
			["SashimiFrankenXWing"] = "Sashimi Franken X-Wing",
			["SiameseFinnedFrankenXWing"] = "Siamese Finned Franken X-Wing",
			["SiameseSashimiFrankenXWing"] = "Siamese Sashimi Franken X-Wing",
			["MutantXWing"] = "Mutant X-Wing",
			["FinnedMutantXWing"] = "Finned Mutant X-Wing",
			["SashimiMutantXWing"] = "Sashimi Mutant X-Wing",
			["SiameseFinnedMutantXWing"] = "Siamese Finned Mutant X-Wing",
			["SiameseSashimiMutantXWing"] = "Siamese Sashimi Mutant X-Wing",
			["Swordfish"] = "Swordfish",
			["FinnedSwordfish"] = "Finned Swordfish",
			["SashimiSwordfish"] = "Sashimi Swordfish",
			["SiameseFinnedSwordfish"] = "Siamese Finned Swordfish",
			["SiameseSashimiSwordfish"] = "Siamese Sashimi Swordfish",
			["FrankenSwordfish"] = "Franken Swordfish",
			["FinnedFrankenSwordfish"] = "Finned Franken Swordfish",
			["SashimiFrankenSwordfish"] = "Sashimi Franken Swordfish",
			["SiameseFinnedFrankenSwordfish"] = "Siamese Finned Franken Swordfish",
			["SiameseSashimiFrankenSwordfish"] = "Siamese Sashimi Franken Swordfish",
			["MutantSwordfish"] = "Mutant Swordfish",
			["FinnedMutantSwordfish"] = "Finned Mutant Swordfish",
			["SashimiMutantSwordfish"] = "Sashimi Mutant Swordfish",
			["SiameseFinnedMutantSwordfish"] = "Siamese Finned Mutant Swordfish",
			["SiameseSashimiMutantSwordfish"] = "Siamese Sashimi Mutant Swordfish",
			["Jellyfish"] = "Jellyfish",
			["FinnedJellyfish"] = "Finned Jellyfish",
			["SashimiJellyfish"] = "Sashimi Jellyfish",
			["SiameseFinnedJellyfish"] = "Siamese Finned Jellyfish",
			["SiameseSashimiJellyfish"] = "Siamese Sashimi Jellyfish",
			["FrankenJellyfish"] = "Franken Jellyfish",
			["FinnedFrankenJellyfish"] = "Finned Franken Jellyfish",
			["SashimiFrankenJellyfish"] = "Sashimi Franken Jellyfish",
			["SiameseFinnedFrankenJellyfish"] = "Siamese Finned Franken Jellyfish",
			["SiameseSashimiFrankenJellyfish"] = "Siamese Sashimi Franken Jellyfish",
			["MutantJellyfish"] = "Mutant Jellyfish",
			["FinnedMutantJellyfish"] = "Finned Mutant Jellyfish",
			["SashimiMutantJellyfish"] = "Sashimi Mutant Jellyfish",
			["SiameseFinnedMutantJellyfish"] = "Siamese Finned Mutant Jellyfish",
			["SiameseSashimiMutantJellyfish"] = "Siamese Sashimi Mutant Jellyfish",
			["Squirmbag"] = "Squirmbag",
			["FinnedSquirmbag"] = "Finned Squirmbag",
			["SashimiSquirmbag"] = "Sashimi Squirmbag",
			["SiameseFinnedSquirmbag"] = "Siamese Finned Squirmbag",
			["SiameseSashimiSquirmbag"] = "Siamese Sashimi Squirmbag",
			["FrankenSquirmbag"] = "Franken Squirmbag",
			["FinnedFrankenSquirmbag"] = "Finned Franken Squirmbag",
			["SashimiFrankenSquirmbag"] = "Sashimi Franken Squirmbag",
			["SiameseFinnedFrankenSquirmbag"] = "Siamese Finned Franken Squirmbag",
			["SiameseSashimiFrankenSquirmbag"] = "Siamese Sashimi Franken Squirmbag",
			["MutantSquirmbag"] = "Mutant Squirmbag",
			["FinnedMutantSquirmbag"] = "Finned Mutant Squirmbag",
			["SashimiMutantSquirmbag"] = "Sashimi Mutant Squirmbag",
			["SiameseFinnedMutantSquirmbag"] = "Siamese Finned Mutant Squirmbag",
			["SiameseSashimiMutantSquirmbag"] = "Siamese Sashimi Mutant Squirmbag",
			["Whale"] = "Whale",
			["FinnedWhale"] = "Finned Whale",
			["SashimiWhale"] = "Sashimi Whale",
			["SiameseFinnedWhale"] = "Siamese Finned Whale",
			["SiameseSashimiWhale"] = "Siamese Sashimi Whale",
			["FrankenWhale"] = "Franken Whale",
			["FinnedFrankenWhale"] = "Finned Franken Whale",
			["SashimiFrankenWhale"] = "Sashimi Franken Whale",
			["SiameseFinnedFrankenWhale"] = "Siamese Finned Franken Whale",
			["SiameseSashimiFrankenWhale"] = "Siamese Sashimi Franken Whale",
			["MutantWhale"] = "Mutant Whale",
			["FinnedMutantWhale"] = "Finned Mutant Whale",
			["SashimiMutantWhale"] = "Sashimi Mutant Whale",
			["SiameseFinnedMutantWhale"] = "Siamese Finned Mutant Whale",
			["SiameseSashimiMutantWhale"] = "Siamese Sashimi Mutant Whale",
			["Leviathan"] = "Leviathan",
			["FinnedLeviathan"] = "Finned Leviathan",
			["SashimiLeviathan"] = "Sashimi Leviathan",
			["SiameseFinnedLeviathan"] = "Siamese Finned Leviathan",
			["SiameseSashimiLeviathan"] = "Siamese Sashimi Leviathan",
			["FrankenLeviathan"] = "Franken Leviathan",
			["FinnedFrankenLeviathan"] = "Finned Franken Leviathan",
			["SashimiFrankenLeviathan"] = "Sashimi Franken Leviathan",
			["SiameseFinnedFrankenLeviathan"] = "Siamese Finned Franken Leviathan",
			["SiameseSashimiFrankenLeviathan"] = "Siamese Sashimi Franken Leviathan",
			["MutantLeviathan"] = "Mutant Leviathan",
			["FinnedMutantLeviathan"] = "Finned Mutant Leviathan",
			["SashimiMutantLeviathan"] = "Sashimi Mutant Leviathan",
			["SiameseFinnedMutantLeviathan"] = "Siamese Finned Mutant Leviathan",
			["SiameseSashimiMutantLeviathan"] = "Siamese Sashimi Mutant Leviathan",
			["XyWing"] = "XY-Wing",
			["XyzWing"] = "XYZ-Wing",
			["WxyzWing"] = "WXYZ-Wing",
			["VwxyzWing"] = "VWXYZ-Wing",
			["UvwxyzWing"] = "UVWXYZ-Wing",
			["TuvwxyzWing"] = "TUVWXYZ-Wing",
			["StuvwxyzWing"] = "STUVWXYZ-Wing",
			["RstuvwxyzWing"] = "RSTUVWXYZ-Wing",
			["IncompleteWxyzWing"] = "Incomplete WXYZ-Wing",
			["IncompleteVwxyzWing"] = "Incomplete VWXYZ-Wing",
			["IncompleteUvwxyzWing"] = "Incomplete UVWXYZ-Wing",
			["IncompleteTuvwxyzWing"] = "Incomplete TUVWXYZ-Wing",
			["IncompleteStuvwxyzWing"] = "Incomplete STUVWXYZ-Wing",
			["IncompleteRstuvwxyzWing"] = "Incomplete RSTUVWXYZ-Wing",
			["WWing"] = "W-Wing",
			["MWing"] = "M-Wing",
			["LocalWing"] = "Local Wing",
			["SplitWing"] = "Split Wing",
			["HybridWing"] = "Hybrid Wing",
			["GroupedXyWing"] = "Grouped Xy Wing",
			["GroupedWWing"] = "Grouped W-Wing",
			["GroupedMWing"] = "Grouped M-Wing",
			["GroupedLocalWing"] = "Grouped Local Wing",
			["GroupedSplitWing"] = "Grouped Split Wing",
			["GroupedHybridWing"] = "Grouped Hybrid Wing",
			["UrType1"] = "Unique Rectangle Type 1",
			["UrType2"] = "Unique Rectangle Type 2",
			["UrType3"] = "Unique Rectangle Type 3",
			["UrType4"] = "Unique Rectangle Type 4",
			["UrType5"] = "Unique Rectangle Type 5",
			["UrType6"] = "Unique Rectangle Type 6",
			["HiddenUr"] = "Hidden Unique Rectangle",
			["UrPlus2D"] = "Unique Rectangle + 2D",
			["UrPlus2B1SL"] = "Unique Rectangle + 2B / 1SL",
			["UrPlus2D1SL"] = "Unique Rectangle + 2D / 1SL",
			["UrPlus3X"] = "Unique Rectangle + 3X",
			["UrPlus3x1SL"] = "Unique Rectangle + 3x / 1SL",
			["UrPlus3X1SL"] = "Unique Rectangle + 3X / 1SL",
			["UrPlus3X2SL"] = "Unique Rectangle + 3X / 2SL",
			["UrPlus3N2SL"] = "Unique Rectangle + 3N / 2SL",
			["UrPlus3U2SL"] = "Unique Rectangle + 3U / 2SL",
			["UrPlus3E2SL"] = "Unique Rectangle + 3E / 2SL",
			["UrPlus4x1SL"] = "Unique Rectangle + 4x / 1SL",
			["UrPlus4X1SL"] = "Unique Rectangle + 4X / 1SL",
			["UrPlus4x2SL"] = "Unique Rectangle + 4x / 2SL",
			["UrPlus4X2SL"] = "Unique Rectangle + 4X / 2SL",
			["UrPlus4X3SL"] = "Unique Rectangle + 4X / 3SL",
			["UrPlus4C3SL"] = "Unique Rectangle + 4C / 3SL",
			["UrXyWing"] = "Unique Rectangle + XY-Wing",
			["UrXyzWing"] = "Unique Rectangle + XYZ-Wing",
			["UrWxyzWing"] = "Unique Rectangle + WXYZ-Wing",
			["UrSdc"] = "Unique Rectangle + Sue de Coq",
			["ArType1"] = "Avoidable Rectangle Type 1",
			["ArType2"] = "Avoidable Rectangle Type 2",
			["ArType3"] = "Avoidable Rectangle Type 3",
			["ArType5"] = "Avoidable Rectangle Type 5",
			["HiddenAr"] = "Hidden Avoidable Rectangle",
			["ArPlus2D"] = "Avoidable Rectangle + 2D",
			["ArPlus3X"] = "Avoidable Rectangle + 3X",
			["ArXyWing"] = "Avoidable Rectangle + XY-Wing",
			["ArXyzWing"] = "Avoidable Rectangle + XYZ-Wing",
			["ArWxyzWing"] = "Avoidable Rectangle + WXYZ-Wing",
			["ArSdc"] = "Avoidable Rectangle + Sue de Coq",
			["UlType1"] = "Unique Loop Type 1",
			["UlType2"] = "Unique Loop Type 2",
			["UlType3"] = "Unique Loop Type 3",
			["UlType4"] = "Unique Loop Type 4",
			["XrType1"] = "Extended Rectangle Type 1",
			["XrType2"] = "Extended Rectangle Type 2",
			["XrType3"] = "Extended Rectangle Type 3",
			["XrType4"] = "Extended Rectangle Type 4",
			["BugType1"] = "Bivalue Universal Grave Type 1",
			["BugType2"] = "Bivalue Universal Grave Type 2",
			["BugType3"] = "Bivalue Universal Grave Type 3",
			["BugType4"] = "Bivalue Universal Grave Type 4",
			["BugMultiple"] = "Bivalue Universal Grave + n",
			["BugMultipleFc"] = "Bivalue Universal Grave + n (+)",
			["BugXz"] = "Bivalue Universal Grave XZ Rule",
			["BugXyzWing"] = "Bug Xyz Wing",
			["BdpType1"] = "Borescoper's Deadly Pattern Type 1",
			["BdpType2"] = "Borescoper's Deadly Pattern Type 2",
			["BdpType3"] = "Borescoper's Deadly Pattern Type 3",
			["BdpType4"] = "Borescoper's Deadly Pattern Type 4",
			["QdpType1"] = "Qiu's Deadly Pattern Type 1",
			["QdpType2"] = "Qiu's Deadly Pattern Type 2",
			["QdpType3"] = "Qiu's Deadly Pattern Type 3",
			["QdpType4"] = "Qiu's Deadly Pattern Type 4",
			["LockedQdp"] = "Locked Qiu's Deadly Pattern",
			["UsType1"] = "Unique Square Type 1",
			["UsType2"] = "Unique Square Type 2",
			["UsType3"] = "Unique Square Type 3",
			["UsType4"] = "Unique Square Type 4",
			["Sdc"] = "Sue de Coq",
			["Sdc3d"] = "3 Dimension Sue de Coq",
			["CannibalizedSdc"] = "Cannibalized Sue de Coq",
			["Skyscraper"] = "Skyscraper",
			["TwoStringKite"] = "Two-string Kite",
			["TurbotFish"] = "Turbot Fish",
			["EmptyRectangle"] = "Empty Rectangle",
			["Guardian"] = "Guardian",
			["XChain"] = "X-Chain",
			["YChain"] = "Y-Chain",
			["FishyCycle"] = "Fishy Cycle",
			["XyChain"] = "XY-Chain",
			["XyCycle"] = "XY-Cycle",
			["XyXChain"] = "XY-X-Chain",
			["PurpleCow"] = "Purple Cow",
			["DiscontinuousNiceLoop"] = "Discontinuous Nice Loop",
			["ContinuousNiceLoop"] = "Continuous Nice Loop",
			["Aic"] = "Alternating Inference Chain",
			["GroupedXChain"] = "Grouped X-Chain",
			["GroupedFishyCycle"] = "Grouped Fishy Cycle",
			["GroupedXyChain"] = "Grouped XY-Chain",
			["GroupedXyCycle"] = "Grouped XY-Cycle",
			["GroupedXyXChain"] = "Grouped XY-X-Chain",
			["GroupedPurpleCow"] = "Grouped Purple Cow",
			["GroupedDiscontinuousNiceLoop"] = "Grouped Discontinuous Nice Loop",
			["GroupedContinuousNiceLoop"] = "Grouped Continuous Nice Loop",
			["GroupedAic"] = "Grouped Alternating Inference Chain",
			["NishioFc"] = "Nishio Forcing Chains",
			["RegionFc"] = "Region Forcing Chains",
			["CellFc"] = "Cell Forcing Chains",
			["DynamicFc"] = "Dynamic Forcing Chains",
			["Erip"] = "Empty Rectangle Intersection Pair",
			["Esp"] = "Extended Subset Principle",
			["SinglyLinkedAlsXz"] = "Singly Linked Almost Locked Sets XZ Rule",
			["DoublyLinkedAlsXz"] = "Doubly Linked Almost Locked Sets XZ Rule",
			["AlsXyWing"] = "Almost Locked Sets XY-Wing",
			["AlsWWing"] = "Almost Locked Sets W-Wing",
			["DeathBlossom"] = "Death Blossom",
			["Gsp"] = "Gurth's Symmetrical Placement",
			["Je"] = "Junior Exocet",
			["Se"] = "Senior Exocet",
			["ComplexSe"] = "Complex Senior Exocet",
			["SiameseJe"] = "Siamese Je",
			["SiameseSe"] = "Siamese Se",
			["SkLoop"] = "Stephen Kurzhal's Loop",
			["Msls"] = "Multi-sector Locked Sets",
			["Pom"] = "Pattern Overlay",
			["TemplateSet"] = "Template Set",
			["TemplateDelete"] = "Template Delete",
			["BowmanBingo"] = "Bowman's Bingo",
			["BruteForce"] = "Brute Force",
		};
	}
}
