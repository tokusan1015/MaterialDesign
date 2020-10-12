using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utility
{
    /// <summary>
    /// ブーリアンユーティリティ
    /// </summary>
    [Utility.Developer(name: "tokusan1015")]
    public static class BooleanUtili
    {
        /// <summary>
        /// 初回のみフラグを表します。
        /// </summary>
        static bool _IsExecuteAtOnce = true;
        /// <summary>
        /// 初回実行フラグを表します。
        /// 初回呼び出しのみtrueを返します。
        /// </summary>
        /// <returns>trueの場合実行可能となります。</returns>
        public static bool IsExecuteAtOnce()
        {
            var result = _IsExecuteAtOnce;
            _IsExecuteAtOnce = false;
            return result;
        }
    }
}
