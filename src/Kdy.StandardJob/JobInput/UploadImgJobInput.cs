namespace Kdy.StandardJob.JobInput
{
    /// <summary>
    /// 图片上传 Job 入参
    /// </summary>
    public class UploadImgJobInput
    {
        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="imgUrl">图片Url</param>
        public UploadImgJobInput(string imgUrl)
        {
            ImgUrl = imgUrl;
        }

        /// <summary>
        /// 图片Url
        /// </summary>
        public string ImgUrl { get; set; }

    }
}
