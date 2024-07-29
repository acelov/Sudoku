namespace Sudoku.Bot.Commands;

/// <summary>
/// 表示一个机器人指令。这个指令不同于配置页面的指令，这个指令可以灵活使用，并不走斜杠 <c>/</c> 触发。也可以是那样的指令。
/// </summary>
public abstract class Command
{
	/// <summary>
	/// 表示指令的名称。
	/// </summary>
	public abstract string CommandName { get; }

	/// <summary>
	/// 表示指令的样例用法。默认情况下，只有带斜杠和指令名称，如“/签到”。
	/// </summary>
	public virtual string HelpCommandString => $"/{CommandName}";

	/// <summary>
	/// 表示默认情况下（参数错误等）反馈的字符串。可以用于在参数校验后返回。
	/// </summary>
	protected string DefaultInfoString => $"写法内容：“{HelpCommandString}”。";


	/// <summary>
	/// 这个方法会在群里艾特并且指令触发时执行。
	/// </summary>
	/// <param name="api">群聊消息 API，用来发送消息。</param>
	/// <param name="message">群消息的提供参数。</param>
	/// <returns>异步函数返回的 <see cref="Task"/> 对象。</returns>
	public abstract Task GroupCallback(ChatMessageApi api, ChatMessage message);
}
