using AutoMapper;

using CMS.Ecommerce;
using CMS.Helpers;

using Kentico.Xperience.StoreApi.Currencies;
using Kentico.Xperience.StoreApi.Customers;
using Kentico.Xperience.StoreApi.Orders;
using Kentico.Xperience.StoreApi.Products.Prices;
using Kentico.Xperience.StoreApi.Products.SKU;
using Kentico.Xperience.StoreApi.ShoppingCart;

namespace Kentico.Xperience.StoreApi.Mapping;

public class StoreApiMappingProfile : Profile
{
    public StoreApiMappingProfile()
    {
        CreateMap<CurrencyInfo, KCurrency>();
        CreateMap<ProductCatalogPrices, KProductCatalogPrices>();
        CreateMap<SKUInfo, KProductVariant>();
        CreateMap<SKUInfo, KProductSKU>()
            .ForMember(d => d.PublicStatus, m => m.MapFrom(s => PublicStatusInfo.Provider.Get(s.SKUPublicStatusID)))
            .ForMember(d => d.Department, m => m.MapFrom(s => DepartmentInfo.Provider.Get(s.SKUDepartmentID)))
            .ForMember(d => d.Manufacturer, m => m.MapFrom(s => ManufacturerInfo.Provider.Get(s.SKUManufacturerID)));
        CreateMap<ManufacturerInfo, KManufacturer>();
        CreateMap<PublicStatusInfo, KPublicStatus>();
        CreateMap<DepartmentInfo, KDepartment>();

        CreateMap<ShoppingCartInfo, KShoppingCartContent>()
            .ForMember(c => c.CouponCodes, m => m.MapFrom(c => c.CouponCodes.Codes))
            .ForMember(c => c.RemainingAmountForFreeShipping, m => m.MapFrom(c => c.CalculateRemainingAmountForFreeShipping()));

        CreateMap<ShoppingCartInfo, KShoppingCartDetails>()
            .ForMember(c => c.BillingAddress, m => m.MapFrom(c => c.ShoppingCartBillingAddress))
            .ForMember(c => c.ShippingAddress, m => m.MapFrom(c => c.ShoppingCartShippingAddress))
            .ForMember(c => c.Note, m => m.MapFrom(c => c.ShoppingCartNote))
            .ForMember(c => c.ShippingOptionId, m => m.MapFrom(c => c.ShoppingCartShippingOptionID))
            .ForMember(c => c.PaymentOptionId, m => m.MapFrom(c => c.ShoppingCartPaymentOptionID))
            .ForMember(c => c.CustomData, m => m.MapFrom(c => c.ShoppingCartCustomData));

        CreateMap<CustomerInfo, KCustomer>().ReverseMap();

        CreateMap<ShoppingCartItemInfo, KShoppingCartItem>()
            .ForMember(c => c.ProductSKU, m => m.MapFrom(c => c.SKU.IsProductVariant ? c.VariantParent : c.SKU))
            .ForMember(c => c.VariantSKU, m => m.MapFrom(c => c.SKU.IsProductVariant ? c.SKU : null));

        CreateMap<AddressInfo, KAddress>().ReverseMap();
        CreateMap<ShippingOptionInfo, KShippingOption>();
        CreateMap<PaymentOptionInfo, KPaymentOption>();

        CreateMap<OrderInfo, KOrder>()
            .AfterMap((source, dest, ctx) =>
            {
                dest.OrderCurrencyCode = CurrencyInfoProvider.ProviderObject.Get(source.OrderCurrencyID)?.CurrencyCode;
                dest.OrderCustomer =
                    ctx.Mapper.Map<KCustomer>(CustomerInfoProvider.ProviderObject.Get(source.OrderCustomerID));
                dest.OrderShippingOption =
                    ctx.Mapper.Map<KShippingOption>(ShippingOptionInfo.Provider.Get(source.OrderShippingOptionID));
                dest.OrderPaymentOption = ctx.Mapper.Map<KPaymentOption>(PaymentOptionInfo.Provider.Get(source.OrderPaymentOptionID));
                dest.OrderItems = ctx.Mapper.Map<IEnumerable<KOrderItem>>(OrderItemInfoProvider.GetOrderItems(source.OrderID));
                dest.OrderStatus = ctx.Mapper.Map<KOrderStatus>(OrderStatusInfoProvider.ProviderObject.Get(source.OrderStatusID));
            });

        CreateMap<OrderItemInfo, KOrderItem>();
        CreateMap<OrderAddressInfo, KAddress>();
        CreateMap<OrderStatusInfo, KOrderStatus>();
        CreateMap<ContainerCustomData, Dictionary<string, object>>().ConvertUsing<ContainerCustomDataToDictionaryConverter>();
        CreateMap<SummaryItem, KSummaryItem>();
    }
}
