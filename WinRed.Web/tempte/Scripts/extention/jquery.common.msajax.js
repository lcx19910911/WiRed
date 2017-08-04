
jQuery.Common = function () {
}
jQuery.Common = {
    $parent: self.parent.$,
    //写入cookie
    setCookie: function (name, value) {
        var exp = new Date();
        exp.setTime(exp.getTime() + 20000);
        document.cookie = name + "=" + escape(value) + ";expires=" + exp.toGMTString() + ";path=/";
    },
    ///删除cookie
    delCookie: function (name) {
        var exp = new Date();
        exp.setTime(exp.getTime() - 1);
        var cval = $.Common.getCookie(name);
        if (cval != null) document.cookie = name + "=" + cval + ";expires=" + exp.toGMTString() + ";path=/";
    },
    //读取cookie
    getCookie: function (name) {
        var arr, reg = new RegExp("(^| )" + name + "=([^;]*)(;|$)");
        if (arr = document.cookie.match(reg))
            return unescape(arr[2]);
        else
            return null;
    },
    dateCompare: function (d1, d2) {
        return Date.parse(d1.replace(/-/g, "/")) > Date.parse(d2.replace(/-/g, "/"));
    },
    dateFormat: function (_d) {
        var result = _d.getFullYear() +
            '-' +
            (parseInt(_d.getMonth() + 1) < 10 ? '0' + parseInt(_d.getMonth() + 1) : parseInt(_d.getMonth() + 1)) +
            '-' +
            (_d.getDate() < 10 ? '0' + _d.getDate() : _d.getDate());
        return result;
    },
    ajaxRequest: function (url, args, datatype, methodcallback, isusedprogress) {
        $.ajax({
            type: "post",
            url: url,
            cache: false,
            async: true,
            data: args || {},
            datatype: datatype || "json",
            beforeSend: function () {
                if (isusedprogress) {
                    setTimeout(function () {
                            $.showLoading('...');
                        },
                        0);
                }
            },
            success: function (json) {
                $.hideLoading();
                if (methodcallback != null) {
                    return methodcallback(json);
                } else {
                    if (json.status) {
                        window.location.reload();
                    } else {
                        $.Common.showMsg('提示信息', json.message);
                    }
                }
            },
            complete: function () {
                    $.hideLoading();
            },
            error: function (httpRequest, textStatus, errorThrownt) {
                $.hideLoading();
                $.Common.showMsg(textStatus, errorThrownt);
            }
        });
    },
    uploadFormImage: function (form, img) {
        $("#" + form)
            .ajaxSubmit({
                url: '/UploadFile/UploadImage',
                type: 'post',
                dataType: 'json',
                beforeSend: function () {
                    setTimeout(function () {
                        $.showLoading('正在上传...');
                    },
                        0);
                },
                complete: function () {
                    setTimeout(function () {
                        $.hideLoading();
                    },
                        0);
                },
                success: function (e) {
                    if (e.error === 0) {
                        $("#" + img).attr("src", e.url);
                        $("#" + img).attr("imageId", e.imageId);
                    }
                }
            });
    },
    showMsg: function (title, msg) {
        $.alert(msg);
    },
    showLoading: function (title) {
        $.showLoading(title);
    },
    hideLoading: function (title) {
        $.hideLoading();
    },
    showImg: function (title, img) {
        //$.alert({ title: title, content: '<img src="' + img + '" width="200" height="200"/>' });
    },
    previewImage: function (img) {
        wx.previewImage({
            current: img, // 当前显示图片的http链接
            urls: [img] // 需要预览的图片http链接列表
        });
    },
    initScrollLoad: function (option) {
        window.page = option.page;
        window.isLoad = false;
        var par = option.par || {};
        $(option.name).scroll(function () {
            var viewH = $(this).height();//可见高度
            var contentH = $(this).get(0).scrollHeight;//内容高度
            var scrollTop = $(this).scrollTop();//滚动高度
            var range = contentH - viewH - scrollTop;
            if (!window.isLoad) {
                if (range <= 130) {
                    window.page++;
                    par.page = window.page;
                    if (window.page < option.totalPage) {
                        window.isLoad = true;
                        $.Common.ajaxRequest(option.url,
                            par,
                            'HTML',
                            function (e) {
                                $(option.target).append(e);
                                window.isLoad = false;
                            });
                    }
                }
            }
        });
    }
};

//全局捕获Ajax异常信息
$.ajaxSetup({
    error: function (httpRequest, textStatus, errorThrown) {
        //$.Common.showMsg(httpRequest, textStatus, errorThrown);
    }
});

function gotoUrl(url) {
    window.location = url;
}

function navBusiness(lat, lng, name, address) {

    wx.openLocation({
        latitude: lat, // 纬度，浮点数，范围为90 ~ -90
        longitude: lng, // 经度，浮点数，范围为180 ~ -180。
        name: name, // 位置名
        address: address, // 地址详情说明
        scale: 10, // 地图缩放级别,整形值,范围从1~28。默认为最大
        infoUrl: 'http://car.skl7.com' // 在查看位置界面底部显示的超链接,可点击跳转
    });
}