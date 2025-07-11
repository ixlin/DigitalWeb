namespace DigitalSolutionsWeb.Services
{
    public class SimpleImageDownloadService : IImageDownloadService
    {
        private readonly HttpClient _httpClient;
        private readonly IWebHostEnvironment _environment;
        private readonly ILogger<SimpleImageDownloadService> _logger;
        private readonly string _stockImagesPath;

        // 预设的图片URL列表
        private readonly Dictionary<string, string> _presetImages = new()
        {
            // 首页轮播图
            { "slide1.jpg", "https://images.unsplash.com/photo-1560472354-b33ff0c44a43?w=1200&h=600&fit=crop&crop=center" },
            { "slide2.jpg", "https://images.unsplash.com/photo-1504384308090-c894fdcc538d?w=1200&h=600&fit=crop&crop=center" },
            { "slide3.jpg", "https://images.unsplash.com/photo-1581090464777-f3220bbe1b8b?w=1200&h=600&fit=crop&crop=center" },
            
            // 服务页面图片
            { "service-web.jpg", "https://images.unsplash.com/photo-1467232004584-a241de8bcf5d?w=400&h=300&fit=crop&crop=center" },
            { "service-mobile.jpg", "https://images.unsplash.com/photo-1512941937669-90a1b58e7e9c?w=400&h=300&fit=crop&crop=center" },
            { "service-cloud.jpg", "https://images.unsplash.com/photo-1451187580459-43490279c0fa?w=400&h=300&fit=crop&crop=center" },
            { "service-ai.jpg", "https://images.unsplash.com/photo-1485827404703-89b55fcc595e?w=400&h=300&fit=crop&crop=center" },
            
            // 案例研究图片
            { "case-study-1.jpg", "https://images.unsplash.com/photo-1460925895917-afdab827c52f?w=600&h=400&fit=crop&crop=center" },
            { "case-study-2.jpg", "https://images.unsplash.com/photo-1551288049-bebda4e38f71?w=600&h=400&fit=crop&crop=center" },
            { "case-study-3.jpg", "https://images.unsplash.com/photo-1519389950473-47ba0277781c?w=600&h=400&fit=crop&crop=center" },
            
            // 团队成员图片（不同的头像）
            { "team-ceo.jpg", "https://images.unsplash.com/photo-1507003211169-0a1dd7228f2d?w=300&h=300&fit=crop&crop=face" },
            { "team-cto.jpg", "https://images.unsplash.com/photo-1472099645785-5658abf4ff4e?w=300&h=300&fit=crop&crop=face" },
            { "team-designer.jpg", "https://images.unsplash.com/photo-1438761681033-6461ffad8d80?w=300&h=300&fit=crop&crop=face" },
            { "team-developer.jpg", "https://images.unsplash.com/photo-1500648767791-00dcc994a43e?w=300&h=300&fit=crop&crop=face" },
            
            // 关于我们页面图片
            { "about-office.jpg", "https://images.unsplash.com/photo-1497366216548-37526070297c?w=800&h=500&fit=crop&crop=center" },
            { "about-team.jpg", "https://images.unsplash.com/photo-1522202176988-66273c2fd55f?w=800&h=500&fit=crop&crop=center" },
            { "company-intro.jpg", "https://images.unsplash.com/photo-1486406146926-c627a92ad1ab?w=800&h=500&fit=crop&crop=center" },
            
            // 客户评价图片（首页）
            { "testimonial-1.jpg", "https://images.unsplash.com/photo-1560250097-0b93528c311a?w=150&h=150&fit=crop&crop=face" },
            { "testimonial-2.jpg", "https://images.unsplash.com/photo-1573496359142-b8d87734a5a2?w=150&h=150&fit=crop&crop=face" },
            { "testimonial-3.jpg", "https://images.unsplash.com/photo-1582750433449-648ed127bb54?w=150&h=150&fit=crop&crop=face" },
            
            // 案例页面客户证言图片
            { "client-testimonial-1.jpg", "https://images.unsplash.com/photo-1556157382-97eda2d62296?w=150&h=150&fit=crop&crop=face" },
            { "client-testimonial-2.jpg", "https://images.unsplash.com/photo-1612349317150-e413f6a5b16d?w=150&h=150&fit=crop&crop=face" }
        };

        public SimpleImageDownloadService(
            HttpClient httpClient,
            IWebHostEnvironment environment,
            ILogger<SimpleImageDownloadService> logger)
        {
            _httpClient = httpClient;
            _environment = environment;
            _logger = logger;
            _stockImagesPath = Path.Combine(_environment.WebRootPath, "images", "stock");

            // 设置User-Agent以避免反爬虫机制
            _httpClient.DefaultRequestHeaders.Add("User-Agent",
                "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_15_7) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.0.0 Safari/537.36");
        }

        public async Task<bool> DownloadImageAsync(string imageUrl, string fileName)
        {
            try
            {
                var filePath = Path.Combine(_stockImagesPath, fileName);
                
                // 如果文件已存在，跳过下载
                if (File.Exists(filePath))
                {
                    _logger.LogInformation($"Image {fileName} already exists, skipping download.");
                    return true;
                }

                // 确保目录存在
                Directory.CreateDirectory(_stockImagesPath);

                // 添加延迟以避免频繁请求
                await Task.Delay(500);

                var response = await _httpClient.GetAsync(imageUrl);
                if (response.IsSuccessStatusCode)
                {
                    var imageBytes = await response.Content.ReadAsByteArrayAsync();
                    await File.WriteAllBytesAsync(filePath, imageBytes);
                    _logger.LogInformation($"Successfully downloaded image: {fileName}");
                    return true;
                }
                else
                {
                    _logger.LogWarning($"Failed to download image {fileName}: {response.StatusCode}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error downloading image {fileName}: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DownloadPresetImagesAsync()
        {
            _logger.LogInformation("Starting preset images download...");
            
            var successCount = 0;
            var totalCount = _presetImages.Count;

            foreach (var (fileName, imageUrl) in _presetImages)
            {
                var success = await DownloadImageAsync(imageUrl, fileName);
                if (success)
                {
                    successCount++;
                }
            }

            _logger.LogInformation($"Preset images download completed: {successCount}/{totalCount} successful");
            return successCount == totalCount;
        }

        public bool ImageExists(string fileName)
        {
            var filePath = Path.Combine(_stockImagesPath, fileName);
            return File.Exists(filePath);
        }
    }
}
