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
                // SQL文は自動生成しています。
                var s10 = c.MakeSelectSql(limit: 10);
                sqla.ExecuteQuery(c.QueryData);
                // データを保存
                list = c.ReadDataReader(sqla.DataReader).ToList();
                // リーダークローズ
                sqla.DataReader.Close();
            }

            // １件目を渡しています。
            var cu = list[0];
            commonDatas.SetCommonDatas(cu);

            // 自動生成されたSQL文を取得します。
            // データはパラメータで渡しています。
            var i = cu.MakeInsertSql();
            var s = cu.MakeSelectSql();
            var u = cu.MakeUpdateSql();
            var d = cu.MakeDeleteSql();

            return true;
        }

        /// <summary>
        /// 共通データをDBに保存します。
        /// </summary>
        /// <param name="commonDatas">共通データオブジェクトを設定します。</param>
        /// <returns>成功した場合trueを返します。</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        public bool SaveCommonDatas(
            Common.TableBase tableBase
            )
        {
            // nullチェック
            if (tableBase == null)
                throw new ArgumentNullException(MethodBase.GetCurrentMethod().Name + " : " +nameof(tableBase));

            // データ保存処理を行います。
            using (var sqla = new SQLiteAccessor())
            {
                tableBase.UpdateQueryDataValue(bindingAttr: Common.ConstDatas.CommonDatasBindingFlags);
                tableBase.MakeUpdateSql();
                var list = new List<Utility.QueryData>
                {
                    tableBase.QueryData
                };
                sqla.Open();
                sqla.ExecuteNonQuery(list);
            }
            return true;
        }

        /// <summary>
        /// Comboboxに設定(ItemSource)する国籍一覧を設定します。
        /// 今回はDBから取得していません。
        /// </summary>
        /// <param name="commonDatas">共通データを設定します。</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        private void SetCountry(Common.CommonDatas commonDatas)
        {
            // nullチェック
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
