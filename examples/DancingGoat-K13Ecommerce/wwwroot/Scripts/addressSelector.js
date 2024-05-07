(function () {
    'use strict';

    document.querySelectorAll('.js-address-selector-div').forEach(function (selectorDiv) {
        selectorDiv.addEventListener('change', function () {
            var addressDiv = selectorDiv.parentElement,
                selector = selectorDiv.querySelector('.js-address-selector'),
                url = selectorDiv.dataset.statelistaction,
                postData = {
                    addressId: selector.value
                };

            if (!postData.addressId) {
                eraseFields(addressDiv);
                return;
            }

            var formData = new FormData();
            formData.append('addressId', postData.addressId);

            fetch(url, {
                method: 'POST',
                body: formData
            })
                .then(function (response) {
                    if (!response.ok) {
                        throw new Error('Network response was not ok');
                    }
                    return response.json();
                })
                .then(function (data) {
                    fillFields(addressDiv, data);
                })
                .catch(function (error) {
                    console.error('There was a problem with the fetch operation:', error.message);
                });
        });
    });

    function fillFields(addressDiv, data) {
        fillBasicFields(addressDiv, data);
        fillCountryStateFields(addressDiv, data);
    }

    function fillBasicFields(addressDiv, data) {
        var basicFields = addressDiv.dataset.fields,
            addressType = addressDiv.dataset.addresstype;

        basicFields.split(',').forEach(function (val) {
            let fieldId = '#' + addressType + '_' + addressType + val;
            let propertyName = val.charAt(0).toLowerCase() + val.slice(1);
            let fieldVal = data[propertyName];

            document.querySelector(fieldId).value = fieldVal;
        });
    }

    function fillCountryStateFields(addressDiv, data) {
        var countryStateSelector = addressDiv.querySelector('.js-country-state-selector'),
            countryField = countryStateSelector.dataset.countryfield,
            stateField = countryStateSelector.dataset.statefield,
            countrySelector = countryStateSelector.querySelector('.js-country-selector');

        countryStateSelector.dataset.stateselectedid = data[stateField];
        countrySelector.value = data[countryField];
        countrySelector.dispatchEvent(new Event('change'));
    }

    function eraseFields(addressDiv) {
        var data = {};
        fillFields(addressDiv, data);
    }
})();
