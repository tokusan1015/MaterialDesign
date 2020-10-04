using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;

namespace Utility
{
    /// <summary>
    /// 数学ユーティリティ
    /// </summary>
    [Utility.Developer(name: "tokusan1015")]
    public class MathUtil
    {
        /// <summary>
        /// 数値型の配列から最小値と最大値を取得する
        /// </summary>
        /// <typeparam name="T">型</typeparam>
        /// <param name="ts">配列</param>
        /// <returns>(最小値, 最大値)</returns>
        public static (T min, T max) GetMinMax<T>(
            [param: Required]IEnumerable<T> ts
            ) where T : struct
        {
            return (ts.Min(), ts.Max());
        }
    }
}
