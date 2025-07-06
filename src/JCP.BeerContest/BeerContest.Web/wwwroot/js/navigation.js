// Enhanced navigation behavior for mobile
window.blazorNavigation = {
    // Initialize navigation event handlers
    init: function() {
        this.setupMobileMenuBehavior();
        this.setupClickOutsideHandler();
    },

    // Setup mobile menu behavior
    setupMobileMenuBehavior: function() {
        const navbarToggler = document.querySelector('.navbar-toggler');
        const navbarCollapse = document.querySelector('.navbar-collapse');
        
        if (navbarToggler && navbarCollapse) {
            // Handle navbar toggler click
            navbarToggler.addEventListener('click', function(e) {
                e.preventDefault();
                const isOpen = navbarCollapse.classList.contains('show');
                
                if (isOpen) {
                    navbarCollapse.classList.remove('show');
                    navbarToggler.setAttribute('aria-expanded', 'false');
                } else {
                    navbarCollapse.classList.add('show');
                    navbarToggler.setAttribute('aria-expanded', 'true');
                }
            });

            // Handle nav link clicks (close menu on mobile)
            const navLinks = document.querySelectorAll('.navbar-nav .nav-link');
            navLinks.forEach(link => {
                link.addEventListener('click', function() {
                    // Close mobile menu when clicking a link
                    if (window.innerWidth <= 991.98) {
                        navbarCollapse.classList.remove('show');
                        navbarToggler.setAttribute('aria-expanded', 'false');
                    }
                });
            });
        }
    },

    // Setup click outside handler to close mobile menu
    setupClickOutsideHandler: function() {
        document.addEventListener('click', function(event) {
            const navbarCollapse = document.querySelector('.navbar-collapse');
            const navbarToggler = document.querySelector('.navbar-toggler');
            
            if (navbarCollapse && navbarToggler) {
                const isClickInsideNav = navbarCollapse.contains(event.target) || 
                                       navbarToggler.contains(event.target);
                
                if (!isClickInsideNav && navbarCollapse.classList.contains('show')) {
                    navbarCollapse.classList.remove('show');
                    navbarToggler.setAttribute('aria-expanded', 'false');
                }
            }
        });
    }
};

// Initialize when DOM is loaded
document.addEventListener('DOMContentLoaded', function() {
    window.blazorNavigation.init();
});

// Re-initialize after Blazor updates (for SPA navigation)
window.addEventListener('load', function() {
    // Small delay to ensure Blazor has finished rendering
    setTimeout(function() {
        window.blazorNavigation.init();
    }, 100);
});
