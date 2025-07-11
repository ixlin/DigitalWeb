// 管理后台JS功能
$(document).ready(function() {
    // 确保用户下拉菜单正常工作
    $('.nav-item.dropdown.user-menu').click(function(e) {
        $(this).toggleClass('show');
        $(this).find('.dropdown-menu').toggleClass('show');
    });
    
    // 点击其他区域关闭下拉菜单
    $(document).click(function(e) {
        if (!$(e.target).closest('.nav-item.dropdown.user-menu').length) {
            $('.nav-item.dropdown.user-menu').removeClass('show');
            $('.nav-item.dropdown.user-menu .dropdown-menu').removeClass('show');
        }
    });

    // 确保AdminLTE侧边栏菜单项高亮
    var url = window.location.pathname;
    var element = $('.nav-sidebar a').filter(function() {
        return this.href.endsWith(url);
    }).addClass('active');
    
    element.parents('li').addClass('menu-open');
    
    // 修复移动设备上的菜单显示
    $('[data-widget="pushmenu"]').on('click', function() {
        $('body').toggleClass('sidebar-collapse');
        $('body').toggleClass('sidebar-open');
    });
});
