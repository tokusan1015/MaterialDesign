using System.Windows.Media;
using System.ComponentModel.DataAnnotations;

namespace Utility
{
    /// <summary>
    /// カラーユーティリティ
    /// </summary>
    [Utility.Developer(name: "tokusan1015")]
    public class ColorUtil
    {
        /// <summary>
        /// 指定した色の反転色を取得します。
        /// </summary>
        /// <param name="color">指定した色を設定します。</param>
        /// <returns>反転色を返します。</returns>
        public static Color GetInvertedColor(
            [param: Required]Color color
            )
        {
            // 反転色を返します。
            return new Color()
            {
                R = (byte)(255 - color.R),
                G = (byte)(255 - color.G),
                B = (byte)(255 - color.B)
            };
        }

        /// <summary>
        /// 指定した色の補色を取得します。
        /// RGBの最小値と最大値を加算した後、
        /// 加算値から各RGBを減算します。
        /// </summary>
        /// <param name="color">指定した色を設定します。</param>
        /// <returns>補色を返します。</returns>
        public static Color GetComplementaryColor(
            [param: Required]Color color
            )
        {
            // RGBをbyte配列として取得します。
            var rgb = new byte[]
            {
                color.R,
                color.G,
                color.B,
            };
            // 最小値と最大値を取得します。
            var (min, max) = MathUtil.GetMinMax(rgb);

            // 最小値と最大値の和(p)を取得します。
            var p = (byte)(min + max);

            // 最大値と最小値の和(p)から各値(RGB)を減算して補色を返します。 
            return new Color()
            {
                R = (byte)(p - rgb[0]),
                G = (byte)(p - rgb[1]),
                B = (byte)(p - rgb[2]),
            };
        }
    }
}
