import { type Locator, type Page } from "@playwright/test";
import { BasePage } from "./BasePage";

export class CheckoutPage extends BasePage {
  readonly $firstName: Locator;
  readonly $lastName: Locator;
  readonly $email: Locator;
  readonly $phone: Locator;
  readonly $address: Locator;
  readonly $address2: Locator;
  readonly $city: Locator;
  readonly $psc: Locator;
  readonly $country: Locator;
  readonly $deliveryMethod: Locator;
  readonly $reviewOrderBtn: Locator;
  readonly $payment: Locator;
  readonly $confirmOrderBtn: Locator;

  constructor(page: Page) {
    super(page);

    this.$firstName = page.locator("#Customer_FirstName");
    this.$lastName = page.locator("#Customer_LastName");
    this.$email = page.locator("#Customer_Email");
    this.$phone = page.locator("#Customer_PhoneNumber");
    this.$address = page.locator("#BillingAddress_BillingAddressLine1");
    this.$address2 = page.locator("#BillingAddress_BillingAddressLine2");
    this.$city = page.locator("#BillingAddress_BillingAddressCity");
    this.$psc = page.locator("#BillingAddress_BillingAddressPostalCode");
    this.$country = page.locator("#BillingAddress_BillingAddressCountryStateSelector_CountryID");
    this.$deliveryMethod = page.locator("#ShippingOption_ShippingOptionID");
    this.$reviewOrderBtn = page.getByTestId("reviewOrder");
    this.$payment = page.locator("#PaymentMethod_PaymentMethodID");
    this.$confirmOrderBtn = page.getByTestId("confirmOrder");
  }

  async fillDetails(details) {
    await this.$firstName.fill(details.firstName);
    await this.$lastName.fill(details.lastName);
    await this.$email.fill(details.email);
    await this.$phone.fill(details.phone);
    await this.$address.fill(details.address);
    await this.$address2.fill(details.address2);
    await this.$city.fill(details.city);
    await this.$psc.fill(details.psc);
    await this.$country.selectOption({ label: details.country });

    const deliveryToSelect = await this.$deliveryMethod
      .locator("option", { hasText: details.deliveryMethod })
      .innerText();
    await this.$deliveryMethod.selectOption({ label: deliveryToSelect });
  }
  async goToSummary() {
    await this.$reviewOrderBtn.click();
  }
  async selectPayment(payment) {
    await this.$payment.selectOption({ label: payment });
  }
  async confirmOrder() {
    await this.$confirmOrderBtn.click();
  }
}
