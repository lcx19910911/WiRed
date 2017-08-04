$(function () {
    $.Common.initScrollLoad({
        page: 1,
        par: { room: $("#searchInput").val() },
        name: '.page-meg',
        totalPage: parseInt($("#hidTotalPage").val()),
        url: '/Group/LoadGroup',
        target: "#list"
    });

    $("#searchInput")
        .keydown(function (event) {
            var key = $("#searchInput").val();
            var e = event || window.event || arguments.callee.caller.arguments[0];
            if (e.keyCode) {
                if (key !== "" && e.keyCode === 13) {
                    window.location = "?room=" + key;
                }
            }
        });

    var $searchBar = $('#searchBar'),
            $searchResult = $('#searchResult'),
            $searchText = $('#searchText'),
            $searchInput = $('#searchInput'),
            $searchClear = $('#searchClear'),
            $searchCancel = $('#searchCancel');

    function hideSearchResult() {
        $searchResult.hide();
        $searchInput.val('');
    }
    function cancelSearch() {
        hideSearchResult();
        $searchBar.removeClass('weui-search-bar_focusing');
        $searchText.show();
    }

    $searchText.on('click', function () {
        $searchBar.addClass('weui-search-bar_focusing');
        $searchInput.focus();
    });
    $searchInput
            .on('blur', function () {
                if (!this.value.length) cancelSearch();
            })
            .on('input', function () {
                if (this.value.length) {
                    $searchResult.show();
                } else {
                    $searchResult.hide();
                }
            })
    ;
    $searchClear.on('click', function () {
        hideSearchResult();
        $searchInput.focus();
    });
    $searchCancel.on('click', function () {
        cancelSearch();
        $searchInput.blur();
    });
});

function intoGroup(groupId) {
    $.Common.ajaxRequest('/Group/AddGroupMember',
        { groupId: groupId },
        'JSON',
        function (e) {
            if (e.status) {
                window.location = e.pageUrl;
            } else {
                $.alert(e.message);
                window.location = e.pageUrl;
            }
        },
        true);
}

function changePrice(obj) {
    var price = $(obj).val();
    var url = $("#routeUrl").val();
    if (url.indexOf('?') >= 0) {
        window.location = url + "&price=" + price;
    } else {
        window.location = url + "?price=" + price;
    }
}