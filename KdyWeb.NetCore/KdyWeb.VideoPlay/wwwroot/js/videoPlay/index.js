//t5播放器初始化
/**
 * 初始化
 */
function InitVideoPlay() {
}

InitVideoPlay.prototype = {

    /**
     * 初始化
     * playId：承载视频divId
     * vurl：播放url
     * epId：弹幕标识
     */
    init: function (playId, vurl, epId) {
        playId = playId || "playercontainer";
        var vtype = "mp4",
            positionMark,
            that = this,
            system = that.checkSys(),
            sourceType = 'video/mp4';
        //var tempV = vurl.split('$');//腾讯的
        if (vurl.indexOf(".m3u8") > 0 || (vurl.indexOf("g3proxy.lecloud.com") > 0 && vurl.indexOf("tss=mp4") === -1)) {
            vtype = "m3u8"; //乐视的 不是所有都是m3u8
            sourceType = "application/x-mpegURL";
        }
        if (system.ios || system.android || system.ipad) { //手机版h5
            document.getElementById(playId)
                .innerHTML =
                '<video id="kdy_video" width="100%" height="100%" controls="controls" autoplay="autoplay" poster="//kdyimg.hcc11.com/Img/1168148903544098816" webkit-playsinline x-webkit-airplay>' +
                '<source src="' +
                vurl +
                '" type="' +
                sourceType +
                '"></video>';
            this.autoNext(vurl);
            return;
        }
        if (epId != null && epId > 0 && (vtype === "mp4" || vtype === "m3u8")) {
            that.intDanMu(playId, vurl, epId);
            return;
        }

        that.initAli(playId, false, vurl);
    },
    /*
     * 初始化阿里播放器
     * playId：承载id
     * isLive：是否为直播流
     * vurl：播放url
     */
    initAli: function (playId, isLive, vurl) {
        playId = playId || "playercontainer";
        isLive = isLive || false;
        // 初始化播放器
        var player = new Aliplayer({
            id: playId, // 容器id
            source: vurl,
            playsinline: true,
            trackLog: false,
            autoplay: true, //自动播放：否
            width: "100%", // 播放器宽度
            height: "100%", // 播放器高度
            useH5Prism: false,
            isLive: isLive
        });
    },
    /*
     * 初始化弹幕播放器
     * playId：承载id
     * vurl:播放地址
     * epId：弹幕标识
     */
    intDanMu: function (playId, vurl, epId) {
        //document.write('');
        $("#" + playId).html('');
        $("body")
            .append('<div class="video"><video id="kdy_video" poster="" preload="auto" autobuffer="true" data-setup="{}" webkit-playsinline autoplay="autoplay"><source src="' + vurl + '"><div><b>您使用的浏览器不支持HTML5视频...</b></div></video></div>');
        if (vurl.indexOf('.m3u8') > 0) {
            //m3u8强制https
            vurl = vurl.replace('http://', 'https://');
            //m3u8地址
            var video = document.getElementById('kdy_video');
            //加载hls m3u8
            var hls = new Hls();
            hls.loadSource(vurl);
            hls.attachMedia(video);
            hls.on(Hls.Events.MANIFEST_PARSED,
                function () {
                    video.play();
                });
        }


        $("body").on('error',
            'video source',
            function (e) {
                var $div = document.createElement('div');
                $div
                    .innerHTML =
                    '<div style="position:relative;top:50%"><div style="position:relative;font-size:16px;line-height:16px;top:-8px">加载视频失败，请刷新、重试或加QQ群：128931211反馈</div></div>';
                $div.setAttribute('style',
                    'width:100%;height:100%;text-align:center;background:rgba(0,0,0,0.8);position:absolute;color:#FFF');
                document.querySelector('.ABP-Video')
                    .insertBefore($div, document.querySelector('.ABP-Video>:first-child'));
                document.getElementById('info-box').parentNode.removeChild(document.getElementById('info-box'));
            });
        var abpOptions = { scale: 1, opacity: 1, speed: 1, useCSS: false, autoOpacity: true }
        var settings = localStorage.html5Settings || '{}';
        settings = JSON.parse(settings);
        abpOptions = Object.assign(abpOptions, settings);
        var inst = ABP.create(document.getElementById(playId),
            {
                src: {
                    playlist: [
                        {
                            video: document.getElementById("kdy_video")
                        }
                    ]
                },
                width: '100%',
                height: '100%',
                config: abpOptions,
                mobile: isMobile()
            });
        window.abpinst = inst;

        //默认加载一次
        loadDandu();
        //然后1分钟在请求一次
        var timer = setInterval(loadDandu, 30000);

        function loadDandu() {
            var url = '/api/VideoDanMu/get/' + epId + '?timestamp=' + Math.round(new Date() / 1000);
            // 弹幕加载
            CommentLoader(url, abpinst.cmManager);
        }

        $('.shield-enrty')[0].addEventListener('click', shield.show);
        $('.shield_top .close')[0].addEventListener('click', shield.show);
        $('.shield_item .add')[0].addEventListener('click', shield.add);
        abpinst.txtText.disabled = false;
        abpinst.playerUnit.addEventListener('sendcomment',
            function (e) {
                //console.log(e);
                var pd = {}, vToken = $("#vToken").val();
                pd.DSize = e.detail.fontsize;
                pd.DColor = e.detail.color;
                pd.Msg = e.detail.message;
                pd.DTime = e.detail.playTime;
                pd.DMode = e.detail.mode;
                pd.EpId = epId;

                $.ajax({
                    type: "put",
                    url: "/api/VideoDanMu/create",
                    headers: { "play.antiforgery": vToken },
                    contentType: "application/json; charset=utf-8",
                    data: JSON.stringify(pd),
                    dataType: "json"
                });
            });
        abpinst.txtText.style.textAlign = 'center';
        //abpinst.btnDm.addEventListener('click', recordTid);
        //toggleCommentByTid();

        var div = document.createElement('div');
        div.className = 'on';
        div.appendChild(document.createTextNode('超清'));
        abpinst.playerUnit.querySelector('.BiliPlus-Scale-Menu .Video-Defination').appendChild(div);

        this.autoNext(vurl);
    },
    /**
     * 获取解析平台播放地址
     * @param {} reqUrl 
     * @param {} enUrl 
     * @returns {} 
     */
    getApiUrl: function (reqUrl, enUrl) {
        var that = this, pd = {};
        pd.url = enUrl;
        $.ajax({
            url: reqUrl + '/KdyApiDown/Parse',
            data: pd,
            type: 'POST',
            dataType: 'jsonp',
            jsonp: 'jsonpCallback',
            jsonpCallback: "success_ck",
            success: function (json) {
                if (json.Code !== 200) {
                    $("#play-content").html("<div style=\"font-size: 25px;color:red;text-align: center\">" +
                        json.ErrMsg +
                        "</div>");
                    return;
                }
                var v = that.jie(json.ResultData);
                that.init('play-content', v, epId);
            }
        });
    },
    /**
    * 检查系统
    * @returns {} 
    */
    checkSys: function () {
        var system = {
            win: false,
            mac: false,
            ios: false,
            android: false,
            ipad: false
        };
        var plat = navigator.platform;
        if (plat == null || plat.length <= 0) {
            return system;
        }
        plat = plat.toLowerCase();
        //检测平台 
        system.win = plat.indexOf("win") === 0;
        system.mac = plat.indexOf("mac") === 0;
        system.ios = plat.indexOf("iphone") === 0;
        system.android = plat.indexOf("linux") === 0;
        system.ipad = (navigator.userAgent.match(/iPad/i) != null) ? true : false;
        return system;
    },
    /**
   * 解密
   * @param {加密url} url 
   * @returns {} 
   */
    jie: function (url) {
        url = this.strRevers(url);
        url = this.htoStr(url);
        return this.decodeStr(url);
    },
    /**
     * 16进制转字符串
     * @param {} hexStr 
     * @returns {} 
     */
    htoStr: function (hexStr) {
        var val = "";
        for (var i = 0; i < hexStr.length; i = i + 2) {
            var temp = hexStr[i] + hexStr[i + 1];
            //两个字符一拼接 然后转为10进制
            temp = parseInt(temp, 16);
            val += String.fromCharCode(temp);
        }
        return val;
    },
    /**
     * 字符串逆序
     * @returns {} 
     */
    strRevers: function (str) {
        return str.split('').reverse();
    },
    /**
     * 还原随机字符串加密
     * @param {} url 
     * @returns {} 
     */
    decodeStr: function (url) {
        var n = (url.length - 6) / 2, o = url.substring(0, n), i = url.substring(n + 6);
        return o + i;
    },
    /**
     * 自动下一集
     * */
    autoNext: function (vurl) {
        var kdyVideo = document.getElementById("kdy_video"),
            positionMark = "kdy666." + SparkMD5.hash(vurl);

        //结束事件
        kdyVideo.addEventListener("ended",
            function () {
                var decodeData = JSON.parse(dataJson);
                var pushData = {},
                    nextEpId = decodeData.data.nextEpId;
                pushData.epId = nextEpId;
                if (pushData.epId != null && pushData.epId > 0) {
                    window.parent.postMessage(pushData, '*');

                    //移除当前记录 跳转下一集
                    localStorage.removeItem(positionMark);
                    location.href = decodeData.data.nextEpUrl;
                }
            });
        if (window.localStorage == false) {
            //浏览器不支持localstorage
            return false;
        }
        //使用事件监听方式捕捉事件， 此事件可作为实时监测video 播放状态
        kdyVideo.addEventListener("timeupdate",
            function () {
                var now = kdyVideo.currentTime, //当前
                    pm = localStorage.getItem(positionMark), //记录值
                    parsePm = parseInt(pm); //记录值转为ini
                if (now > 2) {
                    //播放中设置记录
                    localStorage.setItem(positionMark, now);
                    return;
                }
                if (now > 0 && now < 2 && isNaN(parsePm) === false && parsePm > 1) {
                    //不再播放器中且当前时间是刚开始
                    kdyVideo.currentTime = parsePm;
                }
            }, false);
    }
}

$(function () {

    var host = location.hostname;
    //if (host !== "api.hcc11.com") {
    //    $("#play-content").html("<div style=\"font-size: 25px;color:red;text-align: center\">解析失败，请尝试刷新或更换播放地址！请加QQ群：128931211反馈</div>");
    //    return;
    //}
    var initVideo = new InitVideoPlay();

    var decodeData = JSON.parse(dataJson);
    if (decodeData.isSuccess === false) {
        $("#play-content").html("<div style=\"font-size: 25px;color:red;text-align: center\">" + decodeData.msg + "</div>");
        return;
    }

    var deCodeUrl = initVideo.jie(decodeData.data.playUrl);
    initVideo.init('play-content', deCodeUrl, decodeData.data.epId);
});

