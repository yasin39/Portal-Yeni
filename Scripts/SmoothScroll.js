
    // Detay paneline smooth scroll ve fokus
        function scrollToDetailPanel(panelSelector) {
            // Eğer selector verilmemişse default kullan
            panelSelector = panelSelector || '.detail-panel.show';

        var detailPanel = document.querySelector(panelSelector);
        if (detailPanel) {
            // Smooth scroll ile panele git
            detailPanel.scrollIntoView({
                behavior: 'smooth',
                block: 'start'
            });

        // İlk input alanına fokus ver
        setTimeout(function() {
                var firstInput = detailPanel.querySelector('input[type="text"]:not([readonly]), textarea, select');
        if (firstInput) {
            firstInput.focus();
                }
            }, 500);
        }
    }

        // Herhangi bir elemente scroll
        function scrollToElement(selector, focusFirst) {
            focusFirst = focusFirst !== false; // Default true

        var element = document.querySelector(selector);
        if (element) {
            element.scrollIntoView({
                behavior: 'smooth',
                block: 'start'
            });

        if (focusFirst) {
            setTimeout(function () {
                var firstInput = element.querySelector('input[type="text"]:not([readonly]), textarea, select');
                if (firstInput) {
                    firstInput.focus();
                }
            }, 500);
            }
        }
    }

        // Panel göster ve scroll
        function showPanelAndScroll(panelId, hideOtherPanels) {
        var panel = document.getElementById(panelId);
        if (panel) {
            // Diğer panelleri gizle (opsiyonel)
            if (hideOtherPanels && hideOtherPanels.length > 0) {
            hideOtherPanels.forEach(function (otherId) {
                var otherPanel = document.getElementById(otherId);
                if (otherPanel) {
                    otherPanel.classList.remove('show');
                }
            });
            }

        // Bu paneli göster
        panel.classList.add('show');

        // Scroll ve fokus
        setTimeout(function() {
            scrollToElement('#' + panelId, true);
            }, 100);
        }
    }
    