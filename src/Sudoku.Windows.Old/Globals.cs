﻿global using System;
global using System.Collections.Generic;
global using System.Collections.ObjectModel;
global using System.ComponentModel;
global using System.Diagnostics;
global using System.Diagnostics.CodeAnalysis;
global using System.Drawing;
global using System.Drawing.Drawing2D;
global using System.Drawing.Imaging;
global using System.Drawing.Text;
global using System.Globalization;
global using System.IO;
global using System.Linq;
global using System.Numerics;
global using System.Reflection;
global using System.Runtime.CompilerServices;
global using System.Runtime.InteropServices;
global using System.Text;
global using System.Text.Encodings.Web;
global using System.Text.Json;
global using System.Text.Json.Serialization;
global using System.Text.RegularExpressions;
global using System.Threading;
global using System.Threading.Tasks;
global using System.Windows;
global using System.Windows.Controls;
global using System.Windows.Controls.Primitives;
global using System.Windows.Data;
global using System.Windows.Documents;
global using System.Windows.Input;
global using System.Windows.Media;
global using System.Windows.Media.Imaging;
global using System.Xml;
global using System.Xml.Serialization;
global using Microsoft.Win32;
global using Sudoku.CodeGenerating;
global using Sudoku.Data;
global using Sudoku.Data.Collections;
global using Sudoku.Drawing;
global using Sudoku.Drawing.Converters;
global using Sudoku.Generating;
global using Sudoku.Globalization;
global using Sudoku.IO;
global using Sudoku.Models;
global using Sudoku.Recognition;
global using Sudoku.Resources;
global using Sudoku.Solving;
global using Sudoku.Solving.BruteForces;
global using Sudoku.Solving.Checking;
global using Sudoku.Solving.Manual;
global using Sudoku.Solving.Manual.Symmetry;
global using Sudoku.Techniques;
global using Sudoku.Windows.CustomControls;
global using Sudoku.Windows.Data;
global using Sudoku.Windows.Media;
global using static System.Drawing.StringAlignment;
global using static System.Drawing.Text.TextRenderingHint;
global using static System.Math;
global using static System.Windows.Input.Key;
global using static Sudoku.Windows.MainWindow;
global using WColorPalette = Sudoku.Windows.Media.ColorPalette;
global using StepTriplet = System.Collections.Generic.KeyedTuple<string, int, Sudoku.Solving.Manual.StepInfo>;
global using StepTypeTriplet = System.Collections.Generic.KeyedTuple<string, int, System.Type>;
global using InfoTriplet = System.Collections.Generic.KeyedTuple<string, Sudoku.Solving.Manual.StepInfo, bool>;
global using DBrush = System.Drawing.Brush;
global using DBrushes = System.Drawing.Brushes;
global using DColor = System.Drawing.Color;
global using DFontStyle = System.Drawing.FontStyle;
global using DPoint = System.Drawing.Point;
global using DPointF = System.Drawing.PointF;
global using DSize = System.Drawing.Size;
global using DSizeF = System.Drawing.SizeF;
global using WImage = System.Windows.Controls.Image;
global using WBrushes = System.Windows.Media.Brushes;
global using WColor = System.Windows.Media.Color;
global using WFontFamily = System.Windows.Media.FontFamily;
global using WLinearGradientBrush = System.Windows.Media.LinearGradientBrush;
global using WSolidColorBrush = System.Windows.Media.SolidColorBrush;
global using WPoint = System.Windows.Point;
global using WSize = System.Windows.Size;

[assembly: AssemblyObsolete]

[assembly: AutoDeconstructExtension(typeof(WImage), nameof(WImage.Width), nameof(WImage.Height))]
[assembly: AutoDeconstructExtension(typeof(WPoint), nameof(WPoint.X), nameof(WPoint.Y))]
[assembly: AutoDeconstructExtension(typeof(WColor), nameof(WColor.A), nameof(WColor.R), nameof(WColor.G), nameof(WColor.B))]
[assembly: AutoDeconstructExtension(typeof(WSize), nameof(WSize.Width), nameof(WSize.Height))]