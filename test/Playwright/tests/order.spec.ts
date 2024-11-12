import { test, expect, Locator } from "@playwright/test";
import { CheckoutPage } from "../pageObjects/CheckoutPage";
import { BasePage } from "../pageObjects/BasePage";

test.describe("Orders", () => {
  let basePage: BasePage;

  test.beforeEach(async ({ page }) => {
    basePage = new BasePage(page);
    await basePage.goto("/store");
  });

  test(`Complete order with updated quantity`, async ({ page }) => {
    const customerData = {
      firstName: "Testfirstname",
      lastName: "Testlastname",
      email: "test@test.cz",
      phone: "777777777",
      address: "Botanická 42/99",
      address2: "apartment",
      city: "Brno",
      psc: "60200",
      country: "Czech Republic",
      deliveryMethod: "Standard Delivery",
    };
    test.info().annotations.push({
      type: "Info",
      description: JSON.stringify(customerData),
    });

    await test.step("Select Brewers category from store page", async () => {
      await page.locator(".store-menu-list li", { hasText: "Brewers" }).click();
      await expect(page).toHaveURL("/store/brewers");
    });
    await test.step("Select product Coffee Plunger", async () => {
      await page.locator(".product-tile", { hasText: "Coffee Plunger" }).click();
      await expect(page.locator(".product-detail", { hasText: "Coffee Plunger" })).toBeVisible(); // expecting to be on product page of Coffee Plunger
    });
    await test.step("Add product to cart", async () => {
      await page.getByTestId("addToCart").click();
      await expect(page).toHaveURL("/cart-content");
    });

    await test.step("Update quantity", async () => {
      await page.getByTestId("qtyInput").fill("2");
      await page.getByTestId("updateQty").click();
      await expect(page.locator(".cart-item-subtotal")).toContainText("$59.80");
      await expect(page.locator(".cart-total")).toContainText("$59.80");
    });
    await test.step("Go to checkout", async () => {
      await page.getByTestId("checkout").click();
    });

    const checkoutPage = new CheckoutPage(page);

    await test.step("Fill customer details and go to summary", async () => {
      await checkoutPage.fillDetails(customerData);
      await checkoutPage.goToSummary();
    });
    await test.step("Check data in summary page and select payment", async () => {
      await expect(page.getByTestId("billingDetails").locator("div")).toContainText([
        `${customerData.firstName} ${customerData.lastName}`,
        customerData.email,
        customerData.phone,
        customerData.address,
        customerData.address2,
        customerData.city,
        customerData.psc,
        customerData.country,
      ]);
      await expect(page.getByTestId("shippingDetails")).toContainText(customerData.deliveryMethod);
      await expect(page.getByTestId("shippingDetails").locator("div")).toContainText([
        customerData.address,
        customerData.city,
        customerData.psc,
        customerData.country,
      ]);
      await expect(page.locator(".cart-item-info", { hasText: "Coffee Plunger" })).toBeVisible();
      await expect(page.locator(".cart-item-amount input")).toHaveValue("2");
      await expect(page.locator(".cart-item-subtotal")).toContainText("$59.80");

      await checkoutPage.selectPayment("Cash on delivery");
    });
    await test.step("Confirm order and check thank you page", async () => {
      await checkoutPage.confirmOrder();
      await expect(page).toHaveURL("/order-complete");
      await expect(page.locator(".thank-you-content")).toContainText("Thank You!");
    });
  });
});
