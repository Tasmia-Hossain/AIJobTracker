// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

(() => {
    const root = document.documentElement;
    const savedTheme = localStorage.getItem("aijobtracker-theme");
    const prefersDark = window.matchMedia("(prefers-color-scheme: dark)").matches;

    function applyTheme(theme) {
        root.dataset.theme = theme;
        const toggle = document.querySelector("[data-theme-toggle]");
        if (toggle) {
            const nextTheme = theme === "dark" ? "light" : "dark";
            toggle.setAttribute("aria-label", `Switch to ${nextTheme} mode`);
            toggle.setAttribute("title", `Switch to ${nextTheme} mode`);
        }
    }

    applyTheme(savedTheme || (prefersDark ? "dark" : "light"));
    document.querySelector("[data-theme-toggle]")?.addEventListener("click", () => {
        const nextTheme = root.dataset.theme === "dark" ? "light" : "dark";
        localStorage.setItem("aijobtracker-theme", nextTheme);
        applyTheme(nextTheme);
    });

    const textarea = document.querySelector("#jobDescription");
    const counter = document.querySelector("[data-input-count]");
    const updateCount = () => { if (textarea && counter) counter.textContent = `${textarea.value.length.toLocaleString()} characters`; };
    textarea?.addEventListener("input", updateCount);
    updateCount();

    document.querySelector("[data-analyzer-form]")?.addEventListener("submit", event => {
        const button = event.currentTarget.querySelector("[data-analyze-button]");
        if (!button || button.disabled) return;
        button.disabled = true;
        button.classList.add("is-loading");
        button.setAttribute("aria-busy", "true");
    });
})();
