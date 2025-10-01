using Library.Domain.Cache;
using Library.Domain.Repositories;
using Library.Server.Services;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Library.Server.Background
{
    public class ArticleEventWorker : BackgroundService
    {
        private readonly ILogger<ArticleEventWorker> _logger;
        private readonly IArticleEventRepository _articleEventRepository;
        private readonly ISectionRepository _sectionService;
        private readonly ISectionWriteService _sectionWriteService;
        private readonly ICache _cache;

        private long _lastProcessedEventId;

        public ArticleEventWorker(
            ILogger<ArticleEventWorker> logger,
            IArticleEventRepository articleEventRepository,
            ISectionRepository sectionService,
            ISectionWriteService sectionWriteService,
            ICache cache)
        {
            _logger = logger;
            _articleEventRepository = articleEventRepository;
            _sectionService = sectionService;
            _sectionWriteService = sectionWriteService;
            _cache = cache;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    const int batchSize = 100;

                    var articles = await _articleEventRepository.GetArticlesToBeIndexedAsync(batchSize, _lastProcessedEventId, cancellationToken);

                    if (!articles.Any())
                    {
                        await Task.Delay(TimeSpan.FromSeconds(5), cancellationToken);
                        continue;
                    }

                    var sections = await _sectionService.SectionUpsertBatchAsync(articles);

                    await _sectionWriteService.IndexSectionListAsync(sections);

                    _lastProcessedEventId = articles.Max(a => a.EventId);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error processing article batch");
                    await Task.Delay(TimeSpan.FromSeconds(5), cancellationToken);
                }
            }
        }
    }
}
