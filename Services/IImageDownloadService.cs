namespace DigitalSolutionsWeb.Services
{
    public interface IImageDownloadService
    {
        /// <summary>
        /// 从指定URL下载图片到本地目录
        /// </summary>
        /// <param name="imageUrl">图片URL</param>
        /// <param name="fileName">保存的文件名</param>
        /// <returns>成功返回true，失败返回false</returns>
        Task<bool> DownloadImageAsync(string imageUrl, string fileName);

        /// <summary>
        /// 批量下载预设的图片
        /// </summary>
        /// <returns></returns>
        Task<bool> DownloadPresetImagesAsync();

        /// <summary>
        /// 检查本地图片是否存在
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <returns></returns>
        bool ImageExists(string fileName);
    }
}
