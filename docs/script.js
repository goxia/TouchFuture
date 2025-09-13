// ===== DOM元素 =====
const hamburger = document.querySelector('.hamburger');
const navMenu = document.querySelector('.nav-menu');
const navLinks = document.querySelectorAll('.nav-link');
const navbar = document.querySelector('.navbar');

// ===== 移动端导航菜单 =====
hamburger.addEventListener('click', () => {
    hamburger.classList.toggle('active');
    navMenu.classList.toggle('active');
});

// 点击导航链接时关闭菜单
navLinks.forEach(link => {
    link.addEventListener('click', () => {
        hamburger.classList.remove('active');
        navMenu.classList.remove('active');
    });
});

// ===== 导航栏滚动效果 =====
window.addEventListener('scroll', () => {
    if (window.scrollY > 100) {
        navbar.classList.add('scrolled');
    } else {
        navbar.classList.remove('scrolled');
    }
});

// ===== 平滑滚动到锚点 =====
document.querySelectorAll('a[href^="#"]').forEach(anchor => {
    anchor.addEventListener('click', function (e) {
        e.preventDefault();
        const target = document.querySelector(this.getAttribute('href'));
        if (target) {
            const headerOffset = 70; // 导航栏高度
            const elementPosition = target.getBoundingClientRect().top;
            const offsetPosition = elementPosition + window.pageYOffset - headerOffset;

            window.scrollTo({
                top: offsetPosition,
                behavior: 'smooth'
            });
        }
    });
});

// ===== 滚动动画观察器 =====
const observerOptions = {
    threshold: 0.1,
    rootMargin: '0px 0px -50px 0px'
};

const observer = new IntersectionObserver((entries) => {
    entries.forEach(entry => {
        if (entry.isIntersecting) {
            entry.target.classList.add('animate-in');
        }
    });
}, observerOptions);

// 观察需要动画的元素
document.querySelectorAll('.feature-card, .demo-container, .download-card').forEach(el => {
    observer.observe(el);
});

// ===== 英雄区域动画 =====
const heroCircles = document.querySelectorAll('.floating-circle');

// 鼠标移动视差效果
document.addEventListener('mousemove', (e) => {
    const mouseX = e.clientX / window.innerWidth;
    const mouseY = e.clientY / window.innerHeight;
    
    heroCircles.forEach((circle, index) => {
        const speed = (index + 1) * 0.02;
        const x = (mouseX - 0.5) * speed * 100;
        const y = (mouseY - 0.5) * speed * 100;
        
        circle.style.transform = `translate(${x}px, ${y}px) scale(1)`;
    });
});

// ===== 特性卡片悬停效果 =====
const featureCards = document.querySelectorAll('.feature-card');

featureCards.forEach(card => {
    card.addEventListener('mouseenter', function() {
        this.style.transform = 'translateY(-10px) scale(1.02)';
    });
    
    card.addEventListener('mouseleave', function() {
        this.style.transform = 'translateY(0) scale(1)';
    });
});

// ===== 演示图片交互 =====
const demoImage = document.querySelector('.demo-image');
const demoPlayBtn = document.querySelector('.demo-play-btn');

if (demoPlayBtn) {
    demoPlayBtn.addEventListener('click', function() {
        // 这里可以添加图片放大或视频播放逻辑
        alert('功能演示：此处可集成图片放大查看或视频演示功能');
    });
}

// ===== 数字动画效果 =====
function animateNumbers() {
    const stats = document.querySelectorAll('.stat-number');
    
    stats.forEach(stat => {
        const text = stat.textContent;
        // 如果包含数字，可以添加计数动画
        if (/\d/.test(text)) {
            stat.classList.add('number-animate');
        }
    });
}

// ===== 页面加载完成后的初始化 =====
document.addEventListener('DOMContentLoaded', function() {
    // 添加页面加载动画
    document.body.classList.add('loaded');
    
    // 初始化数字动画
    animateNumbers();
    
    // 添加键盘导航支持
    document.addEventListener('keydown', function(e) {
        if (e.key === 'Escape') {
            hamburger.classList.remove('active');
            navMenu.classList.remove('active');
        }
    });
});

// ===== 性能优化：防抖滚动处理 =====
function throttle(func, delay) {
    let timeoutId;
    let lastExecTime = 0;
    return function (...args) {
        const currentTime = Date.now();
        
        if (currentTime - lastExecTime > delay) {
            func.apply(this, args);
            lastExecTime = currentTime;
        } else {
            clearTimeout(timeoutId);
            timeoutId = setTimeout(() => {
                func.apply(this, args);
                lastExecTime = Date.now();
            }, delay - (currentTime - lastExecTime));
        }
    };
}

// 优化滚动事件处理
const optimizedScrollHandler = throttle(() => {
    const scrollY = window.scrollY;
    
    // 导航栏效果
    if (scrollY > 100) {
        navbar.classList.add('scrolled');
    } else {
        navbar.classList.remove('scrolled');
    }
    
    // 视差效果
    const heroSection = document.querySelector('.hero');
    if (heroSection) {
        const rect = heroSection.getBoundingClientRect();
        if (rect.bottom > 0) {
            const parallaxSpeed = scrollY * 0.5;
            heroSection.style.transform = `translateY(${parallaxSpeed}px)`;
        }
    }
}, 16); // ~60fps

window.addEventListener('scroll', optimizedScrollHandler);

// ===== 添加一些酷炫的CSS动画类 =====
const style = document.createElement('style');
style.textContent = `
    /* 页面加载动画 */
    body:not(.loaded) .hero-content > * {
        opacity: 0;
        transform: translateY(30px);
    }
    
    body.loaded .hero-content > * {
        opacity: 1;
        transform: translateY(0);
        transition: all 0.8s ease-out;
    }
    
    body.loaded .hero-title { transition-delay: 0.1s; }
    body.loaded .hero-subtitle { transition-delay: 0.2s; }
    body.loaded .hero-buttons { transition-delay: 0.3s; }
    body.loaded .hero-stats { transition-delay: 0.4s; }
    
    /* 滚动动画 */
    .feature-card, .demo-container, .download-card {
        opacity: 0;
        transform: translateY(50px);
        transition: all 0.6s ease-out;
    }
    
    .animate-in {
        opacity: 1 !important;
        transform: translateY(0) !important;
    }
    
    /* 导航栏滚动效果 */
    .navbar.scrolled {
        background: rgba(255, 255, 255, 0.98);
        box-shadow: 0 2px 20px rgba(0, 0, 0, 0.1);
    }
    
    /* 汉堡菜单动画 */
    .hamburger.active .bar:nth-child(2) {
        opacity: 0;
    }
    
    .hamburger.active .bar:nth-child(1) {
        transform: translateY(8px) rotate(45deg);
    }
    
    .hamburger.active .bar:nth-child(3) {
        transform: translateY(-8px) rotate(-45deg);
    }
    
    /* 数字动画效果 */
    .number-animate {
        background: linear-gradient(45deg, #667eea, #764ba2);
        -webkit-background-clip: text;
        -webkit-text-fill-color: transparent;
        background-clip: text;
        animation: shimmer 2s ease-in-out infinite;
    }
    
    @keyframes shimmer {
        0% { background-position: -200% center; }
        100% { background-position: 200% center; }
    }
    
    /* 按钮悬停增强效果 */
    .btn:hover {
        box-shadow: 0 10px 25px rgba(0, 0, 0, 0.2);
    }
    
    /* 特性卡片增强悬停效果 */
    .feature-card:hover .feature-icon {
        animation: bounce 0.6s ease;
    }
    
    @keyframes bounce {
        0%, 20%, 60%, 100% { transform: translateY(0); }
        40% { transform: translateY(-10px); }
        80% { transform: translateY(-5px); }
    }
`;
document.head.appendChild(style);

// ===== 错误处理和兼容性 =====
window.addEventListener('error', function(e) {
    console.warn('页面脚本错误:', e.error);
});

// 检查浏览器兼容性
if (!window.IntersectionObserver) {
    // 为不支持 IntersectionObserver 的浏览器提供fallback
    document.querySelectorAll('.feature-card, .demo-container, .download-card').forEach(el => {
        el.classList.add('animate-in');
    });
}

// ===== 添加一些实用工具函数 =====
const Utils = {
    // 获取元素相对视口的位置
    getElementPosition: (element) => {
        const rect = element.getBoundingClientRect();
        return {
            top: rect.top + window.scrollY,
            left: rect.left + window.scrollX,
            bottom: rect.bottom + window.scrollY,
            right: rect.right + window.scrollX
        };
    },
    
    // 检查元素是否在视口内
    isInViewport: (element) => {
        const rect = element.getBoundingClientRect();
        return (
            rect.top >= 0 &&
            rect.left >= 0 &&
            rect.bottom <= (window.innerHeight || document.documentElement.clientHeight) &&
            rect.right <= (window.innerWidth || document.documentElement.clientWidth)
        );
    },
    
    // 延迟执行函数
    debounce: (func, wait) => {
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
};

// 导出工具函数到全局（如果需要的话）
window.TouchFutureUtils = Utils;