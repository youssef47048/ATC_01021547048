// Enable Bootstrap tooltips
document.addEventListener('DOMContentLoaded', function () {
    var tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'));
    var tooltipList = tooltipTriggerList.map(function (tooltipTriggerEl) {
        return new bootstrap.Tooltip(tooltipTriggerEl);
    });

    // Dark mode toggle
    setupDarkMode();
    
    // Initialize any charts if they exist
    if (document.getElementById('eventsChart')) {
        initializeEventsChart();
    }
    
    if (document.getElementById('bookingsChart')) {
        initializeBookingsChart();
    }
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
        
        // Update chart colors if charts exist
        updateChartsTheme(this.checked);
    });
}

// Function to initialize events chart
function initializeEventsChart() {
    const ctx = document.getElementById('eventsChart').getContext('2d');
    const isDarkMode = document.getElementById('htmlElement').getAttribute('data-bs-theme') === 'dark';
    
    // Get data from the data attributes
    const labels = JSON.parse(document.getElementById('eventsChart').getAttribute('data-labels'));
    const data = JSON.parse(document.getElementById('eventsChart').getAttribute('data-values'));
    
    const textColor = isDarkMode ? '#f8f9fa' : '#212529';
    const gridColor = isDarkMode ? 'rgba(255, 255, 255, 0.1)' : 'rgba(0, 0, 0, 0.1)';
    
    const chart = new Chart(ctx, {
        type: 'bar',
        data: {
            labels: labels,
            datasets: [{
                label: 'Events by Category',
                data: data,
                backgroundColor: 'rgba(25, 118, 210, 0.7)',
                borderColor: 'rgb(25, 118, 210)',
                borderWidth: 1
            }]
        },
        options: {
            responsive: true,
            maintainAspectRatio: false,
            scales: {
                y: {
                    beginAtZero: true,
                    grid: {
                        color: gridColor
                    },
                    ticks: {
                        color: textColor
                    }
                },
                x: {
                    grid: {
                        color: gridColor
                    },
                    ticks: {
                        color: textColor
                    }
                }
            },
            plugins: {
                legend: {
                    labels: {
                        color: textColor
                    }
                }
            }
        }
    });
    
    // Store chart in window object for later theme updates
    window.eventsChart = chart;
}

// Function to initialize bookings chart
function initializeBookingsChart() {
    const ctx = document.getElementById('bookingsChart').getContext('2d');
    const isDarkMode = document.getElementById('htmlElement').getAttribute('data-bs-theme') === 'dark';
    
    // Get data from the data attributes
    const labels = JSON.parse(document.getElementById('bookingsChart').getAttribute('data-labels'));
    const data = JSON.parse(document.getElementById('bookingsChart').getAttribute('data-values'));
    
    const textColor = isDarkMode ? '#f8f9fa' : '#212529';
    const gridColor = isDarkMode ? 'rgba(255, 255, 255, 0.1)' : 'rgba(0, 0, 0, 0.1)';
    
    const chart = new Chart(ctx, {
        type: 'line',
        data: {
            labels: labels,
            datasets: [{
                label: 'Bookings by Month',
                data: data,
                backgroundColor: 'rgba(76, 175, 80, 0.2)',
                borderColor: 'rgb(76, 175, 80)',
                borderWidth: 2,
                tension: 0.4
            }]
        },
        options: {
            responsive: true,
            maintainAspectRatio: false,
            scales: {
                y: {
                    beginAtZero: true,
                    grid: {
                        color: gridColor
                    },
                    ticks: {
                        color: textColor
                    }
                },
                x: {
                    grid: {
                        color: gridColor
                    },
                    ticks: {
                        color: textColor
                    }
                }
            },
            plugins: {
                legend: {
                    labels: {
                        color: textColor
                    }
                }
            }
        }
    });
    
    // Store chart in window object for later theme updates
    window.bookingsChart = chart;
}

// Function to update chart themes
function updateChartsTheme(isDarkMode) {
    const textColor = isDarkMode ? '#f8f9fa' : '#212529';
    const gridColor = isDarkMode ? 'rgba(255, 255, 255, 0.1)' : 'rgba(0, 0, 0, 0.1)';
    
    // Update events chart if it exists
    if (window.eventsChart) {
        window.eventsChart.options.scales.y.grid.color = gridColor;
        window.eventsChart.options.scales.x.grid.color = gridColor;
        window.eventsChart.options.scales.y.ticks.color = textColor;
        window.eventsChart.options.scales.x.ticks.color = textColor;
        window.eventsChart.options.plugins.legend.labels.color = textColor;
        window.eventsChart.update();
    }
    
    // Update bookings chart if it exists
    if (window.bookingsChart) {
        window.bookingsChart.options.scales.y.grid.color = gridColor;
        window.bookingsChart.options.scales.x.grid.color = gridColor;
        window.bookingsChart.options.scales.y.ticks.color = textColor;
        window.bookingsChart.options.scales.x.ticks.color = textColor;
        window.bookingsChart.options.plugins.legend.labels.color = textColor;
        window.bookingsChart.update();
    }
} 