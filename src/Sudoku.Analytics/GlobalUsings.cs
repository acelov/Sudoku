global using System;
global using System.Algorithm;
global using System.Collections;
global using System.Collections.Frozen;
global using System.Collections.Generic;
global using System.ComponentModel;
global using System.Diagnostics;
global using System.Diagnostics.CodeAnalysis;
global using System.Globalization;
global using System.Linq;
global using System.Linq.Expressions;
global using System.Numerics;
global using System.Reflection;
global using System.Resources;
global using System.Runtime.CompilerServices;
global using System.Runtime.InteropServices;
global using System.Runtime.Versioning;
global using System.SourceGeneration;
global using System.Text;
global using System.Text.Json;
global using System.Text.Json.Serialization;
global using System.Text.RegularExpressions;
global using System.Threading;
global using Sudoku.Analytics;
global using Sudoku.Analytics.Categorization;
global using Sudoku.Analytics.Configuration;
global using Sudoku.Analytics.Metadata;
global using Sudoku.Analytics.Steps;
global using Sudoku.Analytics.StepSearcherModules;
global using Sudoku.Analytics.StepSearchers;
global using Sudoku.Compatibility;
global using Sudoku.Compatibility.Hodoku;
global using Sudoku.Compatibility.SudokuExplainer;
global using Sudoku.Concepts;
global using Sudoku.Concepts.ObjectModel;
global using Sudoku.Generating;
global using Sudoku.Generating.JustOneCell;
global using Sudoku.Generating.TechniqueBased;
global using Sudoku.Linq;
global using Sudoku.Measuring;
global using Sudoku.Measuring.Factors;
global using Sudoku.Rendering;
global using Sudoku.Rendering.Nodes;
global using Sudoku.Resources;
global using Sudoku.Runtime.CompilerServices;
global using Sudoku.Runtime.MaskServices;
global using Sudoku.Solving;
global using Sudoku.Strategying.Constraints;
global using Sudoku.Text;
global using Sudoku.Text.Converters;
global using Sudoku.Text.Parsers;
global using Sudoku.Traits;
global using static System.Algorithm.Sequences;
global using static System.Numerics.BitOperations;
global using static Sudoku.Analytics.CachedFields;
global using static Sudoku.Concepts.ConclusionType;
global using static Sudoku.Concepts.Intersection;
global using static Sudoku.Rendering.RenderingMode;
global using static Sudoku.SolutionFields;
global using static Sudoku.Text.Languages;
global using TargetCandidatesGroup = Sudoku.Linq.BitStatusMapGrouping<Sudoku.Concepts.CandidateMap, int /*Candidate*/, Sudoku.Concepts.CandidateMap.Enumerator, int /*Cell*/>;
global using TargetCellsGroup = Sudoku.Linq.BitStatusMapGrouping<Sudoku.Concepts.CellMap, int /*Cell*/, Sudoku.Concepts.CellMap.Enumerator, int /*House*/>;
