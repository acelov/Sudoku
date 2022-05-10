﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Sudoku.Bot.Communication.Resources {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class ResourceDictionary {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal ResourceDictionary() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Sudoku.Bot.Communication.Resources.ResourceDictionary", typeof(ResourceDictionary).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to P:\Bot\bot.json.
        /// </summary>
        internal static string @__LocalBotConfigPath {
            get {
                return ResourceManager.GetString("__LocalBotConfigPath", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to P:\Bot\Players.
        /// </summary>
        internal static string @__LocalPlayerConfigPath {
            get {
                return ResourceManager.GetString("__LocalPlayerConfigPath", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 向向的游戏机器人
        ///作者：SunnieShine（小向）
        ///版本：.
        /// </summary>
        internal static string AboutInfo_Segment1 {
            get {
                return ResourceManager.GetString("AboutInfo_Segment1", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 玩法：请通过艾特本机器人，并带上斜杠符号。支持的指令有：
        ///💡 /签到（测试功能）：允许玩家每日参与一次签到。
        ///💡 /关于：提示程序介绍文字相关内容（即本消息）。
        ///💡 /复读：把你发的消息复读一次。.
        /// </summary>
        internal static string AboutInfo_Segment2 {
            get {
                return ResourceManager.GetString("AboutInfo_Segment2", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 签到失败。原因：
        ///用户存档文件里的此数据为 null 值。机器人设计者，你看看你都干了些什么好事！.
        /// </summary>
        internal static string ClockInError_DateStringIsNull {
            get {
                return ResourceManager.GetString("ClockInError_DateStringIsNull", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 对不起，机器人出了点问题。原因：
        ///机器人尝试在读取用户的存档的时候，“日期”一行的文字并不是真的日期的字符串数据，而是别的什么东西。可能在录入数据的时候就出现了问题。请联系机器人设计者修复此问题。.
        /// </summary>
        internal static string ClockInError_InvalidDateValue {
            get {
                return ResourceManager.GetString("ClockInError_InvalidDateValue", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 对不起，机器人出了点问题。原因：
        ///机器人尚未配置用户存档数据的本地路径。该路径对于机器人识别用户的数据来说非常重要。如果没有此数据的话，机器人就相当于直接不知道怎么读写你们的“存档”了。这是很可怕的，对吧。.
        /// </summary>
        internal static string ClockInError_ResourceNotFound {
            get {
                return ResourceManager.GetString("ClockInError_ResourceNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 签到成功。这是你第一次使用机器人哦~ 恭喜你获得 0 经验值~
        ///（非常抱歉，由于是测试功能，所以暂时没有经验值系统。）.
        /// </summary>
        internal static string ClockInSuccess_FileCreated {
            get {
                return ResourceManager.GetString("ClockInSuccess_FileCreated", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 签到成功。这是你第一次签到哦~ 恭喜你获得 0 经验值~
        ///（非常抱歉，由于是测试功能，所以暂时没有经验值系统。）.
        /// </summary>
        internal static string ClockInSuccess_ValueCreated {
            get {
                return ResourceManager.GetString("ClockInSuccess_ValueCreated", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 签到成功。数据已更新~ 恭喜你获得 0 经验值~
        ///（非常抱歉，由于是测试功能，所以暂时没有经验值系统。）.
        /// </summary>
        internal static string ClockInSuccess_ValueUpdated {
            get {
                return ResourceManager.GetString("ClockInSuccess_ValueUpdated", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 签到失败。原因：
        ///本地记录的数据里，你签到的上一回时间就是今天。不能在同一天签到多次。.
        /// </summary>
        internal static string ClockInWarning_CannotClockInInSameDay {
            get {
                return ResourceManager.GetString("ClockInWarning_CannotClockInInSameDay", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ：.
        /// </summary>
        internal static string Colon {
            get {
                return ResourceManager.GetString("Colon", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 关于.
        /// </summary>
        internal static string Command_About {
            get {
                return ResourceManager.GetString("Command_About", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 签到.
        /// </summary>
        internal static string Command_ClockIn {
            get {
                return ResourceManager.GetString("Command_ClockIn", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 复读.
        /// </summary>
        internal static string Command_Repeat {
            get {
                return ResourceManager.GetString("Command_Repeat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 日.
        /// </summary>
        internal static string DateTimeUnit_Day1 {
            get {
                return ResourceManager.GetString("DateTimeUnit_Day1", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 天.
        /// </summary>
        internal static string DateTimeUnit_Day2 {
            get {
                return ResourceManager.GetString("DateTimeUnit_Day2", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 小时.
        /// </summary>
        internal static string DateTimeUnit_Hour1 {
            get {
                return ResourceManager.GetString("DateTimeUnit_Hour1", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 时.
        /// </summary>
        internal static string DateTimeUnit_Hour2 {
            get {
                return ResourceManager.GetString("DateTimeUnit_Hour2", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 分钟.
        /// </summary>
        internal static string DateTimeUnit_Minute1 {
            get {
                return ResourceManager.GetString("DateTimeUnit_Minute1", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 分.
        /// </summary>
        internal static string DateTimeUnit_Minute2 {
            get {
                return ResourceManager.GetString("DateTimeUnit_Minute2", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 星期.
        /// </summary>
        internal static string DateTimeUnit_Week1 {
            get {
                return ResourceManager.GetString("DateTimeUnit_Week1", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 周.
        /// </summary>
        internal static string DateTimeUnit_Week2 {
            get {
                return ResourceManager.GetString("DateTimeUnit_Week2", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 年.
        /// </summary>
        internal static string DateTimeUnit_Year {
            get {
                return ResourceManager.GetString("DateTimeUnit_Year", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 新的日程.
        /// </summary>
        internal static string DefaultScheduleDescription {
            get {
                return ResourceManager.GetString("DefaultScheduleDescription", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 新建日程.
        /// </summary>
        internal static string DefaultScheduleName {
            get {
                return ResourceManager.GetString("DefaultScheduleName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to (\d+)\s*(年|星期|周|日|天|小?时|分钟?|秒钟?).
        /// </summary>
        internal static string JinxTimeCountdownPattern {
            get {
                return ResourceManager.GetString("JinxTimeCountdownPattern", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to (\d{4})[-年](\d\d)[-月](\d\d)[\s日]*(\d\d)[:点时](\d\d)[:分](\d\d)秒?.
        /// </summary>
        internal static string JinxTimestampPattern {
            get {
                return ResourceManager.GetString("JinxTimestampPattern", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 管理员.
        /// </summary>
        internal static string Name_Administrator {
            get {
                return ResourceManager.GetString("Name_Administrator", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 子频道管理员.
        /// </summary>
        internal static string Name_ChannelManager {
            get {
                return ResourceManager.GetString("Name_ChannelManager", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 频道主.
        /// </summary>
        internal static string Name_GuildOwner {
            get {
                return ResourceManager.GetString("Name_GuildOwner", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 普通成员.
        /// </summary>
        internal static string Name_NormalMember {
            get {
                return ResourceManager.GetString("Name_NormalMember", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 私域机器人.
        /// </summary>
        internal static string PrivateDomainBotSuffix {
            get {
                return ResourceManager.GetString("PrivateDomainBotSuffix", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 公域机器人.
        /// </summary>
        internal static string PublicDomainBotSuffix {
            get {
                return ResourceManager.GetString("PublicDomainBotSuffix", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SDK 版本.
        /// </summary>
        internal static string SdkVersionName {
            get {
                return ResourceManager.GetString("SdkVersionName", resourceCulture);
            }
        }
    }
}
