using CMS.ContentEngine;
using CMS.ContentEngine.Internal;
using CMS.Integration.K13Ecommerce;
using CMS.Membership;

using K13Store;

namespace Kentico.Xperience.K13Ecommerce.Synchronization.ContentItems;

internal class ContentItemFolderSynchronizationService : IContentItemFolderSynchronizationService
{
    private readonly IContentQueryExecutor contentQueryExecutor;
    private readonly IContentFolderManager contentFolderManager;

    public ContentItemFolderSynchronizationService(
        IContentQueryExecutor contentQueryExecutor,
        IContentFolderManagerFactory contentFolderManagerFactory)
    {
        this.contentQueryExecutor = contentQueryExecutor;
        contentFolderManager = contentFolderManagerFactory.Create(UserInfoProvider.AdministratorUser.UserID);
    }

    public async Task SynchronizeContentItemFolders(K13EcommerceSettingsInfo ecommerceSettings, CancellationToken cancellationToken = default)
    {
        string workspaceName = ecommerceSettings.K13EcommerceSettingsEffectiveWorkspaceName;

        await SynchronizeProductFolder(ecommerceSettings.K13EcommerceSettingsProductSKUFolderGuid, workspaceName, ProductSKU.CONTENT_TYPE_NAME, cancellationToken);
        await SynchronizeProductFolder(ecommerceSettings.K13EcommerceSettingsProductVariantFolderGuid, workspaceName, ProductVariant.CONTENT_TYPE_NAME, cancellationToken);
        await SynchronizeProductFolder(ecommerceSettings.K13EcommerceSettingsProductImageFolderGuid, workspaceName, ProductImage.CONTENT_TYPE_NAME, cancellationToken);
    }

    private async Task SynchronizeProductFolder(Guid targetFolderGuid, string workspaceName, string contentTypeName, CancellationToken cancellationToken = default)
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
        var root = await contentFolderManager.GetRoot(workspaceName, cancellationToken);

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
