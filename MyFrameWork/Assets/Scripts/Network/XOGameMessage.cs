// 目前该类不是自动生成的已经手动改过了
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using Google.Protobuf;
using pb = Google.Protobuf;
using pbc = Google.Protobuf.Collections;
using pbr = Google.Protobuf.Reflection;
namespace XOProto
{

    /// <summary>Holder for reflection information generated from XOGameMessage.proto</summary>
    public static partial class XOGameMessageReflection
    {

        #region Descriptor
        /// <summary>File descriptor for XOGameMessage.proto</summary>
        public static pbr::FileDescriptor Descriptor
        {
            get { return descriptor; }
        }
        private static pbr::FileDescriptor descriptor;

        static XOGameMessageReflection()
        {
            byte[] descriptorData = System.Convert.FromBase64String(
                string.Concat(
                  "ChNYT0dhbWVNZXNzYWdlLnByb3RvIiYKCkFwcGVuZEl0ZW0SCgoCaWQYASAB",
                  "KAUSDAoEZGF0YRgCIAEoDCIzCgdSZXF1ZXN0Eg0KBWFwaUlkGAEgASgFEgsK",
                  "A2tleRgCIAEoBRIMCgRkYXRhGAMgASgMImQKCFJlc3BvbnNlEg0KBWFwaUlk",
                  "GAEgASgFEgsKA2tleRgCIAEoBRIMCgRjb2RlGAMgASgFEgwKBGRhdGEYBCAB",
                  "KAwSIAoLYXBwZW5kSXRlbXMYBSADKAsyCy5BcHBlbmRJdGVtIh8KCEVycm9y",
                  "TXNnEhMKC2Vycm9yUGFyYW1zGAEgAygJQisKGGNvbS54aW5ncGFuLm9yaWdp",
                  "bi5wcm90b0IHR2FtZU1zZ6oCBVVCYXNlYgZwcm90bzM="));
            descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
                new pbr::FileDescriptor[] { },
                new pbr::GeneratedClrTypeInfo(null, null, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::XOProto.AppendItem), global::XOProto.AppendItem.Parser, new[]{ "Id", "Data" }, null, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::XOProto.Request), global::XOProto.Request.Parser, new[]{ "ApiId", "Key", "Data" }, null, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::XOProto.Response), global::XOProto.Response.Parser, new[]{ "ApiId", "Key", "Code", "Data", "AppendItems" }, null, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::XOProto.ErrorMsg), global::XOProto.ErrorMsg.Parser, new[]{ "ErrorParams" }, null, null, null, null)
                }));
        }
        #endregion

    }
    #region Messages
    /// <summary>
    ///命令附加数据单元数据
    /// </summary>
    public sealed partial class AppendItem : pb::IMessage<AppendItem>
    {
        private static readonly pb::MessageParser<AppendItem> _parser = new pb::MessageParser<AppendItem>(() => new AppendItem());
        private pb::UnknownFieldSet _unknownFields;
        [System.Diagnostics.DebuggerNonUserCode]
        public static pb::MessageParser<AppendItem> Parser { get { return _parser; } }

        [System.Diagnostics.DebuggerNonUserCode]
        public static pbr::MessageDescriptor Descriptor
        {
            get { return global::XOProto.XOGameMessageReflection.Descriptor.MessageTypes[0]; }
        }

        [System.Diagnostics.DebuggerNonUserCode]
        pbr::MessageDescriptor pb::IMessage.Descriptor
        {
            get { return Descriptor; }
        }

        [System.Diagnostics.DebuggerNonUserCode]
        public AppendItem()
        {
            OnConstruction();
        }

        partial void OnConstruction();

        [System.Diagnostics.DebuggerNonUserCode]
        public AppendItem(AppendItem other) : this()
        {
            id_ = other.id_;
            data_ = other.data_;
            _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
        }

        [System.Diagnostics.DebuggerNonUserCode]
        public AppendItem Clone()
        {
            return new AppendItem(this);
        }

        /// <summary>Field number for the "id" field.</summary>
        public const int IdFieldNumber = 1;
        private int id_;
        /// <summary>
        ///附加数据ID  1=WalletDto 2=EquipmentsDto 3=
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCode]
        public int Id
        {
            get { return id_; }
            set
            {
                id_ = value;
            }
        }

        /// <summary>Field number for the "data" field.</summary>
        public const int DataFieldNumber = 2;
        private pb::ByteString data_ = pb::ByteString.Empty;
        /// <summary>
        ///附加单元数据
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCode]
        public pb::ByteString Data
        {
            get { return data_; }
            set
            {
                data_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
            }
        }

        [System.Diagnostics.DebuggerNonUserCode]
        public override bool Equals(object other)
        {
            return Equals(other as AppendItem);
        }

        [System.Diagnostics.DebuggerNonUserCode]
        public bool Equals(AppendItem other)
        {
            if (ReferenceEquals(other, null))
            {
                return false;
            }
            if (ReferenceEquals(other, this))
            {
                return true;
            }
            if (Id != other.Id) return false;
            if (Data != other.Data) return false;
            return Equals(_unknownFields, other._unknownFields);
        }

        [System.Diagnostics.DebuggerNonUserCode]
        public override int GetHashCode()
        {
            int hash = 1;
            if (Id != 0) hash ^= Id.GetHashCode();
            if (Data.Length != 0) hash ^= Data.GetHashCode();
            if (_unknownFields != null)
            {
                hash ^= _unknownFields.GetHashCode();
            }
            return hash;
        }

        [System.Diagnostics.DebuggerNonUserCode]
        public override string ToString()
        {
            return pb::JsonFormatter.ToDiagnosticString(this);
        }

        [System.Diagnostics.DebuggerNonUserCode]
        public void WriteTo(pb::CodedOutputStream output)
        {
            if (Id != 0)
            {
                output.WriteRawTag(8);
                output.WriteInt32(Id);
            }
            if (Data.Length != 0)
            {
                output.WriteRawTag(18);
                output.WriteBytes(Data);
            }
            if (_unknownFields != null)
            {
                _unknownFields.WriteTo(output);
            }
        }

        [System.Diagnostics.DebuggerNonUserCode]
        public int CalculateSize()
        {
            int size = 0;
            if (Id != 0)
            {
                size += 1 + pb::CodedOutputStream.ComputeInt32Size(Id);
            }
            if (Data.Length != 0)
            {
                size += 1 + pb::CodedOutputStream.ComputeBytesSize(Data);
            }
            if (_unknownFields != null)
            {
                size += _unknownFields.CalculateSize();
            }
            return size;
        }

        [System.Diagnostics.DebuggerNonUserCode]
        public void MergeFrom(AppendItem other)
        {
            if (other == null)
            {
                return;
            }
            if (other.Id != 0)
            {
                Id = other.Id;
            }
            if (other.Data.Length != 0)
            {
                Data = other.Data;
            }
            _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
        }

        [System.Diagnostics.DebuggerNonUserCode]
        public void MergeFrom(pb::CodedInputStream input)
        {
            uint tag;
            while ((tag = input.ReadTag()) != 0)
            {
                switch (tag)
                {
                    default:
                        _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
                        break;
                    case 8:
                        {
                            Id = input.ReadInt32();
                            break;
                        }
                    case 18:
                        {
                            Data = input.ReadBytes();
                            break;
                        }
                }
            }
        }
    }

    public interface IProtoMessageCommunicate
    {
        int ApiId { get; set; }
        ByteString Data { get; set; }
    }

    public sealed partial class Request : pb::IMessage<Request>, IProtoMessageCommunicate
    {
        private static readonly pb::MessageParser<Request> _parser = new pb::MessageParser<Request>(() => new Request());
        private pb::UnknownFieldSet _unknownFields;
        [System.Diagnostics.DebuggerNonUserCode]
        public static pb::MessageParser<Request> Parser { get { return _parser; } }

        [System.Diagnostics.DebuggerNonUserCode]
        public static pbr::MessageDescriptor Descriptor
        {
            get { return global::XOProto.XOGameMessageReflection.Descriptor.MessageTypes[1]; }
        }

        [System.Diagnostics.DebuggerNonUserCode]
        pbr::MessageDescriptor pb::IMessage.Descriptor
        {
            get { return Descriptor; }
        }

        [System.Diagnostics.DebuggerNonUserCode]
        public Request()
        {
            OnConstruction();
        }

        partial void OnConstruction();

        [System.Diagnostics.DebuggerNonUserCode]
        public Request(Request other) : this()
        {
            apiId_ = other.apiId_;
            key_ = other.key_;
            data_ = other.data_;
            _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
        }

        [System.Diagnostics.DebuggerNonUserCode]
        public Request Clone()
        {
            return new Request(this);
        }

        /// <summary>Field number for the "apiId" field.</summary>
        public const int ApiIdFieldNumber = 1;
        private int apiId_;
        /// <summary>
        ///命令ID
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCode]
        public int ApiId
        {
            get { return apiId_; }
            set
            {
                apiId_ = value;
            }
        }

        /// <summary>Field number for the "key" field.</summary>
        public const int KeyFieldNumber = 2;
        private int key_;
        /// <summary>
        ///客户端命令序号(用于标识唯一请求)
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCode]
        public int Key
        {
            get { return key_; }
            set
            {
                key_ = value;
            }
        }

        /// <summary>Field number for the "data" field.</summary>
        public const int DataFieldNumber = 3;
        private pb::ByteString data_ = pb::ByteString.Empty;
        /// <summary>
        ///命令主数据
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCode]
        public pb::ByteString Data
        {
            get { return data_; }
            set
            {
                data_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
            }
        }

        [System.Diagnostics.DebuggerNonUserCode]
        public override bool Equals(object other)
        {
            return Equals(other as Request);
        }

        [System.Diagnostics.DebuggerNonUserCode]
        public bool Equals(Request other)
        {
            if (ReferenceEquals(other, null))
            {
                return false;
            }
            if (ReferenceEquals(other, this))
            {
                return true;
            }
            if (ApiId != other.ApiId) return false;
            if (Key != other.Key) return false;
            if (Data != other.Data) return false;
            return Equals(_unknownFields, other._unknownFields);
        }

        [System.Diagnostics.DebuggerNonUserCode]
        public override int GetHashCode()
        {
            int hash = 1;
            if (ApiId != 0) hash ^= ApiId.GetHashCode();
            if (Key != 0) hash ^= Key.GetHashCode();
            if (Data.Length != 0) hash ^= Data.GetHashCode();
            if (_unknownFields != null)
            {
                hash ^= _unknownFields.GetHashCode();
            }
            return hash;
        }

        [System.Diagnostics.DebuggerNonUserCode]
        public override string ToString()
        {
            return pb::JsonFormatter.ToDiagnosticString(this);
        }

        [System.Diagnostics.DebuggerNonUserCode]
        public void WriteTo(pb::CodedOutputStream output)
        {
            if (ApiId != 0)
            {
                output.WriteRawTag(8);
                output.WriteInt32(ApiId);
            }
            if (Key != 0)
            {
                output.WriteRawTag(16);
                output.WriteInt32(Key);
            }
            if (Data.Length != 0)
            {
                output.WriteRawTag(26);
                output.WriteBytes(Data);
            }
            if (_unknownFields != null)
            {
                _unknownFields.WriteTo(output);
            }
        }

        [System.Diagnostics.DebuggerNonUserCode]
        public int CalculateSize()
        {
            int size = 0;
            if (ApiId != 0)
            {
                size += 1 + pb::CodedOutputStream.ComputeInt32Size(ApiId);
            }
            if (Key != 0)
            {
                size += 1 + pb::CodedOutputStream.ComputeInt32Size(Key);
            }
            if (Data.Length != 0)
            {
                size += 1 + pb::CodedOutputStream.ComputeBytesSize(Data);
            }
            if (_unknownFields != null)
            {
                size += _unknownFields.CalculateSize();
            }
            return size;
        }

        [System.Diagnostics.DebuggerNonUserCode]
        public void MergeFrom(Request other)
        {
            if (other == null)
            {
                return;
            }
            if (other.ApiId != 0)
            {
                ApiId = other.ApiId;
            }
            if (other.Key != 0)
            {
                Key = other.Key;
            }
            if (other.Data.Length != 0)
            {
                Data = other.Data;
            }
            _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
        }

        [System.Diagnostics.DebuggerNonUserCode]
        public void MergeFrom(pb::CodedInputStream input)
        {
            uint tag;
            while ((tag = input.ReadTag()) != 0)
            {
                switch (tag)
                {
                    default:
                        _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
                        break;
                    case 8:
                        {
                            ApiId = input.ReadInt32();
                            break;
                        }
                    case 16:
                        {
                            Key = input.ReadInt32();
                            break;
                        }
                    case 26:
                        {
                            Data = input.ReadBytes();
                            break;
                        }
                }
            }
        }

    }

    public sealed partial class Response : pb::IMessage<Response>, IProtoMessageCommunicate
    {
        private static readonly pb::MessageParser<Response> _parser = new pb::MessageParser<Response>(() => new Response());
        private pb::UnknownFieldSet _unknownFields;
        [System.Diagnostics.DebuggerNonUserCode]
        public static pb::MessageParser<Response> Parser { get { return _parser; } }

        [System.Diagnostics.DebuggerNonUserCode]
        public static pbr::MessageDescriptor Descriptor
        {
            get { return global::XOProto.XOGameMessageReflection.Descriptor.MessageTypes[2]; }
        }

        [System.Diagnostics.DebuggerNonUserCode]
        pbr::MessageDescriptor pb::IMessage.Descriptor
        {
            get { return Descriptor; }
        }

        [System.Diagnostics.DebuggerNonUserCode]
        public Response()
        {
            OnConstruction();
        }

        partial void OnConstruction();

        [System.Diagnostics.DebuggerNonUserCode]
        public Response(Response other) : this()
        {
            apiId_ = other.apiId_;
            key_ = other.key_;
            code_ = other.code_;
            data_ = other.data_;
            appendItems_ = other.appendItems_.Clone();
            _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
        }

        [System.Diagnostics.DebuggerNonUserCode]
        public Response Clone()
        {
            return new Response(this);
        }

        /// <summary>Field number for the "apiId" field.</summary>
        public const int ApiIdFieldNumber = 1;
        private int apiId_;
        /// <summary>
        ///命令ID
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCode]
        public int ApiId
        {
            get { return apiId_; }
            set
            {
                apiId_ = value;
            }
        }

        /// <summary>Field number for the "key" field.</summary>
        public const int KeyFieldNumber = 2;
        private int key_;
        /// <summary>
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCode]
        public int Key
        {
            get { return key_; }
            set
            {
                key_ = value;
            }
        }

        /// <summary>Field number for the "code" field.</summary>
        public const int CodeFieldNumber = 3;
        private int code_;
        /// <summary>
        ///错误码
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCode]
        public int Code
        {
            get { return code_; }
            set
            {
                code_ = value;
            }
        }

        /// <summary>Field number for the "data" field.</summary>
        public const int DataFieldNumber = 4;
        private pb::ByteString data_ = pb::ByteString.Empty;
        /// <summary>
        ///命令主数据
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCode]
        public pb::ByteString Data
        {
            get { return data_; }
            set
            {
                data_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
            }
        }

        /// <summary>Field number for the "appendItems" field.</summary>
        public const int AppendItemsFieldNumber = 5;
        private static readonly pb::FieldCodec<global::XOProto.AppendItem> _repeated_appendItems_codec
            = pb::FieldCodec.ForMessage(42, global::XOProto.AppendItem.Parser);
        private readonly pbc::RepeatedField<global::XOProto.AppendItem> appendItems_ = new pbc::RepeatedField<global::XOProto.AppendItem>();
        /// <summary>
        ///命令附加数据列表
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCode]
        public pbc::RepeatedField<global::XOProto.AppendItem> AppendItems
        {
            get { return appendItems_; }
        }

        [System.Diagnostics.DebuggerNonUserCode]
        public override bool Equals(object other)
        {
            return Equals(other as Response);
        }

        [System.Diagnostics.DebuggerNonUserCode]
        public bool Equals(Response other)
        {
            if (ReferenceEquals(other, null))
            {
                return false;
            }
            if (ReferenceEquals(other, this))
            {
                return true;
            }
            if (ApiId != other.ApiId) return false;
            if (Key != other.Key) return false;
            if (Code != other.Code) return false;
            if (Data != other.Data) return false;
            if (!appendItems_.Equals(other.appendItems_)) return false;
            return Equals(_unknownFields, other._unknownFields);
        }

        [System.Diagnostics.DebuggerNonUserCode]
        public override int GetHashCode()
        {
            int hash = 1;
            if (ApiId != 0) hash ^= ApiId.GetHashCode();
            if (Key != 0) hash ^= Key.GetHashCode();
            if (Code != 0) hash ^= Code.GetHashCode();
            if (Data.Length != 0) hash ^= Data.GetHashCode();
            hash ^= appendItems_.GetHashCode();
            if (_unknownFields != null)
            {
                hash ^= _unknownFields.GetHashCode();
            }
            return hash;
        }

        [System.Diagnostics.DebuggerNonUserCode]
        public override string ToString()
        {
            return pb::JsonFormatter.ToDiagnosticString(this);
        }

        [System.Diagnostics.DebuggerNonUserCode]
        public void WriteTo(pb::CodedOutputStream output)
        {
            if (ApiId != 0)
            {
                output.WriteRawTag(8);
                output.WriteInt32(ApiId);
            }
            if (Key != 0)
            {
                output.WriteRawTag(16);
                output.WriteInt32(Key);
            }
            if (Code != 0)
            {
                output.WriteRawTag(24);
                output.WriteInt32(Code);
            }
            if (Data.Length != 0)
            {
                output.WriteRawTag(34);
                output.WriteBytes(Data);
            }
            appendItems_.WriteTo(output, _repeated_appendItems_codec);
            if (_unknownFields != null)
            {
                _unknownFields.WriteTo(output);
            }
        }

        [System.Diagnostics.DebuggerNonUserCode]
        public int CalculateSize()
        {
            int size = 0;
            if (ApiId != 0)
            {
                size += 1 + pb::CodedOutputStream.ComputeInt32Size(ApiId);
            }
            if (Key != 0)
            {
                size += 1 + pb::CodedOutputStream.ComputeInt32Size(Key);
            }
            if (Code != 0)
            {
                size += 1 + pb::CodedOutputStream.ComputeInt32Size(Code);
            }
            if (Data.Length != 0)
            {
                size += 1 + pb::CodedOutputStream.ComputeBytesSize(Data);
            }
            size += appendItems_.CalculateSize(_repeated_appendItems_codec);
            if (_unknownFields != null)
            {
                size += _unknownFields.CalculateSize();
            }
            return size;
        }

        [System.Diagnostics.DebuggerNonUserCode]
        public void MergeFrom(Response other)
        {
            if (other == null)
            {
                return;
            }
            if (other.ApiId != 0)
            {
                ApiId = other.ApiId;
            }
            if (other.Key != 0)
            {
                Key = other.Key;
            }
            if (other.Code != 0)
            {
                Code = other.Code;
            }
            if (other.Data.Length != 0)
            {
                Data = other.Data;
            }
            appendItems_.Add(other.appendItems_);
            _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
        }

        [System.Diagnostics.DebuggerNonUserCode]
        public void MergeFrom(pb::CodedInputStream input)
        {
            uint tag;
            while ((tag = input.ReadTag()) != 0)
            {
                switch (tag)
                {
                    default:
                        _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
                        break;
                    case 8:
                        {
                            ApiId = input.ReadInt32();
                            break;
                        }
                    case 16:
                        {
                            Key = input.ReadInt32();
                            break;
                        }
                    case 24:
                        {
                            Code = input.ReadInt32();
                            break;
                        }
                    case 34:
                        {
                            Data = input.ReadBytes();
                            break;
                        }
                    case 42:
                        {
                            appendItems_.AddEntriesFrom(input, _repeated_appendItems_codec);
                            break;
                        }
                }
            }
        }

    }

    /// <summary>
    ///错误参数（方面后续更改数据结构）
    /// </summary>
    public sealed partial class ErrorMsg : pb::IMessage<ErrorMsg>
    {
        private static readonly pb::MessageParser<ErrorMsg> _parser = new pb::MessageParser<ErrorMsg>(() => new ErrorMsg());
        private pb::UnknownFieldSet _unknownFields;
        [System.Diagnostics.DebuggerNonUserCode]
        public static pb::MessageParser<ErrorMsg> Parser { get { return _parser; } }

        [System.Diagnostics.DebuggerNonUserCode]
        public static pbr::MessageDescriptor Descriptor
        {
            get { return global::XOProto.XOGameMessageReflection.Descriptor.MessageTypes[3]; }
        }

        [System.Diagnostics.DebuggerNonUserCode]
        pbr::MessageDescriptor pb::IMessage.Descriptor
        {
            get { return Descriptor; }
        }

        [System.Diagnostics.DebuggerNonUserCode]
        public ErrorMsg()
        {
            OnConstruction();
        }

        partial void OnConstruction();

        [System.Diagnostics.DebuggerNonUserCode]
        public ErrorMsg(ErrorMsg other) : this()
        {
            errorParams_ = other.errorParams_.Clone();
            _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
        }

        [System.Diagnostics.DebuggerNonUserCode]
        public ErrorMsg Clone()
        {
            return new ErrorMsg(this);
        }

        /// <summary>Field number for the "errorParams" field.</summary>
        public const int ErrorParamsFieldNumber = 1;
        private static readonly pb::FieldCodec<string> _repeated_errorParams_codec
            = pb::FieldCodec.ForString(10);
        private readonly pbc::RepeatedField<string> errorParams_ = new pbc::RepeatedField<string>();
        [System.Diagnostics.DebuggerNonUserCode]
        public pbc::RepeatedField<string> ErrorParams
        {
            get { return errorParams_; }
        }

        [System.Diagnostics.DebuggerNonUserCode]
        public override bool Equals(object other)
        {
            return Equals(other as ErrorMsg);
        }

        [System.Diagnostics.DebuggerNonUserCode]
        public bool Equals(ErrorMsg other)
        {
            if (ReferenceEquals(other, null))
            {
                return false;
            }
            if (ReferenceEquals(other, this))
            {
                return true;
            }
            if (!errorParams_.Equals(other.errorParams_)) return false;
            return Equals(_unknownFields, other._unknownFields);
        }

        [System.Diagnostics.DebuggerNonUserCode]
        public override int GetHashCode()
        {
            int hash = 1;
            hash ^= errorParams_.GetHashCode();
            if (_unknownFields != null)
            {
                hash ^= _unknownFields.GetHashCode();
            }
            return hash;
        }

        [System.Diagnostics.DebuggerNonUserCode]
        public override string ToString()
        {
            return pb::JsonFormatter.ToDiagnosticString(this);
        }

        [System.Diagnostics.DebuggerNonUserCode]
        public void WriteTo(pb::CodedOutputStream output)
        {
            errorParams_.WriteTo(output, _repeated_errorParams_codec);
            if (_unknownFields != null)
            {
                _unknownFields.WriteTo(output);
            }
        }

        [System.Diagnostics.DebuggerNonUserCode]
        public int CalculateSize()
        {
            int size = 0;
            size += errorParams_.CalculateSize(_repeated_errorParams_codec);
            if (_unknownFields != null)
            {
                size += _unknownFields.CalculateSize();
            }
            return size;
        }

        [System.Diagnostics.DebuggerNonUserCode]
        public void MergeFrom(ErrorMsg other)
        {
            if (other == null)
            {
                return;
            }
            errorParams_.Add(other.errorParams_);
            _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
        }

        [System.Diagnostics.DebuggerNonUserCode]
        public void MergeFrom(pb::CodedInputStream input)
        {
            uint tag;
            while ((tag = input.ReadTag()) != 0)
            {
                switch (tag)
                {
                    default:
                        _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
                        break;
                    case 10:
                        {
                            errorParams_.AddEntriesFrom(input, _repeated_errorParams_codec);
                            break;
                        }
                }
            }
        }

    }

    #endregion

}

#endregion Designer generated code
