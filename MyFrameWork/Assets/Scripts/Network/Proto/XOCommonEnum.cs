// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: XOCommonEnum.proto
// </auto-generated>
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace XOProto {

  /// <summary>Holder for reflection information generated from XOCommonEnum.proto</summary>
  public static partial class XOCommonEnumReflection {

    #region Descriptor
    /// <summary>File descriptor for XOCommonEnum.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static XOCommonEnumReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "ChJYT0NvbW1vbkVudW0ucHJvdG8qMwoMUmVzb3VyY2VUeXBlEhkKFVJFU09V",
            "UkNFX1RZUEVfVU5LTk9XThAAEggKBEdPTEQQASozCgdTZXhUeXBlEg4KClNU",
            "X1VOS05PV04QABIKCgZTVF9NQU4QARIMCghTVF9XT01BThACKlIKDlVzZXJT",
            "dGF0dXNUeXBlEg4KClVTVF9DT01NT04QABIMCghVU1RfS0lDSxABEhAKDFVT",
            "VF9CQU5fQ0hBVBACEhAKDFVTVF9CQU5fUExBWRADQiwKFGNvbS50d3gub3Jp",
            "Z2luLnByb3RvQgpDb21tb25FbnVtqgIHWE9Qcm90b2IGcHJvdG8z"));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { },
          new pbr::GeneratedClrTypeInfo(new[] {typeof(global::XOProto.ResourceType), typeof(global::XOProto.SexType), typeof(global::XOProto.UserStatusType), }, null, null));
    }
    #endregion

  }
  #region Enums
  /// <summary>
  ///资源类型
  /// </summary>
  public enum ResourceType {
    [pbr::OriginalName("RESOURCE_TYPE_UNKNOWN")] Unknown = 0,
    /// <summary>
    ///	金币
    /// </summary>
    [pbr::OriginalName("GOLD")] Gold = 1,
  }

  /// <summary>
  ///性别
  /// </summary>
  public enum SexType {
    /// <summary>
    ///未知
    /// </summary>
    [pbr::OriginalName("ST_UNKNOWN")] StUnknown = 0,
    /// <summary>
    ///男
    /// </summary>
    [pbr::OriginalName("ST_MAN")] StMan = 1,
    /// <summary>
    ///女
    /// </summary>
    [pbr::OriginalName("ST_WOMAN")] StWoman = 2,
  }

  public enum UserStatusType {
    /// <summary>
    ///正常
    /// </summary>
    [pbr::OriginalName("UST_COMMON")] UstCommon = 0,
    /// <summary>
    ///踢下线
    /// </summary>
    [pbr::OriginalName("UST_KICK")] UstKick = 1,
    /// <summary>
    ///禁聊
    /// </summary>
    [pbr::OriginalName("UST_BAN_CHAT")] UstBanChat = 2,
    /// <summary>
    ///禁玩
    /// </summary>
    [pbr::OriginalName("UST_BAN_PLAY")] UstBanPlay = 3,
  }

  #endregion

}

#endregion Designer generated code