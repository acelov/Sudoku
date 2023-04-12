global using System;
global using System.Algorithm;
global using System.Buffers;
global using System.Collections;
global using System.Collections.Generic;
global using System.Collections.Immutable;
global using System.ComponentModel;
global using System.Diagnostics;
global using System.Diagnostics.CodeAnalysis;
global using System.Diagnostics.CodeGen;
global using System.Globalization;
global using System.IO;
global using System.Linq;
global using System.Numerics;
global using System.Reflection;
global using System.Runtime.CompilerServices;
global using System.Runtime.Intrinsics;
global using System.Runtime.Messages;
global using System.Text;
global using System.Text.Json;
global using System.Text.Json.Serialization;
global using System.Text.RegularExpressions;
global using System.Threading;
global using System.Threading.Tasks;
global using Sudoku.Algorithms.Collections;
global using Sudoku.Algorithms.Solving;
global using Sudoku.Analytics;
global using Sudoku.Concepts;
global using Sudoku.Rendering.LocalSerialization;
global using Sudoku.Rendering.Nodes;
global using Sudoku.Rendering.Nodes.Grouped;
global using Sudoku.Rendering.Nodes.Shapes;
global using Sudoku.Text.Formatting;
global using Sudoku.Text.Notations;
global using Sudoku.Text.Parsing;
global using static System.Algorithm.Sequences;
global using static System.Algorithm.Sorting;
global using static System.Math;
global using static System.Numerics.BitOperations;
global using static System.Runtime.CompilerServices.Unsafe;
global using static System.Runtime.CompilerServices.Unsafe2;
global using static System.Text.Json.JsonSerializer;
global using static Sudoku.Analytics.ConclusionType;
global using static Sudoku.Resources.MergedResources;
global using static Sudoku.Runtime.MaskServices.MaskOperations;
global using static Sudoku.SolutionWideReadOnlyFields;
global using unsafe RefreshingCandidatesMethodPtr = delegate*<ref Sudoku.Concepts.Grid, void>;
global using unsafe ValueChangedMethodPtr = delegate*<ref Sudoku.Concepts.Grid, int, short, short, int, void>;
global using unsafe ParserMethodPtr = delegate*<ref Sudoku.Text.Parsing.GridParser, Sudoku.Concepts.Grid>;
