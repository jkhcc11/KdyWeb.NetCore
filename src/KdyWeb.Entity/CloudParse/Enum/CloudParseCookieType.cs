namespace KdyWeb.Entity.CloudParse.Enum
{
    /// <summary>
    /// Cookie类型
    /// </summary>
    public enum CloudParseCookieType
    {
        /// <summary>
        /// 国际版Onedrive
        /// </summary>
        OneDriveGj = 50,

        /// <summary>
        ///国内版Onedrive
        /// </summary>
        OneDriveGn = 51,

        #region 天翼
        /// <summary>
        /// 天翼云
        /// </summary>
        /// <remarks>
        ///  子账号获取列表
        /// </remarks>
        TyNormal = 100,

        /// <summary>
        /// 天翼云服务器版
        /// </summary>
        /// <remarks>
        ///  独立服务器Cookie解析
        /// </remarks>
        TyNormalProxy = 101,

        /// <summary>
        /// 天翼云H5版本
        /// </summary>
        /// <remarks>
        ///  独立服务器Cookie解析
        /// </remarks>
        TyH5 = 102,

        /// <summary>
        /// 天翼企业
        /// </summary>
        /// <remarks>
        ///  独立服务器Cookie解析
        /// </remarks>
        TyQy = 103,

        /// <summary>
        /// 天翼企业Web
        /// </summary>
        /// <remarks>
        ///  子账号获取列表
        /// </remarks>
        TyQyWeb = 104,

        /// <summary>
        /// 家庭云Web
        /// </summary>
        /// <remarks>
        ///  子账号获取列表
        /// </remarks>
        TyFamilyWeb = 105,
        #endregion

        /// <summary>
        /// 百度网盘
        /// </summary>
        BaiDu = 150,

        /// <summary>
        /// 超星网盘
        /// </summary>
        ChaoXing = 160,

        /// <summary>
        /// 115
        /// </summary>
        OneOneFive = 170,

        /// <summary>
        /// 36网盘
        /// </summary>
        ShuZi360 = 180,

        /// <summary>
        /// 360企业
        /// </summary>
        ShuZiQy360 = 181,

        /// <summary>
        /// 胜天网盘
        /// </summary>
        /// <remarks>
        /// bitqiu
        /// </remarks>
        St = 190,

        /// <summary>
        /// 阿里个人云
        /// </summary>
        AliDrive = 195
    }
}
