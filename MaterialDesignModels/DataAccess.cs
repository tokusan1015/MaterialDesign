using System.Collections.Generic;
using System.Linq;
using System;

namespace MaterialDesignModels
{
    /// <summary>
    /// データアクセスクラスを表します。
    /// TableクラスとDB間の受け渡しを行います。
    /// </summary>
    [Utility.Developer(name: "tokusan1015")]
    public class DataAccess
    {
        /// <summary>
        /// DBからCommonDatasをロードします。
        /// 共通データのKeyを設定しておく必要があります。
        /// </summary>
        /// <param name="cd">共通データオブジェクトを設定します。</param>
        /// <returns>成功した場合trueを返します。</returns>
        public bool LoadCommonDatas(
            Common.CommonDatas cd
            )
        {
            // nullチェック
            if (cd == null)
                throw new ArgumentNullException("cd");

            // 国籍コンボボックス
            this.SetCountry(cd);

            // SQL生成用として生成
            var c = new Common.CommonDatas();
            var list = new List<Common.CommonDatas>();

            // DBアクセサ生成
            using (var sqla = new SQLiteAccessor())
            {
                // DBオープン
                sqla.Open();
                // SQL実行 １０件取得します。
                c.MakeSelectSql(limit: 10);
                sqla.ExecuteQuery(c.QueryData);
                // データを保存
                list = c.ReadDataReader(sqla.DataReader).ToList();
                // リーダークローズ
                sqla.DataReader.Close();
            }

            var cu = list[0];
            cd.SetCommonDatas(cu);

            cu.MakeInsertSql();
            cu.MakeSelectSql();
            cu.MakeDeleteSql();
            cu.MakeUpdateSql();

            return true;
        }

        /// <summary>
        /// 共通データをDBに保存します。
        /// </summary>
        /// <param name="cd">共通データオブジェクトを設定します。</param>
        /// <returns>成功した場合trueを返します。</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        public bool SaveCommonDatas(
            Common.CommonDatas cd
            )
        {
            // nullチェック
            if (cd == null)
                throw new ArgumentNullException("cd");

            // データ保存処理を行います。
            using (var sqla = new SQLiteAccessor())
            {
                cd.UpdateQueryDataValue(bindingAttr: Common.ConstDatas.CommonDatasBindingFlags);
                cd.MakeUpdateSql();
                var list = new List<Utility.QueryData>
                {
                    cd.QueryData
                };
                sqla.Open();
                sqla.ExecuteNonQuery(list);
            }
            return true;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        private void SetCountry(Common.CommonDatas cd)
        {
            if (cd == null)
                throw new ArgumentNullException("cd");

            cd.CountryDic.Add("CA", "カナダ");
            cd.CountryDic.Add("DE", "ドイツ");
            cd.CountryDic.Add("FR", "フランス");
            cd.CountryDic.Add("GB", "イギリス");
            cd.CountryDic.Add("IT", "イタリア");
            cd.CountryDic.Add("JP", "日本");
            cd.CountryDic.Add("US", "アメリカ合衆国");
        }
    }
}
