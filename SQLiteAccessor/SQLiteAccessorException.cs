﻿using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace SQLiteAccessorBase
{
    #region SQLiteAccessorException
    /// <summary>
    /// SQLiteAccessorException
    /// </summary>
    [Utility.Developer(name: "tokusan1015")]
    [Serializable()]    //クラスがシリアル化可能であることを示す属性
    public class SQLiteAccessorException : Exception
    {
        /// <summary>
        /// 呼び出し元メンバー名
        /// </summary>
        public string CallerMemberName { get; private set; }

        /// <summary>
        /// SQLiteAccessorException
        /// </summary>
        /// <param name="callerMemberName">呼び出し元メンバー名</param>
        public SQLiteAccessorException(
            [System.Runtime.CompilerServices.CallerMemberName] string callerMemberName = ""
            ) : base()
        {
            this.CallerMemberName = callerMemberName;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="message">メッセージを設定します。</param>
        /// <param name="callerMemberName">呼び出し元メンバー名</param>
        public SQLiteAccessorException(
            string message,
            [System.Runtime.CompilerServices.CallerMemberName] string callerMemberName = ""
            ) : base(message)
        {
            this.CallerMemberName = callerMemberName;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="message">メッセージを設定します。</param>
        /// <param name="innerException">内包例外を設定します。</param>
        /// <param name="callerMemberName">呼び出し元メンバー名</param>
        public SQLiteAccessorException(
            string message,
            Exception innerException,
            [System.Runtime.CompilerServices.CallerMemberName] string callerMemberName = ""
            ) : base(message, innerException)
        {
            this.CallerMemberName = callerMemberName;
        }

        /// <summary>
        /// コンストラクタ
        /// 逆シリアル化コンストラクタ。
        /// このクラスの逆シリアル化のために必須。
        /// アクセス修飾子をpublicにしないこと！（詳細は後述）
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        /// <param name="callerMemberName">呼び出し元メンバー名</param>
        protected SQLiteAccessorException(
            SerializationInfo info,
            StreamingContext context,
            [System.Runtime.CompilerServices.CallerMemberName] string callerMemberName = ""
            ) : base(info, context)
        {
            this.CallerMemberName = callerMemberName;
        }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
                throw new ArgumentNullException("info");

            info.AddValue("CallerMemberName", this.CallerMemberName);

            base.GetObjectData(info, context);
        }
    }
    #endregion SQLiteAccessorException
}
