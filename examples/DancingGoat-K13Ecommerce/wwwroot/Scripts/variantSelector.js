(function () {
    'use strict';

    var url = '/api/k13/product/inventory-price-info',
        stockMessage = document.getElementById('stockMessage'),
        totalPrice = document.getElementById('totalPrice'),
        inStockClass = 'available',
        outOfStockClass = "unavailable",
        submitButton = document.getElementById('js-submit-add-to-cart'),
        beforeDiscount = document.getElementById('js-before-discount'),
        savings = document.getElementById("js-savings"),
        discountWrapper = document.querySelector('.discount-price');    

    document.querySelectorAll('.js-variant-selector').forEach(function (element) {
        element.addEventListener('change', function () {
            var selectedOptionIDs = [];
            // Collect selected options in categories in order to create variant
            document.querySelectorAll('.js-variant-selector option:checked, .js-variant-selector input:checked').forEach(function (option) {
                selectedOptionIDs.push(option.value);
            });

            updateVariantSpecificData(getVariantId(selectedOptionIDs));
        });
    });

    function getVariantId(selectedOptionIDs) {
        // currently only one selector with all variant is supported
        return selectedOptionIDs[0];
    }

    function updateVariantSpecificData(variantSKUID) {
        let fullUrl = url + '?variantSKUID=' + encodeURIComponent(variantSKUID);
        fetch(fullUrl, {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json'
            }            
        })
            .then(function (response) {
                if (!response.ok) {
                    throw new Error('Network response was not ok');
                }
                return response.json();
            })
            .then(function (data) {
                stockMessage.textContent = data.stockMessage;
                if (data.inStock) {
                    stockMessage.classList.remove(outOfStockClass);
                    stockMessage.classList.add(inStockClass);
                } else {
                    stockMessage.classList.remove(inStockClass);
                    stockMessage.classList.add(outOfStockClass);
                }
                if (data.allowSale) {
                    submitButton.classList.remove('btn-disabled');
                    submitButton.removeAttribute('disabled');
                } else {
                    submitButton.classList.add('btn-disabled');
                    submitButton.setAttribute('disabled', 'disabled');
                }
                totalPrice.textContent = data.totalPrice;                

                // Update discount price info
                if (data.savings) {
                    beforeDiscount.textContent = data.beforeDiscount;
                    savings.textContent = data.savings;
                    discountWrapper.classList.remove('hidden');
                } else {
                    discountWrapper.classList.add('hidden');
                }
            })
            .catch(function (error) {
                console.error('Error fetching data:', error);
            });
    }
})();
