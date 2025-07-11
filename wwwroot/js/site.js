// 页面加载完成后执行
document.addEventListener('DOMContentLoaded', function() {
    // 激活当前导航项
    activateCurrentNavItem();
    
    // 初始化工具提示
    initTooltips();
    
    // 平滑滚动
    initSmoothScroll();
    
    // 表单验证增强
    enhanceFormValidation();
});

// 激活当前导航项
function activateCurrentNavItem() {
    const currentPath = window.location.pathname;
    document.querySelectorAll('.navbar-nav .nav-link').forEach(link => {
        const href = link.getAttribute('href');
        if (href === currentPath || (currentPath.includes(href) && href !== '/')) {
            link.classList.add('active');
        }
    });
}

// 初始化工具提示
function initTooltips() {
    if (typeof bootstrap !== 'undefined' && bootstrap.Tooltip) {
        const tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'));
        tooltipTriggerList.map(function (tooltipTriggerEl) {
            return new bootstrap.Tooltip(tooltipTriggerEl);
        });
    }
}

// 平滑滚动
function initSmoothScroll() {
    document.querySelectorAll('a[href^="#"]').forEach(anchor => {
        anchor.addEventListener('click', function (e) {
            e.preventDefault();
            
            const targetId = this.getAttribute('href');
            if (targetId === '#') return;
            
            const targetElement = document.querySelector(targetId);
            if (targetElement) {
                window.scrollTo({
                    top: targetElement.offsetTop - 80,
                    behavior: 'smooth'
                });
            }
        });
    });
}

// 表单验证增强
function enhanceFormValidation() {
    const forms = document.querySelectorAll('.needs-validation');
    
    Array.from(forms).forEach(form => {
        form.addEventListener('submit', event => {
            if (!form.checkValidity()) {
                event.preventDefault();
                event.stopPropagation();
            }
            
            form.classList.add('was-validated');
        }, false);
    });
}

// 返回顶部按钮
window.onscroll = function() {
    const backToTopBtn = document.getElementById('backToTop');
    if (backToTopBtn) {
        if (document.body.scrollTop > 20 || document.documentElement.scrollTop > 20) {
            backToTopBtn.style.display = "block";
        } else {
            backToTopBtn.style.display = "none";
        }
    }
};

function scrollToTop() {
    window.scrollTo({
        top: 0,
        behavior: 'smooth'
    });
}
