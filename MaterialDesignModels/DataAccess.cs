using System.Collections.Generic;
using System.Linq;
using System;
using System.Reflection;

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
        /// <param name="commonDatas">共通データオブジェクトを設定します。</param>
        /// <returns>成功した場合trueを返します。</returns>
        public bool LoadCommonDatas(
            Common.CommonDatas commonDatas
            )
        {
            // nullチェック
            if (commonDatas == null)
                throw new ArgumentNullException(MethodBase.GetCurrentMethod().Name + " : " +nameof(commonDatas));

            // 国籍コンボボックス
            this.SetCountry(commonDatas);

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
            commonDatas.SetCommonDatas(cu);

            cu.MakeInsertSql();
            cu.MakeSelectSql();
            cu.MakeDeleteSql();
            cu.MakeUpdateSql();

            return true;
        }

        /// <summary>
        /// 共通データをDBに保存します。
        /// </summary>
        /// <param name="commonDatas">共通データオブジェクトを設定します。</param>
        /// <returns>成功した場合trueを返します。</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        public bool SaveCommonDatas(
            Common.CommonDatas commonDatas
            )
        {
            // nullチェック
            if (commonDatas == null)
                throw new ArgumentNullException(MethodBase.GetCurrentMethod().Name + " : " +nameof(commonDatas));

            // データ保存処理を行います。
            using (var sqla = new SQLiteAccessor())
            {
                commonDatas.UpdateQueryDataValue(bindingAttr: Common.ConstDatas.CommonDatasBindingFlags);
                commonDatas.MakeUpdateSql();
                var list = new List<Utility.QueryData>
                {
                    commonDatas.QueryData
                };
                sqla.Open();
                sqla.ExecuteNonQuery(list);
            }
            return true;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        private void SetCountry(Common.CommonDatas commonDatas)
        {
            if (commonDatas == null)
                throw new ArgumentNullException(MethodBase.GetCurrentMethod().Name + " : " +nameof(commonDatas));

            commonDatas.CountryDic.Add("CA", "カナダ");
            commonDatas.CountryDic.Add("DE", "ドイツ");
            commonDatas.CountryDic.Add("FR", "フランス");
            commonDatas.CountryDic.Add("GB", "イギリス");
            commonDatas.CountryDic.Add("IT", "イタリア");
            commonDatas.CountryDic.Add("JP", "日本");
            commonDatas.CountryDic.Add("US", "アメリカ合衆国");
        }
    }
}
