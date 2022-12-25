﻿global using System;
global using System.Collections;
global using System.Collections.Concurrent;
global using System.Collections.Generic;
global using System.Diagnostics.CodeAnalysis;
global using System.Diagnostics.CodeGen;
global using System.Drawing;
global using System.IO;
global using System.Linq;
global using System.Net.NetworkInformation;
global using System.Numerics;
global using System.Reactive.Linq;
global using System.Resources;
global using System.Runtime.CompilerServices;
global using System.Runtime.Messages;
global using System.Runtime.Versioning;
global using System.Text.Json;
global using System.Text.Json.Serialization;
global using System.Text.RegularExpressions;
global using System.Threading;
global using System.Threading.Tasks;
global using Mirai.Net.Data.Events.Concretes.Bot;
global using Mirai.Net.Data.Events.Concretes.Group;
global using Mirai.Net.Data.Events.Concretes.Request;
global using Mirai.Net.Data.Messages;
global using Mirai.Net.Data.Messages.Concretes;
global using Mirai.Net.Data.Messages.Receivers;
global using Mirai.Net.Data.Shared;
global using Mirai.Net.Sessions.Http.Managers;
global using Mirai.Net.Utils.Scaffolds;
global using OneOf;
global using Sudoku.AutoFiller;
global using Sudoku.Concepts;
global using Sudoku.Drawing;
global using Sudoku.Generating.Puzzlers;
global using Sudoku.Platforms.QQ.AppLifecycle;
global using Sudoku.Platforms.QQ.Configurations;
global using Sudoku.Platforms.QQ.IO;
global using Sudoku.Platforms.QQ.Models;
global using Sudoku.Platforms.QQ.Scoring;
global using Sudoku.Presentation;
global using Sudoku.Presentation.Nodes;
global using Sudoku.Resources;
global using Sudoku.Runtime.AnalysisServices;
global using Sudoku.Solving.Logical;
global using Sudoku.Text.Notations;
global using static System.Algorithm.Sequences;
global using static System.Math;
global using static System.Text.Json.JsonSerializer;
global using static Sudoku.Platforms.QQ.AppLifecycle.EnvironmentData;
global using static Sudoku.Resources.MergedResources;
global using Group = Mirai.Net.Data.Shared.Group;
global using TextResources = Sudoku.Platforms.QQ.Strings.Resources;
global using SpecialFolder = System.Environment.SpecialFolder;
global using File = System.IO.File;
