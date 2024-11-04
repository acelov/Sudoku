using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace SudokuStudio.Views.Data;
public class AnalyzeStep
{
	/// <summary>
	/// Name
	/// </summary>
    public string N;
	/// <summary>
	/// Default Score
	/// </summary>
    public int DS;
	/// <summary>
	/// Score
	/// </summary>
    public int S;
	/// <summary>
	/// Results
	/// </summary>
    public List<Result> RS;
	/// <summary>
	/// Nodes
	/// </summary>
    public List<List<string>> NS;

	public AnalyzeStep()
	{
		N = "";
		RS = new List<Result>();
		NS = new List<List<string>>();
	}
}
