
$(function () {
    var messageHub = $.connection.messageHub;messageHub.hubName = 'messageHub';
    messageHub.connection.start();
    messageHub.client.sendMessage = function (e) {
        //var doc = document.getElementById('main').contentWindow.document;
        $("#notice").html('<span>' + e + '</span>');
    };

    messageHub.client.notifyMemberCount = function (e) {
        //var doc=document.getElementById('main').contentWindow.document;
        $("#memberCount").html(e);
    };
    
    messageHub.client.sendRedPack = function (e) {
        if (e.SweepType === 2) {
            var memberId = parseInt($("#hidMemberId").val());
            if (e.MemberId !== memberId) {
                //var doc = document.getElementById('main').contentWindow.document;
                var template = $("#otherPack1").html();
                var str = $.format(template, e.MemberImage, e.Title, e.Money, e.SweepTypeStr, e.PackCount, e.Sweep, "showBg(" + e.Id + ")", e.Id, e.Member);
                $(str).appendTo($("#chatBox"));
            }
            if (e.MemberId === memberId) {
                //var doc = document.getElementById('main').contentWindow.document;
                var template = $("#myPack1").html();
                var str = $.format(template, e.MemberImage, e.Title, e.Money, e.SweepTypeStr, e.PackCount, e.Sweep, "showBg(" + e.Id + ")", e.Id, e.Member);
                $(str).appendTo($("#chatBox"));
            }
        } else {
            var memberId = parseInt($("#hidMemberId").val());
            if (e.MemberId !== memberId) {
                //var doc = document.getElementById('main').contentWindow.document;
                var template = $("#otherPack").html();
                var str = $.format(template, e.MemberImage, e.Title, e.Money, e.SweepTypeStr, e.PackCount, e.Sweep, "showBg(" + e.Id + ")", e.Id, e.Member);
                $(str).appendTo($("#chatBox"));
            }
            if (e.MemberId === memberId) {
                //var doc = document.getElementById('main').contentWindow.document;
                var template = $("#myPack").html();
                var str = $.format(template, e.MemberImage, e.Title, e.Money, e.SweepTypeStr, e.PackCount, e.Sweep, "showBg(" + e.Id + ")", e.Id, e.Member);
                $(str).appendTo($("#chatBox"));
            }
        }
       
        scrollToEnd();
    };

    messageHub.client.takeRedPack = function (e) {
        var template = $("#takePackInfo").html();
        var str = $.format(template,e.Title);
        $(str).appendTo($("#chatBox"));
        if (e.LessCount > 0) {
            $("#p" + e.GroupPackId).html('*');
        } else {
            $("#p" + e.GroupPackId).html(e.LessCount);
        }
        scrollToEnd();
    };

    messageHub.client.sendBoon = function (e) {
        var template = $("#takePackInfo").html();
        var ary = [];
        for (var i = 0; i < e.length; i++) {
            ary.push($.format(template, e[i]));
        }
        $(ary.join(' ')).appendTo($("#chatBox"));
        scrollToEnd();
    };

    messageHub.client.reLoadBalance = function(e) {
        $("#memberBalance").html(e);
        $.Common.ajaxRequest('/Member/SetBalance', { balance: e }, 'JSON', function(e) {}, false);
    };

    messageHub.client.newGroup = function () {
        //if (!window.refDate) {
        //    window.refDate = new Date();
        //}
        //var curDate = new Date();
        //var days = curDate.getTime() - window.refDate.getTime();
        //var time = parseInt(days / 3600000);
        //if (time > 3) {
        //    var doc = document.getElementById('main').contentWindow.document;
        //    var list = $("#list", doc);
        //    if (list) {
        //        $.Common.ajaxRequest('/Group/LoadGroup',
        //            {
        //                page: 1
        //            },
        //            'HTML',
        //            function (e) {
        //                var doc = document.getElementById('main').contentWindow.document;
        //                $("#list", doc).html(e);
        //                window.refDate = new Date();
        //            },
        //            true);
        //    }
        //}
    }

    messageHub.client.outGroup=function() {
        $.alert("您已被群主移出该房间");
        gotoUrl('/Group/Index');
    }

    messageHub.client.releaseGroup = function () {
        $.alert("房主已经解散了房间");
        gotoUrl('/Group/Index');
    }

    messageHub.client.autoReleaseGroup = function () {
        $.alert("因长时间未操作，系统自动解散了房间");
        gotoUrl('/Group/Index');
    }

    messageHub.client.navTo = function (msg, url) {
        if (msg !== undefined && msg.length > 0) {
            $.alert(msg);
        }
        setTimeout(function () { gotoUrl(url); }, 2000);
        
    }

    messageHub.client.showPlay = function (msg, url) {
        $("#hasMember").show();
        $("#noMember").hide();
        scrollToEnd();
    }

    messageHub.client.setLessCount = function (groupPackId, lessCount) {
        if (e.LessCount > 0) {
            $("#p" + groupPackId).html('*');
        } else {
            $("#p" + groupPackId).html(lessCount);
        }
    }
});

function scrollToEnd() {
    //滚动到底部
    var h = $(document).height() - $(window).height();
    $(document).scrollTop(h);
}