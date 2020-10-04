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
        /// データ名
        /// </summary>
        public enum DataNames
        {
            苗字,
            名前,
            生年月日,
            性別,
            国籍,
            郵便番号,
            都道府県,
            市区町村,
            番地,
            保存パス,
            備考,
        }

        /// <summary>
        /// ボタン名
        /// </summary>
        public enum ButtonNames
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
