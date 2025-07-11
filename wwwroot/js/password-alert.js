// 密码修改提示相关脚本
document.addEventListener('DOMContentLoaded', function() {
    // 检查是否存在密码修改提示
    const passwordAlert = document.querySelector('.password-change-alert');
    
    if (passwordAlert) {
        // 添加标记类到 body
        document.body.classList.add('has-password-alert');
        
        // 调整主内容区域的顶部内边距
        const mainContent = document.querySelector('main');
        if (mainContent) {
            const alertHeight = passwordAlert.offsetHeight;
            mainContent.style.paddingTop = alertHeight + 'px';
        }
    }
});
