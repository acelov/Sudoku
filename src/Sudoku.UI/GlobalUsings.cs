﻿global using System;
global using System.Collections.Generic;
global using System.Collections.ObjectModel;
global using System.Collections.Specialized;
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
global using System.Runtime.InteropServices;
global using System.Runtime.InteropServices.WindowsRuntime;
global using System.Text.Encodings.Web;
global using System.Text.Json;
global using System.Text.Json.Serialization;
global using System.Text.RegularExpressions;
global using System.Threading.Tasks;
global using CommunityToolkit.WinUI.Helpers;
global using Microsoft.Graphics.Canvas.Text;
global using Microsoft.UI;
global using Microsoft.UI.Composition;
global using Microsoft.UI.Input;
global using Microsoft.UI.Windowing;
global using Microsoft.UI.Xaml;
global using Microsoft.UI.Xaml.Controls;
global using Microsoft.UI.Xaml.Controls.Primitives;
global using Microsoft.UI.Xaml.Data;
global using Microsoft.UI.Xaml.Input;
global using Microsoft.UI.Xaml.Markup;
global using Microsoft.UI.Xaml.Media;
global using Microsoft.UI.Xaml.Media.Animation;
global using Microsoft.UI.Xaml.Media.Imaging;
global using Microsoft.UI.Xaml.Navigation;
global using Microsoft.UI.Xaml.Printing;
global using Microsoft.UI.Xaml.Shapes;
global using Microsoft.Windows.AppLifecycle;
global using Sudoku.Concepts;
global using Sudoku.Generating.Puzzlers;
global using Sudoku.Runtime.AnalysisServices;
global using Sudoku.Solving.Manual;
global using Sudoku.UI.AppLifecycle;
global using Sudoku.UI.Configuration;
global using Sudoku.UI.Drawing;
global using Sudoku.UI.Drawing.Shapes;
global using Sudoku.UI.Input;
global using Sudoku.UI.Interoperability;
global using Sudoku.UI.Metadata;
global using Sudoku.UI.Models;
global using Sudoku.UI.Views.Pages;
global using Sudoku.UI.Views.Windows;
global using Windows.ApplicationModel.Activation;
global using Windows.ApplicationModel.DataTransfer;
global using Windows.Foundation;
global using Windows.Graphics.Display;
global using Windows.Graphics.Imaging;
global using Windows.Graphics.Printing;
global using Windows.Storage;
global using Windows.Storage.Pickers;
global using Windows.Storage.Provider;
global using Windows.Storage.Streams;
global using Windows.System;
global using Windows.UI;
global using Windows.UI.Core;
global using Windows.UI.ViewManagement;
global using WinRT;
global using WinRT.Interop;
global using static System.Numerics.BitOperations;
global using static Sudoku.Resources.MergedResources;
global using static Sudoku.Runtime.AnalysisServices.CommonReadOnlies;
global using GridLayout = Microsoft.UI.Xaml.Controls.Grid;
global using MsLaunchActivatedEventArgs = Microsoft.UI.Xaml.LaunchActivatedEventArgs;
global using Grid = Sudoku.Concepts.Collections.Grid;
global using SystemIOFile = System.IO.File;
global using SystemIOPath = System.IO.Path;
