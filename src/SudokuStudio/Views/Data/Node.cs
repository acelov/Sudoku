using System;
namespace SudokuStudio.Views.Data;
public enum NodeType
{
	CellViewNode=0,
	CandidateViewNode=1,
	HouseViewNode=2,
	ChuteViewNode=3,
	BabaGroupViewNode=4,
	ChainLinkViewNode=5,
	CellLinkViewNode=6,
	ConjugateLinkViewNode=7,
	CircleViewNode=10,
	CrossViewNode=11,
	TriangleViewNode=12,
	DiamondViewNode=13,
	StarViewNode=14,
	SquareViewNode=15,
	HeartViewNode=16,
}

[Serializable]
public class Node
{
	/// <summary>
	/// NodeType
	/// </summary>
	public NodeType T;
	/// <summary>
	/// Color Kind
	/// </summary>
	public int K;

	/// <summary>
	/// Cell
	/// </summary>
	public int C;
	/// <summary>
	/// Candidate
	/// </summary>
	public int CA;

	/// <summary>
	/// House
	/// </summary>
	public int H;

	/// <summary>
	/// UnknownValueChar
	/// </summary>
	public string U = "";
	/// <summary>
	/// DigitsMask
	/// </summary>
	public int D;


	/// <summary>
	/// Start
	/// </summary>
	//public List<string> s;
	/// <summary>
	/// End
	/// </summary>
	//public List<string> e;
	/// <summary>
	/// IsStrongLink
	/// </summary>
	//public bool IsStrongLink;

}
