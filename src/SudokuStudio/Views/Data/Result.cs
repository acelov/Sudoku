using System;
namespace SudokuStudio.Views.Data;
public enum ResultType
{
    /// <summary>
    /// 表示分析结论是一个填入单元格的值
    /// </summary>
    Assignment = 0,

    /// <summary>
    /// 表示分析是候选数字从单元格中移除
    /// </summary>
    Elimination = 1
}
    
[Serializable]
public class Result
{
	/// <summary>
	/// 分析结果类型
	/// </summary>
	public ConclusionType T { get; set; }
	/// <summary>
	/// 分析结果的值
	/// </summary>
	public int V { get; set; }
	/// <summary>
	/// 单元格的下标 Value / 9
	/// </summary>
	public int C;
	/// <summary>
	/// 标注数字的下标 Value % 9
	/// </summary>
	public int D;

	public Result(ConclusionType t, int c, int d)
	{
		T = t;
		C = c;
		D = d;
	}

}
