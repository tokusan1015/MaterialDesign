namespace Common
{
    /// <summary>
    /// 列挙型を表します。
    /// </summary>
    [Utility.Developer(name: "tokusan1015")]
    public static class EnumDatas
    {
        /// <summary>
        /// View名
        /// </summary>
        public enum ViewNames
        {
            /// <summary>
            /// メインウィンドウ
            /// </summary>
            Main,
            /// <summary>
            /// 氏名
            /// </summary>
            ViewA,
            /// <summary>
            /// 住所
            /// </summary>
            ViewB,
            /// <summary>
            /// 設定
            /// </summary>
            ViewC,
            /// <summary>
            /// 一覧
            /// </summary>
            ViewD,
        }

        /// <summary>
        /// ViewTitle
        /// </summary>
        public enum ViewTitle
        {
            氏名,
            住所,
            設定,
            一覧,
            反転,
            終了,
        }

        /// <summary>
        /// 性別
        /// </summary>
        public enum Gender
        {
            男性,
            女性,
            不明,
        }
    }
}
