/*
    Swagger UI

    https://github.com/teociaps/SwaggerUI.Themes
*/

const rootElement = document.documentElement;

window.onpageshow = function () {
    let swaggerUILoaded = setInterval(function () {
        if (document.getElementById("swagger-ui") != null) {
            clearInterval(swaggerUILoaded);

            console.log('Hello Swagger UI!');

            suppressNativeDarkModeClass();

            setUpPinnableTopbar({$PINNABLE_TOPBAR});

            setUpPinnableFilterBar({$PINNABLE_FILTER_BAR});

            setUpScrollToTopButton({$BACK_TO_TOP});

            setUpExpandAndCollapseOperationsButtons({$EXPAND_COLLAPSE_ALL_OPERATIONS});

            setUpThemeSwitcher({$THEME_SWITCHER});
        }
    }, 100);
}

function setUpPinnableTopbar(enabled) {
    if (enabled === false)
        return;

    setUpPinnableToggle({
        buttonId: 'pin-topbar-btn',
        container: document.querySelector('.topbar-wrapper'),
        // 3 parentNode hops up from the button (appended to .topbar-wrapper) reaches .topbar.
        pinnedAncestorDepth: 3,
        storageKey: 'swaggerui-pinnable-topbar-preference',
        pinTitle: 'Pin topbar',
        unpinTitle: 'Unpin topbar',
        // No saved preference defaults to pinned, matching the topbar's original
        // always-pinned-on-load behavior.
        defaultPinned: true
    });
}

// Swashbuckle.AspNetCore 10.x's swagger-ui-dist ships its own dark-mode toggle, hidden via CSS
// (see common.css) because it layers swagger-ui's own dark styling on top of this library's
// theme. Hiding the button doesn't stop the toggle's React component from mounting though: it
// still auto-applies a 'dark-mode' class to <html> based on prefers-color-scheme regardless of
// the button's visibility. A MutationObserver strips it back off whenever it appears, since
// there's no reliable synchronous point to catch it once and be done - the toggle component can
// mount before or after this script runs.
function suppressNativeDarkModeClass() {
    // Only remove (and thus only mutate the attribute) when the class is actually present, so
    // this can never trigger the observer with a mutation of its own.
    const strip = () => {
        if (rootElement.classList.contains('dark-mode'))
            rootElement.classList.remove('dark-mode');
    };
    strip();
    new MutationObserver(strip).observe(rootElement, { attributes: true, attributeFilter: ['class'] });
}

function setUpPinnableFilterBar(enabled) {
    if (enabled === false)
        return;

    // Unlike #swagger-ui itself, .filter only renders once the OpenAPI document has been
    // fetched and parsed - it commonly doesn't exist yet at the moment #swagger-ui does, so a
    // single lookup here would silently and permanently no-op. Poll for it with the same
    // bounded-retry pattern used by setUpExpandAndCollapseOperationsButtons below.
    const MAX_ATTEMPTS = 60;
    const RETRY_DELAY_MS = 200;
    let attempts = 0;

    (function waitForFilterBar() {
        attempts++;
        const filterBar = document.querySelector('.filter');

        if (filterBar) {
            initPinnableFilterBar(filterBar);
            return;
        }

        if (attempts < MAX_ATTEMPTS)
            setTimeout(waitForFilterBar, RETRY_DELAY_MS);
    })();
}

function initPinnableFilterBar(filterBar) {
    // CSS alone can't tell whether a sticky element has actually reached its sticky top, so
    // toggle a 'stuck' class here (styled in common.css) by comparing the sticky wrapper's
    // viewport position against its computed sticky offset. The wrapper is 2 parentElement
    // hops up from .filter (.filter -> .filter-container -> sticky div, see common.css).
    const stickyWrapper = filterBar.parentElement?.parentElement;

    function updateStuckState() {
        if (!stickyWrapper)
            return;

        // getComputedStyle().top is 'auto' (NaN) while unpinned, so the comparison alone
        // could never be true then; the explicit pinned check just makes that intent clear.
        const stuck = filterBar.classList.contains('pinned')
            && stickyWrapper.getBoundingClientRect().top <= parseFloat(getComputedStyle(stickyWrapper).top) + 1;
        filterBar.classList.toggle('stuck', stuck);
    }

    window.addEventListener('scroll', updateStuckState, { passive: true });
    window.addEventListener('resize', updateStuckState);

    setUpPinnableToggle({
        buttonId: 'pin-filterbar-btn',
        container: filterBar,
        // 1 parentNode hop up from the button (appended directly to .filter) reaches .filter itself.
        pinnedAncestorDepth: 1,
        storageKey: 'swaggerui-pinnable-filterbar-preference',
        pinTitle: 'Pin filter bar',
        unpinTitle: 'Unpin filter bar',
        defaultPinned: false,
        // Re-evaluate on every pin/unpin so unpinning mid-scroll clears the 'stuck' class
        // immediately instead of waiting for the next scroll event.
        onPinnedStateApplied: updateStuckState
    });
}

// Shared behavior behind every pinnable element (topbar, filter bar, ...): injects a pin
// button into `container`, toggles a 'pinned' class on the ancestor `pinnedAncestorDepth`
// parentNodes up from that button, persists the choice to localStorage, and notifies the
// optional `onPinnedStateApplied` callback after every applied state (initial load included).
function setUpPinnableToggle({ buttonId, container, pinnedAncestorDepth, storageKey, pinTitle, unpinTitle, defaultPinned, onPinnedStateApplied }) {
    if (!container)
        return;

    const pinBtn = document.createElement('button');
    pinBtn.setAttribute('id', buttonId);
    pinBtn.addEventListener('click', () => {
        const currentlyPinned = pinnedAncestorOf(pinBtn).classList.contains('pinned');
        applyPinnedState(!currentlyPinned, true);
        pinBtn?.blur();
    })

    container.appendChild(pinBtn);

    // Apply the saved (or default) pinned state directly, so there's no flash of the wrong
    // state on page load.
    const savedState = localStorage.getItem(storageKey);
    const isPinned = savedState !== null ? savedState === 'pinned' : defaultPinned;
    applyPinnedState(isPinned, false);

    function pinnedAncestorOf(button) {
        let ancestor = button;
        for (let i = 0; i < pinnedAncestorDepth; i++)
            ancestor = ancestor.parentNode;
        return ancestor;
    }

    function applyPinnedState(pinned, saveToStorage) {
        if (pinned) {
            pinnedAncestorOf(pinBtn).classList.add('pinned');
            setPinnedIconTo(pinBtn);
            pinBtn.setAttribute('title', unpinTitle);
        }
        else {
            pinnedAncestorOf(pinBtn).classList.remove('pinned');
            setUnpinnedIconTo(pinBtn);
            pinBtn.setAttribute('title', pinTitle);
        }

        if (saveToStorage) {
            localStorage.setItem(storageKey, pinned ? 'pinned' : 'unpinned');
        }

        onPinnedStateApplied?.();
    }
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

    const MAX_ATTEMPTS = 60;
    const RETRY_DELAY_MS = 200;
    let attempts = 0;

    const rootSwagger = document.getElementById('swagger-ui') || document.querySelector('.swagger-ui');

    function attachToSection(opBlockSection) {
        if (!opBlockSection || opBlockSection.dataset.expandCollapseAttached === 'true')
            return;

        const opBlockSectionHeader = opBlockSection.querySelector('h3');
        const expandOperationButton = opBlockSectionHeader?.querySelector('button.expand-operation');

        // Create expand or collapse button, if header exists
        if (opBlockSectionHeader) {
            // Avoid duplicate button
            if (!opBlockSectionHeader.querySelector('.expand-collapse-all-btn')) {
                const expandOrCollapseButton = document.createElement('button');
                expandOrCollapseButton.setAttribute('title', 'Expand/Collapse all the operations');
                expandOrCollapseButton.classList.add('expand-collapse-all-btn');
                expandOrCollapseButton.innerHTML = 'Expand/Collapse All';

                // Insert before existing expand control if possible, otherwise append
                if (expandOperationButton) {
                    expandOperationButton.before(expandOrCollapseButton);
                } else {
                    opBlockSectionHeader.appendChild(expandOrCollapseButton);
                }

                expandOrCollapseButton.addEventListener('click', (e) => {
                    e.preventDefault();
                    e.stopPropagation();

                    const opBlocks = opBlockSection.querySelectorAll('.opblock .opblock-control-arrow');
                    if (!opBlocks || opBlocks.length === 0) {
                        // Nothing to do
                        return;
                    }

                    const allExpanded = Array.from(opBlocks).every(opBlock => opBlock.getAttribute('aria-expanded') === 'true');

                    if (allExpanded) {
                        // Collapse all
                        opBlocks.forEach(opBlock => {
                            if (opBlock.getAttribute('aria-expanded') === 'true') {
                                opBlock.click();
                            }
                        });
                    } else {
                        // Expand all
                        opBlocks.forEach(opBlock => {
                            if (opBlock.getAttribute('aria-expanded') === 'false') {
                                opBlock.click();
                            }
                        });
                    }
                });
            }
        }

        // mark as processed to prevent duplicates
        opBlockSection.dataset.expandCollapseAttached = 'true';
    }

    function processAllSections() {
        const opBlockSections = document.querySelectorAll('.opblock-tag-section');
        if (!opBlockSections || opBlockSections.length === 0) return false;

        opBlockSections.forEach(section => attachToSection(section));
        return true;
    }

    function waitAndProcess() {
        attempts++;
        const found = processAllSections();

        if (found) {
            // also observe for new sections added later (incremental/rendering)
            try {
                const containerToObserve = rootSwagger || document.body;
                const observer = new MutationObserver(mutations => {
                    for (const m of mutations) {
                        if (m.addedNodes && m.addedNodes.length > 0) {
                            // try to attach to any new sections
                            processAllSections();
                        }
                    }
                });

                observer.observe(containerToObserve, { childList: true, subtree: true });
            } catch (e) {
                // ignore observation errors
            }

            return;
        }

        if (attempts < MAX_ATTEMPTS) {
            setTimeout(waitAndProcess, RETRY_DELAY_MS);
        } else {
            // Give up after multiple attempts
            return;
        }
    }

    // Initial kick-off
    waitAndProcess();
}

function setUpThemeSwitcher(enabled) {
    if (enabled === false)
        return;

    const STORAGE_KEY = 'swaggerui-theme-preference';
    const METADATA_ENDPOINT = '/themes/metadata.json';

    let themesMetadata = null;
    let currentTheme = null;

    // Load theme metadata and initialize
    fetch(METADATA_ENDPOINT)
        .then(response => {
            if (!response.ok) {
                console.warn('[ThemeSwitcher] Failed to load theme metadata');
                return null;
            }
            return response.json();
        })
        .then(data => {
            if (!data?.themes || data.themes.length < 2) {
                console.warn('[ThemeSwitcher] Not enough themes available for switcher');
                return;
            }

            themesMetadata = data;
            currentTheme = detectCurrentTheme(data.themes);
            restoreSavedTheme(data.themes);
            injectThemeSwitcherUI(data);
        })
        .catch(error => {
            console.error('[ThemeSwitcher] Error loading themes:', error);
        });

    function restoreSavedTheme(themes) {
        const saved = localStorage.getItem(STORAGE_KEY);

        if (saved && themes.some(t => t.name === saved)) {
            if (saved !== currentTheme) {
                switchTheme(saved, false);
            }
        }
    }

    function injectThemeSwitcherUI(data) {
        const topbarWrapper = document.querySelector('.topbar-wrapper');
        if (!topbarWrapper) {
            console.warn('[ThemeSwitcher] Topbar not found');
            return;
        }

        const select = document.createElement('select');
        select.id = 'theme-switcher-select';
        select.setAttribute('aria-label', 'Select theme');
        select.title = 'Switch theme';

        data.themes.forEach(theme => {
            const option = document.createElement('option');
            option.value = theme.name;
            option.textContent = formatThemeName(theme.name, data.config?.displayFormat || '{name}');

            if (theme.name === currentTheme) {
                option.selected = true;
            }

            select.appendChild(option);
        });

        select.addEventListener('change', (e) => {
            switchTheme(e.target.value, true);
        });

        // Insert before pin button if it exists
        const pinButton = document.getElementById('pin-topbar-btn');
        if (pinButton) {
            pinButton.before(select);
        } else {
            topbarWrapper.appendChild(select);
        }
    }

    function switchTheme(themeName, saveToStorage) {
        const theme = themesMetadata?.themes.find(t => t.name === themeName);
        if (!theme) {
            console.warn(`[ThemeSwitcher] Theme not found: ${themeName}`);
            return;
        }

        // Find all theme stylesheets
        const allStyleLinks = document.querySelectorAll('link[rel="stylesheet"]');
        const allThemePaths = themesMetadata.themes.map(t => t.cssPath);

        let themeActivated = false;

        allStyleLinks.forEach(link => {
            // Check if this is a theme stylesheet (has data-theme or matches a known theme path)
            const isThemeStylesheet = link.dataset.theme ||
                allThemePaths.some(path => link.href?.endsWith(path));

            if (isThemeStylesheet) {
                // Use exact path matching to avoid substring issues
                const isTargetTheme = (link.dataset.theme === themeName) ||
                    (link.href?.endsWith(theme.cssPath));

                if (isTargetTheme) {
                    link.disabled = false;
                    link.dataset.theme = themeName;
                    themeActivated = true;
                } else {
                    link.disabled = true;
                }
            }
        });

        if (!themeActivated) {
            console.warn(`[ThemeSwitcher] Could not activate theme: ${themeName}`);
        }

        currentTheme = themeName;

        if (saveToStorage) {
            localStorage.setItem(STORAGE_KEY, themeName);
        }

        const dropdown = document.getElementById('theme-switcher-select');
        if (dropdown && dropdown.value !== themeName) {
            dropdown.value = themeName;
        }
    }
}
function detectCurrentTheme(themes) {
    // Check for data-theme attribute first (most reliable)
    const activeLink = document.querySelector('link[rel="stylesheet"]:not([disabled])[data-theme]');
    if (activeLink?.dataset.theme) {
        return activeLink.dataset.theme;
    }

    // Fallback: check href patterns
    const styleElements = document.querySelectorAll('link[rel="stylesheet"]:not([disabled])');

    for (const element of styleElements) {
        const theme = themes.find(t => {
            if (element.href) {
                return element.href.includes(t.cssPath) ||
                    element.href.endsWith(t.cssPath) ||
                    element.href.includes(t.name.toLowerCase());
            }
            return false;
        });

        if (theme) {
            return theme.name;
        }
    }

    // Fallback to first theme
    return themes.length > 0 ? themes[0].name : null;
}
function formatThemeName(name, format) {
    const formatted = name
        .replaceAll(/([A-Z])/g, ' $1')
        .trim()
        .split(/[\s_-]+/)
        .map(word => word.charAt(0).toUpperCase() + word.slice(1).toLowerCase())
        .join(' ');

    return format.replace('{name}', formatted);
}