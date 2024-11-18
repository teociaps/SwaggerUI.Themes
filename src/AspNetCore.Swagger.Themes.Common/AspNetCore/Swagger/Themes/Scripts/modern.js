/*
    Modern UI

    https://github.com/teociaps/SwaggerUI.Themes
*/

const rootElement = document.documentElement;

window.onpageshow = function () {
    let swaggerUILoaded = setInterval(function () {
        if (document.getElementById("swagger-ui") != null) {
            clearInterval(swaggerUILoaded);

            console.log('Hello modern Swagger UI!');

            setUpPinnableTopbar({$PINNABLE_TOPBAR});

            setUpScrollToTopButton({$BACK_TO_TOP});

            setUpExpandAndCollapseOperationsButtons({$EXPAND_COLLAPSE_ALL_OPERATIONS});
        }
    }, 100);
}

function setUpPinnableTopbar(enabled) {
    if (enabled === false)
        return;

    const topbarWrapper = document.querySelector('.topbar-wrapper');

    const pinTopbarBtn = document.createElement('button');
    pinTopbarBtn.setAttribute('id', 'pin-topbar-btn');
    pinTopbarBtn.addEventListener('click', () => pinOrUnpinTopbar(pinTopbarBtn))

    topbarWrapper.appendChild(pinTopbarBtn);

    pinOrUnpinTopbar(pinTopbarBtn);
}

function pinOrUnpinTopbar(pinTopbarBtn) {
    if (pinTopbarBtn.parentNode.parentNode.parentNode.classList.contains('pinned')) {
        pinTopbarBtn.parentNode.parentNode.parentNode.classList.remove('pinned');
        setUnpinnedIconTo(pinTopbarBtn);
        pinTopbarBtn.setAttribute('title', 'Pin topbar');
    }
    else {
        pinTopbarBtn.parentNode.parentNode.parentNode.classList.add('pinned');
        setPinnedIconTo(pinTopbarBtn);
        pinTopbarBtn.setAttribute('title', 'Unpin topbar');
    }

    pinTopbarBtn?.blur();
}

function setPinnedIconTo(element) {
    element.innerHTML = `<svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-pin-fill" viewBox="0 0 16 16">
                           <path d="M4.146.146A.5.5 0 0 1 4.5 0h7a.5.5 0 0 1 .5.5c0 .68-.342 1.174-.646 1.479-.126.125-.25.224-.354.298v4.431l.078.048c.203.127.476.314.751.555C12.36 7.775 13 8.527 13 9.5a.5.5 0 0 1-.5.5h-4v4.5c0 .276-.224 1.5-.5 1.5s-.5-1.224-.5-1.5V10h-4a.5.5 0 0 1-.5-.5c0-.973.64-1.725 1.17-2.189A6 6 0 0 1 5 6.708V2.277a3 3 0 0 1-.354-.298C4.342 1.674 4 1.179 4 .5a.5.5 0 0 1 .146-.354"/>
                         </svg>`;
}

function setUnpinnedIconTo(element) {
    element.innerHTML = `<svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-pin-angle" viewBox="0 0 16 16">
                           <path d="M9.828.722a.5.5 0 0 1 .354.146l4.95 4.95a.5.5 0 0 1 0 .707c-.48.48-1.072.588-1.503.588-.177 0-.335-.018-.46-.039l-3.134 3.134a6 6 0 0 1 .16 1.013c.046.702-.032 1.687-.72 2.375a.5.5 0 0 1-.707 0l-2.829-2.828-3.182 3.182c-.195.195-1.219.902-1.414.707s.512-1.22.707-1.414l3.182-3.182-2.828-2.829a.5.5 0 0 1 0-.707c.688-.688 1.673-.767 2.375-.72a6 6 0 0 1 1.013.16l3.134-3.133a3 3 0 0 1-.04-.461c0-.43.108-1.022.589-1.503a.5.5 0 0 1 .353-.146m.122 2.112v-.002zm0-.002v.002a.5.5 0 0 1-.122.51L6.293 6.878a.5.5 0 0 1-.511.12H5.78l-.014-.004a5 5 0 0 0-.288-.076 5 5 0 0 0-.765-.116c-.422-.028-.836.008-1.175.15l5.51 5.509c.141-.34.177-.753.149-1.175a5 5 0 0 0-.192-1.054l-.004-.013v-.001a.5.5 0 0 1 .12-.512l3.536-3.535a.5.5 0 0 1 .532-.115l.096.022c.087.017.208.034.344.034q.172.002.343-.04L9.927 2.028q-.042.172-.04.343a1.8 1.8 0 0 0 .062.46z"/>
                         </svg>`;
}

function setUpScrollToTopButton(enabled) {
    if (enabled === false)
        return;

    // Create wrapper
    const scrollToTopContainer = document.createElement('div');
    scrollToTopContainer.classList.add('scroll-to-top-wrapper');

    // Create scroll top button
    const scrollToTopButton = document.createElement('button');
    scrollToTopButton.setAttribute('id', 'scroll-to-top-btn');
    scrollToTopButton.setAttribute('title', 'Back to top');
    scrollToTopButton.addEventListener('click', () => {
        scrollToTop();
        scrollToTopButton?.blur();
    });
    scrollToTopContainer.appendChild(scrollToTopButton);

    const swaggerContainer = document.getElementById('swagger-ui');
    swaggerContainer.appendChild(scrollToTopContainer);

    // Show/hide management
    const showHideScrollTopBtn = () => {
        window.scrollY >= 200
            ? scrollToTopButton.classList.add("showBtn")
            : scrollToTopButton.classList.remove("showBtn");
    }

    window.addEventListener("scroll", showHideScrollTopBtn);
    window.addEventListener("resize", showHideScrollTopBtn);
}

function scrollToTop() {
    rootElement.scrollTo({
        top: 0,
        behavior: 'smooth'
    })
}

function setUpExpandAndCollapseOperationsButtons(enabled) {
    if (enabled === false)
        return;

    const opBlockSections = document.querySelectorAll('.opblock-tag-section');

    // Iterate over each operation group
    opBlockSections.forEach(opBlockSection => {
        const opBlockSectionHeader = opBlockSection.querySelector('h3');
        const expandOperationButton = opBlockSectionHeader.querySelector('button.expand-operation');

        // Create expand or collapse button, if needed
        if (expandOperationButton) {
            const expandOrCollapseButton = document.createElement('button');
            expandOrCollapseButton.setAttribute('title', 'Expand/Collapse all the operations');
            expandOrCollapseButton.classList.add('expand-collapse-all-btn');
            expandOrCollapseButton.innerHTML = 'Expand/Collapse All';

            opBlockSectionHeader.insertBefore(expandOrCollapseButton, expandOperationButton);

            expandOrCollapseButton.addEventListener('click', (e) => {
                e.preventDefault();
                e.stopPropagation();

                const opBlocks = opBlockSection.querySelectorAll('.opblock .opblock-control-arrow');
                const allExpanded = Array.from(opBlocks).every(opBlock => opBlock.getAttribute('aria-expanded') === 'true');

                if (allExpanded) {
                    // Collapse all (click to collapse)
                    opBlocks.forEach(opBlock => {
                        if (opBlock.getAttribute('aria-expanded') === 'true') {
                            opBlock.click(); // Collapse
                        }
                    });
                } else {
                    // Expand all (click to expand)
                    opBlocks.forEach(opBlock => {
                        if (opBlock.getAttribute('aria-expanded') === 'false') {
                            opBlock.click(); // Expand
                        }
                    });
                }
            });
        }
    });
}