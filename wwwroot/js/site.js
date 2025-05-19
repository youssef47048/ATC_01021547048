// Enable Bootstrap tooltips
document.addEventListener('DOMContentLoaded', function () {
    var tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'));
    var tooltipList = tooltipTriggerList.map(function (tooltipTriggerEl) {
        return new bootstrap.Tooltip(tooltipTriggerEl);
    });

    // Dark mode toggle
    setupDarkMode();
    
    // Setup RTL for Arabic language
    setupRTL();
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

// Setup RTL for Arabic language
function setupRTL() {
    // Check for Arabic language in the cookie or current HTML lang attribute
    const currentCulture = document.cookie
        .split('; ')
        .find(row => row.startsWith('.AspNetCore.Culture='))
        ?.split('=')[1];
    
    // If we find Arabic culture in the cookie
    if (currentCulture && currentCulture.includes('ar-SA')) {
        setRTL(true);
    } else if (document.documentElement.lang.startsWith('ar')) {
        setRTL(true);
    } else {
        setRTL(false);
    }
}

// Handle RTL support for Arabic language
function setRTL(isRTL) {
    const htmlElement = document.documentElement;
    if (isRTL) {
        htmlElement.setAttribute('dir', 'rtl');
        // Load RTL version of Bootstrap if needed
        loadRTLStylesheet();
    } else {
        htmlElement.setAttribute('dir', 'ltr');
    }
}

// Load RTL version of Bootstrap
function loadRTLStylesheet() {
    // Check if RTL stylesheet is already loaded
    if (!document.getElementById('bootstrap-rtl')) {
        const rtlStylesheet = document.createElement('link');
        rtlStylesheet.id = 'bootstrap-rtl';
        rtlStylesheet.rel = 'stylesheet';
        rtlStylesheet.href = 'https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.rtl.min.css';
        
        // Insert RTL stylesheet right after the main Bootstrap stylesheet
        const mainStylesheet = document.querySelector('link[href*="bootstrap.min.css"]');
        if (mainStylesheet) {
            mainStylesheet.parentNode.insertBefore(rtlStylesheet, mainStylesheet.nextSibling);
        } else {
            document.head.appendChild(rtlStylesheet);
        }
    }
} 