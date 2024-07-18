using CMS.ContentEngine;

using K13Store;

namespace Kentico.Xperience.K13Ecommerce.Synchronization.ContentItems;

internal class ContentItemFolderSynchronizationService : IContentItemFolderSynchronizationService
{
    private readonly IContentFolderManager contentFolderManager;
    private readonly IContentQueryExecutor contentQueryExecutor;

    //TODO: From module
    private const int folderId = 2;


    public ContentItemFolderSynchronizationService(IContentFolderManager contentFolderManager, IContentQueryExecutor contentQueryExecutor)
    {
        this.contentFolderManager = contentFolderManager;
        this.contentQueryExecutor = contentQueryExecutor;
    }

    public async Task SynchronizeContentItemFolders(CancellationToken cancellationToken = default)
    {
        await SynchronizeProductFolder(folderId, ProductSKU.CONTENT_TYPE_NAME, cancellationToken);
        //await SynchronizeProductFolder(folderId, ProductImage.CONTENT_TYPE_NAME, cancellationToken);
        //await SynchronizeProductFolder(folderId, ProductVariant.CONTENT_TYPE_NAME, cancellationToken);
    }

    private async Task SynchronizeProductFolder(int folderId, string contentTypeName, CancellationToken cancellationToken = default)
    {
        var builder = new ContentItemQueryBuilder().ForContentType(contentTypeName);
        var ids = await contentQueryExecutor.GetResult(builder, rowData => rowData.ContentItemID, cancellationToken: cancellationToken);
        await contentFolderManager.MoveItems(folderId, ids, cancellationToken);
    }
}
