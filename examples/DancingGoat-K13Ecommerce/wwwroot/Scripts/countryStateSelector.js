(function () {
    'use strict';

    document.querySelectorAll('.js-country-selector').forEach(function (countrySelector) {
        countrySelector.addEventListener('change', function () {
            var countryStateSelector = countrySelector.closest('.js-country-state-selector'),
                stateSelector = countryStateSelector.querySelector('.js-state-selector'),
                stateSelectorContainer = countryStateSelector.querySelector('.js-state-selector-container'),
                selectedStateId = countryStateSelector.dataset.stateselectedid,
                url = countryStateSelector.dataset.statelistaction,
                postData = {
                    countryId: countrySelector.value
                };

            stateSelectorContainer.style.display = 'none';

            if (!postData.countryId) {
                return;
            }

            var formData = new FormData();
            formData.append('countryId', postData.countryId);

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
                    countryStateSelector.dataset.stateselectedid = 0;
                    stateSelector.value = null;

                    if (data.length === 0) {
                        return;
                    }

                    fillStateSelector(stateSelector, data);
                    stateSelectorContainer.style.display = 'block';

                    if (selectedStateId > 0) {
                        stateSelector.value = selectedStateId;
                    }
                })
                .catch(function (error) {
                    console.error('There was a problem with the fetch operation:', error.message);
                });
        });
    });

    document.querySelectorAll('.js-country-state-selector').forEach(function (selector) {
        var countrySelector = selector.querySelector('.js-country-selector'),
            countryId = selector.dataset.countryselectedid;

        if (countryId > 0) {
            countrySelector.value = countryId;
        }

        countrySelector.dispatchEvent(new Event('change'));
        selector.dataset.countryselectedid = 0;
    });

    function fillStateSelector(stateSelector, data) {
        var items = '<option>' + stateSelector.querySelector('option:first-child').innerHTML + '</option>';

        data.forEach(function (state) {
            items += '<option value="' + state.id + '">' + state.name + '</option>';
        });

        stateSelector.innerHTML = items;
    }
})();
