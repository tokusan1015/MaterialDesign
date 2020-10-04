using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Linq.Mapping;
using System.Data.SQLite;

namespace MaterialDesignModels
{
    /// <summary>
    /// Customerテーブル構造定義クラスを表します。
    /// 整数型は DbType = "INT" とします。INTEGER ではエラーになります。
    /// 文字列型は DbType = "NVARCHAR" とします。TEXT ではエラーになります。
    /// NULLを許可する場合はCanBeNull = trueとします。
    /// SQLiteでは、AUTOINCREMENTのバグでSQLiteToLinqのInsertは正常に機能しません。
    /// その為LinkToSQLを使用する場合のInsertは、SQLを投げる形で対応します。
    /// </summary>
    [Utility.Developer("tokusan1015")] // 開発者名
    [Table(Name = "Customers")] // テーブル名
    public class Customer : SQLiteAccessorBase.TableBase<Customer>
    {
        #region プロパティ(columns)
        /// <summary>
        /// Id
        /// </summary>
        [Column(
            Name = "Id",
            DbType = "INT",
            IsPrimaryKey = true,
            CanBeNull = false
            //IsDbGenerated = true
            )]
        [DisplayName("Id")]
        [Browsable(false)]
        public int Id { get; set; }

        /// <summary>
        /// 苗字
        /// </summary>
        [Column(
            Name = "FirstName",
            DbType = "NVARCHAR",
            CanBeNull = false
            )]
        [DisplayName("苗字")]
        [Browsable(true)]
        public string FirstName { get; set; }

        /// <summary>
        /// 名前
        /// </summary>
        [Column(
            Name = "LastName",
            DbType = "NVARCHAR",
            CanBeNull = false
            )]
        [DisplayName("名前")]
        [Browsable(true)]
        public string LastName { get; set; }

        /// <summary>
        /// 誕生日
        /// </summary>
        [Column(
            Name = "Birthday",
            DbType = "NVARCHAR",
            CanBeNull = false
            )]
        [DisplayName("誕生日")]
        [Browsable(true)]
        public string Birthday { get; set; }

        /// <summary>
        /// 性別
        /// </summary>
        [Column(
            Name = "Gender",
            DbType = "NVARCHAR",
            CanBeNull = false
            )]
        [DisplayName("性別")]
        [Browsable(true)]
        public string Gender { get; set; }

        /// <summary>
        /// 国籍
        /// </summary>
        [Column(
            Name = "Country",
            DbType = "NVARCHAR",
            CanBeNull = false
            )]
        [DisplayName("国籍")]
        [Browsable(true)]
        public string Country { get; set; }

        /// <summary>
        /// 郵便番号
        /// </summary>
        [Column(
            Name = "ZipCode",
            DbType = "NVARCHAR",
            CanBeNull = false
            )]
        [DisplayName("郵便番号")]
        [Browsable(true)]
        public string ZipCode { get; set; }

        /// <summary>
        /// 都道府県
        /// </summary>
        [Column(
            Name = "Prefectures",
            DbType = "NVARCHAR",
            CanBeNull = false
            )]
        [DisplayName("都道府県")]
        [Browsable(true)]
        public string Prefectures { get; set; }

        /// <summary>
        /// 市区町村
        /// </summary>
        [Column(
            Name = "Municipality",
            DbType = "NVARCHAR",
            CanBeNull = false
            )]
        [DisplayName("市区町村")]
        [Browsable(true)]
        public string Municipality { get; set; }

        /// <summary>
        /// 番地
        /// </summary>
        [Column(
            Name = "HouseNumber",
            DbType = "NVARCHAR",
            CanBeNull = false
            )]
        [DisplayName("番地")]
        [Browsable(true)]
        public string HouseNumber { get; set; }

        /// <summary>
        /// 保存パス
        /// </summary>
        [Column(
            Name = "SavePath",
            DbType = "NVARCHAR",
            CanBeNull = false
            )]
        [DisplayName("保存パス")]
        [Browsable(true)]
        public string SavePath { get; set; }

        /// <summary>
        /// 備考
        /// </summary>
        [Column(
            Name = "Etc",
            DbType = "NVARCHAR",
            CanBeNull = true
            )]
        [DisplayName("備考")]
        [Browsable(true)]
        public string Etc { get; set; }
        #endregion プロパティ(columns)

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="cd">共通データ</param>
        public Customer(
            Common.CommonDatas cd = null
            )
        {
            // 共通データが設定されていた場合
            if (cd != null)
                // 共通データからデータを受け取ります。
                this.GetCommonDatas(cd);

            // QueryDataを生成します。
            this.QueryData.MakeParamater<Customer>(this);
        }
        #endregion コンストラクタ

        #region データ設定
        /// <summary>
        /// データリーダーからデータを抽出します。
        /// </summary>
        /// <param name="reader">データリーダーを設定します。</param>
        /// <returns>抽出したCustmerリストを返します。</returns>
        public List<Customer> ReadDataReader(SQLiteDataReader reader)
        {
            var result = new List<Customer>();
            // 先頭の１件のみを処理します。
            while (reader.Read())
            {
                result.Add(new Customer()
                {
                    Id = Utility.StringUtil.IntParse(reader["Id"].ToString()),
                    LastName = reader["LastName"].ToString(),
                    FirstName = reader["FirstName"].ToString(),
                    Birthday = reader["Birthday"].ToString(),
                    Gender = reader["Gender"].ToString(),
                    Country = reader["Country"].ToString(),
                    Prefectures = reader["Prefectures"].ToString(),
                    Municipality = reader["Municipality"].ToString(),
                    HouseNumber = reader["HouseNumber"].ToString(),
                    ZipCode = reader["ZipCode"].ToString(),
                    SavePath = reader["SavePath"].ToString(),
                    Etc = reader["Etc"].ToString()
                });
            }
            return result;
        }

        /// <summary>
        /// DataRowを用いて各プロパティ値を設定します。
        /// </summary>
        /// <param name="row">Rowデータを設定します。</param>
        public void SetValues(DataRow dataRow)
        {
            this.Id = Utility.StringUtil.IntParse(dataRow["Id"].ToString());
            this.LastName = dataRow["LastName"].ToString();
            this.FirstName = dataRow["FirstName"].ToString();
            this.Birthday = dataRow["Birthday"].ToString();
            this.Gender = dataRow["Gender"].ToString();
            this.Country = dataRow["Country"].ToString();
            this.Prefectures = dataRow["Prefectures"].ToString();
            this.Municipality = dataRow["Municipality"].ToString();
            this.HouseNumber = dataRow["HouseNumber"].ToString();
            this.ZipCode = dataRow["ZipCode"].ToString();
            this.SavePath = dataRow["SavePath"].ToString();
            this.Etc = dataRow["Etc"].ToString();
        }

        /// <summary>
        /// 共通データから値を受け取ります。
        /// </summary>
        /// <param name="commonDatas">共通データを設定します。</param>
        public void GetCommonDatas(
            Common.CommonDatas commonDatas
            )
        {
            this.Id = commonDatas.Key;
            this.LastName = commonDatas.LastName.Value;
            this.FirstName = commonDatas.FirstName.Value;
            this.Birthday = commonDatas.Birthday.Value.ToString();
            this.Gender = commonDatas.Gender.Value.ToString();
            this.Country = commonDatas.Country.Value;
            this.Prefectures = commonDatas.Prefectures.Value;
            this.Municipality = commonDatas.Municipality.Value;
            this.HouseNumber = commonDatas.HouseNumber.Value;
            this.ZipCode = commonDatas.ZipCode.Value;
            this.SavePath = commonDatas.SavePath.Value;
            this.Etc = commonDatas.Etc.Value;
        }

        /// <summary>
        /// 共通データに値を設定します。
        /// </summary>
        /// <param name="commonDatas">共通データを設定します。</param>
        public void SetCommonDatas(
            Common.CommonDatas commonDatas
            )
        {
            commonDatas.Key = this.Id;
            commonDatas.LastName.Value = this.LastName;
            commonDatas.FirstName.Value = this.FirstName;
            commonDatas.Birthday.Value = Utility.StringUtil.DateTimeParse(this.Birthday);
            commonDatas.Gender.Value = (Common.EnumDatas.Gender)Utility.EnumUtil.EnumParse(typeof(Common.EnumDatas.Gender), this.Gender);
            commonDatas.Country.Value = this.Country;
            commonDatas.Prefectures.Value = this.Prefectures;
            commonDatas.Municipality.Value = this.Municipality;
            commonDatas.HouseNumber.Value = this.HouseNumber;
            commonDatas.ZipCode.Value = this.ZipCode;
            commonDatas.SavePath.Value = this.SavePath;
            commonDatas.Etc.Value = this.Etc;
        }
        #endregion データ設定
    }
}
