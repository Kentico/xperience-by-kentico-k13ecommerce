using CMS.ContentEngine;
using CMS.ContentEngine.Internal;
using CMS.DataEngine;
using CMS.Integration.K13Ecommerce;
using CMS.Membership;

using K13Store;

namespace Kentico.Xperience.K13Ecommerce.Synchronization.ContentItems;

internal class ContentItemFolderSynchronizationService : IContentItemFolderSynchronizationService
{
    private readonly IContentQueryExecutor contentQueryExecutor;
    private readonly IInfoProvider<K13EcommerceSettingsInfo> k13EcommerceSettingsInfoProvider;
    private readonly IContentFolderManager contentFolderManager;

    public ContentItemFolderSynchronizationService(
        IContentQueryExecutor contentQueryExecutor,
        IInfoProvider<K13EcommerceSettingsInfo> k13EcommerceSettingsInfoProvider,
        IContentFolderManagerFactory contentFolderManagerFactory)
    {
        this.contentQueryExecutor = contentQueryExecutor;
        this.k13EcommerceSettingsInfoProvider = k13EcommerceSettingsInfoProvider;
        contentFolderManager = contentFolderManagerFactory.Create(UserInfoProvider.AdministratorUser.UserID);
    }

    public async Task SynchronizeContentItemFolders(CancellationToken cancellationToken = default)
    {
        var settings = (await k13EcommerceSettingsInfoProvider.Get()
            .TopN(1)
            .GetEnumerableTypedResultAsync())
            .FirstOrDefault();

        if (settings is null)
        {
            return;
        }

        await SynchronizeProductFolder(settings.K13EcommerceSettingsProductSKUFolderGuid, ProductSKU.CONTENT_TYPE_NAME, cancellationToken);
        await SynchronizeProductFolder(settings.K13EcommerceSettingsProductVariantFolderGuid, ProductVariant.CONTENT_TYPE_NAME, cancellationToken);
        await SynchronizeProductFolder(settings.K13EcommerceSettingsProductImageFolderGuid, ProductImage.CONTENT_TYPE_NAME, cancellationToken);
    }

    private async Task SynchronizeProductFolder(Guid targetFolderGuid, string contentTypeName, CancellationToken cancellationToken = default)
    {
        if (targetFolderGuid == Guid.Empty)
        {
            return;
        }

        var folder = await contentFolderManager.Get(targetFolderGuid, cancellationToken);

        if (folder == null)
        {
            return;
        }

        int targetFolderId = folder.ContentFolderID;
        var root = await contentFolderManager.GetRoot(cancellationToken);

        if (targetFolderId == root.ContentFolderID)
        {
            return;
        }

        var builder = new ContentItemQueryBuilder().ForContentType(contentTypeName, config => config
            .Where(p => p
                .WhereEquals(nameof(ContentItemInfo.ContentItemContentFolderID), root.ContentFolderID)
            )
            .Columns(nameof(ContentItemInfo.ContentItemID))
        );
        var ids = (await contentQueryExecutor.GetResult(builder, rowData => rowData.ContentItemID, cancellationToken: cancellationToken))
            .ToList();

        if (ids.Count > 0)
        {
            await contentFolderManager.MoveItems(targetFolderId, ids, cancellationToken);
        }
    }
}
