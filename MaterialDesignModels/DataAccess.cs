using System.Collections.Generic;

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
            // 国籍コンボボックス
            this.SetCountry(cd);

            // SQL生成用として生成
            var c = new Customer();
            var list = new List<Customer>();

            // DBアクセサ生成
            var sqla = new SQLiteAccessor();
            {
                // DBオープン
                sqla.Open();
                // SQL実行 １０件取得します。
                var sql = c.MakeSelectSQL(limit: 10);
                sqla.ExecuteQuery(c.QueryData);
                // データを保存
                list = c.ReadDataReader(sqla.DataReader);
                // リーダークローズ
                sqla.DataReader.Close();
            }

            var cu = list[0];
            cu.SetCommonDatas(cd);

            var i = cu.MakeInsertSQL();
            var s = cu.MakeSelectSQL();
            var d = cu.MakeDeleteSQL();
            var u = cu.MakeUpdateSQL();

            return true;
        }

        /// <summary>
        /// 共通データをDBに保存します。
        /// </summary>
        /// <param name="cd">共通データオブジェクトを設定します。</param>
        /// <returns>成功した場合trueを返します。</returns>
        public bool SaveCommonDatas(
            Common.CommonDatas cd
            )
        {
            // データ保存処理を行います。
            var customer = new Customer(cd);
            var sqla = new SQLiteAccessor();
            {
                var sql = customer.MakeUpdateSQL();
                var list = new List<Utility.QueryData>
                {
                    customer.QueryData
                };
                sqla.Open();
                sqla.ExecuteNonQuery<Customer>(list);
                sqla.Close();
            }
            return true;
        }

        private void SetCountry(Common.CommonDatas cd)
        {
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
