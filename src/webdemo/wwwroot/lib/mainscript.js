var ResourcesJson = ["ALL"];

function documentCheck() {
    ///<summary>
    /// 用于在通过AJAX的形式取得新的页面内容之后,重新刷新页面信息以便实现验证
    ///</summary>

    $.validator.unobtrusive.parse(document);
}

//数组删除元素 传入数组下标
Array.prototype.remove = function(dx) {
    if (isNaN(dx) || dx > this.length) { return false; }
    this.splice(dx, 1);
    return this;
}

//webFileUrl 请求地址
//jsonArgs参数 {}
//callBackFunc 回调 
//是否异步async 默认true
//是否开启遮罩  默认true
$.AjaxServer = function(webFileUrl, jsonArgs, callBackFunc, async, isTip) {
    if (isTip == null) {
        isTip = false;
    }
    try {
        $.ajax({
            url: webFileUrl,
            data: jsonArgs,
            type: 'Post',
            async: false,
            cache: false,
            contentType: 'application/json;charset=UTF-8',
            success: function(data, textStatus, rqinfo) {
                if (typeof(data) == "object" && typeof(data.Status) != "undefined" && data.Status == 0) {
                    if ($.alert == undefined) {
                        alert(data.ErrorSimple);
                    } else {
                        $.alert(data.ErrorSimple);
                    }
                } else {
                    callBackFunc(data, textStatus, rqinfo);
                }
            },
            error: function() {
                $.alert('提交错误');
            },
            complete: function(XHRequest, T) {
                XHRequest = null
            },
            isTip: isTip
        });
    } catch (e) {
        alert(e)
    }
}

//判断浏览器是否是ie6
function isie6() {

    return false;
}

/****************全局遮罩******************/
$(document).ajaxSend(ajaxTips).ajaxComplete(unTips);

function ajaxTips(event, xhr, settings) {
    settings._ajaxartDialogClose = false;
}

function unTips(event, xhr, settings) {
    if (settings._ajaxartDialogClose != null) {
        setTimeout($.unblockUI, 1000);
    }
    if (typeof dataSrc != "undefined") {
        dataSrc();
    }
}
/****************全局遮罩End******************/

//页面加载完成时执行
function PagerInfo() {
    documentCheck();
}

$(PagerInfo)

/****************对话框******************/

//funok可以是事件或者跳转地址
$.confirm = function(content, funok) {
    var confirm = $.scojs_confirm({
        content: content,
        action: function() {
            funok();
            this.close();
            this.destroy();
        }
    });
    confirm.show();
}

$.alert = function(message) {
    var alert = $.scojs_modal({
        title: message,
        content: '<div class="modal-footer"><a class="btn cancel" href="#" data-dismiss="modal">Ok</a></div>'
    });
    alert.show();
}

$.tipsOk = function(message) {
    $.scojs_message(message, $.scojs_message.TYPE_OK);
}

$.tipsError = function(message) {
    $.scojs_message(message, $.scojs_message.TYPE_ERROR);
}

function OnConfirm(obj, onajax) {

    try {
        var col = $(obj);
        var onAction = function() {
            var button = this;
            $.AjaxServer(col.attr("url"), {}, function(obj) {
                eval(onajax)(obj);
                button.close();
                button.destroy();
            });

        };
        var id = newGuid();
        var confirm = $.scojs_confirm({
            content: col.attr("data-title"),
            action: onAction,
            target: "#" + id
        });
        confirm.show();
    } catch (e) {

    }
    return false;
}

var OnDataSelect = function(id, val) {
    Acontent.close();
    return false;
};
var Acontent = null;

$(function() {
    $("[data-src]").each(function(p, obj) {
        $(obj).unbind().click(function() {

            var title = $(this).attr("title");
            if (title == undefined) {
                title = "对话框";
            }
            var inputid = $(this).attr("data-input");
            var inputval = $(this);
            if (inputid != undefined) {
                OnDataSelect = function(id, val) {
                    $("#" + inputid).val(id);
                    inputval.html(val);
                    Acontent.close();
                };
            }
            $.AjaxServer($(this).attr("data-src"), {}, function(content) {
                Acontent = $.scojs_modal({
                    title: title,
                    content: content
                });
                Acontent.show();
                documentCheck();
            });

            return false;
        })
    })
})

//生成对话框ID
function newGuid() {
    var guid = "";
    for (var i = 1; i <= 32; i++) {
        var n = Math.floor(Math.random() * 16.0).toString(16);
        guid += n;
        if ((i == 8) || (i == 12) || (i == 16) || (i == 20))
            guid += "-";
    }
    return guid;
}

$(function() {
    dataSrc();
})

function dataSrc() {
    $("[data-src]").each(function(p, obj) {
        $(obj).unbind().click(function() {
            var title = $(this).attr("title");
            if (title == undefined) {
                title = "对话框";
            }
            var css = $(this).attr("data-css");
            if (css == undefined) {
                css = '';
            }

            var inputid = $(this).attr("data-input");
            var inputval = $(this);
            if (inputid != undefined) {
                OnDataSelect = function(id, val) {
                    $("#" + inputid).val(id);
                    inputval.html(val);
                    Acontent.close();
                };
            }
            $.AjaxServer($(this).attr("data-src"), {}, function(content) {
                Acontent = $.scojs_modal({
                    title: title,
                    cssClass:css,
                    content: content
                });
                Acontent.show();
                documentCheck();
            });
            return false;
        })
    })
    ResourcesFun();
}

function IsResources(obj, txt) {
    if (txt == "#" || txt == "/" || obj.attr("nores") == "true" || ResourcesJson[0] == 'ALL') {
        return true;
    }
    txt = txt.split('?')[0];
    if (txt.indexOf("/") == 0) {
        txt = txt.substring(1, txt.length);
    }
    if (txt.indexOf("/") == -1) {
        txt = txt + "/Index";
    }

    txt = txt.split('/')[0] + "-" + txt.split('/')[1];


    for (var i = 0; i < ResourcesJson.length; i++) {
        //if (ResourcesJson[i] == "wms-" + txt) {
        //    return true;
        //}
        if (ResourcesJson[i] == txt) {
            return true;
        }
    }
    $(obj).hide();
    return false;
}

function ResourcesFun() {
    $("a[href]").each(function(p, obj) {
        IsResources($(obj), $(obj).attr("href"))
    })

    $("a[data-src]").each(function(p, obj) {
        IsResources($(obj), $(obj).attr("data-src"))
    })

    $("input[data-power]").each(function(p, obj) {
        IsResources($(obj), $(obj).attr("data-power"));
    })
    $("[resmenu]").each(function(p, obj) {
        var hide = true;
        $(obj).parent(".dropdown-submenu").find(".dropdown-menu>li>a:hidden").each(function(p, col) {

            if ($(col).css("display") != "none") {
                hide = false;
            }
        });
        if (hide) {
            $(obj).hide();
        }
    })
}
$(ResourcesFun);

