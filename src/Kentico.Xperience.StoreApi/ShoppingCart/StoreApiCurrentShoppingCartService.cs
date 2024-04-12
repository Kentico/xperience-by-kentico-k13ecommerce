using CMS;
using CMS.Core;
using CMS.DataEngine;
using CMS.Ecommerce;
using CMS.Helpers;
using CMS.Membership;

using Kentico.Xperience.StoreApi.Routing;
using Kentico.Xperience.StoreApi.ShoppingCart;

using Microsoft.AspNetCore.Http;

[assembly: RegisterImplementation(typeof(ICurrentShoppingCartService), typeof(StoreApiCurrentShoppingCartService))]

namespace Kentico.Xperience.StoreApi.ShoppingCart;

internal class StoreApiCurrentShoppingCartService : ICurrentShoppingCartService
{
    private readonly ICurrentShoppingCartService defaultService;
    private readonly IHttpContextAccessor httpContextAccessor;
    private readonly ICurrentShoppingCartStrategy mCartStrategy;
    private readonly IShoppingCartRepository mCartRepository;
    private readonly IShoppingCartFactory mCartFactory;
    private readonly IShoppingCartCache mCartCache;
    private readonly IConversionService conversionService;
    private const string ShoppingCartIdentifier = "ShoppingCartGUID";

    public StoreApiCurrentShoppingCartService(
        ICurrentShoppingCartService defaultService,
        IHttpContextAccessor httpContextAccessor,
        ICurrentShoppingCartStrategy cartStrategy,
        IShoppingCartRepository cartRepository,
        IShoppingCartFactory cartFactory,
        IShoppingCartCache cartCache,
        IConversionService conversionService
    )
    {
        this.defaultService = defaultService;
        this.httpContextAccessor = httpContextAccessor;
        mCartStrategy = cartStrategy;
        mCartRepository = cartRepository;
        mCartFactory = cartFactory;
        mCartCache = cartCache;
        this.conversionService = conversionService;
    }

    public ShoppingCartInfo GetCurrentShoppingCart(UserInfo user, SiteInfoIdentifier site)
    {
        if (!IsStoreApiRequest)
        {
            return defaultService.GetCurrentShoppingCart(user, site);
        }

        bool anonymize = false;
        bool evaluationIsNeeded = false;
        int objectId = site.ObjectID;
        var cart = HandleCandidateCart(GetCandidateCart(ref anonymize, ref evaluationIsNeeded),
            user, objectId, anonymize, ref evaluationIsNeeded);
        if (cart == null)
        {
            cart = CreateCart(user, objectId);
        }
        else
        {
            mCartStrategy.RefreshCart(cart);
            if (evaluationIsNeeded)
            {
                EvaluateCartSafely(cart);
            }
        }

        RememberCart(cart);
        return cart;
    }


    public void SetCurrentShoppingCart(ShoppingCartInfo cart)
    {
        if (!IsStoreApiRequest)
        {
            defaultService.SetCurrentShoppingCart(cart);
            return;
        }

        RememberCart(cart);
        ECommerceContext.InvalidateCurrentShoppingCartCache();
    }

    private bool IsStoreApiRequest =>
        httpContextAccessor.HttpContext?.Request.Path.StartsWithSegments("/" + ApiRoute.ApiPrefix) ?? false;

    /// <summary>Looks for current visitor's shopping cart candidate.</summary>
    /// <remarks>
    /// When the candidate cart is not found in cache, session is used to obtain shopping cart identifier.
    /// When the shopping cart identifier is not found in session, client storage is used to obtain it.
    /// </remarks>
    /// <param name="anonymize">Set to true if shopping cart identifier is taken from the client.</param>
    /// <param name="evaluationIsNeeded">Indicates that cart has to be evaluated afterwards.</param>
    /// <returns>Candidate cart or <c>null</c> when no candidate cart found.</returns>
    protected ShoppingCartInfo GetCandidateCart(ref bool anonymize, ref bool evaluationIsNeeded)
    {
        var cart = mCartCache.GetCart();
        if (cart != null)
        {
            return cart;
        }

        evaluationIsNeeded = true;

        //custom code starts
        var shoppingCartGuid = GetCurrentCartGuid();
        cart = shoppingCartGuid != Guid.Empty ? mCartRepository.GetCart(shoppingCartGuid) : null;
        //Don't anonymize cart because getting cart from client is only way how to get it when it isn't cached.
        //On client app (xbyK) session storage is used as primary option and cookie as secondary, so some anonymize flag in REST api
        //can be established in the future
        anonymize = false;
        //custom code ends

        return cart;
    }

    /// <summary>
    /// Examines candidate shopping cart and prepares it to be used by the visitor.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Candidate shopping <paramref name="cart" /> can be dropped, taken over by user or replaced with user older cart from the repository.
    /// </para>
    /// <para>
    /// Candidate shopping <paramref name="cart" /> is dropped if it is not usable on given <paramref name="site" />
    /// or <paramref name="user" /> can not take over the cart according to <see cref="F:CMS.Ecommerce.CurrentShoppingCartService.mCartStrategy" />.
    /// </para>
    /// </remarks>
    /// <param name="cart">Candidate shopping cart. When <c>null</c>, an existing cart from <see cref="F:CMS.Ecommerce.CurrentShoppingCartService.mCartRepository" /> is retrieved.</param>
    /// <param name="user">User in which context is the candidate cart examined.</param>
    /// <param name="site">ID or code name of the site.</param>
    /// <param name="anonymizeCart">Private data are cleared when <c>true</c>. Private data are not cleared when the cart is replaced with user's own cart.</param>
    /// <param name="evaluationIsNeeded">Indicates that cart has to be evaluated afterwards.</param>
    /// <returns>Shopping cart which is suitable for shopping by <paramref name="user" /> on <paramref name="site" />. Returns <c>null</c> when no such cart found.</returns>
    /// <seealso cref="M:CMS.Ecommerce.CurrentShoppingCartService.HandleForeignCart(CMS.Ecommerce.ShoppingCartInfo,CMS.Membership.UserInfo,CMS.DataEngine.SiteInfoIdentifier,System.Boolean,System.Boolean@)" />
    /// <seealso cref="M:CMS.Ecommerce.ICurrentShoppingCartStrategy.CartCanBeUsedOnSite(CMS.Ecommerce.ShoppingCartInfo,CMS.DataEngine.SiteInfoIdentifier)" />
    protected ShoppingCartInfo HandleCandidateCart(
        ShoppingCartInfo cart,
        UserInfo user,
        SiteInfoIdentifier site,
        bool anonymizeCart,
        ref bool evaluationIsNeeded)
    {
        if (cart != null && !mCartStrategy.CartCanBeUsedOnSite(cart, site))
        {
            cart = null;
        }

        if (cart == null)
        {
            cart = GetExistingCart(user, site);
        }
        else if (cart.ShoppingCartUserID != user.UserID)
        {
            cart = HandleForeignCart(cart, user, site, anonymizeCart, ref evaluationIsNeeded);
        }

        return cart;
    }

    /// <summary>
    /// Gets and prepares user's shopping cart from the repository.
    /// </summary>
    /// <param name="user">User for which the shopping cart is returned.</param>
    /// <param name="site">ID or code name of the site to look for cart.</param>
    /// <returns>User's shopping cart retrieved from the <see cref="F:CMS.Ecommerce.CurrentShoppingCartService.mCartRepository" /> or <c>null</c> when not found.</returns>
    protected ShoppingCartInfo GetExistingCart(UserInfo user, SiteInfoIdentifier site)
    {
        var usersCart = mCartRepository.GetUsersCart(user, site);
        EvaluateCartSafely(usersCart);
        return usersCart;
    }

    /// <summary>
    /// Handles candidate shopping cart which does not belong to <paramref name="user" />.
    /// </summary>
    /// <remarks>
    /// Candidate shopping cart is dropped when it can not be taken over by <paramref name="user" />.
    /// </remarks>
    /// <param name="cart">Candidate shopping cart not belonging to <paramref name="user" />.</param>
    /// <param name="user">User in which context is the candidate cart examined.</param>
    /// <param name="site">ID or code name of the site.</param>
    /// <param name="anonymizeCart">Private data are cleared when <c>true</c>.</param>
    /// <param name="evaluationIsNeeded">Indicates that cart has to be evaluated afterwards.</param>
    /// <returns>Shopping cart which is suitable for shopping by <paramref name="user" /> on <paramref name="site" />. Returns <c>null</c> when cart can not be taken over.</returns>
    /// <seealso cref="M:CMS.Ecommerce.ICurrentShoppingCartStrategy.UserCanTakeOverCart(CMS.Ecommerce.ShoppingCartInfo,CMS.Membership.UserInfo)" />
    /// <seealso cref="M:CMS.Ecommerce.ICurrentShoppingCartStrategy.TakeOverCart(CMS.Ecommerce.ShoppingCartInfo,CMS.Membership.UserInfo)" />
    /// <seealso cref="M:CMS.Ecommerce.ICurrentShoppingCartStrategy.AnonymizeShoppingCart(CMS.Ecommerce.ShoppingCartInfo)" />
    protected ShoppingCartInfo HandleForeignCart(
        ShoppingCartInfo cart,
        UserInfo user,
        SiteInfoIdentifier site,
        bool anonymizeCart,
        ref bool evaluationIsNeeded)
    {
        if (!mCartStrategy.UserCanTakeOverCart(cart, user))
        {
            return null;
        }

        if (!user.IsPublic())
        {
            cart = ProcessStoredCarts(cart, user, site, ref anonymizeCart);
            mCartStrategy.TakeOverCart(cart, user);
            evaluationIsNeeded = true;
        }

        if (anonymizeCart)
        {
            mCartStrategy.AnonymizeShoppingCart(cart);
            evaluationIsNeeded = true;
            mCartRepository.SetCart(cart);
        }

        return cart;
    }

    /// <summary>
    /// Handles priority of candidate shopping <paramref name="cart" /> over shopping cart already stored for <paramref name="user" /> in the <see cref="F:CMS.Ecommerce.CurrentShoppingCartService.mCartRepository" />.
    /// </summary>
    /// <param name="cart">Candidate shopping cart.</param>
    /// <param name="user">User for which is the preference examined.</param>
    /// <param name="site">Site on which is the preference examined.</param>
    /// <param name="anonymizeCart">Flag set to <c>true</c> when candidate shopping cart is used. </param>
    /// <returns>Preferred shopping cart.</returns>
    protected ShoppingCartInfo ProcessStoredCarts(
        ShoppingCartInfo cart,
        UserInfo user,
        SiteInfoIdentifier site,
        ref bool anonymizeCart)
    {
        if (mCartStrategy.PreferStoredCart(cart, user))
        {
            cart = GetExistingCart(user, site) ?? cart;
        }
        else
        {
            mCartRepository.DeleteUsersCart(user, site);
            anonymizeCart = true;
        }

        return cart;
    }

    /// <summary>
    /// Creates new cart for <paramref name="user" /> on <paramref name="site" /> using <see cref="F:CMS.Ecommerce.CurrentShoppingCartService.mCartFactory" />.
    /// </summary>
    /// <param name="user">User for who the cart is created.</param>
    /// <param name="site">ID or site codename where the cart is created.</param>
    /// <returns>Shopping cart prepared to be used by <paramref name="user" />.</returns>
    protected ShoppingCartInfo CreateCart(UserInfo user, SiteInfoIdentifier site)
    {
        var cart = mCartFactory.CreateCart(site, user);
        mCartStrategy.TakeOverCart(cart, user);
        return cart;
    }

    /// <summary>
    /// Stores given shopping cart into cache and carts identifier to client's storage.
    /// </summary>
    /// <param name="cart">Shopping cart to be remembered.</param>
    protected void RememberCart(ShoppingCartInfo cart)
    {
        //custom: session/client storage isn't used because REST API nature
        mCartCache.StoreCart(cart);
        var cartGuid = cart != null ? cart.ShoppingCartGUID : Guid.Empty;
        //save cart identifier to request stock to ensure correct cart is retrieved later (cache can be cleared)
        if (cartGuid != Guid.Empty)
        {
            RequestStockHelper.Add(ShoppingCartIdentifier, cartGuid);
        }
    }

    /// <summary>
    /// Stores shopping cart to cache temporarily and then evaluates it.
    /// This is prevention from infinite recursion of cart evaluation in case that macro using <see cref="P:CMS.Ecommerce.ECommerceContext.CurrentShoppingCart" /> is resolved during evaluation.
    /// </summary>
    private void EvaluateCartSafely(ShoppingCartInfo cart)
    {
        if (cart == null)
        {
            return;
        }

        mCartCache.StoreCart(cart);
        cart.Evaluate();
    }

    private Guid GetCurrentCartGuid()
    {
        var cartGuid = conversionService.GetGuid(RequestStockHelper.GetItem(ShoppingCartIdentifier), Guid.Empty);
        if (cartGuid == Guid.Empty)
        {
            cartGuid = GetCartGuidFromHeader();
        }
        return cartGuid;
    }

    private Guid GetCartGuidFromHeader() =>
            httpContextAccessor.HttpContext?.Request.Headers.TryGetValue(ShoppingCartIdentifier, out var scGuid) ??
            false
                ? conversionService.GetGuid(scGuid.ToString(), Guid.Empty)
                : Guid.Empty;
}
