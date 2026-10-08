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
