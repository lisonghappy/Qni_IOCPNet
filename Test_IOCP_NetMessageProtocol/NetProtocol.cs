﻿// <auto-generated>
//   This file was generated by a tool; you should avoid making direct changes.
//   Consider using 'partial classes' to extend these types
//   Input: my.proto
// </auto-generated>

#region Designer generated code
#pragma warning disable CS0612, CS0618, CS1591, CS3021, IDE0079, IDE1006, RCS1036, RCS1057, RCS1085, RCS1192
namespace NetProtocol {

    [global::ProtoBuf.ProtoContract()]
    public partial class NetMessage : global::ProtoBuf.IExtensible {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject (bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(1, Name = @"header", IsRequired = true)]
        public NetMessageHeader Header {
            get; set;
        }

        [global::ProtoBuf.ProtoMember(2, Name = @"body")]
        public NetMessageBody Body {
            get; set;
        }

    }

    [global::ProtoBuf.ProtoContract()]
    public partial class NetMessageHeader : global::ProtoBuf.IExtensible {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject (bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(1, Name = @"cmd", IsRequired = true)]
        public Cmd Cmd {
            get; set;
        }

        [global::ProtoBuf.ProtoMember(2, Name = @"seq", IsRequired = true)]
        public int Seq {
            get; set;
        }

        [global::ProtoBuf.ProtoMember(3)]
        public int errorCode {
            get => __pbn__errorCode.GetValueOrDefault();
            set => __pbn__errorCode = value;
        }
        public bool ShouldSerializeerrorCode () => __pbn__errorCode != null;
        public void ReseterrorCode () => __pbn__errorCode = null;
        private int? __pbn__errorCode;

    }

    [global::ProtoBuf.ProtoContract()]
    public partial class NetMessageBody : global::ProtoBuf.IExtensible {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject (bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(1)]
        public NetMessageRequestLogin requestLogin {
            get; set;
        }

        [global::ProtoBuf.ProtoMember(2)]
        public NetMessageResponseLogin responseLogin {
            get; set;
        }

        [global::ProtoBuf.ProtoMember(3)]
        public NetMessageRequestBagInfo requestBagInfo {
            get; set;
        }

        [global::ProtoBuf.ProtoMember(4)]
        public NetMessageResponseBagInfo responseBagInfo {
            get; set;
        }

    }

    [global::ProtoBuf.ProtoContract()]
    public partial class NetMessageRequestLogin : global::ProtoBuf.IExtensible {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject (bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(1, Name = @"username", IsRequired = true)]
        public string Username {
            get; set;
        }

        [global::ProtoBuf.ProtoMember(2, Name = @"password", IsRequired = true)]
        public string Password {
            get; set;
        }

    }

    [global::ProtoBuf.ProtoContract()]
    public partial class NetMessageResponseLogin : global::ProtoBuf.IExtensible {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject (bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(1, Name = @"success", IsRequired = true)]
        public bool Success {
            get; set;
        }

        [global::ProtoBuf.ProtoMember(2, Name = @"message")]
        [global::System.ComponentModel.DefaultValue("")]
        public string Message {
            get => __pbn__Message ?? "";
            set => __pbn__Message = value;
        }
        public bool ShouldSerializeMessage () => __pbn__Message != null;
        public void ResetMessage () => __pbn__Message = null;
        private string __pbn__Message;

    }

    [global::ProtoBuf.ProtoContract()]
    public partial class NetMessageRequestBagInfo : global::ProtoBuf.IExtensible {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject (bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(1, IsRequired = true)]
        public int userId {
            get; set;
        }

    }

    [global::ProtoBuf.ProtoContract()]
    public partial class NetMessageResponseBagInfo : global::ProtoBuf.IExtensible {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject (bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(1, Name = @"items")]
        public global::System.Collections.Generic.List<NetMessageBagInfoItem> Items { get; set; } = new global::System.Collections.Generic.List<NetMessageBagInfoItem>();

    }

    [global::ProtoBuf.ProtoContract()]
    public partial class NetMessageBagInfoItem : global::ProtoBuf.IExtensible {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject (bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(1, Name = @"id", IsRequired = true)]
        public int Id {
            get; set;
        }

        [global::ProtoBuf.ProtoMember(2, Name = @"name", IsRequired = true)]
        public string Name {
            get; set;
        }

        [global::ProtoBuf.ProtoMember(3, Name = @"quantity", IsRequired = true)]
        public int Quantity {
            get; set;
        }

    }

    [global::ProtoBuf.ProtoContract(Name = @"CMD")]
    public enum Cmd {
        [global::ProtoBuf.ProtoEnum(Name = @"NONE")]
        None = 0,
        Login = 1,
        BagInfo = 2,
    }

}

#pragma warning restore CS0612, CS0618, CS1591, CS3021, IDE0079, IDE1006, RCS1036, RCS1057, RCS1085, RCS1192
#endregion
