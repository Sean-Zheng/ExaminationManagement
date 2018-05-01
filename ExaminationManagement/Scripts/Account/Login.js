$(document).ready(function () {
    $('#login_btn').click(function () {
        if (checkInput() === false)
            return;
        var username = $('#uername_input').val();
        var password = $('#password_input').val();
        $.ajax({
            beforeSend: loaddingAppear(),
            async: false,
            type: 'post',
            data: {
                username: username,
                password: password
            },
            success: function (result) {
                if (result === 'NotFound') {
                    setTimeout('ErrorPassword()', 500);
                }
                else if (result == 'Admin')
                    setTimeout('window.location.href = "/Admin/Index"',500);
                else if (result == 'Teacher')
                    setTimeout('window.location.href = "/Admin/Index"', 500);
                else if (result == 'Student')
                    setTimeout('window.location.href = "/Admin/Index"', 500);
                else
                    loaddingDisappear();
            },
            error: function () {
                setTimeout('loaddingDisappear()', 1500);
            }
        });
    });
    $('#uername_input').focus(function () {
        tooltipDisappear();
    });
    $('#password_input').focus(function () {
        tooltipDisappear();
    });
});
function ss() {
    alert('ssss');
}
//出现加载图标
function loaddingAppear() {
    $('#login_div').css('display', 'none');
    $('#spinner-container').css('display', 'flex');
}
//消失加载图标
function loaddingDisappear() {
    $('#login_div').css('display', 'block');
    $('#spinner-container').css('display', 'none');
}
//出现提示框
function tooltipAppear(message) {
    $('#tooltip').children('span').text(message);
    $('#tooltip').slideDown();
}
//消失提示框
function tooltipDisappear() {
    if ($('#tooltip').css('display') === 'none')
        return;
    $('#tooltip').fadeOut(1000);
}
//检查输入
function checkInput() {
    var legitimate = true;
    if ($('#uername_input').val() === '') {
        tooltipAppear('用户名不能为空');
        legitimate = false;
    }
    else if ($('#password_input').val() === '') {
        tooltipAppear('密码不能为空');
        legitimate = false;
    }
    return legitimate;
}
//密码错误执行
function ErrorPassword() {
    loaddingDisappear();
    tooltipAppear("用户名或密码不正确");
    $('#password_input').val('');
}