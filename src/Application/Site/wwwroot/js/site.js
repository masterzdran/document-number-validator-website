// Theme toggle — persisted in localStorage (set early in <head> to avoid FOUC).
(function () {
    var btn = document.getElementById('theme-toggle');
    if (!btn) return;
    btn.addEventListener('click', function () {
        var next = document.documentElement.dataset.theme === 'dark' ? 'light' : 'dark';
        document.documentElement.dataset.theme = next;
        try { localStorage.setItem('theme', next); } catch (e) { /* private mode */ }
    });
})();

// Accessible tabs (roving tabindex + arrow keys).
(function () {
    document.querySelectorAll('[data-tabs]').forEach(function (root) {
        var tabs = Array.prototype.slice.call(root.querySelectorAll('[role="tab"]'));
        function select(tab) {
            tabs.forEach(function (t) {
                var active = t === tab;
                t.setAttribute('aria-selected', active ? 'true' : 'false');
                t.tabIndex = active ? 0 : -1;
                var panel = document.getElementById(t.getAttribute('aria-controls'));
                if (panel) panel.hidden = !active;
            });
            tab.focus();
        }
        tabs.forEach(function (tab, i) {
            tab.addEventListener('click', function () { select(tab); });
            tab.addEventListener('keydown', function (e) {
                if (e.key === 'ArrowRight') { e.preventDefault(); select(tabs[(i + 1) % tabs.length]); }
                if (e.key === 'ArrowLeft') { e.preventDefault(); select(tabs[(i - 1 + tabs.length) % tabs.length]); }
                if (e.key === 'Home') { e.preventDefault(); select(tabs[0]); }
                if (e.key === 'End') { e.preventDefault(); select(tabs[tabs.length - 1]); }
            });
        });
    });
})();

// Copy-to-clipboard buttons.
document.querySelectorAll('[data-copy-for]').forEach(function (btn) {
    btn.addEventListener('click', function () {
        var target = document.getElementById(btn.getAttribute('data-copy-for'));
        if (!target || !navigator.clipboard) return;
        navigator.clipboard.writeText(target.textContent).then(function () {
            var old = btn.textContent;
            btn.textContent = 'Copied!';
            btn.classList.add('copied');
            setTimeout(function () {
                btn.textContent = old;
                btn.classList.remove('copied');
            }, 1500);
        });
    });
});

// Google Analytics consent gate. The tag is only loaded after the visitor
// accepts; the choice is remembered in localStorage. See /privacy.
(function () {
    var KEY = 'analytics-consent';
    var ID = 'G-972WK5CM96';
    var banner = document.getElementById('consent-banner');
    var settings = document.querySelectorAll('[data-consent-settings]');
    var loaded = false;

    function read() {
        try { return localStorage.getItem(KEY); } catch (e) { return null; }
    }

    function load() {
        if (loaded) return;
        loaded = true;
        window.dataLayer = window.dataLayer || [];
        window.gtag = function () { window.dataLayer.push(arguments); };
        gtag('js', new Date());
        gtag('config', ID);
        var s = document.createElement('script');
        s.async = true;
        s.src = 'https://www.googletagmanager.com/gtag/js?id=' + ID;
        document.head.appendChild(s);
    }

    function decide(choice) {
        try { localStorage.setItem(KEY, choice); } catch (e) { /* private mode */ }
        if (banner) banner.hidden = true;
        if (choice === 'granted') load();
    }

    // Reveal "Cookie settings" once a decision can be changed.
    if (read()) settings.forEach(function (el) { el.hidden = false; });

    if (read() === 'granted') {
        load();
    } else if (!read() && banner) {
        banner.hidden = false;
    }

    if (banner) {
        banner.addEventListener('click', function (event) {
            var target = event.target.closest('[data-consent-accept], [data-consent-decline]');
            if (!target) return;
            decide(target.hasAttribute('data-consent-accept') ? 'granted' : 'denied');
        });
    }

    settings.forEach(function (el) {
        el.addEventListener('click', function () {
            if (banner) banner.hidden = false;
        });
    });
})();

// Interactive demo — progressive enhancement.
// Posts to the JSON handler (OnPostDemoJson) so the page does not reload; the
// plain form submit (post/redirect/get) stays as the no-JavaScript fallback and
// is also used whenever fetch fails.
(function () {
    var form = document.querySelector('form[data-demo-ajax]');
    if (!form || !window.fetch) return;

    var result = document.getElementById('demo-result');
    var valueInput = document.getElementById('demo-value');

    form.addEventListener('submit', function (event) {
        event.preventDefault();

        var data = new FormData(form);
        data.set('action', (event.submitter && event.submitter.value) || 'validate');

        // NOTE: read the action attribute, not the form.action property — a form
        // control named "action" (our submit buttons) shadows it via named access.
        var url = (form.getAttribute('action') || location.pathname)
            .replace('handler=Demo', 'handler=DemoJson');

        fetch(url, {
            method: 'POST',
            body: data,
            headers: { 'X-Requested-With': 'fetch', Accept: 'application/json' },
            credentials: 'same-origin'
        })
            .then(function (response) {
                if (!response.ok) throw new Error('HTTP ' + response.status);
                return response.json();
            })
            .then(function (body) {
                if (valueInput && body.value != null) valueInput.value = body.value;
                if (result) {
                    result.textContent = body.message;
                    result.className = 'alert alert-' + body.level;
                    result.hidden = false;
                }
            })
            .catch(function () { form.submit(); });
    });
})();
