/***********************************

* Author    : lisonghappy
* Email     : lisonghappy@gmail.com
* Date      : 2025-04-11
* Desc      : IOCP Net Utils

************************************/


using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace IOCPNet {

    public static class IOCPUtils {

        /// <summary>
        /// Log utils
        /// </summary>
        public static class Logger {

            public enum ELogColor {
                None,
                Red,
                Green,
                Blue,
                Cyan,
                Magenta,
                Yellow,
                White,
                Gray,
                Black
            }

            public static Action<string> LogFunc;
            public static Action<string> LogWarningFunc;
            public static Action<string> LogErrorFunc;
            public static Action<ELogColor, string> LogWithColorFunc;

            #region --------------------- Log ---------------------
            public static void Log (object msg) {
                var _msg = msg.ToString();
                if (LogFunc != null) {
                    LogFunc(_msg);
                }
                else {
                    OutputLogInfo(ELogColor.None, _msg);
                }
            }

            public static void LogFormat (string format, params object[] args) {
                var _msg = string.Format(format, args);
                if (LogFunc != null) {
                    LogFunc(_msg);
                }
                else {
                    OutputLogInfo(ELogColor.None, _msg);
                }
            }
            #endregion

            #region --------------------- LogWarning ---------------------

            public static void LogWarning (object msg) {
                var _msg = msg.ToString();
                if (LogWarningFunc != null) {
                    LogWarningFunc(_msg);
                }
                else {
                    OutputLogInfo(ELogColor.Yellow, _msg);
                }
            }

            public static void LogWarningFormat (string format, params object[] args) {
                var _msg = string.Format(format, args);
                if (LogWarningFunc != null) {
                    LogWarningFunc(_msg);
                }
                else {
                    OutputLogInfo(ELogColor.Yellow, _msg);
                }
            }
            #endregion

            #region --------------------- LogError ---------------------
            public static void LogError (object msg) {
                var _msg = msg.ToString();
                if (LogErrorFunc != null) {
                    LogErrorFunc(_msg);
                }
                else {
                    OutputLogInfo(ELogColor.Red, _msg);
                }
            }
            public static void LogErrorFormat (string format, params object[] args) {
                var _msg = string.Format(format, args);
                if (LogErrorFunc != null) {
                    LogErrorFunc(_msg);
                }
                else {
                    OutputLogInfo(ELogColor.Red, _msg);
                }
            }
            #endregion

            #region --------------------- LogColor ---------------------

            public static void LogWithColor (ELogColor color, object msg) {
                var _msg = msg.ToString();
                if (LogWithColorFunc != null) {
                    LogWithColorFunc(color, _msg);
                }
                else {
                    OutputLogInfo(color, _msg);
                }
            }

            public static void LogWithColorFormat (ELogColor color, string format, params object[] args) {
                var _msg = string.Format(format, args);
                if (LogWithColorFunc != null) {
                    LogWithColorFunc(color, _msg);
                }
                else {
                    OutputLogInfo(color, _msg);
                }
            }
            #endregion


            private static void OutputLogInfo (ELogColor color, string msg) {
                StringBuilder sb = new StringBuilder();
                TimeZoneInfo currentTimeZone = TimeZoneInfo.Local;
                sb.AppendFormat(" [{0}(UTC,{1},{2})]", DateTime.UtcNow.ToString("yyyy.MM.dd.HH:mm:ss.fff"), currentTimeZone.BaseUtcOffset, currentTimeZone.Id);
                sb.AppendFormat(" ThreadID:{0} >", Thread.CurrentThread.ManagedThreadId);
                sb.Append(msg);

                var _beforeColor = Console.ForegroundColor;
                var _newColor = _beforeColor;

                switch (color) {
                    case ELogColor.Red:
                        _newColor = ConsoleColor.DarkRed;
                        break;
                    case ELogColor.Green:
                        _newColor = ConsoleColor.Green;
                        break;
                    case ELogColor.Blue:
                        _newColor = ConsoleColor.Blue;
                        break;
                    case ELogColor.Cyan:
                        _newColor = ConsoleColor.Cyan;
                        break;
                    case ELogColor.Magenta:
                        _newColor = ConsoleColor.Magenta;
                        break;
                    case ELogColor.Yellow:
                        _newColor = ConsoleColor.DarkYellow;
                        break;
                    case ELogColor.White:
                        _newColor = ConsoleColor.White;
                        break;
                    case ELogColor.Gray:
                        _newColor = ConsoleColor.Gray;
                        break;
                    case ELogColor.Black:
                        _newColor = ConsoleColor.Black;
                        break;
                    case ELogColor.None:
                    default:
                        break;
                }


                Console.ForegroundColor = _newColor;
                Console.WriteLine(sb.ToString());
                Console.ForegroundColor = _beforeColor;
            }
        }


        /// <summary>
        /// Net Messsage utils
        /// </summary>
        public static class NetMessage {

            /// <summary>
            /// 将消息数据添加上网络数据头
            /// / Add network data headers to the message data.
            /// </summary>
            /// <param name="origin"></param>
            /// <returns></returns>
            public static byte[] PackingNetMessage (byte[] origin) {
                if (origin == null) {
                    return null;
                }

                int _dateLen = origin.Length;
                byte[] _package = new byte[_dateLen + IOCPConfig.NET_PACKAGE_HEADER_SIZE];
                byte[] _header = BitConverter.GetBytes(_dateLen);
                _header.CopyTo(_package, 0);
                origin.CopyTo(_package, IOCPConfig.NET_PACKAGE_HEADER_SIZE);
                return _package;
            }

            /// <summary>
            /// 从网络字节中，拆分出数据部分
            /// From the network bytes, split out the data part. 
            /// </summary>
            /// <param name="byteList"></param>
            /// <returns></returns>
            public static byte[] SplitNetMessageBytes (ref List<byte> byteList) {
                byte[] _netBuff = null;

                if (byteList == null) {
                    return _netBuff;
                }

                if (byteList.Count > IOCPConfig.NET_PACKAGE_HEADER_SIZE) {
                    byte[] _data = byteList.ToArray();
                    int _len = BitConverter.ToInt32(_data, 0);
                    if (byteList.Count >= _len + IOCPConfig.NET_PACKAGE_HEADER_SIZE) {
                        _netBuff = new byte[_len];
                        Buffer.BlockCopy(_data, IOCPConfig.NET_PACKAGE_HEADER_SIZE, _netBuff, 0, _len);
                        byteList.RemoveRange(0, _len + IOCPConfig.NET_PACKAGE_HEADER_SIZE);
                    }
                }
                return _netBuff;
            }

        }


    }
}

