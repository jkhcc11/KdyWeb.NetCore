//视频加载错误加载
function videoError() {
    var div = document.createElement('div');
    div.innerHTML = '<div style="position:relative;top:50%"><div style="position:relative;font-size:16px;line-height:16px;top:-8px">加载视频失败，请更换播放源或刷新尝试</div></div>';
    div.setAttribute('style', 'width:100%;height:100%;text-align:center;background:rgba(0,0,0,0.8);position:absolute;color:#FFF');
    document.querySelector('.ABP-Video').insertBefore(div, document.querySelector('.ABP-Video>:first-child'));
    document.getElementById('info-box').parentNode.removeChild(document.getElementById('info-box'));
}
//发送弹幕
function sendComment(e) {
    console.log(e);
    var pd = {};
    pd.Size = e.detail.fontsize;
    pd.Color = e.detail.color;
    pd.Message = e.detail.message;

    pd.Stime = e.detail.playTime;
    pd.Mode = e.detail.mode;
    pd.UserId = 0;

    $.post('/TestVideo/SendDanMu', pd, function (rd) {
        console.log(rd);
    });
    //var xhr = new XMLHttpRequest();
    //xhr.open('POST', 'https://www.5dm.tv/player/post.php', true);
    //var param = [
    //    'size=' + e.detail.fontsize,
    //    'color=' + e.detail.color,
    //    'message=' + e.detail.message,
    //    'cid=byzgyddf1',
    //    'stime=' + e.detail.playTime,
    //    'mode=' + e.detail.mode,
    //    'uid=0'
    //];
    //xhr.addEventListener('readystatecange', sendComment.result);
    //xhr.setRequestHeader("Content-type", "application/x-www-form-urlencoded");
    //xhr.send(param.join('&'));
}
//sendComment.result = function (e) {
//    if (e.target.readyState == 4) {
//        if (e.target.status == 200) {
//            var code = parseInt(e.target.response);
//            if (isNaN(code))
//                abpinst.createPopup('弹幕发送失败	网络错误', 3000);
//        } else {
//            abpinst.createPopup('弹幕发送失败	网络错误', 3000);
//        }
//    }
//}
$(function () {
    var abpOptions = { scale: 1, opacity: 1, speed: 1, useCSS: false, autoOpacity: true }
    var settings = localStorage.html5Settings || '{}';
    settings = JSON.parse(settings);
    abpOptions = Object.assign(abpOptions, settings);

    var inst = ABP.create(document.getElementById("player"), {
        src: {
            playlist: [{
                video: document.getElementById("video")
            }]
        },
        width: '100%',
        height: '100%',
        config: abpOptions,
        mobile: isMobile()
    });
    window.abpinst = inst;
   // 弹幕加载
    CommentLoader('/TestVideo/DanMu', abpinst.cmManager);
    // dots.init({ 
    //    id:'dots',
    //    width:'100%',
    //    height:'100%', 
    //    r:16, 
    //    thick:4 
    //}); 
    // dots.runTimer();
    //sendComment = function (e) {
    //}
    window.tid = 76;
    //function toggleCommentByTid() {
    //    var tidSet = JSON.parse(localStorage.tidComment || '{}');
    //    if (tidSet[tid] === false) {
    //        abpinst.btnDm.classList.contains('on') && abpinst.btnDm.click();
    //    } else {
    //        !abpinst.btnDm.classList.contains('on') && abpinst.btnDm.click();
    //    }
    //}
    //function recordTid() {
    //    var tidSet = JSON.parse(localStorage.tidComment || '{}');
    //    tidSet[tid] = abpinst.btnDm.classList.contains('on');
    //    localStorage.tidComment = JSON.stringify(tidSet);
    //}

    $('.shield-enrty')[0].addEventListener('click', shield.show);
    $('.shield_top .close')[0].addEventListener('click', shield.show);
    $('.shield_item .add')[0].addEventListener('click', shield.add);
    abpinst.txtText.disabled = false;
    abpinst.playerUnit.addEventListener('sendcomment', sendComment);
    abpinst.txtText.style.textAlign = 'center';
    //abpinst.btnDm.addEventListener('click', recordTid);
    //toggleCommentByTid();

    var div = document.createElement('div');
    div.className = 'on';
    div.appendChild(document.createTextNode('超清'));
    abpinst.playerUnit.querySelector('.BiliPlus-Scale-Menu .Video-Defination').appendChild(div);
});