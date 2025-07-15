// Modern Navigation JavaScript
document.addEventListener('DOMContentLoaded', function() {
    // Mobile menu toggle functionality
    const mobileMenuToggle = document.querySelector('.mobile-menu-toggle');
    const navLinks = document.querySelector('.nav-links');
    const hamburgerLines = document.querySelectorAll('.hamburger-line');
    
    if (mobileMenuToggle) {
        mobileMenuToggle.addEventListener('click', function() {
            navLinks.classList.toggle('nav-open');
            
            // Animate hamburger lines
            hamburgerLines.forEach((line, index) => {
                if (navLinks.classList.contains('nav-open')) {
                    if (index === 0) {
                        line.style.transform = 'rotate(45deg) translateY(8px)';
                    } else if (index === 1) {
                        line.style.opacity = '0';
                    } else if (index === 2) {
                        line.style.transform = 'rotate(-45deg) translateY(-8px)';
                    }
                } else {
                    line.style.transform = 'rotate(0) translateY(0)';
                    line.style.opacity = '1';
                }
            });
        });
    }
    
    // Close mobile menu when clicking on nav links
    const navLinksElements = document.querySelectorAll('.nav-link');
    navLinksElements.forEach(link => {
        link.addEventListener('click', function() {
            if (navLinks.classList.contains('nav-open')) {
                navLinks.classList.remove('nav-open');
                hamburgerLines.forEach(line => {
                    line.style.transform = 'rotate(0) translateY(0)';
                    line.style.opacity = '1';
                });
            }
        });
    });
    
    // Close mobile menu when clicking outside
    document.addEventListener('click', function(event) {
        if (!event.target.closest('.modern-navbar')) {
            if (navLinks.classList.contains('nav-open')) {
                navLinks.classList.remove('nav-open');
                hamburgerLines.forEach(line => {
                    line.style.transform = 'rotate(0) translateY(0)';
                    line.style.opacity = '1';
                });
            }
        }
    });
    
    // Smooth scrolling for anchor links
    document.querySelectorAll('a[href^="#"]').forEach(anchor => {
        anchor.addEventListener('click', function (e) {
            e.preventDefault();
            const target = document.querySelector(this.getAttribute('href'));
            if (target) {
                target.scrollIntoView({
                    behavior: 'smooth'
                });
            }
        });
    });
    
    // Navbar scroll effect
    let lastScrollY = window.scrollY;
    
    window.addEventListener('scroll', function() {
        const navbar = document.querySelector('.modern-navbar');
        const currentScrollY = window.scrollY;
        
        if (currentScrollY > lastScrollY) {
            // Scrolling down
            navbar.style.transform = 'translateY(-100%)';
        } else {
            // Scrolling up
            navbar.style.transform = 'translateY(0)';
        }
        
        // Add/remove backdrop blur based on scroll position
        if (currentScrollY > 50) {
            navbar.style.background = 'rgba(26, 26, 26, 0.98)';
            navbar.style.backdropFilter = 'blur(20px)';
        } else {
            navbar.style.background = 'rgba(26, 26, 26, 0.95)';
            navbar.style.backdropFilter = 'blur(10px)';
        }
        
        lastScrollY = currentScrollY;
    });
    
    // Intersection Observer for animations
    const observerOptions = {
        threshold: 0.1,
        rootMargin: '0px 0px -50px 0px'
    };
    
    const observer = new IntersectionObserver(function(entries) {
        entries.forEach(entry => {
            if (entry.isIntersecting) {
                entry.target.classList.add('animate-in');
            }
        });
    }, observerOptions);
    
    // Observe elements for animation
    const animatedElements = document.querySelectorAll('.hero-text, .hero-visual, .contest-cta');
    animatedElements.forEach(el => {
        observer.observe(el);
    });
    
    // Parallax effect for hero background
    window.addEventListener('scroll', function() {
        const heroBackground = document.querySelector('.hero-background');
        if (heroBackground) {
            const scrolled = window.pageYOffset;
            const parallaxSpeed = 0.5;
            heroBackground.style.transform = `translateY(${scrolled * parallaxSpeed}px)`;
        }
    });
});

// Utility function to debounce scroll events
function debounce(func, wait) {
    let timeout;
    return function executedFunction(...args) {
        const later = () => {
            clearTimeout(timeout);
            func(...args);
        };
        clearTimeout(timeout);
        timeout = setTimeout(later, wait);
    };
}

// Add loading animation
function showLoading() {
    const loadingElement = document.createElement('div');
    loadingElement.id = 'page-loading';
    loadingElement.innerHTML = `
        <div class="loading-spinner">
            <div class="spinner"></div>
            <p>Loading...</p>
        </div>
    `;
    document.body.appendChild(loadingElement);
}

function hideLoading() {
    const loadingElement = document.getElementById('page-loading');
    if (loadingElement) {
        loadingElement.remove();
    }
}

// Add CSS for loading animation
const loadingCSS = `
    #page-loading {
        position: fixed;
        top: 0;
        left: 0;
        width: 100%;
        height: 100%;
        background: rgba(26, 26, 26, 0.9);
        display: flex;
        justify-content: center;
        align-items: center;
        z-index: 9999;
    }
    
    .loading-spinner {
        text-align: center;
        color: #FFA500;
    }
    
    .spinner {
        width: 50px;
        height: 50px;
        border: 3px solid rgba(255, 165, 0, 0.3);
        border-top: 3px solid #FFA500;
        border-radius: 50%;
        animation: spin 1s linear infinite;
        margin: 0 auto 20px;
    }
    
    @keyframes spin {
        0% { transform: rotate(0deg); }
        100% { transform: rotate(360deg); }
    }
`;

// Inject loading CSS
const style = document.createElement('style');
style.textContent = loadingCSS;
document.head.appendChild(style);
