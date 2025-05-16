// Enable Bootstrap tooltips
document.addEventListener('DOMContentLoaded', function () {
    var tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'));
    var tooltipList = tooltipTriggerList.map(function (tooltipTriggerEl) {
        return new bootstrap.Tooltip(tooltipTriggerEl);
    });

    // Dark mode toggle
    setupDarkMode();
});

// Dark mode functionality
function setupDarkMode() {
    const darkModeToggle = document.getElementById('darkModeToggle');
    const htmlElement = document.getElementById('htmlElement');
    
    // Check for saved theme preference or use device default
    const savedTheme = localStorage.getItem('theme');
    const prefersDarkMode = window.matchMedia('(prefers-color-scheme: dark)').matches;
    
    // Set initial theme
    if (savedTheme === 'dark' || (!savedTheme && prefersDarkMode)) {
        htmlElement.setAttribute('data-bs-theme', 'dark');
        darkModeToggle.checked = true;
    } else {
        htmlElement.setAttribute('data-bs-theme', 'light');
        darkModeToggle.checked = false;
    }
    
    // Toggle theme when checkbox changes
    darkModeToggle.addEventListener('change', function() {
        if (this.checked) {
            htmlElement.setAttribute('data-bs-theme', 'dark');
            localStorage.setItem('theme', 'dark');
        } else {
            htmlElement.setAttribute('data-bs-theme', 'light');
            localStorage.setItem('theme', 'light');
        }
    });
}

// Handle RTL support for Arabic language
function setRTL(isRTL) {
    const htmlElement = document.documentElement;
    if (isRTL) {
        htmlElement.setAttribute('dir', 'rtl');
    } else {
        htmlElement.setAttribute('dir', 'ltr');
    }
} 