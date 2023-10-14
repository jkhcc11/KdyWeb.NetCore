//解析客户使用
/**
 * 播放器初始化 初始化弹幕或腾讯
 */
function InitPlay(config) {
    var palyUrl = config.url,
        playId = config.elem,
        url = location.hostname,
        sourceType = 'video/mp4';
    palyUrl = this.jie(palyUrl);
    var system = this.checkSys();
    if (palyUrl.indexOf(".m3u8") !== -1) {
        sourceType = "application/x-mpegURL";
    }
    if (system.ios || system.ipad || system.android || system.mac) {
        document.getElementById(playId).innerHTML = '<video width="100%" height="100%" controls="controls" autoplay="autoplay" poster="//kdyimg.hcc11.com/Img/1168149110100987904" webkit-playsinline x-webkit-airplay>' +
            '<source src="' + palyUrl + '" type="' + sourceType + '"></video>';
        return;
    }
    $("#" + playId).html('');
    this.initDplayer(playId, palyUrl);
}
InitPlay.prototype = {
    /**
     * 初始化dplayer
     * @param {承载id} playId
     * @param {url} vurl 
     * @returns {} 
     */
    initDplayer: function (playId, vurl) {
        var positionMark = "parse_flag_" + vurl.substring(0, 100), type = 'normal';
        if (vurl.lastIndexOf('.m3u8') !== -1) {
            type = 'hls';
        }
        var dplayer = new DPlayer({
            element: document.getElementById(playId),
            autoplay: true,
            video: {
                url: vurl,
                type: type
            }
        });
        dplayer.on('progress', function () {
            //console.log(dplayer.video.currentTime);
            var now = dplayer.video.currentTime, //当前
                pm = localStorage.getItem(positionMark), //记录值,
                parsePm = parseInt(pm); //记录值转为ini
            if (now > 3) {
                //播放中设置记录
                localStorage.setItem(positionMark, now);
                return;
            }
            if (now > 0 && now < 1 && isNaN(parsePm) === false && parsePm > 1) {
                //不再播放器中且当前时间是刚开始
                dplayer.seek(parsePm);
            }
        });
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
        var n = (url.length - 7) / 2, o = url.substring(0, n), i = url.substring(n + 7);
        return o + i;
    }
};
if (result_v2.isSuccess === false) {
    document.getElementById('div_player').innerHTML = '解析失败,请稍后重试或联系管理员';
} else {
    var c = {
        url: result_v2.data,
        elem: 'div_player',
        dmId: ''
    };
    if (window.navigator.webdriver ||
        window.callPhantom ||
        window._phantom ||
        window.webdriver) {
        console.log('403');
    } else {
        var play = new InitPlay(c);
    }
}