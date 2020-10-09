using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.Linq.Mapping;
using System.Data.SQLite;

namespace Common
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
    public class CommonDatas : Common.TableBase
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
        private int Id { get; set; }

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
        private string FirstName { get; set; }

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
        private string LastName { get; set; }

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
        private string Birthday { get; set; }

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
        private string Gender { get; set; }

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
        private string Country { get; set; }

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
        private string ZipCode { get; set; }

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
        private string Prefectures { get; set; }

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
        private string Municipality { get; set; }

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
        private string HouseNumber { get; set; }

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
        private string SavePath { get; set; }

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
        private string Etc { get; set; }
        /// <summary>
        /// 国籍一覧
        /// </summary>
        public Dictionary<string, string> CountryDic { get; } = new Dictionary<string, string>();
        #endregion プロパティ(columns)

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="cd">共通データ</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed")]
        public CommonDatas()
        {
            // QueryDataを生成します。
            this.QueryData.MakeParamater(
                tableClassInstance: this,
                bindingAttr: Common.ConstDatas.CommonDatasBindingFlags
                );
        }
        #endregion コンストラクタ

        #region データ設定
        /// <summary>
        /// CommonDatasにデータを設定します。
        /// </summary>
        /// <param name="commonDatas">CommonDatasを設定します。</param>
        public void SetCommonDatas(
            [param: Required]CommonDatas commonDatas
            )
        {
            // nullチェック
            if (commonDatas == null) throw new ArgumentNullException("commonDatas");

            this.Id = commonDatas.Id;
            this.LastName = commonDatas.LastName;
            this.FirstName = commonDatas.FirstName;
            this.Birthday = commonDatas.Birthday;
            this.Gender = commonDatas.Gender;
            this.Country = commonDatas.Country;
            this.Prefectures = commonDatas.Prefectures;
            this.Municipality = commonDatas.Municipality;
            this.HouseNumber = commonDatas.HouseNumber;
            this.ZipCode = commonDatas.ZipCode;
            this.SavePath = commonDatas.SavePath;
            this.Etc = commonDatas.Etc;
        }
        /// <summary>
        /// データリーダーからデータを抽出します。
        /// </summary>
        /// <param name="reader">データリーダーを設定します。</param>
        /// <returns>抽出したCustmerリストを返します。</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        public IEnumerable<CommonDatas> ReadDataReader(
            [param: Required]SQLiteDataReader reader
            )
        {
            // nullチェック
            if (reader == null)
                throw new ArgumentNullException("reader");

            var result = new List<CommonDatas>();
            // 先頭の１件のみを処理します。
            while (reader.Read())
            {
                var cd = new CommonDatas
                {
                    Id = Utility.StringUtil.IntdataParse(reader["Id"].ToString()),
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
                };
                result.Add(cd);
            }
            return result;
        }

        /// <summary>
        /// DataRowを用いて各プロパティ値を設定します。
        /// </summary>
        /// <param name="row">Rowデータを設定します。</param>
        public void SetValues(
            [param: Required]DataRow dataRow
            )
        {
            // nullチェック
            if (dataRow == null)
                throw new ArgumentNullException("dataRow");

            this.Id = Utility.StringUtil.IntdataParse(dataRow["Id"].ToString());
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

        #endregion データ設定

        #region ViewAdatas
        /// <summary>
        /// ViewAdataから値を受け取ります。
        /// </summary>
        /// <param name="viewAdatas">ViewAdatasを設定します。</param>
        public void SetViewDatas(
            [param: Required]Common.ViewAdatas viewAdatas
            )
        {
            // nullチェック
            if (viewAdatas == null)
                throw new ArgumentNullException("viewAdatas");

            this.Id = viewAdatas.Key;
            this.LastName = viewAdatas.LastName.Value;
            this.FirstName = viewAdatas.FirstName.Value;
            this.Birthday = viewAdatas.Birthday.Value.ToString();
            this.Gender = viewAdatas.Gender.Value.ToString();
            this.Country = viewAdatas.Country.Value;
        }

        /// <summary>
        /// ViewAdatasに値を設定します。
        /// </summary>
        /// <param name="viewAdatas">ViewAdatasを設定します。</param>
        public void GetViewDatas(
            [param: Required]Common.ViewAdatas viewAdatas
            )
        {
            // nullチェック
            if (viewAdatas == null)
                throw new ArgumentNullException("viewAdatas");

            viewAdatas.Key = this.Id;
            viewAdatas.LastName.Value = this.LastName;
            viewAdatas.FirstName.Value = this.FirstName;
            viewAdatas.Birthday.Value = Utility.StringUtil.DateTimeParse(this.Birthday);
            viewAdatas.Gender.Value = (Common.EnumDatas.Gender)Utility.EnumUtil.EnumParse(typeof(Common.EnumDatas.Gender), this.Gender);
            viewAdatas.Country.Value = this.Country;
            viewAdatas.CountryDic = this.CountryDic;
        }
        #endregion ViewAdatas

        #region ViewBdatas
        /// <summary>
        /// ViewBdataから値を受け取ります。
        /// </summary>
        /// <param name="viewDatas">ViewBdatasを設定します。</param>
        public void SetViewDatas(
            [param: Required]Common.ViewBdatas viewBdatas
            )
        {
            // nullチェック
            if (viewBdatas == null)
                throw new ArgumentNullException("viewBdatas");

            this.Id = viewBdatas.Key;
            this.Prefectures = viewBdatas.Prefectures.Value;
            this.Municipality = viewBdatas.Municipality.Value;
            this.HouseNumber = viewBdatas.HouseNumber.Value;
            this.ZipCode = viewBdatas.ZipCode.Value;
        }

        /// <summary>
        /// ViewBdatasに値を設定します。
        /// </summary>
        /// <param name="viewBdatas">ViewBdatasを設定します。</param>
        public void GetViewDatas(
            [param: Required]Common.ViewBdatas viewBdatas
            )
        {
            // nullチェック
            if (viewBdatas == null)
                throw new ArgumentNullException("viewBdatas");

            viewBdatas.Key = this.Id;
            viewBdatas.Prefectures.Value = this.Prefectures;
            viewBdatas.Municipality.Value = this.Municipality;
            viewBdatas.HouseNumber.Value = this.HouseNumber;
            viewBdatas.ZipCode.Value = this.ZipCode;
        }
        #endregion ViewBdatas

        #region ViewCdatas
        /// <summary>
        /// ViewCdataから値を受け取ります。
        /// </summary>
        /// <param name="viewCdatas">ViewCdatasを設定します。</param>
        public void SetViewDatas(
            [param: Required]Common.ViewCdatas viewCdatas
            )
        {
            // nullチェック
            if (viewCdatas == null)
                throw new ArgumentNullException("viewCdatas");

            this.Id = viewCdatas.Key;
            this.SavePath = viewCdatas.SavePath.Value;
            this.Etc = viewCdatas.Etc.Value;
        }

        /// <summary>
        /// ViewCdatasに値を設定します。
        /// </summary>
        /// <param name="viewCdatas">ViewCdatasを設定します。</param>
        public void GetViewDatas(
            [param: Required]Common.ViewCdatas viewCdatas
            )
        {
            // nullチェック
            if (viewCdatas == null)
                throw new ArgumentNullException("viewCdatas");

            viewCdatas.Key = this.Id;
            viewCdatas.SavePath.Value = this.SavePath;
            viewCdatas.Etc.Value = this.Etc;
        }
        #endregion ViewCdatas

    }
}
